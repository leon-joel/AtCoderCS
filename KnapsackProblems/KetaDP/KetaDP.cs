using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// 桁DP
//
namespace KetaDP
{
	public static class Util
	{
		public static string DumpToString<T>(IEnumerable<T> array) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var item in array) {
				sb.Append(item);
				sb.Append(", ");
			}
			return sb.ToString();
		}

		public static void InitDP<T>(T[,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					dp[i, j] = value;
				}
			}
		}
	}

	public class SolverBase
	{
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
		virtual protected int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		virtual protected long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);
		virtual protected void WriteLine(double d) => Console.WriteLine($"{d:F9}");

		[Conditional("DEBUG")]
		protected void Dump(double d) => Console.WriteLine($"{d:F9}");
		[Conditional("DEBUG")]
		protected void Dump<T>(T value) => Console.WriteLine(value);
		[Conditional("DEBUG")]
		protected void Dump<T>(IEnumerable<T> array) where T : IFormattable {
			string s = Util.DumpToString(array);
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(s);
			//_writer.WriteLine(s);
		}
		[Conditional("DEBUG")]
		protected void DumpBit(int value) => Console.WriteLine(Convert.ToString(value, 2));
		[Conditional("DEBUG")]
		protected void DumpBit(long value) => Console.WriteLine(Convert.ToString(value, 2));

		[Conditional("DEBUG")]
		protected void DumpGrid<T>(IEnumerable<IEnumerable<T>> arrayOfArray) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				sb.AppendLine(Util.DumpToString(a));
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}
		[Conditional("DEBUG")]
		protected void DumpDP<T>(T[,] dp) where T : IFormattable {
			var sb = new StringBuilder();
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					sb.Append(dp[i, j]);
					sb.Append(", ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
		}
	}

	// ABC007-D: 禁止された数字
	// https://abc007.contest.atcoder.jp/tasks/abc007_4
	public class Solver : SolverBase
	{
		long[,] DP;
		readonly int[] Digits = { 0, 1, 2, 3, 5, 6, 7, 8 };
		long SolveDP(string S) {
			// 問題は[A, B]範囲だが、ここでは下限をAではなく常に1として考える。
			//   ※最終的には ans = f(B) - f(A-1) とする。
			//
			// DP[pos桁目][制約の有無rist] = 禁止数が含まれないパターン数 
			//   posは文字列の先頭を1とする
			//   rist は制約あり:1 制約なし:0 とする
			// 
			// ※禁止数が含まれないパターン数をカウントするほうが簡単な気がする。
			//  最後にint(s)から引いて、禁止数が含まれるパターン数を返している。
			// 
			DP = new long[S.Length+1, 2];
			// 初期状態 ※1桁目は必ず制約あり
			DP[0, 1] = 1;

			// 配るDP
			for (int pos = 0; pos < S.Length; pos++) {
				for (int rist = 0; rist <= 1; rist++) {
					if (DP[pos, rist] == 0) continue;

					if (rist == 0) {
						// 制約なし
						// 禁止数字は4,9の2つあるので、配布先には8倍で配る
						DP[pos + 1, rist] += DP[pos, rist] * 8;
					} else {
						// 制約ありなので、Sから制約となる数字を取得する
						var upper = (char)S[pos] - (char)'0';

						foreach (var v in Digits) {
							if (v == upper) {
								DP[pos + 1, 1] += DP[pos, rist];

							} else if (v < upper) {
								DP[pos + 1, 0] += DP[pos, rist];
							}
						}
					}
				}
			}
			//Console.WriteLine($"{S} ==========");
			//DumpDP(DP);

			var l = long.Parse(S);
			// 制限なし ＋ 制限あり - オール0の分
			return l - (DP[S.Length, 0] + DP[S.Length, 1] - 1);
		}

		public void Run() {
			var ary = ReadStringArray();
			var A = ary[0];
			var B = ary[1];

			var A1 = (long.Parse(A) - 1).ToString();
			var ans = SolveDP(B) - SolveDP(A1);
			WriteLine(ans);
		}
#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	// ABC029-D: 桁DPで解くこともできる問題
	// https://abc029.contest.atcoder.jp/tasks/abc029_d
	public class SolverABC029D : SolverBase
	{
		public void Run() {
			var S = ReadLine();

			// DP[pos桁目][1を使った回数j][制約の有無rist] = 条件を満たす数の総数
			//   posは文字列の先頭を1とする
			//   121xxx, 110xxx なら j は 2 ※N<=10^9なので最大10個
			//   rist は制約あり:1 制約なし:0 とする
			//      ※制約あり: pos桁目まで上限数Sに張り付いていて、
			//                次の桁の数字選択に制約が生じるという意味
			// 
			var DP = new long[S.Length + 1, 11, 2];
			// [0桁目, 1の登場回数0回, 制約あり] = 1個 とする
			DP[0, 0, 1] = 1;

			Action<long[,,]> DumpDP2 = (dp) => {
				var sb = new StringBuilder();
				for (int i = 0; i < dp.GetLength(0); i++) {
					sb.Append($"{i}: ");
					for (int j = 0; j < dp.GetLength(1); j++) {
						sb.Append($"{dp[i, j, 0]}-{dp[i, j, 1]}");
						sb.Append(", ");
					}
					sb.AppendLine();
				}
				Console.WriteLine(sb.ToString());
			};
			// 配るDP
			for (int pos = 0; pos < S.Length; pos++) {
				for (int j = 0; j < 11; j++) {
					for (int rist = 0; rist <= 1; rist++) {
						if (DP[pos, j, rist] == 0) continue;

						if (rist == 0) {
							// 制約なし
							// ここで1を選択した場合:
							// pos+1桁目までに1がj+1回登場した数の総数 = pos桁目までに1がj回登場した数の総数
							DP[pos + 1, j + 1, 0] += DP[pos, j, 0];
							// 1以外を選択した場合:
							// pos+1桁目までに1がj回登場した数の総数 = pos桁目までに1がj回登場した数の総数 * 9
							DP[pos + 1, j, 0] += DP[pos, j, 0] * 9;
						} else {
							// 制約ありなので、制約となる数字を取得する
							var upper = S[pos] - '0';

							if (upper == 0) {
								// 登場回数を増やさず、そのまま加算
								DP[pos + 1, j, 1] += DP[pos, j, 1];
							} else if (upper == 1) {
								// 0を選んだ場合: 登場回数を増やさず、制約なしの方に加算
								DP[pos + 1, j, 0] += DP[pos, j, 1];
								// 1を選んだ場合: 登場回数+1して、制約ありの方に加算
								DP[pos + 1, j + 1, 1] += DP[pos, j, 1];
							} else if (1 < upper) {
								// 1を選んだ場合: 登場回数+1して、制約なしの方に加算
								DP[pos + 1, j + 1, 0] += DP[pos, j, 1];
								// upperをを選んだ場合: 登場回数を増やさず制約ありの方に加算
								DP[pos + 1, j, 1] += DP[pos, j, 1];
								// 上記以外を選んだ場合（upper-1通り）:
								//                    登場回数を増やさず、制約なしの方に加算
								DP[pos + 1, j, 0] += DP[pos, j, 1] * (upper - 1);
							}
						}
					}
				}
				//Console.WriteLine($"pos: {pos} =======");
				//DumpDP2(DP);
			}
			long ans = 0;
			for (int j = 1; j < 11; j++) {
				// += (制約なし + 制約あり) * 出現個数
				ans += (DP[S.Length, j, 0] + DP[S.Length, j, 1]) * j;
			}
			WriteLine(ans);
		}

#if !MYHOME
		public static void Main(string[] args) {
			new SolverABC029D().Run();
		}
#endif
	}

	// Code Festival 2014 予選A: D問題: 桁DPで解くこともできる問題
	// https://code-festival-2014-quala.contest.atcoder.jp/tasks/code_festival_qualA_d
	public class SolverCodeFes2014D : SolverBase {
		string A;
		long LA;
		int K;
		public void Run() {
			var ary = ReadStringArray();
			A = ary[0];
			LA = long.Parse(A);
			Dump(LA);
			K = int.Parse(ary[1]);

			// 何種類必要？
			int bit = 0;
			int cnt = 0;
			for (int i = 0; i < A.Length; i++) {
				var n = A[i] - '0';

				// 1桁目=0, 2桁目=1, ...
				if (0 < (bit & (1 << n))) {
					bit |= 1 << n;
					++cnt;
				}
			}
			//Dump(A);
			//DumpBit(bit);
			//Dump(cnt);

			// K種類以下しか使わなくていいなら即Return
			if (cnt <= K) {
				WriteLine(0);
				return;
			}

			// K-1 使うまではAと同じ数字をトレース
			// K種類目で
			//   Aと同じ数字   : 以降は再帰的に調べる
			//   Aと同じ数字-1 : 以降は最大数字を使用
			//   Aと同じ数字+1 : 以降は最小数字を使用
			// の3パターンを試していく
		}

		int FindMinNum(int bit) {
			for (int i = 0; i < 10; i++) {
				if (0 < (bit & (1 << i)))
					return i;
			}
			Debug.Assert(false);
			return 0;
		}
		long Recursive(string s, int bit) {
			// s: これまでに確定した数字列
			// bit: 使用できる数字

			if (A.Length < s.Length) {
				var ls = long.Parse(s);
				return Math.Abs(ls - LA);
			}
			if (s.Length == A.Length) {
				var ls = long.Parse(s);
				if (LA <= ls) {
					return ls - LA;
				} else {
					var d = LA - ls;
					var minNum = FindMinNum(bit);
					var ls10 = ls * 10 + minNum;
					var d10 = Math.Abs(ls10 - LA);


				}
			}
			return long.Parse(s);

			//for (int i = 0; i < 10; i++) {
			//	if (0 < (bit & (1 << i))) {
			//		// bitが立っている＝使える数字

			//	}

			//}
		}

#if !MYHOME
		public static void Main(string[] args) {
			new SolverCodeFes2014D().Run();
		}
#endif
	}
}

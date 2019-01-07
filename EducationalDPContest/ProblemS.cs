using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Educational DP Contest
// https://atcoder.jp/contests/dp/tasks
namespace EducationalDPContestS

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

		public static void InitArray<T>(T[] ary, T value) {
			for (int i = 0; i < ary.Length; i++) {
				ary[i] = value;
			}
		}

		public static void InitDP<T>(T[,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					dp[i, j] = value;
				}
			}
		}

		public static T Max<T>(params T[] nums) where T : IComparable {
			if (nums.Length == 0) return default(T);

			T max = nums[0];
			for (int i = 1; i < nums.Length; i++) {
				max = max.CompareTo(nums[i]) > 0 ? max : nums[i];
			}
			return max;
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
		virtual protected double[] ReadDoubleArray() => ReadLine().Split(' ').Select<string, double>(s => double.Parse(s)).ToArray();
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine(double d) => Console.WriteLine($"{d:F9}");
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);

		[Conditional("DEBUG")]
		protected void Dump(string s) => Console.WriteLine(s);
		[Conditional("DEBUG")]
		protected void Dump(char c) => Console.WriteLine(c);
		[Conditional("DEBUG")]
		protected void Dump(double d) => Console.WriteLine($"{d:F9}");
		[Conditional("DEBUG")]
		protected void Dump<T>(IEnumerable<T> array) where T : IFormattable {
			string s = Util.DumpToString(array);
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(s);
			//_writer.WriteLine(s);
		}
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

	public class Solver : SolverBase
	{
		const int MOD = 1000000007;
		public void Run() {
			var S = ReadLine();
			var D = ReadInt();
			//for (int i = 0; i < S.Length; i++) {
			//	// 文字列の先頭からダンプ
			//	Dump(S[i]);
			//}

			// DP[pos桁目][各桁の和sum][制約の有無rist] = パターン数
			//   posは文字列の先頭を1とする
			//   sumは mod D したもの
			//   rist は制約あり:1 制約なし:0 とする
			//   パターン数は mod MOD したもの
			long[,,] DP = new long[S.Length + 1, D, 2];
			// 初期状態 ※1桁目は必ず制約あり
			DP[0, 0, 1] = 1;

			// 配るDP
			for (int pos = 0; pos < S.Length; pos++) {
				for (int sum = 0; sum < D; sum++) {
					for (int rist = 0; rist <= 1; rist++) {
						if (DP[pos, sum, rist] == 0) continue;

						if (rist == 0) {
							for (int d = 0; d < 10; d++) {
								// 配布
								DP[pos + 1, (sum + d) % D, rist] += DP[pos, sum, rist];
								DP[pos + 1, (sum + d) % D, rist] %= MOD;
							}
						} else {
							// 制約ありなので、Sから制約となる数字を取得する
							var upper = (char)S[pos] - (char)'0';

							for (int d = 0; d < upper; d++) {
								// 制約なしの部分なので、配布先は制約なしとなる
								DP[pos + 1, (sum + d) % D, 0] += DP[pos, sum, rist];
								DP[pos + 1, (sum + d) % D, 0] %= MOD;
							}
							// 制約ありの部分
							DP[pos + 1, (sum + upper) % D, 1] += DP[pos, sum, rist];
							DP[pos + 1, (sum + upper) % D, 1] %= MOD;
						}
					}
				}
			}
			// 制限なし ＋ 制限あり - オール0の分
			// ★★★ 引き算する場合,MODを加算してから行わないと、ansが負になってしまう場合がある
			var ans = (DP[S.Length, 0, 0] + DP[S.Length, 0, 1] + MOD - 1) % MOD;
			WriteLine(ans);
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

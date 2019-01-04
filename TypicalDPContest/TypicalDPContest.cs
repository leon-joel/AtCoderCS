using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TypicalDPContest
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
	}

	// 典型的なDPコンテスト
	// https://tdpc.contest.atcoder.jp/tasks/tdpc_contest
	//
	// A: コンテスト 
	public class Solver
	{
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
		virtual protected int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		virtual protected long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);

		[Conditional("DEBUG")]
		void Dump<T>(IEnumerable<T> array) {
			var sb = new StringBuilder();
			foreach (var item in array) {
				sb.Append(item);
				sb.Append(", ");
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}

		int N;
		int[] Nums;
		int UpperW = 100 * 100;

		public void Run() {
			N = ReadInt();
			Nums = ReadIntArray();

			//var ans = DpSolve();
			var ans = DistributeDP();
			WriteLine(ans);
		}

		// 動的計画法（DP）（＝漸化式＋ループ）での実装
		// 貰うDP
		int DpSolve() {
			var DP = new bool[N + 1, UpperW + 1];
			//for (int i = 0; i < N + 1; i++) {
			//	for (int j = 0; j < UpperW + 1; j++) {
			//		DP[i, j] = false;
			//	}
			//}
			DP[0, 0] = true;

			for (int i = 0; i < N; i++) {
				for (int w = 0; w < UpperW + 1; w++) {
					if (w < Nums[i]) {
						// 現在itemの重さより左側（現在itemが使えない部分）は上から下ろしてくるだけ
						DP[i + 1, w] = DP[i, w];
					} else {
						// 現在itemの重さ以上の部分（右側）は
						// 重さ分左のvalue | 現在value or 下ろしてくるだけ
						DP[i + 1, w] = DP[i, w] | DP[i, w - Nums[i]];
					}
				}
			}

			int cnt = 0;
			for (int i = 0; i < UpperW+1; i++) {
				if (DP[N, i]) ++cnt;
			}

			return cnt;
		}

		// 配るDP ＋ 1次元配列DP
		int DistributeDP() {
			// 配るDPは、配布先のIndexOutOfRangeチェックが面倒なので、
			// DP配列の範囲を必要なだけ拡げておく
			var DP = new bool[UpperW + 1 + 100];
			DP[0] = true;

			// 配るDP --- dp[w] からの遷移先を更新する＝遷移元でループする
			foreach (var point in Nums) {
				// ※1次元DPなので、2度配りしないように末尾からループする
				for (int w = UpperW-1; 0 <= w; w--) {
					// 配布元がfalse（＝配布元にくばる経路がない）はスキップ
					if (DP[w]) {
						// 点数リストでループし、配布先に配る
						DP[w + point] = true;
					}
				}
			}
			int cnt = 0;
			for (int i = 0; i < UpperW; i++) {
				if (DP[i]) ++cnt;
			}
			return cnt;
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	// B: ゲーム 2次元DP
	public class SolverB
	{
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
		virtual protected int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		virtual protected long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);

		[Conditional("DEBUG")]
		void Dump<T>(IEnumerable<T> array) where T : IFormattable {
			string s = Util.DumpToString(array);
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(s);
			//_writer.WriteLine(s);
		}
		[Conditional("DEBUG")]
		void DumpGrid<T>(IEnumerable<IEnumerable<T>> arrayOfArray) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				sb.AppendLine(Util.DumpToString(a));
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}
		[Conditional("DEBUG")]
		void DumpDP<T>(T[,] dp) where T : IFormattable {
			var sb = new StringBuilder();
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					sb.Append(dp[i,j]);
					sb.Append(", ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
		}

		public void Run() {
			var ary = ReadIntArray();
			var nA = ary[0];
			var nB = ary[1];

			var numsA = ReadIntArray();
			var numsB = ReadIntArray();

			// 2次元DP
			int[,] dp = new int[nA + 1, nB + 1];
			// DP[A山の残り数, B山の残り数] = すぬけが取りうる最大の価値
			// 初期化: DP[0, 0] = 0
			//
			// A山[1] B山[2, 10]の場合 ※先手は常にすぬけ君
			// ※^^はすぬけ君が取った手。無印は相手の手。
			//       10    2  ←B山
			//   0 ← 10 ← 10
			//   ↑   ^^   ↑
			// 1 1 ←  1   11
			//  ^^        ^^
			// A山

			// すぬけ君の手番 ※トータル数が奇数なら残数奇数が手番になる
			int myTurn = (nA + nB) % 2;

			// 左端を先に全部埋める ※余計なIndex境界判定を不要にできる
			for (int i = 1; i < nA + 1; i++) {
				if (i % 2 == myTurn) {
					// すぬけ番なので上のに現在価値を加算して格納
					dp[i, 0] = dp[i - 1, 0] + numsA[nA - i];
				} else {
					// 相手の番なので上のをそのまま格納
					dp[i, 0] = dp[i - 1, 0];
				}
			}
			// 同様に上端を全部埋める
			for (int j = 1; j < nB + 1; j++) {
				if (j % 2 == myTurn) {
					// すぬけ番なので上のに現在価値を加算して格納
					dp[0, j] = dp[0, j - 1] + numsB[nB - j];
				} else {
					// 相手の番なので上のをそのまま格納
					dp[0, j] = dp[0, j - 1];
				}
			}
			//DumpDP(dp);

			// 貰うDP
			for (int i = 1; i < nA + 1; i++) {
				for (int j = 1; j < nB + 1; j++) {
					if ((i + j) % 2 == myTurn) {
						// すぬけ君の手番（＝価値を最大化する）
						// 上＋A or 左＋B の大きい方を選択し、書き込み
						var a = dp[i - 1, j] + numsA[nA - i];
						var b = dp[i, j - 1] + numsB[nB - j];
						dp[i, j] = Math.Max(a, b);
					} else {
						// 相手の手番（＝すぬけの価値を最小化する）
						// 上 or 左 の小さい方を選択し、書き込み
						dp[i, j] = Math.Min(dp[i - 1, j], dp[i, j - 1]);
					}
				}
			}
			//DumpDP(dp);

			WriteLine(dp[nA, nB]);
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TypicalDPContest
{
	// 典型的なDPコンテスト
	// https://tdpc.contest.atcoder.jp/tasks/tdpc_contest
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
}

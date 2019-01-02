using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PartialSumProblems
{
	// DP練習 〜 部分和問題とその応用たち
	// https://qiita.com/drken/items/a5e6fe22863b7992efdb#3-%E9%83%A8%E5%88%86%E5%92%8C%E5%95%8F%E9%A1%8C%E3%81%A8%E3%81%9D%E3%81%AE%E5%BF%9C%E7%94%A8%E3%81%9F%E3%81%A1
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
		int UpperW;
		int[] Nums;

		// 問題 3:　部分和問題　
		public void Run() {
			var ary = ReadIntArray();
			N = ary[0];
			UpperW = ary[1];

			Nums = ReadIntArray();

			var ans = DpSolve();
			//var ans = Bfs();
			WriteLine(ans ? "Yes" : "No");
		}

		// 動的計画法（DP）（＝漸化式＋ループ）での実装
		bool DpSolve() {
			var DP = new bool[N + 1, UpperW + 1];
			// boolのデフォルト値はfalseなので明示的な初期化不要
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

			return DP[N, UpperW];
		}

		// 幅優先探索
		// ※queueに格納される個数が多くなりすぎないか心配
		bool Bfs() {
			var DP = new bool[UpperW + 1];
			DP[0] = true;

			var que = new Queue<int>();
			que.Enqueue(0);

			// BFS
			while (0 < que.Count) {
				// queueに入っている＝配布可能
				var v = que.Dequeue();

				foreach (var w in Nums) {
					if (v + w <= UpperW && !DP[v + w]) {
						// 配布先に初めて配る＝キューに入れる
						que.Enqueue(v + w);
						DP[v + w] = true;
					}
				}
			}

			return DP[UpperW];
		}

		// 問題 4:　部分和数え上げ問題　
		public void RunPartialCountUp() {
			var ary = ReadIntArray();
			N = ary[0];
			UpperW = ary[1];

			Nums = ReadIntArray();

			var ans = DpSolveForPartialCountUp();
			WriteLine(ans);
		}
		// 動的計画法（DP）（＝漸化式＋ループ）での実装
		const int MOD = 1000000009;
		int DpSolveForPartialCountUp() {
			var DP = new int[N + 1, UpperW + 1];
			for (int i = 0; i < N + 1; i++) {
				for (int j = 0; j < UpperW + 1; j++) {
					DP[i, j] = 0;
				}
			}
			DP[0, 0] = 1;

			for (int i = 0; i < N; i++) {
				for (int w = 0; w < UpperW + 1; w++) {
					if (w < Nums[i]) {
						// 現在itemの重さより左側（現在itemが使えない部分）は上から下ろしてくるだけ
						DP[i + 1, w] = DP[i, w];
					} else {
						// 現在itemの重さ以上の部分（右側）は
						// 重さ分左のvalue | 現在value or 下ろしてくるだけ
						DP[i + 1, w] = (DP[i, w] + DP[i, w - Nums[i]]) % MOD;
					}
				}
			}

			return DP[N, UpperW];
		}


#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

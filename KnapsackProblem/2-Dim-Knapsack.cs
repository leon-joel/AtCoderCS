using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TwoDimensionalKnapsack
{
	// 2次元ナップサックDP
	// https://qiita.com/drken/items/a5e6fe22863b7992efdb#4-%E4%BA%8C%E6%AC%A1%E5%85%83%E3%83%8A%E3%83%83%E3%83%97%E3%82%B5%E3%83%83%E3%82%AF-dp-----%E5%BC%BE%E6%80%A7%E3%83%9E%E3%83%83%E3%83%81%E3%83%B3%E3%82%B0%E3%82%84diff%E3%82%B3%E3%83%9E%E3%83%B3%E3%83%89%E3%81%AE%E4%BB%95%E7%B5%84%E3%81%BF%E3%81%AA%E3%81%A9
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


		public static T Max<T>(params T[] nums) where T : IComparable {
			if (nums.Length == 0) return default(T);

			T max = nums[0];
			for (int i = 1; i < nums.Length; i++) {
				max = max.CompareTo(nums[i]) > 0 ? max : nums[i];
			}
			return max;
		}

		// 問題 8:　最長共通部分列(LCS) 問題
		public void Run() {
			var s0 = ReadLine();
			var s1 = ReadLine();

			var ans = DpSolve(s0, s1);
			WriteLine(ans);
		}

		// 動的計画法（DP）（＝漸化式＋ループ）での実装
		// http://www.deqnotes.net/acmicpc/1458/
		// http://d.hatena.ne.jp/naoya/20090328/1238251033
		int DpSolve(string s0, string s1) {
			int[,] DP = new int[s0.Length + 1, s1.Length + 1];
			//for (int i = 0; i < s0.Length + 1; i++) {
			//	for (int j = 0; j < s1.Length + 1; j++) {
			//		DP[i, j] = 0;
			//	}
			//}

			// 貰うDP
			for (int i = 1; i < s0.Length + 1; i++) {
				for (int j = 1; j < s1.Length + 1; j++) {
					if (s0[i - 1] != s1[j - 1]) {
						DP[i, j] = Max(DP[i - 1, j - 1], DP[i - 1, j], DP[i, j - 1]);
					} else {
						DP[i, j] = Max(DP[i - 1, j - 1] + 1, DP[i - 1, j], DP[i, j - 1]);
					}
				}
			}

			return DP[s0.Length, s1.Length];
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

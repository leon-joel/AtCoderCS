using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Educational DP Contest
// https://atcoder.jp/contests/dp/tasks
namespace EducationalDPContest.F
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
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);
		virtual protected void WriteLine(double d) => Console.WriteLine($"{d:F9}");

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
		string DpSolve(string s0, string s1) {
			int[,] DP = new int[s0.Length + 1, s1.Length + 1];

			// DP[s0文字列i番目まで][s1文字列j番目まで] = 最長部分文字列長

			// 貰うDP
			for (int i = 1; i < s0.Length + 1; i++) {
				for (int j = 1; j < s1.Length + 1; j++) {
					if (s0[i - 1] != s1[j - 1]) {
						// 一致しなかったので、上から左から大きい方をコピーしてくる
						DP[i, j] = Util.Max(DP[i - 1, j - 1], DP[i - 1, j], DP[i, j - 1]);
					} else {
						// 一致したので、左上＋1, 上, 左 から最大のものを選ぶ
						// ★これは常に 左上＋1 でいいような気がする。
						//  https://qiita.com/_rdtr/items/c49aa20f8d48fbea8bd2
						DP[i, j] = Util.Max(DP[i - 1, j - 1] + 1, DP[i - 1, j], DP[i, j - 1]);
					}
				}
			}

			DumpDP(DP);

			// 部分文字列を逆ダンプ
			int x = s0.Length, y = s1.Length;
			char[] ret = new char[DP[x, y]];
			while (0 < x && 0 < y) {
				var v = DP[x, y];
				if (v == DP[x - 1, y] || v == DP[x, y - 1]) {
					// 確定しない -> 同じ値の方に進む
					if (v == DP[x - 1, y]) {
						--x;
					} else {
						--y;
					}
				} else {
					// 文字確定 -> 斜め上に進む
					--x;
					--y;
					ret[v - 1] = s0[x];
				}
			}

			var s = string.Join("", ret);
			return s;
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

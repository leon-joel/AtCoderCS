using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Educational DP Contest
// https://atcoder.jp/contests/dp/tasks
namespace EducationalDPContest.D
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
		struct Item
		{
			public readonly int Weight;
			public readonly int Value;

			public Item(int v, int w) {
				Value = v;
				Weight = w;
			}
			public Item(int[] ary) {
				Weight = ary[0];
				Value = ary[1];
			}
			public override string ToString() {
				return string.Format($"V:{Value} W:{Weight}");
			}
		}

		int N;
		int UpperW;
		Item[] Items;

		public void Run() {
			var ary = ReadIntArray();
			N = ary[0];
			UpperW = ary[1];

			Items = new Item[N];
			for (int i = 0; i < N; i++) {
				var item = new Item(ReadIntArray());
				//Console.WriteLine(item.ToString());
				Items[i] = item;
			}
			//Dump(items);

			var ans = DpSolve();
			WriteLine(ans);
		}

		// 動的計画法（DP）（＝漸化式＋ループ）での実装
		long[,] DP;
		long DpSolve() {
			DP = new long[N + 1, UpperW + 1];
			for (int i = 0; i < N + 1; i++) {
				for (int j = 0; j < UpperW + 1; j++) {
					DP[i, j] = 0;
				}
			}

			for (int i = 0; i < N; i++) {
				var item = Items[i];
				for (int w = 0; w < UpperW + 1; w++) {
					if (w < item.Weight) {
						// 現在itemの重さより左側（現在itemが使えない部分）は上から下ろしてくるだけ
						DP[i + 1, w] = DP[i, w];
					} else {
						// 現在itemの重さ以上の部分（右側）は
						// 重さ分左のvalue + 現在value or 下ろしてくるだけ
						DP[i + 1, w] = Math.Max(DP[i, w - item.Weight] + item.Value, DP[i, w]);
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

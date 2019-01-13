using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Educational DP Contest
// https://atcoder.jp/contests/dp/tasks
namespace EducationalDPContest.B
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
		public void Run() {
			var ary = ReadIntArray();
			var N = ary[0];
			var K = ary[1];

			var Steps = ReadIntArray();

			var DP = new int[N];

			// 貰うDP
			for (int i = 1; i < N; i++) {
				// K歩前から飛んでくるときの総コストを算出し、最小値を採択
				int minCost = int.MaxValue;
				for (int k = 1; k <= K && 0 <= i - k ; k++) {
					var cost = Math.Abs(Steps[i] - Steps[i - k]) + DP[i - k];
					if (cost < minCost) minCost = cost;
				}
				DP[i] = minCost;

				//Console.WriteLine($"{i} ====");
				//Dump(DP);
			}

			WriteLine(DP[N - 1]);
		}
#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

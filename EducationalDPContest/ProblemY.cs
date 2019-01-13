using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Educational DP Contest
// https://atcoder.jp/contests/dp/tasks
namespace EducationalDPContest.Y
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

	public class XY : IComparable<XY>, IFormattable
	{
		public readonly int X;
		public readonly int Y;

		public XY(int x, int y) {
			X = x;
			Y = y;
		}
		public XY(int[] ary) {
			X = ary[0];
			Y = ary[1];
		}

		public int CompareTo(XY other) {
			var dx = this.X - other.X;
			if (0 < dx)
				return 1;
			else if (dx < 0)
				return -1;
			else {
				var dy = this.Y - other.Y;
				if (0 < dy)
					return 1;
				else if (dy < 0)
					return -1;
				else
					return 0;
			}
		}
		public override bool Equals(object obj) {
			if (!(obj is XY other)) return false;
			return CompareTo(other) == 0;
		}

		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public override string ToString() {
			return ToString(null, null);
		}
		// format等の引数は一切無視
		public string ToString(string format, IFormatProvider formatProvider) {
			return $"({X}, {Y})";
		}
	}

	public class Solver : SolverBase
	{
		readonly int MOD = (int)Math.Pow(10, 9) + 7;
		public void Run() {
			var ary = ReadIntArray();
			var H = ary[0];
			var W = ary[1];
			var N = ary[2];

			var Walls = new HashSet<XY>();
			for (int i = 0; i < N; i++) {
				Walls.Add(new XY(ReadIntArray()));
			}
			Dump(Walls);

			// 左と上に1行/列ずつ追加しておく（ループ内でのIndexチェックを不要にするため）
			//int[,] DP = new int[H + 1, W + 1];
			int[] Row0 = new int[W + 1];
			Row0[1] = 1;

			// 貰うDP
			for (int i = 1; i < H + 1; i++) {
				int[] Row1 = new int[W + 1];
				for (int j = 1; j < W + 1; j++) {
					if (Walls.Contains(new XY(i, j))) continue;

					Row1[j] = (Row0[j] + Row1[j - 1]) % MOD;
				}
				Row0 = Row1;
				Dump(Row0);
			}

			WriteLine(Row0[W]);
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

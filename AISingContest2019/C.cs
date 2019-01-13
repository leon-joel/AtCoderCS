using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// AISingContest2019
// https://atcoder.jp/contests/aising2019
namespace AISingContest2019.C
{
	class Node
	{
		public int Parent;
		public int Rank;

		public Node(int id) {
			Parent = id;
			Rank = 0;
		}
	}

	public class UnionFindTree
	{
		readonly Node[] Nodes;

		// 0 から n-1 まで（計 n個）のNodeを作る
		public UnionFindTree(int n) {
			Nodes = new Node[n];
			for (int i = 0; i < n; i++) {
				Nodes[i] = new Node(i);
			}
		}

		// x要素のルート親を返す
		public int Find(int x) {
			if (Nodes[x].Parent == x)
				return x;

			return Nodes[x].Parent = Find(Nodes[x].Parent);
		}

		// a, b を同一グループ化する
		// 既に同じグループだったらfalseを返す
		public bool Unite(int a, int b) {
			var ra = Find(a);
			var rb = Find(b);

			if (ra == rb)
				return false;

			if (Nodes[ra].Rank < Nodes[rb].Rank) {
				Nodes[ra].Parent = rb;
			} else {
				Nodes[rb].Parent = ra;
				if (Nodes[ra].Rank == Nodes[rb].Rank)
					Nodes[ra].Rank += 1;
			}
			return true;
		}

		// a, b が同一グループ？
		public bool IsSame(int a, int b) {
			return Find(a) == Find(b);
		}
	}
	public class XY : IComparable<XY>, IFormattable
	{
		public int X;
		public int Y;

		public XY() { }
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
		public void Run() {
			var ary = ReadIntArray();
			var H = ary[0];
			var W = ary[1];

			char[][] Grid = new char[H][];
			for (int i = 0; i < H; i++) {
				Grid[i] = ReadLine().ToCharArray();
			}

			// 隣接チェック
			var tree = new UnionFindTree(H * W + 1);

			for (int i = 0; i < H; i++) {
				for (int j = 0; j < W; j++) {
					if (j < W - 1) {
						if (Grid[i][j] != Grid[i][j + 1])
							tree.Unite(i * W + j, i * W + j + 1);
					}
					if (i < H - 1) {
						if (Grid[i][j] != Grid[i + 1][j])
							tree.Unite(i * W + j, (i + 1) * W + j);
					}
				}
			}

			// root親番号ごとにメンバーの白黒数をカウント
			Dictionary<int, XY> roots = new Dictionary<int, XY>();
			for (int i = 0; i < H; i++) {
				for (int j = 0; j < W; j++) {
					// # -> X
					// . -> Y とする
					var root = tree.Find(i * W + j);
					XY xy;
					if (!roots.TryGetValue(root, out xy))
						xy = new XY();

					if (Grid[i][j] == '#')
						xy.X += 1;
					else
						xy.Y += 1;

					roots[root] = xy;
				}
			}

			// グループごとの 黒数 * 白数 を積算していく
			long ans = 0;
			foreach (KeyValuePair<int, XY> root in roots) {
				if (1 <= root.Value.X && 1 <= root.Value.Y) {
					ans += (long)root.Value.X * (long)root.Value.Y;
				}
			}
			WriteLine(ans);
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}


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
}

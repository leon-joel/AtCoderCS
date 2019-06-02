using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AOJ_GRL_2_A.D
{
	using static Util;

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

	public class Edge : IComparable<Edge>, IFormattable
	{
		public int V0;
		public int V1;
		public int Cost;

		/// <summary>
		/// aryには [頂点0, 頂点1, Cost]
		/// </summary>
		public Edge(int[] ary) {
			V0 = ary[0];
			V1 = ary[1];
			Cost = ary[2];
		}
		public Edge(int v0, int v1, int cost) {
			V0 = v0;
			V1 = v1;
			Cost = cost;
		}

		public int CompareTo(Edge other) {
			return Cost.CompareTo(other.Cost);
		}

		public override string ToString() {
			return ToString(null, null);
		}
		public string ToString(string format, IFormatProvider formatProvider) {
			return $"({V0}-{V1}: {Cost})";
		}
	}

	// ★ UnionFindTree が必要

	/// <summary>
	/// 無向グラフにおける最小全域木アルゴリズム
	/// ★ UnionFindTree が必要
	/// </summary>
	public class MST
	{
		// 頂点数
		readonly public int NumOfV;

		// すべての辺
		readonly public List<Edge> Edges;

		/// <summary>
		/// コンストラクタ: 頂点数を与える
		/// </summary>
		public MST(int numOfV) {
			NumOfV = numOfV;
			Edges = new List<Edge>();
		}
		/// <summary>
		/// コンストラクタ: 頂点数とエッジ数を与える ※エッジ数は ListへのAddを効率よく行うため
		/// </summary>
		public MST(int numOfV, int numOfE) {
			NumOfV = numOfV;
			Edges = new List<Edge>(numOfE);
		}
		/// <summary>
		/// コンストラクタ: エッジ数をエッジリストを与える
		/// ※edgesはCalcの最初にCostでソートするので事前ソートは不要
		/// </summary>
		public MST(int numOfV, List<Edge> edges) {
			NumOfV = numOfV;
			Edges = edges;
		}

		/// <summary>
		/// aryには [頂点0, 頂点1, Cost]
		/// </summary>
		public void Add(int[] ary) {
			Edges.Add(new Edge(ary));
		}
		public void Add(int v0, int v1, int cost) {
			Edges.Add(new Edge(v0, v1, cost));
		}

		/// <summary>
		/// クラスカル法
		/// </summary>
		/// <returns>最小全域木のコスト合計</returns>
		public long CalcMinCostByKruskal() {
			Edges.Sort();

			var uft = new UnionFindTree(NumOfV);

			long minCost = 0;

			foreach (var e in Edges) {
				if (!uft.IsSame(e.V0, e.V1)) {
					// 別グループ、つまりこの辺を採用しても閉路ができない。よって採用！
					minCost += e.Cost;
					uft.Unite(e.V0, e.V1);
				}
			}

			return minCost;
		}
	}

	public class XY : IComparable<XY>, IFormattable
	{
		// XYを村番号付きに改造した
		public readonly int Id;
		public readonly int X;
		public readonly int Y;

		public XY(int id) { Id = id; }
		public XY(int x, int y) {
			X = x;
			Y = y;
		}
		public XY(int id, int x, int y) {
			Id = id;
			X = x;
			Y = y;
		}
		public XY(int id, int[] ary) {
			Id = id;
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
			return $"[{Id}]({X}, {Y})";
		}

		public int Dist(XY other) {
			return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
		}

		public IEnumerable<XY> Neighbors() {
			yield return new XY(X - 1, Y);  // 上
			yield return new XY(X, Y + 1);  // 右
			yield return new XY(X + 1, Y);  // 下
			yield return new XY(X, Y - 1);  // 左
		}
	}


	public class Solver : SolverBase
	{
		public void Run() {
			var ary = ReadIntArray();
			var V = ary[0];
			var E = ary[1];

			var mst = new MST(V, E);
			for (int i = 0; i < E; i++) {
				mst.Add(ReadIntArray());
			}
			Dump(mst.Edges);

			// 最小全域木の全長を計算
			var ans = mst.CalcMinCostByKruskal();
			WriteLine(ans);
		}

#if !MYHOME
		static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	public static class Util
	{
		public readonly static long MOD = 1000000007;

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
		public static void InitDP2<T>(T[,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					dp[i, j] = value;
				}
			}
		}
		public static void InitDP3<T>(T[,,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					for (int k = 0; k < dp.GetLength(2); k++) {
						dp[i, j, k] = value;
					}
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
		public static T Min<T>(params T[] nums) where T : IComparable {
			if (nums.Length == 0) return default(T);

			T min = nums[0];
			for (int i = 1; i < nums.Length; i++) {
				min = min.CompareTo(nums[i]) < 0 ? min : nums[i];
			}
			return min;
		}

		/// <summary>
		/// ソート済み配列 ary に同じ値の要素が含まれている？
		/// ※ソート順は昇順/降順どちらでもよい
		/// </summary>
		public static bool HasDuplicateInSortedArray<T>(T[] ary) where T : IComparable, IComparable<T> {
			if (ary.Length <= 1) return false;

			var lastNum = ary[ary.Length - 1];

			foreach (var n in ary) {
				if (lastNum.CompareTo(n) == 0) {
					return true;
				} else {
					lastNum = n;
				}
			}
			return false;
		}

		public static void ReplaceIfBigger<T>(ref T r, T v) where T : IComparable {
			if (r.CompareTo(v) < 0) r = v;
		}
		public static void ReplaceIfSmaller<T>(ref T r, T v) where T : IComparable {
			if (0 < r.CompareTo(v)) r = v;
		}

		/// <summary>
		/// 二分探索
		/// ※条件を満たす最小のidxを返す
		/// ※満たすものがない場合は ary.Length を返す
		/// ※『aryの先頭側が条件を満たさない、末尾側が条件を満たす』という前提
		/// ただし、IsOK(...)の戻り値を逆転させれば、逆でも同じことが可能。
		/// </summary>
		/// <param name="ary">探索対象配列 ★ソート済みであること</param>
		/// <param name="key">探索値 ※これ以上の値を持つ（IsOKがtrueを返す）最小のindexを返す</param>
		public static int BinarySearch<T>(T[] ary, T key) where T : IComparable, IComparable<T> {
			int left = -1;
			int right = ary.Length;

			while (1 < right - left) {
				var mid = left + (right - left) / 2;

				if (IsOK(ary, mid, key)) {
					right = mid;
				} else {
					left = mid;
				}
			}

			// left は条件を満たさない最大の値、right は条件を満たす最小の値になっている
			return right;
		}
		public static bool IsOK<T>(T[] ary, int idx, T key) where T : IComparable, IComparable<T> {
			// key <= ary[idx] と同じ意味
			return key.CompareTo(ary[idx]) <= 0;
		}
	}

	public class SolverBase
	{
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		//virtual protected void ReadInt2(out int x, out int y) {
		//	var aryS = ReadLine().Split(' ');
		//	x = int.Parse(aryS[0]);
		//	y = int.Parse(aryS[1]);
		//}
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
		protected void Dump(int x) => Console.WriteLine(x);
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

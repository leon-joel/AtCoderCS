using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Paiza.S024_Sea
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
			a = Find(a);
			b = Find(b);

			if (a == b)
				return false;

			if (Nodes[a].Rank < Nodes[b].Rank) {
				Nodes[a].Parent = b;
			} else {
				Nodes[b].Parent = a;
				if (Nodes[a].Rank == Nodes[b].Rank)
					Nodes[a].Rank += 1;
			}
			return true;
		}

		// a, b が同一グループ？
		public bool IsSame(int a, int b) {
			return Find(a) == Find(b);
		}
	}

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

		int H;
		int W;
		int N;
		int[,] Grid;
		int[,] GridT;

		// LandID(1〜)の連結処理に使用する
		UnionFindTree Tree;

		public void Run() {
			var ary = ReadIntArray();
			H = ary[0];
			W = ary[1];
			N = ary[2];
			//Console.WriteLine($"{H}, {W}, {N}");

			Tree = new UnionFindTree(H * W + 1);
			Grid = new int[H + 2, W + 2];
			var g1 = new List<int>(H * W);
			for (int i = 1; i < H + 1; i++) {
				var row = ReadIntArray();
				// 陸地を配列化
				g1.AddRange(row);

				for (int j = 1; j < W + 1; j++) {
					Grid[i, j] = row[j - 1];
				}
			}
			//DumpGrid(Grid);

			// 配列化陸地をソート
			var heightArray = g1.Distinct().ToArray();
			Array.Sort(heightArray);

			// ソート結果をもとに陸地データを圧縮（最大10億 ➟ 最大900に）
			//Console.WriteLine("unique------");
			//Dump(heightArray);

			// N==1 は即終了
			if (N == 1) {
				WriteLine(0);
				return;
			}

			// 海面の高さをMaxから下げていく
			int lowerAns = int.MaxValue;
			int landNum = 0;
			GridT = new int[H + 2, W + 2];
			for (int i = heightArray.Length - 1; 0 <= i; i--) {
				landNum += AddLands(heightArray[i]);

				// 島数Nになったら高さを更新
				if (landNum == N) {
					// 最低の海面高なるように海面の高さを調整する
					lowerAns = 0 < i ? heightArray[i - 1] : 0;
				}
			}
			WriteLine(lowerAns);
		}

		// 陸地追加処理
		// 島の増加数を返す
		int AddLands(int height) {
			int ret = 0;
			for (int i = 1; i < H + 1; i++) {
				for (int j = 1; j < W + 1; j++) {
					if (Grid[i, j] != height) continue;

					// 新たな陸地を発見
					ret += ConnectLand(i, j);
				}
			}
			return ret;
		}
		int[] DX = { 1, -1, 0, 0 };
		int[] DY = { 0, 0, 1, -1 };
		int landId = 1;
		int ConnectLand(int x, int y) {
			GridT[x, y] = landId++;
			int ret = 1;

			for (int i = 0; i < 4; i++) {
				int px = x + DX[i];
				int py = y + DY[i];
				if (0 < GridT[px, py]) {
					if (Tree.Unite(GridT[px, py], GridT[x, y]))
						--ret;
				}
			}
			return ret;
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

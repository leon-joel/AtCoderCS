using System;
using System.Collections.Generic;

namespace AtCoderCS_Tools
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
}

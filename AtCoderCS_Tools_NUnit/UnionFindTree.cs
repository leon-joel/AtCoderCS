using System;
using System.Collections.Generic;

namespace Tools
{
	class Node
	{
		public int Parent;
		//自分も含めたグループ全体のサイズ
		//★ただし、自分がrootの場合にのみ正しい値が入っている
		public int Size;

		public Node(int id) {
			Parent = id;
			Size = 1;
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

			if (Nodes[ra].Size < Nodes[rb].Size) {
				int tmp = ra;
				ra = rb;
				rb = tmp;
			}
			// b を a にぶら下げてサイズを合計する
			Nodes[rb].Parent = ra;
			Nodes[ra].Size += Nodes[rb].Size;
			return true;
		}

		// a, b が同一グループ？
		public bool IsSame(int a, int b) {
			return Find(a) == Find(b);
		}

		// aの所属グループのサイズ(要素数) ※aを含む
		public int Size(int a) {
			return Nodes[Find(a)].Size;
		}
	}
}

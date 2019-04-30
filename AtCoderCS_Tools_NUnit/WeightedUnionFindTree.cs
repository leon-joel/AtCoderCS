using System;
using System.Collections.Generic;

namespace Tools
{
	class NodeWithWeight
	{
		public int Parent;
		// 木の高さ
		public int Rank;
		// 自分の親との重さの差分
		public int DiffWeight;

		public NodeWithWeight(int id) {
			Parent = id;
			Rank = 0;
			DiffWeight = 0;
		}
	}

	/// <summary>
	/// 重み付きUnionFindTree
	/// https://qiita.com/drken/items/cce6fc5c579051e64fab#%E9%87%8D%E3%81%BF%E3%81%A4%E3%81%8D-unionfind-%E6%9C%A8%E3%81%AE%E5%87%A6%E7%90%86
	/// http://at274.hatenablog.com/entry/2018/02/03/140504
	/// </summary>
	public class WeightedUnionFindTree
	{
		readonly NodeWithWeight[] Nodes;

		/// <summary>
		/// 0 から n-1 まで（計 n個）のNodeを作る
		/// </summary>
		public WeightedUnionFindTree(int n) {
			Nodes = new NodeWithWeight[n];
			for (int i = 0; i < n; i++) {
				Nodes[i] = new NodeWithWeight(i);
			}
		}

		/// <summary>
		/// x要素のルート親を返す
		/// ※同時に【経路圧縮】を行い、xのparentをroot親にする。
		///  つまり、rootに直接ぶら下がる形にする。
		/// </summary>
		public int Root(int x) {
			if (Nodes[x].Parent == x)
				return x;

			// 再帰的にrootまでたどり、
			// 経路上のNodeをすべてroot直下に配置していく【経路圧縮】
			// ★この際、Rankを更新していないようだが、それはOKなんだろうか？？？
			var root = Root(Nodes[x].Parent);
			// 自分の親のDiffWeightを自分に加算
			// ※自分の親は上のRoot処理でrootの直下に配置されているので、
			//  そのDiffWeightは親からの差分重みの合計になっている.
			//  自分もRoot直下に配置するので、そのDiffWeightを加算しなければならない。
			Nodes[x].DiffWeight += Nodes[Nodes[x].Parent].DiffWeight;
			Nodes[x].Parent = root;
			return root;
		}

		/// <summary>
		/// MergeのAlias
		/// </summary>
		public bool Unite(int a, int b, int weight) {
			var ra = Root(a);
			var rb = Root(b);

			if (ra == rb)
				return false;

			// なるべく木の高さが高くならないようにする
			if (Nodes[ra].Rank < Nodes[rb].Rank) {
				// bのrootにaのrootをつなぐので、aのrootのDiffWeightをUpdate
				Nodes[ra].Parent = rb;
				Nodes[ra].DiffWeight = weight - Nodes[a].DiffWeight + Nodes[b].DiffWeight;
			} else {
				Nodes[rb].Parent = ra;
				Nodes[rb].DiffWeight = -weight - Nodes[b].DiffWeight + Nodes[a].DiffWeight;
				// 高さが同じもの同士をつなげるときは、
				// どちらかの高さが高くなってしまうので、この場合は +1 しておく。
				if (Nodes[ra].Rank == Nodes[rb].Rank)
					Nodes[ra].Rank += 1;
			}
			return true;
		}
		/// <summary>
		/// a, b を同一グループ化する。
		/// weigth数直線---> 上の a から見たときに b がどこにいるか weight で与える。
		/// 既に同じグループだったらfalseを返す
		/// </summary>
		/// <returns>既に同じグループだったらfalseを返す</returns>
		public bool Merge(int a, int b, int weight) => Unite(a, b, weight);

		/// <summary>
		/// a, b が同一グループ？
		/// </summary>
		public bool IsSame(int a, int b) {
			return Root(a) == Root(b);
		}

		/// <summary>
		/// weigth数直線---> 上の a から見たときに b がどこにいるかを返す
		/// ※おそらく、これを呼び出す前に IsSame(a, b)もしくはMerge(a, b)が呼び出されていることが前提となっている。
		///   もしそうじゃない場合は正しい距離が求められないはず。
		/// </summary>
		public int Diff(int a, int b) {
			// ※オリジナルのソースでは b - a になっていたが、それだと
			//    Unite(a, b, w) に対して、
			//    Diff (a, b) すると -w が返ってきて
			//  違和感があるので、逆転させるようにした。
			return Nodes[a].DiffWeight - Nodes[b].DiffWeight;
		}
	}
}

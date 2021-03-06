﻿using System;
using System.Collections.Generic;

namespace Tools
{
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
}

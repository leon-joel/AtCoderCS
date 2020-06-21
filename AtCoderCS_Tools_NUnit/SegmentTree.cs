using System;
namespace Tools
{
	/// <summary>
	/// 区間最小値をlogNで更新・取得できる
	/// ※関数 Compare() を変更すれば最小値以外にも応用可能
	/// ※区間和であれば BIT の方が効率がいい ※logNだけど定数が速い
	/// </summary>
	class SegmentTree
	{
		public static int INF = int.MaxValue;

		// 元の配列を包含する2べき数
		public int N { get; set; }
		// セグメント木のデータ(2^n-1)
		public int[] Node { get; set; }

		// 元データの配列を引数で渡す
		public SegmentTree(int[] a) {
			// 要素数を2のべき乗とする
			N = 1;
			while (N < a.Length) N *= 2;

			// セグメント木の管理する要素数を 2^n-1 で初期化する
			this.Node = new int[2 * N - 1];
			for (int i = 0; i < this.Node.Length; i++) this.Node[i] = INF;

			// 各ノードの値を初期化する
			// 葉は元の配列のデータ
			for (int i = 0; i < a.Length; i++) this.Node[N + i - 1] = a[i];
			// 子ノードを比較して親ノードの値を決めていく
			for (int i = N - 2; i >= 0; i--) {
				this.Node[i] = this.Compare(this.Node[2 * i + 1], this.Node[2 * i + 2]);
			}
		}

		///<summary>i番目のデータをvに更新する</summary>
		public void Update(int i, int v) {
			// 元の配列のデータが格納される葉のデータを更新する
			i += (this.N - 1);
			this.Node[i] = v;

			// 更新したノードの親をさかのぼってルートまで更新する
			while (i > 0) {
				// 親ノードのインデックス
				i = (i - 1) / 2;
				this.Node[i] = this.Compare(this.Node[2 * i + 1], this.Node[2 * i + 2]);
			}
		}
		///<summary>[a, b) の区間最小値を求める</summary>
		public int GetMin(int a, int b) {
			return this.GetMin(a, b, 0, 0, this.N);
		}
		// [a, b) の区間最小値を求める
		// 現在のノードはk番目、[l, r)
		private int GetMin(int a, int b, int k, int l, int r) {
			// 1. 現在のノードの対応区間が、求める区間に含まれない場合
			//    対象外の区間のデータなので、最小値に関係のない値(INF)を返す
			if (r <= a || b <= l) return INF;

			// 2. 現在のノードの対応区間が、求める区間に完全に含まれる場合
			//    現在のノードの値が最小値の候補になるのでそのまま返す
			if (a <= l && r <= b) return this.Node[k];

			// 3. 上記以外の場合、現在のノードの対応区間が、求める区間を一部を含む場合
			//    子ノードを評価し、比較した結果を返す
			var vl = this.GetMin(a, b, 2 * k + 1, l, (l + r) / 2);
			var vr = this.GetMin(a, b, 2 * k + 2, (l + r) / 2, r);
			return this.Compare(vl, vr);
		}

		// 2要素を比較して小さい値を返す
		private int Compare(int a, int b) { return Math.Min(a, b); }
	}
}

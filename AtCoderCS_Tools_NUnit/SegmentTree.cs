using System;
namespace Tools
{
	//
	// https://atcoder.jp/contests/abc170/submissions/14320054
	//

	/// <summary>
	/// SegmentTree
	/// BITは和しか取得できないし、値を加算することしかできないが、
	/// SegmentTreeはmin/maxなど operation で表現できるものなら
	/// 何でも(?)取得できる上に、値の置き換えが可能。
	/// 
	/// ■使い方
	/// <code>
	/// // 要素数, 比較演算, 初期値 を与えられる
	/// var st = new SegmentTree<int>(200000, Math.Min, int.MaxValue);
	/// // 更新
	/// st[0] = 100;
	/// st[1] = 120;
	/// st[0] = int.MaxValue;
	/// // 範囲内の最小値(比較演算子による)を取得
	/// var minV = st.Query(0, 200000);
	/// </code>
	/// </summary>
	public class SegmentTree<T>
	{
		// 制約に合った2の冪
		private readonly int N;
		private T[] _array;

		// コンストラクタで与えられた値 ※引数なしQueryを実装するために追加
		private int _size;

		private T _identity;
		private Func<T, T, T> _operation;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="size">Treeの幅（内部Arrayの要素数ではない）</param>
		/// <param name="operation">Query/Update時の操作関数</param>
		/// <param name="identity">初期値</param>
		public SegmentTree(int size, Func<T, T, T> operation, T identity) {
			_size = size;
			N = 1;
			while (N < size) {
				N *= 2;
			}

			_identity = identity;
			_operation = operation;
			_array = new T[N * 2];
			for (int i = 1; i < N * 2; i++) {
				_array[i] = _identity;
			}
		}

		/// <summary>
		/// A[i]をnに更新 O(log N)
		/// </summary>
		public void Update(int i, T n) {
			i += N;
			_array[i] = n;
			while (i > 1) {
				i /= 2;
				_array[i] = _operation(_array[i * 2], _array[i * 2 + 1]);
			}
		}

		/// <summary>
		/// 全範囲（[0, size)）から検索
		/// </summary>
		public T Query() => Query(0, _size);

		/// <summary>
		/// [left, right) から検索。
		/// A[left] op A[left+1] ... op A[right-1]を求める
		/// </summary>
		public T Query(int left, int right) => Query(left, right, 1, 0, N);

		private T Query(int left, int right, int k, int l, int r) {
			if (r <= left || right <= l) {
				return _identity;
			}

			if (left <= l && r <= right) {
				return _array[k];
			}

			return _operation(Query(left, right, k * 2, l, (l + r) / 2),
				Query(left, right, k * 2 + 1, (l + r) / 2, r));
		}

		public T this[int i] {
			set { Update(i, value); }
			get { return _array[i + N]; }
		}
	}

	// 自分で作った SegmentTree
	// ※拾いものの方が使い勝手がいいのでお蔵入りに
	namespace SegmentTree_Mine
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
}

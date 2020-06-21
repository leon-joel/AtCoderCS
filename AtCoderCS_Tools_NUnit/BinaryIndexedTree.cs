using System;
namespace Tools
{
	/// <summary>
	/// Binary Indexed Tree(BIT) [1-indexed] (別名:Fenwick Tree)
	/// ＜用途＞
	/// ・区間の和の取得 ※更新は加算のみ（代入・減算は不可）
	/// ・LIS
	/// ＜解説放送＞
	/// ABC136-F
	/// https://www.youtube.com/watch?v=lyHk98daDJo&feature=youtu.be&t=7915
	/// </summary>
	public class BinaryIndexedTree
	{
		public int MaxNum { get; private set; }

		int[] bit;

		// maxNum: 要素数 ※2べき(2のN乗)じゃなくてもよい
		public BinaryIndexedTree(int maxNum) {
			MaxNum = maxNum;
			bit = new int[maxNum + 1];
		}

		/// <summary>
		/// i の個数を v 増加
		/// ※i は1以上
		/// </summary>
		public void Add(int i, int v) {
			for (int x = i; x <= MaxNum; x += x & -x)
				bit[x] += v;
		}

		/// <summary>
		/// i 以下の総数を返す
		/// ※0以下のi を与えた場合は0を返す
		/// </summary>
		public int Sum(int i) {
			int ret = 0;
			for (int x = i; 0 < x; x -= x & -x)
				ret += bit[x];
			return ret;
		}

		/// <summary>
		/// [lower, upper] つまり lower以上 upper以下 の総数を返す 
		/// </summary>
		public int RangeSum(int lower, int upper) {
			return Sum(upper) - Sum(lower-1);
		}

		/// <summary>
		/// i の個数を返す
		/// </summary>
		public int Count(int i) {
			return Sum(i) - Sum(i - 1);
		}
	}
}

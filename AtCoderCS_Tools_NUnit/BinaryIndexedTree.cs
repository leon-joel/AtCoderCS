using System;
namespace Tools
{
	public class BinaryIndexedTree
	{
		public int MaxNum { get; private set; }

		int[] bit;

		public BinaryIndexedTree(int maxNum) {
			MaxNum = maxNum;
			bit = new int[maxNum + 1];
		}

		// i の個数を x 増加
		// ※i は1以上
		public void Add(int i, int v) {
			for (int x = i; x <= MaxNum; x += x & -x)
				bit[x] += v;
		}

		// i 以下の総数を返す
		// ※0以下のi を与えた場合は0を返す
		public int Sum(int i) {
			int ret = 0;
			for (int x = i; 0 < x; x -= x & -x)
				ret += bit[x];
			return ret;
		}

		// [lower, upper] つまり lower以上 upper以下 の総数を返す
		public int RangeSum(int lower, int upper) {
			return Sum(upper) - Sum(lower-1);
		}

		// i の個数を返す
		public int Count(int i) {
			return Sum(i) - Sum(i - 1);
		}
	}
}

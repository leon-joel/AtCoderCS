using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools
{
	[TestFixture()]
	public class BinarySearchTest
	{
		[Test]
		public void Operation() {
			int[] ary = new int[10];
			for (int i = 0; i < 10; i++) {
				ary[i] = i;
			}

			// 第2引数で与えた数字よりも大きい値が格納されている最小のindexを返す
			Assert.AreEqual(0, Util.BinarySearch(ary, -1));
			Assert.AreEqual(0, Util.BinarySearch(ary, 0));
			Assert.AreEqual(5, Util.BinarySearch(ary, 5));
			Assert.AreEqual(9, Util.BinarySearch(ary, 9));

			// 該当なしの場合（全部falseの場合）は ary.Length を返す
			Assert.AreEqual(10, Util.BinarySearch(ary, 10));
			Assert.AreEqual(10, Util.BinarySearch(ary, 100));

			// 範囲付きBinarySearch
			// 0 1 2 3 4 5 6 7 8 9
			// [3, 4] からの探索の場合、l=2, r=5 を与える
			// すべてが条件を満たす場合は l+1 が返される
			Assert.AreEqual(3, Util.BinarySearch(ary, 2, 5, 1));
			Assert.AreEqual(3, Util.BinarySearch(ary, 2, 5, 2));
			Assert.AreEqual(3, Util.BinarySearch(ary, 2, 5, 3));

			Assert.AreEqual(4, Util.BinarySearch(ary, 2, 5, 4));
			// 条件を満たすものがない場合は r が返される
			Assert.AreEqual(5, Util.BinarySearch(ary, 2, 5, 5));
			Assert.AreEqual(5, Util.BinarySearch(ary, 2, 5, 6));
		}
	}
}

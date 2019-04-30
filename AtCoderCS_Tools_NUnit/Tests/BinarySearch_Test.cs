using NUnit.Framework;
using System;
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
		}
	}
}

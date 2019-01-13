using NUnit.Framework;
using System;
namespace AtCoderCS_Tools
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
			Assert.AreEqual(0, Utils.BinarySearch(ary, -1));
			Assert.AreEqual(0, Utils.BinarySearch(ary, 0));
			Assert.AreEqual(5, Utils.BinarySearch(ary, 5));
			Assert.AreEqual(9, Utils.BinarySearch(ary, 9));

			// 該当なしの場合（全部falseの場合）は ary.Length を返す
			Assert.AreEqual(10, Utils.BinarySearch(ary, 10));
			Assert.AreEqual(10, Utils.BinarySearch(ary, 100));
		}
	}
}

using NUnit.Framework;
using System;
namespace AtCoderCS_Tools
{
	[TestFixture()]
	public class BinaryIndexedTreeTest
	{
		[Test()]
		public void Init() {
			var bit = new BinaryIndexedTree(10);

			Assert.AreEqual(10, bit.MaxNum);
			Assert.AreEqual(0, bit.Sum(0));
			Assert.AreEqual(0, bit.Sum(1));
			Assert.AreEqual(0, bit.Sum(10));
			//Assert.AreEqual(0, bit.Sum(11));

			Assert.AreEqual(0, bit.RangeSum(2, 4));

			Assert.AreEqual(0, bit.Count(2));
		}

		[Test]
		public void Operation() {
			var bit = new BinaryIndexedTree(10);
			Assert.AreEqual(0, bit.Sum(10));

			bit.Add(2, 1);
			Assert.AreEqual(0, bit.Sum(1));
			Assert.AreEqual(1, bit.Sum(2));
			bit.Add(5, 3);
			Assert.AreEqual(1, bit.Sum(4));
			Assert.AreEqual(4, bit.Sum(5));
			bit.Add(10, 10);
			Assert.AreEqual(4, bit.Sum(9));
			Assert.AreEqual(14, bit.Sum(10));
			bit.Add(9, -100);
			Assert.AreEqual(4, bit.Sum(8));
			Assert.AreEqual(-96, bit.Sum(9));
			Assert.AreEqual(-86, bit.Sum(10));

			Assert.AreEqual(-97, bit.RangeSum(3, 9));
			Assert.AreEqual(-87, bit.RangeSum(5, 10));

			Assert.AreEqual(3, bit.Count(5));
			Assert.AreEqual(0, bit.Count(6));
			Assert.AreEqual(-100, bit.Count(9));
			Assert.AreEqual(10, bit.Count(10));
		}
	}
}

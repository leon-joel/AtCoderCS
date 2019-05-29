using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tools
{
	using static Util;

	[TestFixture()]
	public class MSTTests
	{
		[Test]
		public void CalcMinCostByKruskalTest() {
			var mst = new MST(5);
			mst.Add(2, 4, 10000);
			mst.Add(0, 1, 10);
			mst.Add(0, 3, 5);
			mst.Add(1, 2, 1);
			mst.Add(1, 3, 1000);
			mst.Add(1, 4, 500);
			mst.Add(2, 3, 100);
			mst.Add(3, 4, 5000);

			var minCost = mst.CalcMinCostByKruskal();
			Assert.AreEqual(516, minCost);
		}
	}
}

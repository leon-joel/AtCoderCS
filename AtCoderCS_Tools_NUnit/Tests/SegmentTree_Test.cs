using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tools
{
	using static Util;

	[TestFixture()]
	public class SegmentTreeTests
	{
		[Test]
		public void Operations() {
			var ary = new int[5] { 5, 2, 3, 1, 4 };
			var st = new SegmentTree(ary);

			Assert.AreEqual(8, st.N);
			Assert.AreEqual(15, st.Node.Length);
			Assert.AreEqual(1, st.GetMin(0, 4));
			Assert.AreEqual(2, st.GetMin(1, 2));
			Assert.AreEqual(2, st.GetMin(1, 3));

			st.Update(1, 6);
			Assert.AreEqual(1, st.GetMin(0, 4));
			Assert.AreEqual(6, st.GetMin(1, 2));
			Assert.AreEqual(3, st.GetMin(1, 3));
		}
	}
}

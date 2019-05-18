using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Tools
{
	using static Util;

	[TestFixture]
	public class XYTests
	{
		[Test]
		public void DistTest() {
			var p0 = new XY(1, 1);
			var p1 = new XY(1, 2);

			Assert.AreEqual(1, p0.Dist(p1));
			Assert.AreEqual(1, p1.Dist(p0));
		}
	}
}
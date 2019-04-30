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
	public class ReplaceIBiggerSmallerTests
	{
		[Test]
		public void IntTest() {
			int i = int.MinValue;
			ReplaceIfBigger(ref i, int.MinValue);
			Assert.AreEqual(i, int.MinValue);
			ReplaceIfBigger(ref i, -10);
			Assert.AreEqual(i, -10);
			ReplaceIfBigger(ref i, -9);
			Assert.AreEqual(i, -9);
			ReplaceIfBigger(ref i, 10);
			Assert.AreEqual(i, 10);
			ReplaceIfBigger(ref i, 11);
			Assert.AreEqual(i, 11);
			ReplaceIfBigger(ref i, int.MaxValue);
			Assert.AreEqual(i, int.MaxValue);

			ReplaceIfSmaller(ref i, int.MaxValue);
			Assert.AreEqual(i, int.MaxValue);
			ReplaceIfSmaller(ref i, 12);
			Assert.AreEqual(i, 12);
			ReplaceIfSmaller(ref i, 0);
			Assert.AreEqual(i, 0);
			ReplaceIfSmaller(ref i, -10);
			Assert.AreEqual(i, -10);
			ReplaceIfSmaller(ref i, int.MinValue);
			Assert.AreEqual(i, int.MinValue);
		}

		[Test]
		public void ULongTest() {
			ulong u = ulong.MinValue;
			ReplaceIfBigger(ref u, ulong.MinValue);
			Assert.AreEqual(u, ulong.MinValue);
			ReplaceIfBigger(ref u, 1UL);
			Assert.AreEqual(u, 1UL);
			ReplaceIfBigger(ref u, ulong.MaxValue);
			Assert.AreEqual(u, ulong.MaxValue);
			ReplaceIfBigger(ref u, 1UL);
			Assert.AreEqual(u, ulong.MaxValue);

			ReplaceIfSmaller(ref u, ulong.MaxValue);
			Assert.AreEqual(u, ulong.MaxValue);
			ReplaceIfSmaller(ref u, 1UL);
			Assert.AreEqual(u, 1UL);
			ReplaceIfSmaller(ref u, 0UL);
			Assert.AreEqual(u, 0UL);
		}
	}
}
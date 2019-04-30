using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Tools
{
	using static Util;

	[TestFixture()]
	public class PrimeNumberTests
	{	
		[Test]
		public void FactoringTest() {
			long[] e10 = { 2, 5 };
			CollectionAssert.AreEqual(e10, PrimeNumber.Factoring(10));

			long[] e100000 = { 2, 2, 2, 2, 2, 5, 5, 5, 5, 5, };
			List<long> ans100000 = PrimeNumber.Factoring(100000).ToList();
			//Dump(ans100000);
			CollectionAssert.AreEqual(e100000, ans100000);

			long[] e99999 = { 3, 3, 41, 271, };
			List<long> ans99999 = PrimeNumber.Factoring(99999).ToList();
			//Dump(ans99999);
			CollectionAssert.AreEqual(e99999, ans99999);

			CollectionAssert.AreEqual(new long[] { 271 }, PrimeNumber.Factoring(271));

			long[] eNone = { };
			CollectionAssert.AreEqual(eNone, PrimeNumber.Factoring(1));
			CollectionAssert.AreEqual(eNone, PrimeNumber.Factoring(0));
		}
	}
}
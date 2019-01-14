using NUnit.Framework;
using System;
namespace AtCoderCS_Tools
{
	using static Utils;

	[TestFixture()]
	public class GcdLcmTests
	{
		[Test]
		public void GcdTest() {
			Assert.AreEqual(3, Gcd(6, 15));
			Assert.AreEqual(2, Gcd(2, 4));
			Assert.AreEqual(2, Gcd(2, 2));
			Assert.AreEqual(1, Gcd(7, 13));

			// 異常系
			Assert.AreEqual(3, Gcd(0, 3));
			Assert.AreEqual(3, Gcd(3, 0));
			Assert.AreEqual(0, Gcd(0, 0));
		}

		[Test]
		public void LcmTest() {
			Assert.AreEqual(30, Lcm(6, 15));
			Assert.AreEqual(4, Lcm(2, 4));
			Assert.AreEqual(2, Lcm(2, 2));
			Assert.AreEqual(91, Lcm(7, 13));

			// 異常系
			Assert.AreEqual(0, Lcm(0, 3));
			Assert.AreEqual(0, Lcm(3, 0));
			Assert.Throws<DivideByZeroException>(() => Lcm(0, 0));
		}
	}
}

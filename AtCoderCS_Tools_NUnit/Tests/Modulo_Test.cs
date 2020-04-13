using NUnit.Framework;
using System;
namespace Tools
{
	using static Modulo;

	[TestFixture()]
	public class ModuloTests
	{
		[Test]
		public void AddTest() {
			Assert.AreEqual(0, Add(0, 0));
			Assert.AreEqual(1000000006, Add(1000000000, 6));
			Assert.AreEqual(0, Add(1000000000, 7));
			Assert.AreEqual(1, Add(8, 1000000000));
		}
		[Test]
		public void MulTest() {
			Assert.AreEqual(6, Mul(2, 3));
			Assert.AreEqual(1000000006, Mul(1000000006, 1));
			Assert.AreEqual(0, Mul(1000000007, 1));
			Assert.AreEqual(1, Mul(1000000008, 1));
		}

		[Test]
		public void PowTest() {
			Assert.AreEqual(1, Pow(2, 0));
			Assert.AreEqual(2, Pow(2, 1));
			Assert.AreEqual(8, Pow(2, 3));
			Assert.AreEqual(1000000000, Pow(10, 9));

			Assert.AreEqual(1000000006, Pow(1000000006, 1));
			Assert.AreEqual(1, Pow(1000000006, 2));
			Assert.AreEqual(1000000006, Pow(1000000006, 3));

			Assert.AreEqual(0, Pow(1000000007, 1));
			Assert.AreEqual(0, Pow(1000000007, 2));

			Assert.AreEqual(1, Pow(1000000008, 1));
			Assert.AreEqual(1, Pow(1000000008, 2));
			Assert.AreEqual(1, Pow(1000000008, 3));
		}

		[Test]
		public void DivTest() {
			// 以下のような結果になるがこの意味・妥当性についてはよくわかってない
			Assert.AreEqual(6, Div(6, 1));
			Assert.AreEqual(3, Div(6, 2));
			Assert.AreEqual(500000005, Div(6, 4));
			Assert.AreEqual(1, Div(6, 6));
			Assert.AreEqual(857142864, Div(6, 7));

			Assert.AreEqual(0, Div(0, 6));

			//Assert.AreEqual(0, Div(6, 0));
		}

		[Test]
		public void Modulo_Factorial_Test() {
			var modulo = new Modulo(10);
			Assert.AreEqual(1, modulo.Fac(1));
			Assert.AreEqual(2, modulo.Fac(2));
			Assert.AreEqual(6, modulo.Fac(3));
			Assert.AreEqual(24, modulo.Fac(4));
			Assert.AreEqual(120, modulo.Fac(5));
			Assert.AreEqual(720, modulo.Fac(6));
			Assert.AreEqual(5040, modulo.Fac(7));
			Assert.AreEqual(40320, modulo.Fac(8));
			Assert.AreEqual(362880, modulo.Fac(9));
			Assert.AreEqual(3628800, modulo.Fac(10));

			Assert.Throws<IndexOutOfRangeException>(() => modulo.Fac(11));

			modulo = new Modulo(100);
			Assert.AreEqual(437918130, modulo.Fac(100));
		}

		[Test]
		public void Modulo_nCr_Test() {
			var modulo = new Modulo(10);
			Assert.AreEqual(1, modulo.Ncr(3, 3));
			Assert.AreEqual(3, modulo.Ncr(3, 2));
			Assert.AreEqual(3, modulo.Ncr(3, 1));
			Assert.AreEqual(1, modulo.Ncr(3, 0));

			Assert.AreEqual(1, modulo.Ncr(10, 0));
			Assert.AreEqual(10, modulo.Ncr(10, 1));
			Assert.AreEqual(45, modulo.Ncr(10, 2));
			Assert.AreEqual(45, modulo.Ncr(10, 8));
			Assert.AreEqual(10, modulo.Ncr(10, 9));
			Assert.AreEqual(1, modulo.Ncr(10, 10));

			Assert.Throws<IndexOutOfRangeException>(() => modulo.Ncr(11, 1));

			modulo = new Modulo(100);
			Assert.AreEqual(1, modulo.Ncr(100, 0));
			Assert.AreEqual(100, modulo.Ncr(100, 1));
			Assert.AreEqual(320509008, modulo.Ncr(100, 47));
			Assert.AreEqual(353895363, modulo.Ncr(100, 48));
			Assert.AreEqual(273521609, modulo.Ncr(100, 49));
			Assert.AreEqual(538992043, modulo.Ncr(100, 50));
			Assert.AreEqual(273521609, modulo.Ncr(100, 51));
			Assert.AreEqual(353895363, modulo.Ncr(100, 52));
			Assert.AreEqual(320509008, modulo.Ncr(100, 53));
			Assert.AreEqual(100, modulo.Ncr(100, 99));
			Assert.AreEqual(1, modulo.Ncr(100, 100));
		}
	}
}

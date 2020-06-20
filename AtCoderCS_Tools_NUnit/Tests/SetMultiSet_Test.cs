using NUnit.Framework;
using System;

using MySet;
using System.Diagnostics;

namespace Tools
{
	[TestFixture()]
	public class SetTest
	{
		[Test]
		public void NoData() {
			var set = new Set<int>();

			Assert.AreEqual(0, set.Count);
			Assert.AreEqual(0, set.CountByValue(7));
			Assert.IsFalse(set.Contains(999));
			Assert.Throws<IndexOutOfRangeException>(() => set.ElementAt(0));
			Assert.AreEqual(-1, set.LowerBound(3));
			Assert.AreEqual(-1, set.UpperBound(3));
			Assert.DoesNotThrow(() => set.Remove(10));
		}
		[Test]
		public void Operation() {
			var set = new Set<int>();
			set.Insert(10);
			set.Insert(2);
			set.Insert(7);

			Assert.AreEqual(3, set.Count);
			Assert.AreEqual(1, set.CountByValue(7));
			Assert.IsTrue(set.Contains(2));
			Assert.AreEqual(2, set.ElementAt(0));
			Assert.AreEqual(7, set.ElementAt(1));
			Assert.AreEqual(10, set.ElementAt(2));
			Assert.DoesNotThrow(() => set.Remove(999));

			// 前に挿入する場合のidxが返される
			Assert.AreEqual(0, set.LowerBound(0));
			Assert.AreEqual(0, set.LowerBound(2));
			Assert.AreEqual(1, set.LowerBound(3));
			Assert.AreEqual(1, set.LowerBound(7));
			Assert.AreEqual(2, set.LowerBound(8));
			Assert.AreEqual(2, set.LowerBound(10));
			Assert.AreEqual(3, set.LowerBound(11));

			// 後ろに挿入する場合のidxが返される
			Assert.AreEqual(0, set.UpperBound(0));
			Assert.AreEqual(1, set.UpperBound(2));
			Assert.AreEqual(1, set.UpperBound(3));
			Assert.AreEqual(2, set.UpperBound(7));
			Assert.AreEqual(3, set.UpperBound(10));
			Assert.AreEqual(3, set.UpperBound(11));

			// 重複では格納されない
			set.Insert(7);
			Assert.AreEqual(3, set.Count);
			Assert.AreEqual(1, set.CountByValue(7));

			set.Remove(7);
			Assert.AreEqual(0, set.CountByValue(7));

			set.Clear();
			Assert.AreEqual(0, set.Count);
		}
	}

	[TestFixture]
	public class MultiSetTest
	{
		[Test]
		public void NoData() {
			var set = new MultiSet<int>();

			Assert.AreEqual(0, set.Count);
			Assert.AreEqual(0, set.CountByValue(7));
			Assert.IsFalse(set.Contains(999));
			Assert.Throws<IndexOutOfRangeException>(() => set.ElementAt(0));
			Assert.AreEqual(-1, set.LowerBound(3));
			Assert.AreEqual(-1, set.UpperBound(3));
			Assert.DoesNotThrow(() => set.Remove(10));
		}

		[Test]
		public void Operation() {
			var set = new MultiSet<int>();
			set.Insert(10);
			set.Insert(2);
			set.Insert(7);

			Assert.AreEqual(3, set.Count);
			Assert.AreEqual(1, set.CountByValue(7));
			Assert.IsTrue(set.Contains(2));
			Assert.AreEqual(2, set.ElementAt(0));
			Assert.AreEqual(7, set.ElementAt(1));
			Assert.AreEqual(10, set.ElementAt(2));
			Assert.DoesNotThrow(() => set.Remove(999));

			// 前に挿入する場合のidxが返される
			Assert.AreEqual(0, set.LowerBound(0));
			Assert.AreEqual(0, set.LowerBound(2));
			Assert.AreEqual(1, set.LowerBound(3));
			Assert.AreEqual(1, set.LowerBound(7));
			Assert.AreEqual(2, set.LowerBound(8));
			Assert.AreEqual(2, set.LowerBound(10));
			Assert.AreEqual(3, set.LowerBound(11));

			// 後ろに挿入する場合のidxが返される
			Assert.AreEqual(0, set.UpperBound(0));
			Assert.AreEqual(1, set.UpperBound(2));
			Assert.AreEqual(1, set.UpperBound(3));
			Assert.AreEqual(2, set.UpperBound(7));
			Assert.AreEqual(3, set.UpperBound(10));
			Assert.AreEqual(3, set.UpperBound(11));

			// 重複格納できる
			set.Insert(7);
			Assert.AreEqual(4, set.Count);
			Assert.AreEqual(2, set.CountByValue(7));

			// 同値が複数ある場合でも1つしか削除されない ★C++のmultisetのerase()と異なる点
			set.Remove(7);
			Assert.AreEqual(3, set.Count);
			Assert.AreEqual(1, set.CountByValue(7));

			set.Insert(7);
			Assert.AreEqual(4, set.Count);
			Assert.AreEqual(2, set.CountByValue(7));
			// 2 7 7 10
			Assert.AreEqual(0, set.LowerBound(1));
			Assert.AreEqual(0, set.LowerBound(2));
			Assert.AreEqual(1, set.LowerBound(3));
			Assert.AreEqual(1, set.LowerBound(7));
			Assert.AreEqual(3, set.LowerBound(8));
			Assert.AreEqual(3, set.LowerBound(10));
			Assert.AreEqual(4, set.LowerBound(11));

			Assert.AreEqual(0, set.UpperBound(1));
			Assert.AreEqual(1, set.UpperBound(2));
			Assert.AreEqual(1, set.UpperBound(3));
			Assert.AreEqual(3, set.UpperBound(7));
			Assert.AreEqual(3, set.UpperBound(8));
			Assert.AreEqual(4, set.UpperBound(10));
			Assert.AreEqual(4, set.UpperBound(11));

			set.Clear();
			Assert.AreEqual(0, set.Count);
		}

		[Test]
		public void BigData() {
			var set = new MultiSet<int>();
			var _rnd = new Random();

			var sw = Stopwatch.StartNew();
			for (int i = 0; i < 10000; i++) {
				set.Insert(_rnd.Next());
			}
			var elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: {elapsed}");

			sw = Stopwatch.StartNew();
			var v = set.First();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: First: {elapsed}");
			sw.Restart();
			v = set.Last();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}:  Last: {elapsed}");

			sw.Restart();
			for (int i = 0; i < 90000; i++) {
				set.Insert(_rnd.Next());
			}
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: {elapsed}");
			sw.Restart();
			v = set.First();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: First: {elapsed}");
			sw.Restart();
			v = set.Last();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}:  Last: {elapsed}");

			sw.Restart();
			for (int i = 0; i < 900000; i++) {
				set.Insert(_rnd.Next());
			}
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: {elapsed}");
			sw.Restart();
			v = set.First();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: First: {elapsed}");
			sw.Restart();
			v = set.Last();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}:  Last: {elapsed}");

			sw.Restart();
			for (int i = 0; i < 9000000; i++) {
				set.Insert(_rnd.Next());
			}
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: {elapsed}");
			sw.Restart();
			v = set.First();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}: First: {elapsed}");
			sw.Restart();
			v = set.Last();
			elapsed = sw.Elapsed;
			Console.WriteLine($"{set.Count}:  Last: {elapsed}");
		}
	}
}

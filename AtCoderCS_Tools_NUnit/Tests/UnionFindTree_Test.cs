using NUnit.Framework;
using System;
namespace AtCoderCS_Tools
{
	[TestFixture()]
	public class UnionFindTreeTest
	{
		[Test()]
		public void Init() {
			var tree = new UnionFindTree(10);

			Assert.AreEqual(0, tree.Find(0));
		}

		[Test]
		public void Operation() {
			var tree = new UnionFindTree(10);

			Assert.AreEqual(0, tree.Find(0));
			Assert.AreEqual(1, tree.Find(1));
			Assert.IsTrue(tree.Unite(0, 1));
			Assert.AreEqual(0, tree.Find(0));
			Assert.AreEqual(0, tree.Find(1));
			Assert.IsTrue(tree.IsSame(0, 1));

			// 既に同一グループ
			Assert.IsFalse(tree.Unite(0, 1));

			// 別グループ
			Assert.IsFalse(tree.IsSame(1, 2));

			// 同一グループ化
			Assert.IsTrue(tree.Unite(1, 2));
			Assert.IsTrue(tree.IsSame(1, 2));
		}
	}
}

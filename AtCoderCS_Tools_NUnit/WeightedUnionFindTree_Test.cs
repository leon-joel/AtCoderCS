using AtCoderCS_Tools;
using NUnit.Framework;
using System;
namespace AtCoderCS_Tools_NUnit
{
	[TestFixture()]
	public class WeightedUnionFindTreeTest {
		[Test()]
		public void Init() {
			var tree = new WeightedUnionFindTree(10);

			Assert.AreEqual(0, tree.Root(0));
			Assert.IsFalse(tree.IsSame(0, 1));
		}

		[Test]
		public void Operation() {
			var tree = new WeightedUnionFindTree(10);

			Assert.IsTrue(tree.Unite(1, 2, 1));
			Assert.AreEqual(1, tree.Root(1));
			Assert.AreEqual(1, tree.Root(2));
			Assert.IsTrue(tree.IsSame(1, 2));

			Assert.AreEqual(1, tree.Diff(1, 2));
			Assert.AreEqual(-1, tree.Diff(2, 1));

			// 既に同一グループ
			Assert.IsFalse(tree.Unite(1, 2, 4));
			Assert.AreEqual(1, tree.Diff(1, 2));

			// 別グループ
			Assert.IsFalse(tree.IsSame(2, 3));

			// 同一グループ化
			Assert.IsTrue(tree.Unite(2, 3, 1));
			Assert.IsTrue(tree.IsSame(2, 3));
			Assert.AreEqual(1, tree.Diff(1, 2));
			Assert.AreEqual(1, tree.Diff(2, 3));
			Assert.AreEqual(2, tree.Diff(1, 3));

			// 左側をmerge
			Assert.IsTrue(tree.Merge(0, 1, 1));
			Assert.AreEqual(1, tree.Diff(0, 1));

			// id   0 1 2 3
			// diff  1 1 1

			// 右側から負のweightでmerge
			Assert.IsTrue(tree.Merge(3, 4, -1));
			Assert.AreEqual(-1, tree.Diff(3, 4));
			Assert.AreEqual(1, tree.Diff(4, 3));
			Assert.AreEqual(2, tree.Diff(0, 4));
			Assert.AreEqual(0, tree.Diff(2, 4));

			//          4
			// id   0 1 2 3
			// diff  1 1 1
		}
	}
}

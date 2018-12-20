using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace AtCoderCS
{
	[TestFixture()]
	public class SolverTest
	{
		[TestCaseSource(typeof(TestDataFactory), "Cases")]
		public void TestCase(TestData data) {
			var sb = new StringBuilder();
			var solver = new Solver(new DataReader(data.Input), new DataWriter(sb));
			solver.Run();
			Assert.AreEqual(data.Expected, sb.ToString().TrimEnd());
		}
	}

	public class TestData
	{
		public string Name { get; private set; }
		public string Input { get; private set; }
		public string Expected { get; private set; }

		public override string ToString() => Name;

		public TestData(string name, string input, string expected) {
			Name = name;
			Input = input;
			Expected = expected;
		}
	}
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("test1",
							 @"125",
							 @"176"),
				new TestData("test2",
				             @"9999999999",
				             @"12656242944"),
				new TestData("Test3",
							 @"1",
							 @"1"),

				new TestData("Test4",
							 @"111",
				             @"138"),
			};
		}
	}
}

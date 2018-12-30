using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AtCoderCS
{
	[TestFixture()]
	public class SolverTest {
		[TestCaseSource(typeof(TestDataFactory), "Cases")]
		public void TestCase(TestData data) {
			var sb = new StringBuilder();
			var solver = new Solver(new DataReader(data.Input), new DataWriter(sb));
			var sw = Stopwatch.StartNew();
			solver.Run();
			sw.Stop();
			Console.WriteLine($"Elapsed: {sw.Elapsed}");
			Assert.AreEqual(data.Expected, sb.ToString().TrimEnd());
		}
	}

	public class TestData {
		public string Name { get; private set; }
		public string Input { get; private set; }
		public string Expected { get; private set; }

		public override string ToString() => Name;

		public TestData(string name, string input, string expected) {
			Name = name;
			Input = input;
			Expected = expected.Replace("\r\n", "\n");
		}
	}
	public static class TestDataFactory {
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("test1",
							 @"5
3 1 5 4 2",
							 @"5"),
				new TestData("test2",
							 @"6
1 2 3 4 5 6",
							 @"0"),
				new TestData("Test3",
@"7
7 6 5 4 3 2 1",
@"21"),
				new TestData("Test4",
@"20
19 11 10 7 8 9 17 18 20 4 3 15 16 1 5 14 6 2 13 12",
@"114"),
			};
		}
	}
}

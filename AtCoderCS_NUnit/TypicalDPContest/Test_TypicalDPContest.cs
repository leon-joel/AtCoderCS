using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TypicalDPContest
{
	public class NunitSolver : Solver
	{
		readonly StringReader _reader;
		readonly StringBuilder _sb;
		public NunitSolver(string input, StringBuilder writer) {
			_reader = new StringReader(input);
			_sb = writer;
		}
		override protected string ReadLine() => _reader.ReadLine();
		override protected int ReadInt() => int.Parse(_reader.ReadLine());
		override protected long ReadLong() => long.Parse(_reader.ReadLine());
		override protected string[] ReadStringArray() => _reader.ReadLine().Split(' ');
		override protected int[] ReadIntArray() => _reader.ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		override protected long[] ReadLongArray() => _reader.ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		override protected void WriteLine(string line) => _sb.AppendLine(line);
		override protected void WriteLine<T>(T value) =>_sb.AppendLine(value.ToString());
	}
	[TestFixture()]
	public class SolverTest {
		[TestCaseSource(typeof(TestDataFactory), "Cases")]
		public void TestCase(TestData data) {
			var sb = new StringBuilder();
			var solver = new NunitSolver(data.Input, sb);
			var sw = Stopwatch.StartNew();
			solver.Run();
			sw.Stop();
			Console.WriteLine($"Elapsed: {sw.Elapsed}");
			Assert.AreEqual(data.Expected, sb.ToString().TrimEnd());
		}
		[TestCaseSource(typeof(TestDataFactoryForPartialCountUp), "Cases")]
		public void TestCaseForPartialCountUp(TestData data) {
			var sb = new StringBuilder();
			var solver = new NunitSolver(data.Input, sb);
			var sw = Stopwatch.StartNew();
			solver.RunPartialCountUp();
			sw.Stop();
			Console.WriteLine($"Elapsed: {sw.Elapsed}");
			Assert.AreEqual(data.Expected, sb.ToString().TrimEnd());
		}
		[TestCaseSource(typeof(TestDataFactoryForMinimumPartialCountUp), "Cases")]
		public void TestCaseForMinimumPartialCountUp(TestData data) {
			var sb = new StringBuilder();
			var solver = new NunitSolver(data.Input, sb);
			var sw = Stopwatch.StartNew();
			solver.RunMinimumPartialCountUp();
			sw.Stop();
			Console.WriteLine($"Elapsed: {sw.Elapsed}");
			Assert.AreEqual(data.Expected, sb.ToString().TrimEnd());
		}
		[TestCaseSource(typeof(TestDataFactoryForPartialSumWithCntLimitation), "Cases")]
		public void TestCaseForPartialSumWithCntLimitation(TestData data) {
			var sb = new StringBuilder();
			var solver = new NunitSolver(data.Input, sb);
			var sw = Stopwatch.StartNew();
			solver.RunPartialSumWithCntLimitation();
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
				new TestData("Test1",
@"3
2 3 5",
@"7"),
				new TestData("Test2",
@"10
1 1 1 1 1 1 1 1 1 1",
@"11"),
//				new TestData("Test3",
//@"3 10
//7 4 8",
//@"No"),
//				new TestData("Test4",
//@"3 10
//7 10 3",
//@"Yes"),
//				new TestData("Test5",
//@"37",
//@"2"),
//				new TestData("Test6",
//@"35",
//@"6"),
//				new TestData("Test7",
//@"36",
//@"1"),
//				new TestData("Test8",
//@"81",
//@"1"),
//				new TestData("Test9",
//@"13122",
//@"2"),
//				new TestData("Test10",
//@"13123",
//@"3"),
//				new TestData("Test11",
//@"93312",
//@"2"),
			};
		}
	}

	public static class TestDataFactoryForPartialCountUp
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"5 12
7 5 3 1 8",
@"2"),
				new TestData("Test2",
@"4 5
4 1 1 1",
@"3"),
			};
		}
	}

	public static class TestDataFactoryForMinimumPartialCountUp
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"5 12
7 5 3 1 8",
@"2"),
				new TestData("Test2",
@"2 6
7 5",
@"-1"),
			};
		}
	}
	public static class TestDataFactoryForPartialSumWithCntLimitation
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"3 10
1 2 4 2 1 1
2 5
1 4 2 1
0 0",
@"8
4"),
//				new TestData("Test2",
//@"2 6
//7 5",
//@"-1"),
			};
		}
	}
}

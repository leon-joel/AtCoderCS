using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace EducationalDPContest.E
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
		override protected void WriteLine<T>(T value) => _sb.AppendLine(value.ToString());
	}

	[TestFixture()]
	public class SolverTest
	{
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
			Expected = expected.Replace("\r\n", "\n");
		}
	}

	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"3 8
3 30
4 50
5 60",
@"90"),
				new TestData("Test2",
@"1 1000000000
1000000000 10",
@"10"),
				new TestData("Test3",
@"6 15
6 5
5 6
6 4
6 6
3 5
7 2",
@"17"),
				new TestData("Test4",
@"1 1
2 1000",
@"0"),
				new TestData("Test5",
@"2 10
1 1
11 1000",
@"1"),
				new TestData("Test6",
@"100 100
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000
1 1000",
@"100000"),
			};
		}
	}
}

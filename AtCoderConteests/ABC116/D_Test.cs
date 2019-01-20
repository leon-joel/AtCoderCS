using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ABC116.D
{
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"5 3
1 9
1 7
2 6
2 5
3 1
",
@"26"),
				new TestData("Test1r",
@"7 4
1 1
2 1
3 1
4 6
4 5
4 5
4 5
",
@"25"),
				new TestData("Test2",
@"6 5
5 1000000000
2 990000000
3 980000000
6 970000000
6 960000000
4 950000000
",
@"4900000016"),
//				new TestData("Test3",
//@"15 10000000000
//400000000 1000000000
//800000000 1000000000
//1900000000 1000000000
//2400000000 1000000000
//2900000000 1000000000
//3300000000 1000000000
//3700000000 1000000000
//3800000000 1000000000
//4000000000 1000000000
//4100000000 1000000000
//5200000000 1000000000
//6600000000 1000000000
//8000000000 1000000000
//9300000000 1000000000
//9700000000 1000000000
//",
//@"6500000000"),
//				new TestData("Test4",
//@"10
//1 3 2 3 3 2 3 2 1 3
//",
//@"54.48064457488221"),
//				new TestData("Test5",
//@"1 1
//1
//1",
//@"1"),
//				new TestData("Test6",
//@"1 2
//2
//2 1
//",
//@"1"),
//				new TestData("Test7",
//@"3 1
//3 2 1
//3
//",
//@"1"),
//				new TestData("Test8",
//@"3 3
//1 2 3
//1 2 3",
//@"0"),
//				new TestData("Test8-1",
//@"1000000005
//1",
//@"1000000005"),
//				new TestData("Test9",
//@"1000000008
//1",
//@"1"),
//				new TestData("Test9-1",
//@"1000000009
//1",
//@"2"),

//				new TestData("Test10",
//@"2000000013
//1",
//@"1000000006"),
//				new TestData("Test11",
//@"2000000014
//1",
//@"0"),
//				new TestData("Test12",
//@"2000000015
//1",
//@"1"),
			};
		}
	}

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
		override protected double[] ReadDoubleArray() => _reader.ReadLine().Split(' ').Select<string, double>(s => double.Parse(s)).ToArray();
		override protected void WriteLine(string line) => _sb.AppendLine(line);
		override protected void WriteLine(double d) => _sb.AppendLine($"{d:F9}");
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
			// 文字列比較
			Assert.AreEqual(data.Expected, sb.ToString().TrimEnd());
			// 浮動小数比較
			//Assert.AreEqual(double.Parse(data.Expected), double.Parse(sb.ToString().TrimEnd()), 0.000000001);
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
}

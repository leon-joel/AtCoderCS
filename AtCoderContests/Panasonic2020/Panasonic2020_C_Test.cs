using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Panasonic2020.C
{
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"2 3 9",
@"No"),
				new TestData("Test2",
@"2 3 10",
@"Yes"),
				new TestData("Test3",
@"1000000000 1000000000 1000000000",
@"No"),
				new TestData("Test4",
@"1 1 4
",
@"No
"),
				new TestData("Test5",
@"25 25 100
",
@"No
"),
				new TestData("Test6",
@"2500 2500 10000
",
@"No"),
				new TestData("Test7",
@"3 3 12
",
@"No"),
				new TestData("Test8",
@"13 13 52
",
@"No"),
				new TestData("Test8-1",
@"10 10 40
",
@"No"),
				new TestData("Test9",
@"20 20 80
",
@"No"),
				new TestData("Test9-1",
@"1 100000000 100020001
",
@"No"),

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
		override protected string ReadString() => ReadLine();
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
			Assert.AreEqual(data.Expected.TrimEnd(), sb.ToString().TrimEnd());
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

﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ABC117.D
{
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"3 7
1 6 3
",
@"14"),
				new TestData("Test2",
@"4 9
7 4 0 3
",
@"46"),
				new TestData("Test3",
@"1 0
1000000000000
",
@"1000000000000"),
				new TestData("Test4",
@"3 10
8 1 2
",
@"26"),
				new TestData("Test5",
@"3 4
4 0 0
",
@"13"),
				new TestData("Test6",
@"1 1
1000000000000
",
@"1000000000001"),
				new TestData("Test7",
@"3 10
8 7 6
",
@"30"),
				new TestData("Test8",
@"3 4
1 2 4
",
@"11"),
				new TestData("Test9",
@"3 4
1 1 4",
@"12"),
				new TestData("Test10",
@"1 4
1",
@"5"),
				new TestData("Test11",
@"1 7
1",
@"7"),
				new TestData("Test12",
@"5 2
0 0 0 2 2",
@"9"),
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

﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// KEYENCE Programming Contest 2019
// https://atcoder.jp/contests/keyence2019
namespace KeyenceContest2019_D
{
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"2 2
4 3
3 4
",
@"2"),
				new TestData("Test2",
@"3 3
5 9 7
3 6 9
",
@"0"),
				new TestData("Test3",
@"2 2
4 4
4 4
",
@"0"),
				new TestData("Test4",
@"14 13
158 167 181 147 178 151 179 182 176 169 180 129 175 168
181 150 178 179 167 180 176 169 182 177 175 159 173
",
@"343772227"),
				new TestData("Test5",
@"1 1
1
1",
@"1"),
				new TestData("Test6",
@"1 2
2
2 1
",
@"1"),
				new TestData("Test7",
@"3 1
3 2 1
3
",
@"1"),
				new TestData("Test8",
@"3 3
1 2 3
1 2 3",
@"0"),
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
}

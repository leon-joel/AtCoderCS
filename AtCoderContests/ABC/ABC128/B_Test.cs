﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ABC128.B
{
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"6
khabarovsk 20
moscow 10
kazan 50
kazan 35
moscow 60
khabarovsk 40
",
@"3
4
6
1
5
2
"),
				new TestData("Test2",
@"10
yakutsk 10
yakutsk 20
yakutsk 30
yakutsk 40
yakutsk 50
yakutsk 60
yakutsk 70
yakutsk 80
yakutsk 90
yakutsk 100
",
@"10
9
8
7
6
5
4
3
2
1
"),
				new TestData("Test3",
@"1
ddd 10
",
@"1
"),
//				new TestData("Test4",
//@"1
//50
//1
//",
//@"49"),
//				new TestData("Test5",
//@"3
//3 3 3
//",
//@"3"),
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
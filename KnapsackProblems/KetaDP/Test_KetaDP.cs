﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace KetaDP
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
	public class NunitSolverABC029D : SolverABC029D
	{
		readonly StringReader _reader;
		readonly StringBuilder _sb;
		public NunitSolverABC029D(string input, StringBuilder writer) {
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
	public class NunitSolverCodeFes2014D : SolverCodeFes2014D
	{
		readonly StringReader _reader;
		readonly StringBuilder _sb;
		public NunitSolverCodeFes2014D(string input, StringBuilder writer) {
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
		[TestCaseSource(typeof(TestDataFactoryABC029D), "Cases")]
		public void TestCaseABC029D(TestData data) {
			var sb = new StringBuilder();
			var solver = new NunitSolverABC029D(data.Input, sb);
			var sw = Stopwatch.StartNew();
			solver.Run();
			sw.Stop();
			Console.WriteLine($"Elapsed: {sw.Elapsed}");
			Assert.AreEqual(data.Expected, sb.ToString().TrimEnd());
		}
		[TestCaseSource(typeof(TestDataFactoryCodeFes2014D), "Cases")]
		public void TestCaseCodeFes2014D(TestData data) {
			var sb = new StringBuilder();
			var solver = new NunitSolverCodeFes2014D(data.Input, sb);
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
@"1 9",
@"2"),
				new TestData("Test2",
@"40 49",
@"10"),
				new TestData("Test3",
@"1 1000",
@"488"),
				new TestData("Test4",
@"1 1000000000000000000",
@"981985601490518016"),
				new TestData("Test5",
@"4 19",
@"4"),
			};
		}
	}

	public static class TestDataFactoryABC029D
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"12",
@"5"),
				new TestData("Test2",
@"345",
@"175"),
				new TestData("Test3",
@"999999999",
@"900000000"),
//				new TestData("Test4",
//@"1 1000000000000000000",
//@"981985601490518016"),
//				new TestData("Test5",
//@"4 19",
//@"4"),
			};
		}
	}

	public static class TestDataFactoryCodeFes2014D
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"1234 2",
@"12"),
				new TestData("Test2",
@"800000 1",
@"22223"),
				new TestData("Test3",
@"7328495 10",
@"0"),
				new TestData("Test4",
@"262004 2",
@"218"),
				new TestData("Test5",
@"1000 1",
@"1"),
				new TestData("Test6",
@"100000000000000 1",
@"1"),
			};
		}
	}
}

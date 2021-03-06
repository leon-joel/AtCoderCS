﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using KnapsackProblem;

namespace KnapsackProblem
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
@"4 5
4 2
5 2
2 1
8 3",
@"13"),
				new TestData("test2",
@"2 20
5 9
4 10",
@"9"),
				new TestData("Test3",
@"6 8
3 2
2 1
6 3
1 2
3 1
85 5",
@"91"),
				new TestData("Test4",
@"200 50
10 10
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
1 2
3 1
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
1 2
3 1
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
1 2
3 1
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
3 2
2 1
6 3
1 2
3 1
85 5
1 2
3 1",
@"850"),
			};
		}
	}
}

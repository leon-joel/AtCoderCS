using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using KnapsackProblem;

namespace ABC099C_StrangeBank
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
@"127",
@"4"),
				new TestData("test2",
@"3",
@"3"),
				new TestData("Test3",
@"44852",
@"16"),
				new TestData("Test4",
@"1",
@"1"),
//				new TestData("Test5",
//@"1",
//@"1"),
				new TestData("Test6",
@"35",
@"6"),
				new TestData("Test7",
@"36",
@"1"),
				new TestData("Test8",
@"81",
@"1"),
				new TestData("Test9",
@"13122",
@"2"),
				new TestData("Test10",
@"13123",
@"3"),
				new TestData("Test11",
@"93312",
@"2"),
			};
		}
	}
}

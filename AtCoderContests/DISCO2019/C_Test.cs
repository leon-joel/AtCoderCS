using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DISCO2019.C
{
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"3 3 5
#.#
.#.
#.#
",
@"1 2 2
1 3 4
5 5 4
"),
				new TestData("Test2",
@"3 7 7
#...#.#
..#...#
.#..#..
",
@"1 1 2 2 3 4 4
6 6 2 2 3 5 5
6 6 7 7 7 7 7
"),
				new TestData("Test3",
@"13 21 106
.....................
.####.####.####.####.
..#.#..#.#.#....#....
..#.#..#.#.#....#....
..#.#..#.#.#....#....
.####.####.####.####.
.....................
.####.####.####.####.
....#.#..#....#.#..#.
.####.#..#.####.#..#.
.#....#..#.#....#..#.
.####.####.####.####.
.....................
",
@"12 12 23 34 45 45 60 71 82 93 93 2 13 24 35 35 17 28 39 50 50
12 12 23 34 45 45 60 71 82 93 93 2 13 24 35 35 17 28 39 50 50
12 12 56 89 89 89 60 104 82 31 31 46 13 24 35 35 61 61 39 50 50
12 12 67 67 100 100 60 9 9 42 42 57 13 24 6 72 72 72 72 72 72
12 12 78 5 5 5 20 20 20 53 68 68 90 24 6 83 83 83 83 83 83
16 16 27 38 49 49 64 75 86 97 79 79 90 101 6 94 94 105 10 21 21
16 16 27 38 49 49 64 75 86 97 79 79 90 101 6 94 94 105 10 21 21
32 32 43 54 65 65 80 11 106 95 22 22 33 44 55 55 70 1 96 85 85
32 32 43 54 76 76 91 11 106 84 84 4 99 66 66 66 81 1 96 74 74
14 14 3 98 87 87 102 11 73 73 73 4 99 88 77 77 92 92 63 63 63
25 25 3 98 87 87 7 29 62 62 62 15 99 88 77 77 103 19 30 52 52
36 36 47 58 69 69 18 29 40 51 51 26 37 48 59 59 8 19 30 41 41
36 36 47 58 69 69 18 29 40 51 51 26 37 48 59 59 8 19 30 41 41
"),
//				new TestData("Test4",
//@"100000
//",
//@"108000 108000"),
//				new TestData("Test5",
//@"1
//1 1
//2
//",
//@"2"),
//				new TestData("Test6",
//@"2020/01/01
//",
//@"TBD"),
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
		override protected void WriteGrid<T>(T[,] dp) {
			var w = dp.GetLength(1);
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					_sb.Append(dp[i, j]);
					if (j < w - 1)
						_sb.Append(" ");
				}
				_sb.AppendLine();
			}
		}
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
			//Assert.AreEqual(double.Parse(data.Expected.TrimEnd()), double.Parse(sb.ToString().TrimEnd()), 0.000000001);
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

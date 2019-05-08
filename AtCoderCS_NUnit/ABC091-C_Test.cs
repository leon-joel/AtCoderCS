using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ABC091.C
{
	public static class TestDataFactory
	{
		public static TestData[] Cases() {
			return new TestData[] {
				new TestData("Test1",
@"3
2 0
3 1
1 3
4 2
0 4
5 5
",
@"2
"),
				new TestData("Test2",
@"3
0 0
1 1
5 2
2 3
3 4
4 5
",
@"2
"),
				new TestData("Test3",
@"2
2 2
3 3
0 0
1 1
",
@"0
"),
				new TestData("Test4",
@"5
0 0
7 3
2 2
4 8
1 6
8 5
6 9
5 4
9 1
3 7
",
@"5
"),
				new TestData("Test5",
@"5
0 0
1 1
5 5
6 6
7 7
2 2
3 3
4 4
8 8
9 9
",
@"4"),
				new TestData("Test6",
@"44
0 87
1 86
2 85
3 84
4 83
5 82
6 81
7 80
8 79
9 78
10 77
11 76
12 75
13 74
14 73
15 72
16 71
17 70
18 69
19 68
20 67
21 66
22 65
23 64
24 63
25 62
26 61
27 60
28 59
29 58
30 57
31 56
32 55
33 54
34 53
35 52
36 51
37 50
38 49
39 48
40 47
41 46
42 45
43 44
44 43
45 42
46 41
47 40
48 39
49 38
50 37
51 36
52 35
53 34
54 33
55 32
56 31
57 30
58 29
59 28
60 27
61 26
62 25
63 24
64 23
65 22
66 21
67 20
68 19
69 18
70 17
71 16
72 15
73 14
74 13
75 12
76 11
77 10
78 9
79 8
80 7
81 6
82 5
83 4
84 3
85 2
86 1
87 0
",
@"0"),
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

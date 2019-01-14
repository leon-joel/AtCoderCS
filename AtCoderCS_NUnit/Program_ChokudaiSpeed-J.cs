using System;
using System.Collections.Generic;
using System.Linq;

namespace AtCoderCS
{
	public interface IReader
	{
		string ReadLine();
		int ReadInt();
		long ReadLong();
		string[] ReadStringArray();
		int[] ReadIntArray();
		long[] ReadLongArray();
	}
	public class ConsoleReader : IReader
	{
		public string ReadLine() => Console.ReadLine();
		public int ReadInt() => int.Parse(ReadLine());
		public long ReadLong() => long.Parse(ReadLine());
		public string[] ReadStringArray() => ReadLine().Split(' ');
		public int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		public long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
	}

	public interface IWriter
	{
		void WriteLine(string line);
		void WriteLine<T>(T value) where T : IFormattable;
	}
	public class ConsoleWriter : IWriter
	{
		public void WriteLine(string line) => Console.WriteLine(line);
		public void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);
	}

	public class BinaryIndexedTree
	{
		public int MaxNum { get; private set; }

		int[] bit;

		public BinaryIndexedTree(int maxNum) {
			MaxNum = maxNum;
			bit = new int[maxNum + 1];
		}

		// i の個数を x 増加
		// ※i は1以上
		public void Add(int i, int v) {
			for (int x = i; x <= MaxNum; x += x & -x)
				bit[x] += v;
		}

		// i 以下の総数を返す
		// ※0以下のi を与えた場合は0を返す
		public int Sum(int i) {
			int ret = 0;
			for (int x = i; 0 < x; x -= x & -x)
				ret += bit[x];
			return ret;
		}

		// [lower, upper] つまり lower以上 upper以下 の総数を返す
		public int RangeSum(int lower, int upper) {
			return Sum(upper) - Sum(lower - 1);
		}

		// i の個数を返す
		public int Count(int i) {
			return Sum(i) - Sum(i - 1);
		}
	}
	public partial class Solver
	{
		readonly IReader _reader;
		readonly IWriter _writer;

		public Solver(IReader reader = null, IWriter writer = null) {
			_reader = reader ?? new ConsoleReader();
			_writer = writer ?? new ConsoleWriter();
		}

		partial void Dump<T>(IEnumerable<T> array);

		public void Run() {

			var n = _reader.ReadInt();
			var nums = _reader.ReadIntArray();

			var bit = new BinaryIndexedTree(n);

			long ans = 0;
			for (int i = 0; i < n; i++) {
				ans += i - bit.Sum(nums[i]);
				bit.Add(nums[i], 1);
			}

			_writer.WriteLine(ans);
		}
	}

	class MainClass
	{
		public static void Main(string[] args) {
			new Solver().Run();
		}
	}
}

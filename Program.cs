using System;
using System.Linq;

namespace AtCoderCS
{
	public interface IReader
	{
		string ReadLine();
	}
	public class ConsoleReader : IReader
	{
		public string ReadLine() {
			return Console.ReadLine();
		}
	}

	public interface IWriter
	{
		void WriteLine(string line);
		void WriteLine(int value);
		void WriteLine(ulong value);
	}
	public class ConsoleWriter : IWriter
	{
		public void WriteLine(string line){
			Console.WriteLine(line);
		}
		public void WriteLine(ulong value) { Console.WriteLine(value); }
		public void WriteLine(int value) { Console.WriteLine(value); }
	}

	public class SPoint
	{
		public SPoint(uint left) {
			Left = left;
		}
		public uint Left { get; set; }
		public uint Right { get; set; }
		public uint Upper { get; set; }
		public uint Lower { get; set; }
		public override string ToString() {
			return $"[LRUD=({Left},{Right},{Upper},{Lower})]";
		}

		public uint PermuteCount() {
			return (Left + Right) * (Upper + Lower);
		}
	}

	public class Solver
	{
		IReader _reader;
		IWriter _writer;

		public Solver(IReader reader = null, IWriter writer = null) {
			if (reader == null) {
				_reader = new ConsoleReader();
			} else {
				_reader = reader;
			}

			if (writer == null) {
				_writer = new ConsoleWriter();
			} else {
				_writer = writer;
			}
		}

		public void Run() {
			//// 整数の入力
			//int a = int.Parse(Console.ReadLine());
			//// スペース区切りの整数の入力
			//string[] input = Console.ReadLine().Split(' ');
			//int b = int.Parse(input[0]);
			//int c = int.Parse(input[1]);
			//// 文字列の入力
			//string s = Console.ReadLine();
			////出力
			//Console.WriteLine((a + b + c) + " " + s);

			var nums = _reader.ReadLine().Split().Select(int.Parse).ToArray();
			var height = nums[0];
			var width = nums[1];
			//Console.WriteLine($"{height}, {width}");

			SPoint[,] grid = new SPoint[height, width];
			for (int r = 0; r < height; r++) {
				uint s = 0;
				var line = _reader.ReadLine();
				for (int c = 0; c < width; c++) {
					if (line[c] == '#') {
						s = 0;
					} else {
						grid[r, c] = new SPoint(s);
						++s;
					}
				}

				// 右からのスペース数をカウントアップ
				s = 0;
				for (int c = width - 1; 0 <= c; c--) {
					if (grid[r, c] == null)
						s = 0;
					else {
						grid[r, c].Right += s;
						++s;
					}
				}

				// 上からのスペースをカウントアップ
				if (0 < r) {
					for (int c = 0; c < width; c++) {
						if (grid[r, c] != null && grid[r - 1, c] != null) {
							grid[r, c].Upper = grid[r - 1, c].Upper + 1;
						}
					}
				}
			}

			for (int r = height - 1; 0 <= r; r--) {
				if (r < height - 1) {
					// 下からのスペースをカウントアップ
					for (int c = 0; c < width; c++) {
						if (grid[r, c] != null && grid[r + 1, c] != null) {
							grid[r, c].Lower = grid[r + 1, c].Lower + 1;
						}
					}
				}
			}
			//DumpGrid(height, width, grid);

			// 順序対をカウントアップ
			ulong ans = 0;
			foreach (var sp in grid) {
				if (sp != null) {
					ans += sp.PermuteCount();
				}
			}
			_writer.WriteLine(ans);
		}

		private static void DumpGrid(int height, int width, SPoint[,] grid) {
			for (int r = 0; r < height; r++) {
				for (int c = 0; c < width; c++) {
					if (grid[r, c] == null)
						Console.Write("#, ");
					else
						Console.Write($"{grid[r, c]}, ");
				}
				Console.WriteLine();
			}
		}
	}

	class MainClass
	{
		public static void Main(string[] args) {
			new Solver().Run();
		}
	}
}

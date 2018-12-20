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

			var s = _reader.ReadLine();
			//_writer.WriteLine(s);

			//bit全探索
			//+ を入れられる場所の数
			var n = s.Length - 1;

			ulong sum = 0;
			// bit mask の生成
			// 2^n     = 1 << n
			for (ulong mask = 0; mask <= ((1ul << n) - 1); mask++) {
				int lastIdx = 0;

				// 各bitを下から舐める
				for (int i = 0; i < n; i++) {
					//iビット目が立っている？
					if (0 != (mask & (1ul << i))) {
						// '+'を i番目 と i+1番目の間に入れる
						var sub = s.Substring(lastIdx, i - lastIdx + 1);
						sum += ulong.Parse(sub);
						lastIdx = i + 1;
					}
				}
				// 残り全部
				var rest = s.Substring(lastIdx, s.Length - lastIdx);
				sum += ulong.Parse(rest);
			}
			_writer.WriteLine(sum);
		}
	}

	class MainClass
	{
		public static void Main(string[] args) {
			new Solver().Run();
		}
	}
}

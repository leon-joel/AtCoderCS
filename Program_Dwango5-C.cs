using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		void WriteLine(int value);
		void WriteLine(ulong value);
	}
	public class ConsoleWriter : IWriter
	{
		public void WriteLine(string line) => Console.WriteLine(line);
		public void WriteLine(ulong value) => Console.WriteLine(value);
		public void WriteLine(int value) => Console.WriteLine(value);
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
			var N = _reader.ReadInt();
			//Console.WriteLine(N);

			// DMCを数値化
			var S = _reader.ReadLine();
			byte[] s_ary = new byte[S.Length];
			for (int i = 0; i < S.Length; i++) {
				var c = S[i];
				switch (c) {
					case 'D': s_ary[i] = 1; break;
					case 'M': s_ary[i] = 2; break;
					case 'C': s_ary[i] = 3; break;
				}
			}
			Dump(s_ary);
			_reader.ReadInt();

			int[] k_ary = _reader.ReadIntArray();
			//k_ary.Dump();

			foreach (var k in k_ary) {
				int sect_len = k - 1;
				ulong d_num = 0;
				ulong m_num = 0;
				ulong dm_num = 0;
				ulong ans = 0;

				// 右端のindexをインクリメントしていく
				for (int r = 0; r < N; r++) {
					// 追加される文字の処理
					var c = s_ary[r];
					if (c == 3) // C
						ans += dm_num;
					else if (c == 1) // D
						++d_num;
					else if (c == 2) {// M
						dm_num += d_num;
						++m_num;
					}

					// 左端から区間外に飛び出す文字の処理
					if (0 <= r - sect_len) {
						var lc = s_ary[r - sect_len];
						if (lc == 1) {// D
							dm_num -= m_num;
							--d_num;
						} else if (lc == 2) {// M
							--m_num;
						}
					}
				}
				_writer.WriteLine(ans);
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

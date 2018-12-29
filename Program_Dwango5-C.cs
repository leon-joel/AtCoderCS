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

			var S = _reader.ReadLine();

			// D/M数, DM組数を累積和で保持する ★i番目の前までの和 を格納する
			ulong[] accumD = new ulong[S.Length];
			ulong[] accumM = new ulong[S.Length];
			ulong[] accumDM = new ulong[S.Length];
			ulong sumD = 0, sumM = 0, sumDM = 0;
			for (int i = 0; i < S.Length; i++) {
				accumD[i] = sumD;
				accumM[i] = sumM;
				accumDM[i] = sumDM;
				switch (S[i]) {
					case 'D': ++sumD; break;
					case 'M':
						sumDM += sumD;
						++sumM;
						break;
				}
			}
			_reader.ReadInt();

			int[] k_ary = _reader.ReadIntArray();
			//k_ary.Dump();

			foreach (var k in k_ary) {
				int sect_len = k - 1;
				int l = 0;
				ulong ans = 0;

				// 右端のindexをインクリメントしていく
				for (int r = 2; r < N; r++) {
					// 左端
					if (S[r] == 'C') {
						if (0 <= r - sect_len) l = r - sect_len;
						ans += accumDM[r] - accumDM[l] - accumD[l] * (accumM[r] - accumM[l]);
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

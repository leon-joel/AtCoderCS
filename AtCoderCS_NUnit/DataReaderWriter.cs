using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtCoderCS
{
	public class DataReader : IReader
	{
		TextReader _reader;
		public DataReader(string inputSource){
			_reader = new StringReader(inputSource);
		}
		public DataReader(TextReader reader) {
			_reader = reader;
		}

		public string ReadLine() => _reader.ReadLine();
		public int ReadInt() => int.Parse(_reader.ReadLine());
		public long ReadLong() => long.Parse(_reader.ReadLine());
		public string[] ReadStringArray() => _reader.ReadLine().Split(' ');
		public int[] ReadIntArray() => _reader.ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		public long[] ReadLongArray() => _reader.ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
	}

	public class DataWriter : IWriter
	{
		StringBuilder _sb;
		public DataWriter(StringBuilder sb) {
			_sb = sb;
		}
		public void WriteLine(string line) { _sb.AppendLine(line); }
		public void WriteLine<T>(T value) where T : IFormattable => _sb.AppendLine(value.ToString());
	}
}

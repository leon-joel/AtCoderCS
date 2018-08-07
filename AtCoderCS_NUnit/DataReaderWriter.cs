using System;
using System.IO;
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

		public string ReadLine() {
			return _reader.ReadLine();
		}
	}

	public class DataWriter : IWriter
	{
		StringBuilder _sb;
		public DataWriter(StringBuilder sb) {
			_sb = sb;
		}
		public void WriteLine(string line) { _sb.AppendLine(line); }
		public void WriteLine(ulong value) { _sb.AppendLine(value.ToString()); }
		public void WriteLine(int value) { _sb.AppendLine(value.ToString()); }
	}
}

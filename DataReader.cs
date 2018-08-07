using System;
using System.IO;

namespace AtCoderCS
{
	public class DataReader : IReader
	{
		TextReader _reader;
		public DataReader(TextReader reader) {
			_reader = reader;
		}

		public string ReadLine() {
			return _reader.ReadLine();
		}
	}
}

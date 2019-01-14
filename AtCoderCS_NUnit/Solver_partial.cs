using System;
using System.Collections.Generic;
using System.Text;

namespace AtCoderCS
{
	public partial class Solver
	{
		// partialメソッドにすることで、万が一提出コードにDump()呼び出しが残っていても、
		// それが無視されるようになる
		partial void Dump<T>(IEnumerable<T> array) {
			var sb = new StringBuilder();
			foreach (var item in array) {
				sb.Append(item);
				sb.Append(", ");
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}
	}
}

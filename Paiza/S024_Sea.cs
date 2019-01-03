using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Paiza.S024_Sea
{
	public static class Util
	{
		public static string DumpToString<T>(IEnumerable<T> array) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var item in array) {
				sb.Append(item);
				sb.Append(", ");
			}
			return sb.ToString();
		}
	}

	public class Solver
	{
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
		virtual protected int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		virtual protected long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);

		[Conditional("DEBUG")]
		void Dump<T>(IEnumerable<T> array) where T : IFormattable {
			string s = Util.DumpToString(array);
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(s);
			//_writer.WriteLine(s);
		}
		[Conditional("DEBUG")]
		void DumpGrid<T>(IEnumerable<IEnumerable<T>> arrayOfArray) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				sb.AppendLine(Util.DumpToString(a));
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}

		int H;
		int W;
		int N;
		int[,] Grid;
		int[,] GridT;

		public void Run() {
			var ary = ReadIntArray();
			H = ary[0];
			W = ary[1];
			N = ary[2];
			//Console.WriteLine($"{H}, {W}, {N}");

			Grid = new int[H+2, W+2];
			var g1 = new List<int>(H * W);
			for (int i = 1; i < H+1; i++) {
				var row = ReadIntArray();
				// 陸地を配列化
				g1.AddRange(row);

				for (int j = 1; j < W+1; j++) {
					Grid[i, j] = row[j - 1];
				}
			}
			//DumpGrid(Grid);

			// 配列化陸地をソート
			var heightArray = g1.Distinct().ToArray();
			Array.Sort(heightArray);

			// ソート結果をもとに陸地データを圧縮（最大10億 ➟ 最大900に）
			//Console.WriteLine("unique------");
			//Dump(heightArray);

			// N==1 は即終了
			if (N == 1) {
				WriteLine(0);
				return;
			}

			// 海面の高さを0から上げていく
			GridT = new int[H + 2, W + 2];
			for (int i = 0; i < heightArray.Length; i++) {
				var num = CountIslandsNum(heightArray[i]);

				// 最初に島数Nになった点で終了
				if (num == N) {
					// 海面の高さを圧縮前のデータに復元
					WriteLine(heightArray[i]);
					return;
				}
			}
		}

		int CountIslandsNum(int level) {
			for (int i = 1; i < H+1; i++) {
				for (int j = 1; j < W+1; j++) {
					GridT[i, j] = level < Grid[i, j] ? 1 : 0;
				}
			}
			int cnt = 0;
			for (int i = 1; i < H+1; i++) {
				for (int j = 1; j < W+1; j++) {
					if (GridT[i, j] == 1) {
						// 陸地
						Recurse(i, j);
						++cnt;
					}
				}
			}
			return cnt;
		}
		int[] DX = { 1, -1, 0, 0 };
		int[] DY = { 0, 0, 1, -1 };
		void Recurse(int x, int y) {
			GridT[x, y] = 0;
			for (int i = 0; i < 4; i++) {
				int px = x + DX[i];
				int py = y + DY[i];
				if (GridT[px, py] == 0) continue;
				Recurse(px, py);
			}
		}


#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

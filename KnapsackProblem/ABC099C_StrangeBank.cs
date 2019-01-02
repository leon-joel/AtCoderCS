using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ABC099C_StrangeBank
{
	// DP, メモ探索, BFS など、様々な解法で実装
	// https://atcoder.jp/contests/abc099/tasks/abc099_c
	// https://qiita.com/drken/items/ace3142967c4f01d42e9#%E8%A7%A3%E6%B3%95-1-1-%E3%83%88%E3%83%83%E3%83%97%E3%83%80%E3%82%A6%E3%83%B3%E3%81%AB-n-%E3%82%92%E9%99%8D%E4%B8%8B%E3%81%97%E3%81%A6%E3%81%84%E3%81%8F%E3%83%A1%E3%83%A2%E5%8C%96%E5%86%8D%E5%B8%B0
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
		void Dump<T>(IEnumerable<T> array) {
			var sb = new StringBuilder();
			foreach (var item in array) {
				sb.Append(item);
				sb.Append(", ");
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}

		struct Item
		{
			public readonly int Weight;
			public readonly int Value;

			public Item(int v, int w) {
				Value = v;
				Weight = w;
			}
			public Item(int[] ary) {
				Value = ary[0];
				Weight = ary[1];
			}
			public override string ToString() {
				return string.Format($"V:{Value} W:{Weight}");
			}
		}

		int N;
		int UpperW;
		Item[] Items;

		public void Run() {
			var ary = ReadIntArray();
			N = ary[0];
			UpperW = ary[1];

			Items = new Item[N];
			for (int i = 0; i < N; i++) {
				var item = new Item(ReadIntArray());
				//Console.WriteLine(item.ToString());
				Items[i] = item;
			}
			//Dump(items);

			// 動的計画法（DP）で解く
			var ans = DpSolve();
			// メモ探索で解く
			//var ans = MemoizeRecursive();
			// 全探索で解く
			//var ans = SearchAll(0, UpperW);
			WriteLine(ans);
		}

		// 動的計画法（DP）（＝漸化式＋ループ）での実装
		// ※この実装はDP表を上から埋めていくが、多分下から埋めても同じこと。
		//   https://www.slideshare.net/iwiwi/ss-3578511
		int[,] DP;
		int DpSolve() {
			DP = new int[N + 1, UpperW + 1];
			for (int i = 0; i < N + 1; i++) {
				for (int j = 0; j < UpperW + 1; j++) {
					DP[i, j] = 0;
				}
			}

			for (int i = 0; i < N; i++) {
				var item = Items[i];
				for (int w = 0; w < UpperW + 1; w++) {
					if (w < item.Weight) {
						// 現在itemの重さより左側（現在itemが使えない部分）は上から下ろしてくるだけ
						DP[i + 1, w] = DP[i, w];
					} else {
						// 現在itemの重さ以上の部分（右側）は
						// 重さ分左のvalue + 現在value or 下ろしてくるだけ
						DP[i + 1, w] = Math.Max(DP[i, w - item.Weight] + item.Value, DP[i, w]);
					}
				}
			}

			return DP[N, UpperW];
		}

		// メモ探索（＝再帰関数のメモ化）での実装
		// https://www.slideshare.net/iwiwi/ss-3578511
		int[,] Memo;
		int MemoizeRecursive() {
			Memo = new int[N, UpperW + 1];
			for (int i = 0; i < N; i++) {
				for (int j = 0; j < UpperW + 1; j++) {
					Memo[i, j] = -1;
				}
			}
			return Search(0, UpperW);
		}
		int Search(int i, int upper) {
			if (N <= i) return 0;

			// 既に計算済みの場合はそれを返す
			if (Memo[i, upper] != -1)
				return Memo[i, upper];

			var item = Items[i];

			// もうこの品物は入らない
			if (upper < item.Weight)
				return Search(i + 1, upper);

			// この品物を入れない場合と入れる場合の両方で探索
			var value1 = Search(i + 1, upper);
			var value2 = Search(i + 1, upper - item.Weight) + item.Value;

			var value = Math.Max(value1, value2);
			Memo[i, upper] = value;
			return value;
		}

		// 全探索で解く実装
		// https://www.slideshare.net/iwiwi/ss-3578511
		// これだと200品目でも終わらない O(2^n)
		int SearchAll(int i, int upper) {
			// 品物が残ってない
			if (N <= i) return 0;

			Item item = Items[i];

			// もうこの品物は入らない
			if (upper < item.Weight)
				return SearchAll(i + 1, upper);

			// この品物を入れない場合と入れる場合の両方で探索
			var value1 = SearchAll(i + 1, upper);
			var value2 = SearchAll(i + 1, upper - item.Weight) + item.Value;
			return Math.Max(value1, value2);
		}
#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}
}

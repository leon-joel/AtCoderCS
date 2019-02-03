﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ABC117.D
{
	using static Util;

	public class Solver : SolverBase
	{
		int CountBinaryDigits(long n) {
			int i = 0;
			while (true) {
				if (n < (1L << i)) 
					return i;
				else if (n == (1L << i))
					return i + 1;

				++i;
			}
		}
		// nのd桁目（2進 0〜）のBitが立っている？
		int GetBinaryDigit(long n, int d) {
			if (0 == (n & (1 << d)))
				return 0;
			else
				return 1;
		}
		int N;
		long K;
		public void Run() {
			var ary = ReadLongArray();
			N = (int)ary[0];
			K = ary[1];
			// Kは2進数で何桁？
			int KDigit = CountBinaryDigits(K);

			long[] Nums = ReadLongArray();
			Array.Sort(Nums);

			// 最大のNは2進数で何桁？
			int NDigit = CountBinaryDigits(Nums[Nums.Length - 1]);
		
			var dpLen = Math.Max(NDigit, KDigit) + 1;

			// i桁目(0-)のbitONの総数をカウント
			var dnums = new int[dpLen];
			for (int i = 0; i < N; i++) {
				for (int j = 0; j < dpLen; j++) {
					if (0 < (Nums[i] & (1L << j))) {
						dnums[j] += 1;
					}
				}
			}

			// DP[i桁目(2進数)][制約有無] = i桁目までの最大f(x)
			long[,] DP = new long[dpLen, 2];
			InitDP2(DP, -1);
			DP[dpLen - 1, 1] = 0;

			// 配るDP（配列の後ろから敷き詰めていく）
			for (int i = dpLen-2; 0 <= i; i--) {
				// 制約有無 0:制約なし 1:制約あり
				for (int r = 0; r <= 1; r++) {
					if (DP[i+1, r] == -1) continue;

					if (r == 0) {
						// 制約なし
						// 上からi桁目は0 or 1 どちらか良い方を選択
						// DP[i, 0] = DP[i+1, 0] + i桁ベター選択
						DP[i, 0] = DP[i+1, 0] + Math.Max(N - dnums[i], dnums[i]) * (1L << i);

					} else {
						// 制約あり
						if (1 == GetBinaryDigit(K, i)) {
							// Kの上からi桁目が1の場合:
							// 0を選択した場合は制約なし格納 ※既存の値と比べて大きい方を格納
							// DP[i, 0] = DP[i + 1, 1] + 0選択
							var res = DP[i + 1, 1] + dnums[i] * (1L << i);
							DP[i, 0] = Math.Max(res, DP[i, 0]);

							// 1を選択した場合は制約ありに格納
							// DP[i, 1] = DP[i + 1, 1] + 1選択
							DP[i, 1] = DP[i + 1, 1] + (N - dnums[i]) * (1L << i);

						} else {
							// Kの上からi桁目が0の場合: 
							// 0を選択し、制約ありに格納
							// DP[i, 1] = DP[i + 1, 1] + 0選択
							DP[i, 1] = DP[i + 1, 1] + dnums[i] * (1L << i);
						}
					}
				}
			}

			WriteLine(Math.Max(DP[0, 0], DP[0, 1]));
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	public static class Util
	{
		public readonly static long MOD = 1000000007;

		public static string DumpToString<T>(IEnumerable<T> array) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var item in array) {
				sb.Append(item);
				sb.Append(", ");
			}
			return sb.ToString();
		}

		public static void InitArray<T>(T[] ary, T value) {
			for (int i = 0; i < ary.Length; i++) {
				ary[i] = value;
			}
		}
		public static void InitDP2<T>(T[,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					dp[i, j] = value;
				}
			}
		}
		public static void InitDP3<T>(T[,,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					for (int k = 0; k < dp.GetLength(2); k++) {
						dp[i, j, k] = value;
					}
				}
			}
		}

		public static T Max<T>(params T[] nums) where T : IComparable {
			if (nums.Length == 0) return default(T);

			T max = nums[0];
			for (int i = 1; i < nums.Length; i++) {
				max = max.CompareTo(nums[i]) > 0 ? max : nums[i];
			}
			return max;
		}
		public static T Min<T>(params T[] nums) where T : IComparable {
			if (nums.Length == 0) return default(T);

			T min = nums[0];
			for (int i = 1; i < nums.Length; i++) {
				min = min.CompareTo(nums[i]) < 0 ? min : nums[i];
			}
			return min;
		}

		/// <summary>
		/// ソート済み配列 ary に同じ値の要素が含まれている？
		/// ※ソート順は昇順/降順どちらでもよい
		/// </summary>
		public static bool HasDuplicateInSortedArray<T>(T[] ary) where T : IComparable, IComparable<T> {
			if (ary.Length <= 1) return false;

			var lastNum = ary[ary.Length - 1];

			foreach (var n in ary) {
				if (lastNum.CompareTo(n) == 0) {
					return true;
				} else {
					lastNum = n;
				}
			}
			return false;
		}

		/// <summary>
		/// 二分探索
		/// ※条件を満たす最小のidxを返す
		/// ※満たすものがない場合は ary.Length を返す
		/// ※『aryの先頭側が条件を満たさない、末尾側が条件を満たす』という前提
		/// ただし、IsOK(...)の戻り値を逆転させれば、逆でも同じことが可能。
		/// </summary>
		/// <param name="ary">探索対象配列 ★ソート済みであること</param>
		/// <param name="key">探索値 ※これ以上の値を持つ（IsOKがtrueを返す）最小のindexを返す</param>
		public static int BinarySearch<T>(T[] ary, T key) where T : IComparable, IComparable<T> {
			int left = -1;
			int right = ary.Length;

			while (1 < right - left) {
				var mid = left + (right - left) / 2;

				if (IsOK(ary, mid, key)) {
					right = mid;
				} else {
					left = mid;
				}
			}

			// left は条件を満たさない最大の値、right は条件を満たす最小の値になっている
			return right;
		}
		public static bool IsOK<T>(T[] ary, int idx, T key) where T : IComparable, IComparable<T> {
			// key <= ary[idx] と同じ意味
			return key.CompareTo(ary[idx]) <= 0;
		}
	}

	public class SolverBase
	{
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		//virtual protected void ReadInt2(out int x, out int y) {
		//	var aryS = ReadLine().Split(' ');
		//	x = int.Parse(aryS[0]);
		//	y = int.Parse(aryS[1]);
		//}
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
		virtual protected int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		virtual protected long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		virtual protected double[] ReadDoubleArray() => ReadLine().Split(' ').Select<string, double>(s => double.Parse(s)).ToArray();
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine(double d) => Console.WriteLine($"{d:F9}");
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);

		[Conditional("DEBUG")]
		protected void Dump(string s) => Console.WriteLine(s);
		[Conditional("DEBUG")]
		protected void Dump(char c) => Console.WriteLine(c);
		[Conditional("DEBUG")]
		protected void Dump(double d) => Console.WriteLine($"{d:F9}");
		[Conditional("DEBUG")]
		protected void Dump<T>(IEnumerable<T> array) where T : IFormattable {
			string s = Util.DumpToString(array);
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(s);
			//_writer.WriteLine(s);
		}
		[Conditional("DEBUG")]
		protected void DumpGrid<T>(IEnumerable<IEnumerable<T>> arrayOfArray) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				sb.AppendLine(Util.DumpToString(a));
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}
		[Conditional("DEBUG")]
		protected void DumpDP<T>(T[,] dp) where T : IFormattable {
			var sb = new StringBuilder();
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					sb.Append(dp[i, j]);
					sb.Append(", ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ABC089.C
{
	using static Util;

	public class Solver : SolverBase
	{
		public void Run() {
			var N = ReadInt();
			// M, A, R, C, H
			int[] Ns = new int[5];
			for (int i = 0; i < N; i++) {
				var s = ReadLine();
				if (s[0] == 'M') ++Ns[0];
				else if (s[0] == 'A') ++Ns[1];
				else if (s[0] == 'R') ++Ns[2];
				else if (s[0] == 'C') ++Ns[3];
				else if (s[0] == 'H') ++Ns[4];
			}

			int c = 0;
			for (int i = 0; i < 5; i++) {
				if (0 < Ns[i])
					++c;
			}
			if (c < 3) {
				WriteLine(0);
				return;

			} else if (c == 3) {
				long ans = 1;
				for (int i = 0; i < 5; i++) {
					if (0 < Ns[i])
						ans *= Ns[i];
				}
				WriteLine(ans);
				return;

			}else if (c == 4) {
				long ans = 1;
				List<long> ps = new List<long>();
				for (int i = 0; i < 5; i++) {
					if (0 < Ns[i])
						ps.Add(Ns[i]);
				}
				ans = ps[0] * ps[1] * ps[2] +
					ps[0] * ps[1] * ps[3] +
					ps[0] * ps[2] * ps[3] +
					ps[1] * ps[2] * ps[3];
				WriteLine(ans);
				return;

			}else if (c == 5) {
				long ans = 1;
				List<long> ps = new List<long>();
				for (int i = 0; i < 5; i++) {
					if (0 < Ns[i])
						ps.Add(Ns[i]);
				}
				ans =
					ps[0] * ps[1] * ps[2] +
					ps[0] * ps[1] * ps[3] +
					ps[0] * ps[1] * ps[4] +
					ps[0] * ps[2] * ps[3] +
					ps[0] * ps[2] * ps[4] +
					ps[0] * ps[3] * ps[4] +
					ps[1] * ps[2] * ps[3] +
					ps[1] * ps[2] * ps[4] +
					ps[1] * ps[3] * ps[4] +
					ps[2] * ps[3] * ps[4];
				WriteLine(ans);
				return;
			}
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

		public static void ReplaceIfBigger<T>(ref T r, T v) where T : IComparable {
			if (r.CompareTo(v) < 0) r = v;
		}
		public static void ReplaceIfSmaller<T>(ref T r, T v) where T : IComparable {
			if (0 < r.CompareTo(v)) r = v;
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
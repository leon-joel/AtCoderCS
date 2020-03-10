﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DISCO2019.C
{
	public class XY : IComparable<XY>, IFormattable
	{
		public readonly int X;
		public readonly int Y;

		public XY() { }
		public XY(int x, int y) {
			X = x;
			Y = y;
		}
		public XY(int[] ary) {
			X = ary[0];
			Y = ary[1];
		}

		public int CompareTo(XY other) {
			var dx = this.X - other.X;
			if (0 < dx)
				return 1;
			else if (dx < 0)
				return -1;
			else {
				var dy = this.Y - other.Y;
				if (0 < dy)
					return 1;
				else if (dy < 0)
					return -1;
				else
					return 0;
			}
		}

		public override string ToString() {
			return ToString(null, null);
		}
		// format等の引数は一切無視
		public string ToString(string format, IFormatProvider formatProvider) {
			return $"({X}, {Y})";
		}

		public int Dist(XY other) {
			return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
		}

		public IEnumerable<XY> Neighbors() {
			yield return new XY(X - 1, Y);  // 上
			yield return new XY(X, Y + 1);  // 右
			yield return new XY(X + 1, Y);  // 下
			yield return new XY(X, Y - 1);  // 左
		}
	}

	public class Solver : SolverBase
	{
		int[,] _g;
		public void Run() {
			var ary = ReadIntArray();
			var H = ary[0];
			var W = ary[1];
			var K = ary[2];

			int kn = 0;
			List<XY> ks = new List<XY>(K);

			_g = new int[H, W];
			for (int i = 0; i < H; i++) {
				var s = ReadLine();
				for (int j = 0; j < W; j++) {
					if (s[j] == '.') {
						_g[i, j] = 0;
					} else {
						_g[i, j] = ++kn;
						ks.Add(new XY(i, j));
					}
				}
			}
			//Dump(ks);

			//とりあえず1マス延ばす
			for (int k = 0; k < K; k++) {
				var p = ks[k];

				// 上に伸ばす
				var t = p.X;
				var b = p.X;
				var l = p.Y;
				var r = p.Y;
				if (0 <= t - 1 && _g[t - 1, p.Y] == 0) {
					--t;
					_g[t, p.Y] = k + 1;
				}else if (b + 1 < H && _g[b + 1, p.Y] == 0) {
					++b;
					_g[b, p.Y] = k + 1;
				} else if (0 <= l - 1 && _g[p.X, l - 1] == 0) {
					--l;
					_g[p.X, l] = k + 1;
				} else if (r + 1 < W && _g[p.X, r + 1] == 0) {
					++r;
					_g[p.X, r] = k + 1;
				}
			}

			for (int k = 0; k < K; k++) {
				var p = ks[k];
				// 上に伸ばす
				var t = p.X;
				var b = p.X;
				var l = p.Y;
				var r = p.Y;

				if (0 <= t - 1 && _g[t - 1, p.Y] == k + 1)
					t = t - 1;
				else if (b + 1 < H && _g[b + 1, p.Y] == k + 1)
					b = b + 1;
				else if (0 <= l - 1 && _g[p.X, l - 1] == k + 1)
					l = l - 1;
				else
					r = r + 1;

				while (0 <= t - 1 && _g[t - 1, p.Y] == 0) {
					--t;
					_g[t, p.Y] = k + 1;
				}
				// 下に延ばす
				while (b + 1 < H && _g[b + 1, p.Y] == 0) {
					++b;
					_g[b, p.Y] = k + 1;
				}
				// 左に延ばす
				while (0 <= l - 1 && SetColorIfVacant(l - 1, t, b, k + 1)) {
					--l;
				}
				// 右に延ばす
				while (r + 1 < W && SetColorIfVacant(r + 1, t, b, k + 1)) {
					++r;
				}
			}
			//DumpDP(_g);
			WriteGrid(_g);
			
		}

		// y列の t行~b行 が全部空なら col を塗る
		bool SetColorIfVacant(int y, int t, int b, int col) {
			for (int x = t; x <= b; x++) {
				if (_g[x, y] != 0) return false;
			}

			for (int x = t; x <= b; x++) {
				_g[x, y] = col;
			}
			return true;
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

		public static bool ReplaceIfBigger<T>(ref T r, T v) where T : IComparable {
			if (r.CompareTo(v) < 0) {
				r = v;
				return true;
			} else {
				return false;
			}
		}
		public static bool ReplaceIfSmaller<T>(ref T r, T v) where T : IComparable {
			if (0 < r.CompareTo(v)) {
				r = v;
				return true;
			} else {
				return false;
			}
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

		virtual protected void WriteGrid<T>(T[,] dp) {
			var sb = new StringBuilder();
			var w = dp.GetLength(1);
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					sb.Append(dp[i, j]);
					if (j < w - 1)
						sb.Append(" ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
		}

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
		protected void DumpDP<T>(T[,] dp) {
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
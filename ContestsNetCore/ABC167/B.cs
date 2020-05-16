using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ABC167.B
{
	using static Util;
	using static Math;

	public class Solver : SolverBase
	{
		public void Run() {
			ReadInt4(out var A, out var B, out var C, out var K);

			if (K <= A) {
				WriteLine(K);
			} else if (K <= A + B) {
				WriteLine(A);
			} else {
				var c = K - A - B;
				WriteLine(A - c);
			}
		}

#if !MYHOME
		static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	public static class Util
	{
		public static int Gcd(int a, int b) {
			if (a < b)
				// 引数を入替えて自分を呼び出す
				return Gcd(b, a);
			while (b != 0) {
				var remainder = a % b;
				a = b;
				b = remainder;
			}
			return a;
		}
		public readonly static long MOD = 1000000007;

		public static string JoinString<T>(IEnumerable<T> array) {
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

		/// <summary>charでも対応可能なMax</summary>
		public static T Max<T>(T a, T b) where T : IComparable {
			return 0 <= a.CompareTo(b) ? a : b;
		}
		/// <summary>3要素以上に対応するMax</summary>
		public static T Max<T>(params T[] nums) where T : IComparable {
			if (nums.Length == 0) return default(T);

			T max = nums[0];
			for (int i = 1; i < nums.Length; i++) {
				max = max.CompareTo(nums[i]) > 0 ? max : nums[i];
			}
			return max;
		}
		/// <summary>charでも対応可能なMin</summary>
		public static T Min<T>(T a, T b) where T : IComparable {
			return 0 < a.CompareTo(b) ? b : a;
		}
		/// <summary>3要素以上に対応するMin</summary>
		public static T Min<T>(params T[] nums) where T : IComparable {
			if (nums.Length == 0) return default(T);

			T min = nums[0];
			for (int i = 1; i < nums.Length; i++) {
				min = min.CompareTo(nums[i]) < 0 ? min : nums[i];
			}
			return min;
		}

		///<summary>targetValueに一番近い値を返す</summary>
		public static long Nearest(long targetValue, params long[] values) {
			Debug.Assert(0 < values.Length);
			long minDiff = long.MaxValue;
			long ans = long.MaxValue;
			foreach (var v in values) {
				var diff = Math.Abs(v - targetValue);
				if (ReplaceIfSmaller(ref minDiff, diff)) {
					ans = v;
				}
			}
			return ans;
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

		///<summary>v が r より大きい場合、r に v を代入し、trueを返す。それ以外（同値の場合を含む）は何もせずfalseを返す</summary>
		public static bool ReplaceIfBigger<T>(ref T r, T v) where T : IComparable {
			if (r.CompareTo(v) < 0) {
				r = v;
				return true;
			} else {
				return false;
			}
		}
		///<summary>v が r よりが小さい場合、r に v を代入し、trueを返す。それ以外（同値の場合を含む）は何もせずfalseを返す</summary>
		public static bool ReplaceIfSmaller<T>(ref T r, T v) where T : IComparable {
			if (0 < r.CompareTo(v)) {
				r = v;
				return true;
			} else {
				return false;
			}
		}

		public static void Swap<T>(ref T a, ref T b) where T : class {
			var tmp = a;
			a = b;
			b = tmp;
		}

		/// <summary>
		/// dic[key]にadderを加算する。keyが存在しなかった場合はdic[key]=adder をセットする。
		/// </summary>
		public static void AddTo<TKey>(this Dictionary<TKey, int> dic, TKey key, int adder) {
			if (dic.ContainsKey(key)) {
				dic[key] += adder;
			} else {
				dic[key] = adder;
			}
		}

		/// <summary>
		/// 文字列 s が chars に含まれる文字を含んでいるか？
		/// </summary>
		public static bool ContainsAny(this string s, char[] chars) {
			for (int j = 0; j < s.Length; j++) {
				if (chars.Contains(s[j])) return true;
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

		/// <summary>
		/// nの 2進数における下からd(0-indexed)桁目のbitが立っている？
		/// </summary>
		public static int GetBit(long n, int d) {
			if (0 == (n & (1L << d)))
				return 0;
			else
				return 1;
		}
	}

	public class SolverBase
	{
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected string ReadString() => ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
		virtual protected void ReadString2(out string a, out string b) {
			var ary = ReadStringArray();
			a = ary[0];
			b = ary[1];
		}
		virtual protected void ReadString3(out string a, out string b, out string c) {
			var ary = ReadStringArray();
			a = ary[0];
			b = ary[1];
			c = ary[2];
		}
		virtual protected char[] ReadCharArray() => ReadLine().Split(' ').Select<string, char>(s => s[0]).ToArray();
		virtual protected int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		virtual protected void ReadInt2(out int a, out int b) {
			var ary = ReadIntArray();
			a = ary[0];
			b = ary[1];
		}
		virtual protected void ReadInt3(out int a, out int b, out int c) {
			var ary = ReadIntArray();
			a = ary[0];
			b = ary[1];
			c = ary[2];
		}
		virtual protected void ReadInt4(out int a, out int b, out int c, out int d) {
			var ary = ReadIntArray();
			a = ary[0];
			b = ary[1];
			c = ary[2];
			d = ary[3];
		}
		virtual protected long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		virtual protected void ReadLong2(out long a, out long b) {
			var ary = ReadLongArray();
			a = ary[0];
			b = ary[1];
		}
		virtual protected void ReadLong3(out long a, out long b, out long c) {
			var ary = ReadLongArray();
			a = ary[0];
			b = ary[1];
			c = ary[2];
		}
		virtual protected void ReadLong4(out long a, out long b, out long c, out long d) {
			var ary = ReadLongArray();
			a = ary[0];
			b = ary[1];
			c = ary[2];
			d = ary[3];
		}
		virtual protected double[] ReadDoubleArray() => ReadLine().Split(' ').Select<string, double>(s => double.Parse(s)).ToArray();

		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine(double d) => Console.WriteLine($"{d:F9}");
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);
		virtual protected void WriteGrid(IEnumerable<IEnumerable<char>> arrayOfArray) {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				foreach (var c in a) {
					sb.Append(c);
				}
				sb.AppendLine();
			}
			WriteLine(sb.ToString());
		}

		[Conditional("DEBUG")]
		protected void Dump(double d) => Console.WriteLine($"{d:F9}");
		[Conditional("DEBUG")]
		protected void Dump<T>(T x) => Console.WriteLine(x);
		[Conditional("DEBUG")]
		protected void DumpArray<T>(IEnumerable<T> array) {
			string s = Util.JoinString(array);
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(s);
			//_writer.WriteLine(s);
		}
		[Conditional("DEBUG")]
		protected void DumpGrid(IEnumerable<IEnumerable<char>> arrayOfArray) {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				foreach (var c in a) {
					sb.Append(c);
				}
				sb.AppendLine();
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}
		[Conditional("DEBUG")]
		protected void DumpGrid<T>(IEnumerable<IEnumerable<T>> arrayOfArray) {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				sb.AppendLine(Util.JoinString(a));
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}
		[Conditional("DEBUG")]
		protected void DumpGrid(bool[,] grid) {
			var sb = new StringBuilder();
			for (int i = 0; i < grid.GetLength(0); i++) {
				for (int j = 0; j < grid.GetLength(1); j++) {
					sb.Append(grid[i, j] ? "x " : ". ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
		}
		[Conditional("DEBUG")]
		protected void DumpGrid(char[,] grid) {
			var sb = new StringBuilder();
			for (int i = 0; i < grid.GetLength(0); i++) {
				for (int j = 0; j < grid.GetLength(1); j++) {
					sb.Append(grid[i, j]);
					sb.Append(" ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
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
		[Conditional("DEBUG")]
		protected void DumpDP3_Keta<T>(T[,,] dp) where T : IFormattable {
			var sb = new StringBuilder();
			for (int i = 0; i < dp.GetLength(0); i++) {
				sb.Append($"{i,2}: ");
				for (int j = 0; j < dp.GetLength(1); j++) {
					sb.Append($"{dp[i, j, 0]}-{dp[i, j, 1]}");
					sb.Append(", ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
		}
	}
}

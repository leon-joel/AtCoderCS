using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ABC154.C
{
	using static Util;

	public class Modulo
	{
		private readonly static int M = 1000000007;

		/// <summary>(a * b) % M</summary>
		public static int Mul(int a, int b) {
			return (int)(Math.BigMul(a, b) % M);
		}

		/// <summary>(aのm乗) % M</summary>
		/// <see cref="https://www.youtube.com/watch?v=gdQxKESnXKs のD問題"/>
		public static int Pow(int a, int m) {
			switch (m) {
				case 0:
					return 1;
				case 1:
					return a % M;
				default:
					int p1 = Pow(a, m / 2);
					int p2 = Mul(p1, p1);
					return ((m % 2) == 0) ? p2 : Mul(p2, a);
			}
		}

		/// <summary>
		/// (a / b) % M
		/// ※フェルマーの小定理による
		/// </summary>
		/// <see cref="https://www.youtube.com/watch?v=gdQxKESnXKs のD問題"/>
		public static int Div(int a, int b) {
			return Mul(a, Pow(b, M - 2));
		}

		/// <summary>
		/// コンストラクタ ※n! % M および nCr % M を使用する場合には必要
		/// </summary>
		private readonly int[] m_facs;
		public Modulo(int n) {
			// x が n までの、x! % M の結果を配列に保持する
			m_facs = new int[n + 1];
			m_facs[0] = 1;
			for (int i = 1; i <= n; ++i) {
				m_facs[i] = Mul(m_facs[i - 1], i);
			}
		}

		/// <summary>n! % M</summary>
		public int Fac(int n) {
			return m_facs[n];
		}
		/// <summary>
		/// 組み合わせ nCr % M
		/// </summary>
		/// <see cref="https://www.youtube.com/watch?v=gdQxKESnXKs のD問題"/>
		public int Ncr(int n, int r) {
			if (n < r) { return 0; }
			if (n == r) { return 1; }
			int res = Fac(n);
			res = Div(res, Fac(r));
			res = Div(res, Fac(n - r));
			return res;
		}
	}

	public class Solver : SolverBase
	{
		public long NCM(long n, long m) {
			if (m == 0) return 1;
			if (n == 0) return 0;
			return n * NCM(n - 1, m - 1) / m;
		}

		public static long Combination(long n, long r) {
			if (n < 0 || r < 0 || r > n) throw new ArgumentException("不正な引数です．");

			if (n - r < r) r = n - r;
			if (r == 0) return 1;
			if (r == 1) return n;

			var numerator = new long[r];
			var denominator = new long[r];

			for (long k = 0; k < r; k++) {
				numerator[k] = n - r + k + 1;
				denominator[k] = k + 1;
			}

			for (long p = 2; p <= r; p++) {
				var pivot = denominator[p - 1];
				if (pivot > 1) {
					var offset = (n - r) % p;
					for (long k = p - 1; k < r; k += p) {
						numerator[k - offset] /= pivot;
						denominator[k] /= pivot;
					}
				}
			}

			long result = 1;
			for (long k = 0; k < r; k++) {
				if (numerator[k] > 1) result *= numerator[k];
			}

			return result;
		}


		public void Run() {
			var N = ReadInt();

			//var cord = new int[10,10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10];
			var cord = new Dictionary<string, int>();

			for (int i = 0; i < N; i++) {
				var t = new int[26];
				var s = ReadLine();
				for (int j = 0; j < 10; j++) {
					t[s[j] - 'a'] += 1;
				}
				//Dump(cnum);
				//cord[t[0], t[1], t[2], t[3], t[4], t[5], t[6], t[7], t[8], t[9], t[10], t[11], t[12], t[13], t[14], t[15], t[16], t[17], t[18], t[19], t[20], t[21], t[22], t[23], t[24], t[25]] += 1;

				string vs = "";
				for (int j = 0; j < 26; j++) {
					vs += (char)('a' + t[j]);
				}

				if (cord.ContainsKey(vs)) {
					cord[vs] += 1;
				} else {
					cord.Add(vs, 1);
				}
			}

			long ans = 0;
			//var m = new Modulo(100000);
			foreach (var kv in cord) {
				if (1 < kv.Value) {
					//ans += m.Ncr(kv.Value, 2);
					//ans += NCM(kv.Value, 2);
					ans += Combination(kv.Value, 2);
				}
			}

			WriteLine(ans);
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Numerics;

namespace ABC057.C
{
	using static Util;
	public static class PrimeNumber
	{
		/// <summary>
		/// 素数判定 【素朴な実装版】 ※拡張用に
		/// </summary>
		static bool IsPrimeNumber(long num) {
			if (num < 2) return false;

			for (long i = 2; i * i <= num; i++) {
				if (num % i == 0) return false;
			}
			return true;
		}


		static long[] seedPrimes = {
		      /*1,2,3,4, 5, 6, 7  8, 9,10,11,12,13,14,15,*/
			2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97
			};
		/// <summary>
		/// 素数判定 【高速版】
		/// ★ System.Numerics.dll 必要 ※AtCoderは標準でloadされる
		/// * using System.Numerics 必要
		/// </summary>
		public static bool IsPrime(long num) {
			if (num == 1)
				return false;
			if (seedPrimes.Contains(num))
				return true;
			if (seedPrimes.Any(x => num % x == 0))
				return false;

			return (num < 2000000) ? IsPrimeBruteforce(num) : IsPrimeMillarRrabin(num);
		}

		/// <summary>
		/// 素因数分解 ※1は返さない
		/// </summary>
		public static IEnumerable<long> Factoring(long n) {
			while (1 < n) {
				long factor = GetFactor(n);
				yield return factor;
				n /= factor;
			}
		}
		private static long GetFactor(long n, int seed = 1) {
			if (n % 2 == 0)
				return 2;
			if (IsPrime(n))
				return n;
			long x = 2;
			long y = 2;
			long d = 1;
			long count = 0;
			while (d == 1) {
				count++;
				x = f(x, n, seed);
				y = f(f(y, n, seed), n, seed);
				d = Gcd(Math.Abs(x - y), n);
			}
			if (d == n)
				// 見つからなかった、乱数発生のシードを変えて再挑戦。
				return GetFactor(n, seed + 1);
			// 素数でない可能性もあるので、再度呼び出す
			return GetFactor(d);
		}
		private static long f(long x, long n, int seed) {
			return (seedPrimes[seed % 6] * x + seed) % n;
		}
		private static long Gcd(long a, long b) {
			if (a < b)
				return Gcd(b, a);  // 引数を入替えて自分を呼び出す
			if (b == 0)
				return a;
			long d = 0;
			do {
				d = a % b;
				a = b;
				b = d;
			} while (d != 0);
			return a;
		}

		private static bool IsPrimeBruteforce(long num) {
			if (num == 1)
				return false;
			if (num != 2 && num % 2 == 0)
				return false;
			if (num != 3 && num % 3 == 0)
				return false;
			if (num != 5 && num % 5 == 0)
				return false;
			long i = 0;
			while (true) {
				foreach (var p in seedPrimes.Skip(3).Take(8)) {
					// 30m+2, 30m+3, 30m+4, 30m+5, 30m+6、30m+8、30m+9、30m+12... は割る必要はない。
					var primeCandidte = p + i;
					if (primeCandidte > Math.Sqrt(num))
						return true;
					if (num % (primeCandidte) == 0)
						return false;
				}
				i += 30;
			}
		}

		private static bool IsPrimeMillarRrabin(long num) {
			if (num <= 1)
				return false;
			if ((num & 1) == 0)
				return num == 2;

			if (num < 100 && seedPrimes.Contains((int)num))
				return true;

			var WitnessMax = GetWitnessMax(num);

			long d = num - 1;
			long s = 0;
			while ((d & 1) == 0) {
				s++;
				d >>= 1;
			}
			foreach (var w in seedPrimes.Take(WitnessMax)) {
				if (!MillarRrabin(num, s, d, w))
					return false;
			}
			return true;
		}


		private static int GetWitnessMax(long num) {
			if (num < 2047)
				return 1;
			if (num < 1373653)
				return 2;
			if (num < 25326001)
				return 3;
			if (num < 3215031751)
				return 4;
			if (num < 2152302898747)
				return 5;
			if (num < 3474749660383)
				return 6;
			if (num < 341550071728321)
				return 7;
			if (num < 3825123056546413051)
				return 9;
			return 12;
		}

		private static bool MillarRrabin(long num, long s, long d, long witness) {
			long x = ModPow(witness, d, num);
			if (x == 1)
				return true;
			for (long r = 0; r < s; r++) {
				if (x == num - 1)
					return true;
				BigInteger rem;
				BigInteger.DivRem(BigInteger.Multiply(x, x), num, out rem);
				x = (long)(rem);
			}
			return false;
		}

		private static long ModPow(long baseValue, long exponent, long modulus) {
			return (long)BigInteger.ModPow(baseValue, exponent, modulus);
		}
	}

	public class Solver : SolverBase
	{
		public void Run() {
			var N = ReadLong();
			// 素因数分解
			List<long> primes = new List<long>();
			foreach (var p in PrimeNumber.Factoring(N)) {
				primes.Add(p);
			}

			// bit全探索
			// bit数
			var n = primes.Count;

			int ans = int.MaxValue;

			// bit mask の生成
			// 2^n     = 1 << n
			for (ulong mask = 0; mask <= ((1ul << n) - 1); mask++) {
				// 各bitを下から舐める
				long ac = 1;
				for (int i = 0; i < n; i++) {
					//iビット目が立っている？
					if (0 != (mask & (1ul << i))) {
						var p = primes[i];
						ac *= p;
					}
				}

				var ac2 = N / ac;
				var digit = Math.Max(ac.ToString().Length, ac2.ToString().Length);
				ReplaceIfSmaller(ref ans, digit);
			}

			WriteLine(ans);
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
		virtual protected string ReadString() => ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		//virtual protected void ReadInt2(out int x, out int y) {
		//	var aryS = ReadLine().Split(' ');
		//	x = int.Parse(aryS[0]);
		//	y = int.Parse(aryS[1]);
		//}
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
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
		protected void Dump(string s) => Console.WriteLine(s);
		[Conditional("DEBUG")]
		protected void Dump(char c) => Console.WriteLine(c);
		[Conditional("DEBUG")]
		protected void Dump(int x) => Console.WriteLine(x);
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
	}
}

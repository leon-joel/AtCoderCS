using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Tools
{
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
		/// 素因数分解
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
}

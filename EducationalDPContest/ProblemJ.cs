using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// J - Sushi : 期待値DP
// https://atcoder.jp/contests/dp/tasks/dp_j
// https://qiita.com/drken/items/03c7db44ccd27820ea0d
namespace EducationalDPContest.J
{
	using static Util;

	// メモ化再帰による解法
	public class Solver : SolverBase
	{
		double[,,] DP;
		int N;
		public void Run() {
			N = ReadInt();
			var Nums = ReadIntArray();

			// DP[寿司1個の皿数i][寿司2個j][寿司3個k]
			//   = 寿司完食までのサイコロ回数期待値
			// DP[0][0][0] = 0

			// DP[i][j][k]からサイコロを1回振ったときの遷移
			// -> 0個皿に当たる: 遷移先 DP[i][j][k]     発生確率: (N-i-j-k) / N
			// -> 1個皿に当たる: 遷移先 DP[i-1][j][k]   発生確率: i/N
			// -> 2個皿に当たる: 遷移先 DP[i+1][j-1][k] 発生確率: j/N
			// -> 3個皿に当たる: 遷移先 DP[i][j+1][k-1] 発生確率: k/N
			// ※『X個皿が減ってX-1個皿が増える』という遷移をする点に注意！

			// したがって、
			// DP[i][j][k]の期待値は上記の和 + 1となる。
			// ただし、DP[i][j][k] が両辺に存在する（自己ループ、自己参照 が存在する）ので、
			// 式変形して自己ループを除去する。

			int cnt1 = 0, cnt2 = 0, cnt3 = 0;
			for (int i = 0; i < N; i++) {
				switch (Nums[i]) {
					case 1: ++cnt1; break;
					case 2: ++cnt2; break;
					case 3: ++cnt3; break;
				}
			}

			DP = new double[N+1, N+1, N+1];
			InitDP3(DP, -1.0);

			var ans = Recurse(cnt1, cnt2, cnt3);
			WriteLine(ans);
		}

		double Recurse(int c1, int c2, int c3) {
			if (c1 == 0 && c2 == 0 && c3 == 0) return 0.0;
			if (0.0 <= DP[c1, c2, c3]) return DP[c1, c2, c3];

			double res = 0.0;
			if (0 < c1) res += Recurse(c1 - 1, c2, c3) * c1;
			if (0 < c2) res += Recurse(c1 + 1, c2 - 1, c3) * c2;
			if (0 < c3) res += Recurse(c1, c2 + 1, c3 - 1) * c3;
			res += N;

			res /= c1 + c2 + c3;

			return DP[c1, c2, c3] = res;
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

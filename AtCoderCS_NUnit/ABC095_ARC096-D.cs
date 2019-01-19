using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// 円周上の移動距離問題
// https://atcoder.jp/contests/abc095/tasks/arc096_b
namespace ABC095_ARC096.D
{
	using static Util;

	public class Solver : SolverBase
	{
		public void Run() {
			var ary = ReadLongArray();
			int N = (int)ary[0];
			long C = ary[1];

			var AryS = new long[N][];
			for (int i = 0; i < N; i++) {
				AryS[i] = ReadLongArray();
			}

			// 時計回り側
			// CW[i] = maxV
			// i番目までの寿司を食べられるとしたときに得られる最大のカロリーmaxVの配列
			var CW = new long[N];
			long lastPos = 0;
			long maxCal = 0;
			long curCal = 0;
			for (int i = 0; i < N; i++) {
				var x = AryS[i][0];
				var v = AryS[i][1];
				// 追加カロリー = 寿司カロリー - 消費カロリー（距離）
				long addCal = v - (x - lastPos);
				lastPos = x;
				// iまでに得られるカロリー
				curCal += addCal;

				// iまでに得られる最大カロリーを更新
				if (maxCal < curCal) {
					maxCal = curCal;
				}

				CW[i] = maxCal;
			}
			// 反時計回り側
			// CCW[i] = maxV
			// 反時計回りにi番目までの寿司を食べられるとしたときに得られる最大のカロリーmaxVの配列
			var CCW = new long[N];
			lastPos = C;
			maxCal = 0;
			curCal = 0;
			for (int i = 0; i < N; i++) {
				var x = AryS[N-i-1][0];
				var v = AryS[N-i-1][1];
				// 追加カロリー = 寿司カロリー - 消費カロリー（距離）
				long addCal = v - (lastPos - x);
				lastPos = x;
				// iまでに得られるカロリー
				curCal += addCal;

				// iまでに得られる最大カロリーを更新
				if (maxCal < curCal) {
					maxCal = curCal;
				}

				CCW[i] = maxCal;
			}

			// 反時計回りに全探索 ※最初に反時計回りに進む場合
			lastPos = C;
			curCal = 0;
			maxCal = 0;
			// 0個〜N-1個 取るパターンを検討 ※N個＝全部とる場合は考えない。それは帰ってこなくていいパターンなので、CW first 検討に含まれる
			for (int i = 0; i <= N-1; i++) {
				if (0 < i) {
					var x = AryS[N-i][0];
					var v = AryS[N-i][1];
					// 追加カロリー = 寿司カロリー - 消費カロリー（距離 * 2)
					long addCal = v - (lastPos - x) * 2;
					lastPos = x;
					// iまでに得られるカロリー
					curCal += addCal;
				}
				// 反時計回り側の現在カロリー ＋ 時計回り側で得られる最大カロリー
				if (maxCal < curCal + CW[N - 1 - i])
					maxCal = curCal + CW[N - 1 - i];
			}

			// 時計回りに全探索 ※最初に時計回りに進む場合
			lastPos = 0;
			curCal = 0;
			// 0個〜N-1個 取るパターンを検討 ※N個＝全部とる場合は考えない。それは帰ってこなくていいパターンなので、CCW first 検討に含まれる
			for (int i = 0; i <= N - 1; i++) {
				if (0 < i) {
					var x = AryS[i-1][0];
					var v = AryS[i-1][1];
					// 追加カロリー = 寿司カロリー - 消費カロリー（距離 * 2)
					long addCal = v - (x - lastPos) * 2;
					lastPos = x;
					// iまでに得られるカロリー
					curCal += addCal;
				}
				// 時計回り側の現在カロリー ＋ 反時計回り側で得られる最大カロリー
				if (maxCal < curCal + CCW[N - 1 - i])
					maxCal = curCal + CCW[N - 1 - i];
			}

			WriteLine(maxCal);
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

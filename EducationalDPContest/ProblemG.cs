using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// Educational DP Contest
// https://atcoder.jp/contests/dp/tasks
namespace EducationalDPContest.G
{
	using static Util;

	// メモ化再帰による解法
	// https://qiita.com/drken/items/03c7db44ccd27820ea0d
	// 『DP の更新順序が非自明』な場合にはメモ化再帰が大きなメリットを生むという一例
	// ※明示的なトポロジカルソートが不要になる
	public class Solver : SolverBase
	{
		int[] DP;
		List<int>[] Edges;
		public void Run() {
			var ary = ReadIntArray();
			var N = ary[0];
			var M = ary[1];

			// Listの配列 に x -> y を格納していく
			Edges = new List<int>[N];
			for (int i = 0; i < N; i++) {
				Edges[i] = new List<int>();
			}
			for (int i = 0; i < M; i++) {
				var xy = ReadIntArray();
				var x = xy[0] - 1;
				var y = xy[1] - 1;

				Edges[x].Add(y);
			}

			DP = new int[N + 1];
			InitArray(DP, -1);

			// 全ノードをメモ化再帰で回して、最長を更新していく
			int maxLen = 0;
			for (int x = 0; x < N; x++) {
				var len = Recurse(x);
				if (maxLen < len) maxLen = len;
			}
			WriteLine(maxLen);
		}

		// xからの最長経路長を返す
		int Recurse(int x) {
			if (DP[x] != -1) return DP[x];

			// x->y の y でループ再帰
			int maxLen = 0;
			foreach (var y in Edges[x]) {
				var len = Recurse(y) + 1;
				if (maxLen < len) maxLen = len;
			}

			// メモしながら返す
			return DP[x] = maxLen;
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	// BFS 式にトポロジカルソートしながら DP
	// https://qiita.com/drken/items/03c7db44ccd27820ea0d
	public class Solver2 : SolverBase
	{
		public void Run() {
			var ary = ReadIntArray();
			var N = ary[0];
			var M = ary[1];

			// [y] = 当該頂点への入次数（頂点に入ってくる数）
			int[] indegrees = new int[N];
			// Listの配列 に x -> y を格納していく
			List<int>[] Edges = new List<int>[N];
			for (int i = 0; i < N; i++) {
				Edges[i] = new List<int>();
			}
			for (int i = 0; i < M; i++) {
				var xy = ReadIntArray();
				var x = xy[0] - 1;
				var y = xy[1] - 1;

				Edges[x].Add(y);
				indegrees[y]++;
			}

			// BFSの起点（入次数==0 の頂点９
			Queue<int> que = new Queue<int>();
			for (int i = 0; i < N; i++) {
				if (indegrees[i] == 0)
					que.Enqueue(i);
			}

			// 当該頂点までの最長パス長 ※初期値はすべて0
			int[] DP = new int[N + 1];

			// BFS
			while (0 < que.Count) {
				var x = que.Dequeue();

				foreach (var y in Edges[x]) {
					// 入次数を減らし
					indegrees[y]--;

					if (indegrees[y] == 0) {
						// yへの入次数が0なら、yをqueueに入れる
						que.Enqueue(y);

						// yへの入次数が0になった＝yまでの最長距離が確定
						DP[y] = DP[x] + 1;
					}
				}
			}

			int maxLen = 0;
			for (int i = 0; i < N; i++) {
				if (maxLen < DP[i]) maxLen = DP[i];
			}
			WriteLine(maxLen);
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

		public static void InitDP<T>(T[,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					dp[i, j] = value;
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

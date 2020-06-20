using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;

// ここから一番下までを提出コードの一番上にコピペする。
// もしくはnamespace内のコードを提出コードのnamespace内にコピペする。
using MySet;
namespace MySet
{
	/// <summary>
	/// C-like multiset
	/// ・要素は昇順に並ぶ
	/// ・同値を複数格納可
	/// ・ほぼすべての操作が O(logN) ※Count()は O(1)
	/// </summary>
	public class MultiSet<T> : Set<T> where T : IComparable
	{
		public override void Insert(T v) {
			if (_root == null) _root = new SB_BinarySearchTree<T>.Node(v);
			else _root = SB_BinarySearchTree<T>.Insert(_root, v);
		}
	}

	/// <summary>
	/// C-like set
	/// ・要素は昇順に並ぶ
	/// ・ほぼすべての操作が O(logN) ※Count()は O(1)
	/// </summary>
	public class Set<T> where T : IComparable
	{
		protected SB_BinarySearchTree<T>.Node _root;

		public T this[int idx] { get { return ElementAt(idx); } }

		///<summary>先頭要素の値（最小値）を返す ※見つからなかったら例外がthrowされる</summary>
		public T First() => SB_BinarySearchTree<T>.First(_root).Value;
		public T Min() => First();
		///<summary>末尾要素の値（最大値）を返す ※見つからなかったら例外がthrowされる</summary>
		public T Last() => SB_BinarySearchTree<T>.Last(_root).Value;
		public T Max() => Last();

		public int Count => SB_BinarySearchTree<T>.Count(_root);

		public virtual void Insert(T v) {
			if (_root == null) _root = new SB_BinarySearchTree<T>.Node(v);
			else {
				if (SB_BinarySearchTree<T>.Find(_root, v) != null) return;
				_root = SB_BinarySearchTree<T>.Insert(_root, v);
			}
		}

		public void Clear() {
			_root = null;
		}

		///<summary>指定値要素を1つだけ削除 ★C++のmultisetは指定値要素をすべて削除する</summary>
		public void Remove(T v) {
			_root = SB_BinarySearchTree<T>.Remove(_root, v);
		}

		public bool Contains(T v) {
			return SB_BinarySearchTree<T>.Contains(_root, v);
		}
		/// <summary>Indexによる取り出し ※見つからない場合は IndexOutOfRangeException がthrowされる</summary>
		public T ElementAt(int idx) {
			var node = SB_BinarySearchTree<T>.FindByIndex(_root, idx);
			if (node == null) throw new IndexOutOfRangeException();
			return node.Value;
		}
		///<summary>指定値要素の格納数</summary>
		public int CountByValue(T v) {
			return SB_BinarySearchTree<T>.UpperBound(_root, v) - SB_BinarySearchTree<T>.LowerBound(_root, v);
		}

		///<summary>前に挿入する場合のidxが返される</summary>
		public int LowerBound(T v) {
			return SB_BinarySearchTree<T>.LowerBound(_root, v);
		}
		///<summary>後ろに挿入する場合のidxが返される</summary>
		public int UpperBound(T v) {
			return SB_BinarySearchTree<T>.UpperBound(_root, v);
		}

		public Tuple<int, int> EqualRange(T v) {
			if (!Contains(v)) return new Tuple<int, int>(-1, -1);
			return new Tuple<int, int>(SB_BinarySearchTree<T>.LowerBound(_root, v), SB_BinarySearchTree<T>.UpperBound(_root, v) - 1);
		}

		public List<T> ToList() {
			return new List<T>(SB_BinarySearchTree<T>.Enumerate(_root));
		}
	}

	/// <summary>
	/// Self-Balancing Binary Search Tree
	/// (using Randamized BST)
	/// </summary>
	/// <remarks>Thanks to http://yambe2002.hatenablog.com/entry/2017/02/07/122421 </remarks>
	public class SB_BinarySearchTree<T> where T : IComparable
	{
		public class Node
		{
			public T Value;
			public Node LChild;
			public Node RChild;
			public int Count;     //size of the sub tree

			public Node(T v) {
				Value = v;
				Count = 1;
			}
		}

		static readonly Random _rnd = new Random();

		public static int Count(Node t) {
			return t == null ? 0 : t.Count;
		}

		static Node Update(Node t) {
			t.Count = Count(t.LChild) + Count(t.RChild) + 1;
			return t;
		}

		public static Node Merge(Node l, Node r) {
			if (l == null || r == null) return l == null ? r : l;

			if ((double)Count(l) / (double)(Count(l) + Count(r)) > _rnd.NextDouble()) {
				l.RChild = Merge(l.RChild, r);
				return Update(l);
			} else {
				r.LChild = Merge(l, r.LChild);
				return Update(r);
			}
		}

		/// <summary>
		/// split as [0, k), [k, n)
		/// </summary>
		public static (Node left, Node right) Split(Node t, int k) {
			if (t == null) return (null, null);
			if (k <= Count(t.LChild)) {
				var s = Split(t.LChild, k);
				t.LChild = s.right;
				return (s.left, Update(t));
			} else {
				var s = Split(t.RChild, k - Count(t.LChild) - 1);
				t.RChild = s.left;
				return (Update(t), s.right);
			}
		}

		public static Node Remove(Node t, T v) {
			if (Find(t, v) == null) return t;
			return RemoveAt(t, LowerBound(t, v));
		}

		public static Node RemoveAt(Node t, int k) {
			var s = Split(t, k);
			var s2 = Split(s.Item2, 1);
			return Merge(s.Item1, s2.Item2);
		}

		public static bool Contains(Node t, T v) {
			return Find(t, v) != null;
		}

		public static Node Find(Node t, T v) {
			while (t != null) {
				var cmp = t.Value.CompareTo(v);
				if (cmp > 0) t = t.LChild;
				else if (cmp < 0) t = t.RChild;
				else break;
			}
			return t;
		}

		public static Node FindByIndex(Node t, int idx) {
			if (t == null) return null;

			var currentIdx = Count(t) - Count(t.RChild) - 1;
			while (t != null) {
				if (currentIdx == idx) return t;
				if (currentIdx > idx) {
					t = t.LChild;
					currentIdx -= (Count(t == null ? null : t.RChild) + 1);
				} else {
					t = t.RChild;
					currentIdx += (Count(t == null ? null : t.LChild) + 1);
				}
			}

			return null;
		}

		public static Node First(Node root) {
			while (root != null) {
				if (root.LChild == null) return root;
				else root = root.LChild;
			}
			return null;
		}
		public static Node Last(Node root) {
			while (root != null) {
				if (root.RChild == null) return root;
				else root = root.RChild;
			}
			return null;
		}

		public static int UpperBound(Node t, T v) {
			var torg = t;
			if (t == null) return -1;

			var ret = Int32.MaxValue;
			var idx = Count(t) - Count(t.RChild) - 1;
			while (t != null) {
				var cmp = t.Value.CompareTo(v);

				if (cmp > 0) {
					ret = Math.Min(ret, idx);
					t = t.LChild;
					idx -= (Count(t == null ? null : t.RChild) + 1);
				} else if (cmp <= 0) {
					t = t.RChild;
					idx += (Count(t == null ? null : t.LChild) + 1);
				}
			}
			return ret == Int32.MaxValue ? Count(torg) : ret;
		}

		public static int LowerBound(Node t, T v) {
			var torg = t;
			if (t == null) return -1;

			var idx = Count(t) - Count(t.RChild) - 1;
			var ret = Int32.MaxValue;
			while (t != null) {
				var cmp = t.Value.CompareTo(v);
				if (cmp >= 0) {
					if (cmp == 0) ret = Math.Min(ret, idx);
					t = t.LChild;
					if (t == null) ret = Math.Min(ret, idx);
					idx -= t == null ? 0 : (Count(t.RChild) + 1);
				} else if (cmp < 0) {
					t = t.RChild;
					idx += (Count(t == null ? null : t.LChild) + 1);
					if (t == null) return idx;
				}
			}
			return ret == Int32.MaxValue ? Count(torg) : ret;
		}

		public static Node Insert(Node t, T v) {
			var ub = LowerBound(t, v);
			return InsertByIdx(t, ub, v);
		}

		static Node InsertByIdx(Node t, int k, T v) {
			var s = Split(t, k);
			return Merge(Merge(s.Item1, new Node(v)), s.Item2);
		}

		public static IEnumerable<T> Enumerate(Node t) {
			var ret = new List<T>();
			Enumerate(t, ret);
			return ret;
		}

		static void Enumerate(Node t, List<T> ret) {
			if (t == null) return;
			Enumerate(t.LChild, ret);
			ret.Add(t.Value);
			Enumerate(t.RChild, ret);
		}
	}
}

namespace ABC170.E
{
	using static Util;
	using static Math;

	struct CI
	{
		public int Rate;
		public int Garden;

		public CI(int rate, int garden) {
			Rate = rate;
			Garden = garden;
		}
	}
	public class Solver : SolverBase
	{

		public void Run() {
			ReadInt2(out var N, out var Q);
			// 児童リスト cs[園児idx] = {レート, 園番号}
			var cs = new CI[N];
			// 園ごとのレート集合 [園番号] = multiset<レート>
			var gs = new MultiSet<int>[200005];
			// 各園の最強園児達の集合: multiset<レート>
			var maxs = new MultiSet<int>();

			#region ローカル関数
			// ■転園時の処理
			// 現在所属園idx(from園)を取得
			// 所属園情報を転園先園idx(to園)に更新

			// from園の最強レートを maxs からいったん削除する
			// from園から当該幼児Rateを削除する
			// from園の最強レートを maxs に追加する
			void delEnji(int i) {
				var ci = cs[i];
				maxs.Remove(gs[ci.Garden].Max());
				gs[ci.Garden].Remove(ci.Rate);
				if (0 < gs[ci.Garden].Count) {
					maxs.Insert(gs[ci.Garden].Max());
				}
			};
			// to園  の最強レートを maxs からいったん削除する
			// to園  に当該幼児idxを追加する
			// to園  の最強レートを maxs に追加する
			void addEnji(int i) {
				var ci = cs[i];
				if (gs[ci.Garden] == null) {
					gs[ci.Garden] = new MultiSet<int>();
				}
				var g = gs[ci.Garden];
				if (0 < g.Count) {
					maxs.Remove(gs[ci.Garden].Max());
				}
				gs[ci.Garden].Insert(ci.Rate);
				maxs.Insert(gs[ci.Garden].Max());
			};
			#endregion

			for (int i = 0; i < N; i++) {
				ReadInt2(out var rate, out var gidx);
				cs[i] = new CI(rate, gidx);	// new ではなくバラで代入しても速度は全然変わらない
				//cs[i].Rate = rate;
				//cs[i].Garden = gidx;
				addEnji(i);
			}

			for (int i = 0; i < Q; i++) {
				ReadInt2(out var C, out var D);
				--C;    //園児0-indexed
				delEnji(C);
				cs[C].Garden = D;
				addEnji(C);

				var ans = maxs.First();
				WriteLine(ans);
			}
		}

#if !MYHOME
		static void Main(string[] args) {
			//var sw = new StreamWriter(Console.OpenStandardInput()) {
			//	AutoFlush = false,
			//};
			//Console.SetOut(sw);
			new Solver().Run();
			Console.Out.Flush();
		}
#endif
	}

	public static class Util
	{
		/// <summary>反転した新しいstringを返す</summary>
		public static string ReverseStr(this string s) {
			return new string(s.Reverse().ToArray());
		}

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
		public static void AddTo<TKey>(this IDictionary<TKey, int> dic, TKey key, int adder) {
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

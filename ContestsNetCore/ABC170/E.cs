using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;

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
			var sc = new Scanner();
			var N = sc.NextInt();
			var Q = sc.NextInt();
			// 児童リスト cs[園児idx] = {レート, 園番号}
			var cs = new CI[N];
			// 園ごとのレート集合 [園番号] = Set<CI>
			var gs = new Set<CI>[200005];
			for (int i = 0; i < 200005; i++) {
				// 降順に並ぶようにラムダをセットする
				gs[i] = new Set<CI>((l, r) => {
					if (l.Rate != r.Rate)
						return r.Rate.CompareTo(l.Rate);
					else 
						return l.Garden.CompareTo(r.Garden);
				});
			}

			// 各園の最強園児RateをSegTreeに格納 ※minをO(logN)で取得
			var maxs = new SegmentTree<int>(200005, Math.Min, int.MaxValue);

			#region ローカル関数
			// ■転園時の処理
			// 現在所属園idx(from園)を取得
			// 所属園情報を転園先園idx(to園)に更新

			// from園から当該幼児Rateを削除する
			// maxsのfrom園最強レートを更新する
			void delEnji(int i) {
				var ci = cs[i];
				gs[ci.Garden].Remove(ci);
				maxs[ci.Garden] = gs[ci.Garden].Count == 0 ? int.MaxValue : gs[ci.Garden][0].Rate;
			};
			// to園  に当該幼児idxを追加する
			// maxsのto園最強レートを更新する
			void addEnji(int i) {
				var ci = cs[i];
				gs[ci.Garden].Add(ci);
				maxs[ci.Garden] = gs[ci.Garden].Count == 0 ? int.MaxValue : gs[ci.Garden][0].Rate;
			};
			#endregion

			for (int i = 0; i < N; i++) {
				var rate = sc.NextInt();
				var gidx = sc.NextInt();
				cs[i] = new CI(rate, gidx);	// new ではなくバラで代入しても速度は全然変わらない
				//cs[i].Rate = rate;
				//cs[i].Garden = gidx;
				addEnji(i);
			}

			for (int i = 0; i < Q; i++) {
				var C = sc.NextInt() - 1;   //園児0-indexed
				var D = sc.NextInt();
				delEnji(C);
				cs[C].Garden = D;
				addEnji(C);

				var ans = maxs.Query();
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

	public class Set<T>
	{
		Node root;
		readonly IComparer<T> comparer;
		readonly Node nil;
		public bool IsMultiSet { get; set; }
		public Set(IComparer<T> comparer) {
			nil = new Node(default(T));
			root = nil;
			this.comparer = comparer;
		}
		public Set(Comparison<T> comaprison) : this(Comparer<T>.Create(comaprison)) { }
		public Set() : this(Comparer<T>.Default) { }
		public bool Add(T v) {
			return insert(ref root, v);
		}
		public bool Remove(T v) {
			return remove(ref root, v);
		}
		public T this[int index] { get { return find(root, index); } }
		public int Count { get { return root.Count; } }
		public void RemoveAt(int k) {
			if (k < 0 || k >= root.Count) throw new ArgumentOutOfRangeException();
			removeAt(ref root, k);
		}
		public T[] Items {
			get {
				var ret = new T[root.Count];
				var k = 0;
				walk(root, ret, ref k);
				return ret;
			}
		}
		void walk(Node t, T[] a, ref int k) {
			if (t.Count == 0) return;
			walk(t.lst, a, ref k);
			a[k++] = t.Key;
			walk(t.rst, a, ref k);
		}

		bool insert(ref Node t, T key) {
			if (t.Count == 0) { t = new Node(key); t.lst = t.rst = nil; t.Update(); return true; }
			var cmp = comparer.Compare(t.Key, key);
			bool res;
			if (cmp > 0)
				res = insert(ref t.lst, key);
			else if (cmp == 0) {
				if (IsMultiSet) res = insert(ref t.lst, key);
				else return false;
			} else res = insert(ref t.rst, key);
			balance(ref t);
			return res;
		}
		bool remove(ref Node t, T key) {
			if (t.Count == 0) return false;
			var cmp = comparer.Compare(key, t.Key);
			bool ret;
			if (cmp < 0) ret = remove(ref t.lst, key);
			else if (cmp > 0) ret = remove(ref t.rst, key);
			else {
				ret = true;
				var k = t.lst.Count;
				if (k == 0) { t = t.rst; return true; }
				if (t.rst.Count == 0) { t = t.lst; return true; }


				t.Key = find(t.lst, k - 1);
				removeAt(ref t.lst, k - 1);
			}
			balance(ref t);
			return ret;
		}
		void removeAt(ref Node t, int k) {
			var cnt = t.lst.Count;
			if (cnt < k) removeAt(ref t.rst, k - cnt - 1);
			else if (cnt > k) removeAt(ref t.lst, k);
			else {
				if (cnt == 0) { t = t.rst; return; }
				if (t.rst.Count == 0) { t = t.lst; return; }

				t.Key = find(t.lst, k - 1);
				removeAt(ref t.lst, k - 1);
			}
			balance(ref t);
		}
		void balance(ref Node t) {
			var balance = t.lst.Height - t.rst.Height;
			if (balance == -2) {
				if (t.rst.lst.Height - t.rst.rst.Height > 0) { rotR(ref t.rst); }
				rotL(ref t);
			} else if (balance == 2) {
				if (t.lst.lst.Height - t.lst.rst.Height < 0) rotL(ref t.lst);
				rotR(ref t);
			} else t.Update();
		}

		T find(Node t, int k) {
			if (k < 0 || k > root.Count) throw new ArgumentOutOfRangeException();
			for (; ; )
			{
				if (k == t.lst.Count) return t.Key;
				else if (k < t.lst.Count) t = t.lst;
				else { k -= t.lst.Count + 1; t = t.rst; }
			}
		}
		public int LowerBound(T v) {
			var k = 0;
			var t = root;
			for (; ; )
			{
				if (t.Count == 0) return k;
				if (comparer.Compare(v, t.Key) <= 0) t = t.lst;
				else { k += t.lst.Count + 1; t = t.rst; }
			}
		}
		public int UpperBound(T v) {
			var k = 0;
			var t = root;
			for (; ; )
			{
				if (t.Count == 0) return k;
				if (comparer.Compare(t.Key, v) <= 0) { k += t.lst.Count + 1; t = t.rst; } else t = t.lst;
			}
		}

		void rotR(ref Node t) {
			var l = t.lst;
			t.lst = l.rst;
			l.rst = t;
			t.Update();
			l.Update();
			t = l;
		}
		void rotL(ref Node t) {
			var r = t.rst;
			t.rst = r.lst;
			r.lst = t;
			t.Update();
			r.Update();
			t = r;
		}


		class Node
		{
			public Node(T key) {
				Key = key;
			}
			public int Count { get; private set; }
			public sbyte Height { get; private set; }
			public T Key { get; set; }
			public Node lst, rst;
			public void Update() {
				Count = 1 + lst.Count + rst.Count;
				Height = (sbyte)(1 + Math.Max(lst.Height, rst.Height));
			}
			public override string ToString() {
				return string.Format("Count = {0}, Key = {1}", Count, Key);
			}
		}
	}

	/// <summary>
	/// ■使い方
	/// <code>
	/// // 要素数, 比較演算, 初期値 を与えられる
	/// var st = new SegmentTree<int>(200000, Math.Min, int.MaxValue);
	/// // 更新
	/// st[0] = 100;
	/// st[1] = 120;
	/// st[0] = int.MaxValue;
	/// // 範囲内の最小値(比較演算子による)を取得
	/// var minV = st.Query(0, 200000);
	/// </code>
	/// </summary>
	public class SegmentTree<T>
	{
		// 制約に合った2の冪
		private readonly int N;
		private T[] _array;

		// コンストラクタで与えられた値 ※引数なしQueryを実装するために追加
		private int _size;

		private T _identity;
		private Func<T, T, T> _operation;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="size">Treeの幅（内部Arrayの要素数ではない）</param>
		/// <param name="operation">Query/Update時の操作関数</param>
		/// <param name="identity">初期値</param>
		public SegmentTree(int size, Func<T, T, T> operation, T identity) {
			_size = size;
			N = 1;
			while (N < size) {
				N *= 2;
			}

			_identity = identity;
			_operation = operation;
			_array = new T[N * 2];
			for (int i = 1; i < N * 2; i++) {
				_array[i] = _identity;
			}
		}

		/// <summary>
		/// A[i]をnに更新 O(log N)
		/// </summary>
		public void Update(int i, T n) {
			i += N;
			_array[i] = n;
			while (i > 1) {
				i /= 2;
				_array[i] = _operation(_array[i * 2], _array[i * 2 + 1]);
			}
		}

		/// <summary>
		/// [0, size) から検索
		/// </summary>
		public T Query() => Query(0, _size);

		/// <summary>
		/// [left, right) から検索
		/// A[left] op A[left+1] ... op A[right-1]を求める
		/// </summary>
		public T Query(int left, int right) => Query(left, right, 1, 0, N);

		private T Query(int left, int right, int k, int l, int r) {
			if (r <= left || right <= l) {
				return _identity;
			}

			if (left <= l && r <= right) {
				return _array[k];
			}

			return _operation(Query(left, right, k * 2, l, (l + r) / 2),
				Query(left, right, k * 2 + 1, (l + r) / 2, r));
		}

		public T this[int i] {
			set { Update(i, value); }
			get { return _array[i + N]; }
		}
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

	class Scanner
	{
		private string[] _line;
		private int _index;
		private const char Separator = ' ';

		public Scanner() {
			_line = new string[0];
			_index = 0;
		}

		public string Next() {
			while (_index >= _line.Length) {
				_line = Console.ReadLine().Split(Separator);
				_index = 0;
			}

			return _line[_index++];
		}

		public int NextInt() => int.Parse(Next());
		public long NextLong() => long.Parse(Next());
		public double NextDouble() => double.Parse(Next());
		public decimal NextDecimal() => decimal.Parse(Next());
		public char NextChar() => Next()[0];
		public char[] NextCharArray() => Next().ToCharArray();

		public string[] Array() {
			_line = Console.ReadLine().Split(Separator);
			_index = _line.Length;
			return _line;
		}

		public int[] IntArray() => Array().Select(int.Parse).ToArray();
		public long[] LongArray() => Array().Select(long.Parse).ToArray();
		public double[] DoubleArray() => Array().Select(double.Parse).ToArray();
		public decimal[] DecimalArray() => Array().Select(decimal.Parse).ToArray();
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

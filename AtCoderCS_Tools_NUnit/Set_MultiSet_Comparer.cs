
// https://bitbucket.org/camypaper/complib
// ・IComparerを外部から与えられる
// ・IsMultiSetをtrueにすることでMultiSetになる

using System;
using System.Collections.Generic;

namespace CompLib.Collections
{
    #region Set
    /// <summary>
	/// Set/MultiSet
	/// <code>
	/// // 降順 ＆ 重複あり
	/// var set = new Set<int>((l, r) => r - l) { IsMultiSet = true };
	/// // 降順の場合 0番目＝最大値
	/// if (0 < set.Count)
	///     var maxValue = set[0];
	/// // MultiSetの場合でも削除されるのは1要素のみ ※C++のmultiset.erase(x)とは異なる
	/// set.Remove(213);
	/// </code>
	///
	/// struct を要素に使う場合はこんな感じ
	/// <code>
	/// var set = new Set<Child>((l, r) => {
	///     if (l.Rate != r.Rate)
	///         // 第1ソートキー: 降順
    ///         //return r.Rate.CompareTo(l.Rate);
	///         return r.Rate - l.Rate; // box化が発生しないのでこちらの方が速い？→殆ど変わらない
    ///     else
	///         // 第2ソートキー: 昇順
    ///         //return l.Garden.CompareTo(r.Garden);
    ///         return l.Garden - r.Garden;
	/// });
	/// </code>
	/// </summary>
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
		///<summary>
		///当該値を1つ削除する
		///※C++のmultisetのerase()は当該値をすべて削除するが、これは1つだけ削除するように実装している
		///</summary>
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
    #endregion
}
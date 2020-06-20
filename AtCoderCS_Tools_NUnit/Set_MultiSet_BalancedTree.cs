using System;
using System.Collections.Generic;


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
        public T First() =>  SB_BinarySearchTree<T>.First(_root).Value;
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

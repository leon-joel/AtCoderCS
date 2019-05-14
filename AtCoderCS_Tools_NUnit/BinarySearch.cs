using System;
using System.Collections.Generic;

namespace Tools
{
	public static partial class Util {
		public static bool IsOK<T>(IList<T> ary, int idx, T key) where T : IComparable, IComparable<T> {
			// key <= ary[idx] と同じ意味
			return key.CompareTo(ary[idx]) <= 0;
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
		public static int BinarySearch<T>(IList<T> ary, T key) where T : IComparable, IComparable<T> {
			return BinarySearch(ary, -1, ary.Count, key);
		}
		/// <summary>
		/// 範囲付き二分探索
		/// ※条件を満たす最小のidxを返す
		/// 例) 0 1 2 3 4 5 6 7 8 9
		/// [3, 4] からの探索の場合、l=2, r=5 を与える
		/// すべてが条件を満たす場合は l+1 が返される
		/// 条件を満たすものがない場合は r が返される
		/// </summary>
		public static int BinarySearch<T>(IList<T> ary, int left, int right, T key) where T : IComparable, IComparable<T> {
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

		/// <summary>
		/// listのi番目以降で初めて list[i]がv以上 となる i を返す
		/// ※BinarySearchではなく、シーケンシャルサーチしている
		/// </summary>
		public static int Incr<T>(IList<T> list, int i, T v) where T : IComparable<T> {
			while (i < list.Count && list[i].CompareTo(v) < 0) ++i;
			return i;
		}
	}
}

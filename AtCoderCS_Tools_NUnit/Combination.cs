using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools
{
    public static class Combination
    {
		/// <summary>
		/// itemsから cnt個を選びだす全組み合わせを列挙
		/// ※ただし、begin番目(0-indexed)以降から選び出す
		/// </summary>
		public static IEnumerable<IList<T>> Create<T>(
			IList<T> items, int cnt, int begin = 0) {
			if (cnt == 1) {
				for (int i = begin; i < items.Count; i++) {
					yield return new List<T> { items[i] };
				}

			} else {
				for (int i = begin; i <= items.Count - cnt; i++) {
					// 選んだものよりあとのものから cnt-1個 選び出す
					foreach (var right in Create(items, cnt - 1, i + 1)) {
						var list = new List<T>(cnt) { items[i] };
						list.AddRange(right);
						yield return list;
					}
				}
			}
		}
	}
}

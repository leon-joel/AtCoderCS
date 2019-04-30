using System;
using System.Collections.Generic;

namespace Tools
{
	/// <summary>
	/// 優先度付きキュー
	/// http://kumikomiya.com/competitive-programming-with-c-sharp/
	/// </summary>
	public class PriorityQueue<T>
	{
		private readonly List<T> m_list;
		private readonly Func<T, T, int> m_compare;
		private int m_count;
		public PriorityQueue(int capacity, Func<T, T, int> compare) {
			m_list = new List<T>(capacity);
			m_compare = compare;
			m_count = 0;
		}
		int Add(T value) {
			if (m_count == m_list.Count) {
				m_list.Add(value);
			} else {
				m_list[m_count] = value;
			}
			return m_count++;
		}
		void Swap(int a, int b) {
			T tmp = m_list[a];
			m_list[a] = m_list[b];
			m_list[b] = tmp;
		}

		public void Enqueue(T value) {
			int c = Add(value);
			while (c > 0) {
				int p = (c - 1) / 2;
				if (m_compare(m_list[c], m_list[p]) < 0) { Swap(p, c); } else { break; }
				c = p;
			}
		}
		public T Dequeue() {
			T value = m_list[0];
			m_list[0] = m_list[--m_count];
			int p = 0;
			while (true) {
				int c1 = p * 2 + 1;
				int c2 = p * 2 + 2;
				if (c1 >= m_count) { break; }
				int c = (c2 >= m_count || m_compare(m_list[c1], m_list[c2]) < 0) ? c1 : c2;
				if (m_compare(m_list[c], m_list[p]) < 0) { Swap(p, c); } else { break; }
				p = c;
			}
			return value;
		}
	}
}

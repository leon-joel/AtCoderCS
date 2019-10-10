using System;
using System.Collections.Generic;

namespace Tools
{
	/// <summary>
	/// 優先度付きキュー
	/// </summary>
	class PriorityQueue<T> where T : IComparable<T>
	{
		public T[] _heap;
		public int _size;
		public int _sign;
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="initialSize">Heap(List)の初期サイズ</param>
		/// <param name="descend">降順の場合はtrueを与える</param>
		public PriorityQueue(int initialSize, bool descend = false) {
			_heap = new T[initialSize];
			if (descend) _sign = -1;
		}
		public int Compare(T x, T y) {
			return x.CompareTo(y) * _sign;
		}
		public void Push(T x) {
			int i = _size++;
			while (i > 0) {
				int p = (i - 1) / 2;
				if (Compare(x, _heap[p]) >= 0) {
					break;
				}
				_heap[i] = _heap[p];
				i = p;
			}
			_heap[i] = x;
		}
		/// <summary>
		/// Countが0の場合はランタイムエラー
		/// </summary>
		public T Pop() {
			T ret = _heap[0];
			T x = _heap[--_size];
			int i = 0;
			while (i * 2 + 1 < _size) {
				int a = i * 2 + 1;
				int b = i * 2 + 2;
				if (b < _size && Compare(_heap[a], _heap[b]) > 0) {
					a = b;
				}
				if (Compare(_heap[a], x) >= 0) {
					break;
				}
				_heap[i] = _heap[a];
				i = a;
			}
			_heap[i] = x;
			return ret;
		}
		/// <summary>
		/// Countが0の場合は引数に与えた値を返す
		/// </summary>
		public T Pop(T defaultValue) {
			if (Count() == 0)
				return defaultValue;
			else
				return Pop();
		}
		public int Count() {
			return _size;
		}
	}

	///// <summary>
	///// 優先度付きキュー
	///// http://kumikomiya.com/competitive-programming-with-c-sharp/
	///// </summary>
	//public class PriorityQueue<T>
	//{
	//	private readonly List<T> m_list;
	//	private readonly Func<T, T, int> m_compare;
	//	private int m_count;
	//	public PriorityQueue(int capacity, Func<T, T, int> compare) {
	//		m_list = new List<T>(capacity);
	//		m_compare = compare;
	//		m_count = 0;
	//	}
	//	int Add(T value) {
	//		if (m_count == m_list.Count) {
	//			m_list.Add(value);
	//		} else {
	//			m_list[m_count] = value;
	//		}
	//		return m_count++;
	//	}
	//	void Swap(int a, int b) {
	//		T tmp = m_list[a];
	//		m_list[a] = m_list[b];
	//		m_list[b] = tmp;
	//	}

	//	public void Enqueue(T value) {
	//		int c = Add(value);
	//		while (c > 0) {
	//			int p = (c - 1) / 2;
	//			if (m_compare(m_list[c], m_list[p]) < 0) { Swap(p, c); } else { break; }
	//			c = p;
	//		}
	//	}
	//	public T Dequeue() {
	//		T value = m_list[0];
	//		m_list[0] = m_list[--m_count];
	//		int p = 0;
	//		while (true) {
	//			int c1 = p * 2 + 1;
	//			int c2 = p * 2 + 2;
	//			if (c1 >= m_count) { break; }
	//			int c = (c2 >= m_count || m_compare(m_list[c1], m_list[c2]) < 0) ? c1 : c2;
	//			if (m_compare(m_list[c], m_list[p]) < 0) { Swap(p, c); } else { break; }
	//			p = c;
	//		}
	//		return value;
	//	}
	//}
}

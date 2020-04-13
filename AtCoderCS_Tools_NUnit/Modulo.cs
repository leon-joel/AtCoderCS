using System;
namespace Tools
{
	/// <summary>
	/// Ｍを法とする除算、累乗、階乗、nCr
	/// http://kumikomiya.com/competitive-programming-with-c-sharp/
	/// </summary>
	public class Modulo
	{
		private readonly static int M = 1000000007;

		/// <summary>(a * b) % M</summary>
		public static int Mul(int a, int b) {
			return (int)(Math.BigMul(a, b) % M);
		}
		/// <summary>(a + b) % M</summary>
		public static int Add(int a, int b) {
			long v = a + b;
			if (M <= v)
				v -= M;
			return (int)v;
		}
		/// <summary>(a - b) % M</summary>
		public static int Sub(int a, int b) {
			int v = a - b;
			if (v < 0) v = M + v;
			return v;
		}

		/// <summary>(aのm乗) % M</summary>
		/// <see cref="https://www.youtube.com/watch?v=gdQxKESnXKs のD問題"/>
		public static int Pow(int a, int m) {
			switch (m) {
				case 0:
					return 1;
				case 1:
					return a % M;
				default:
					int p1 = Pow(a, m / 2);
					int p2 = Mul(p1, p1);
					return ((m % 2) == 0) ? p2 : Mul(p2, a);
			}
		}

		/// <summary>
		/// (a / b) % M
		/// ※フェルマーの小定理による
		/// </summary>
		/// <see cref="https://www.youtube.com/watch?v=gdQxKESnXKs のD問題"/>
		public static int Div(int a, int b) {
			return Mul(a, Pow(b, M - 2));
		}

		/// <summary>
		/// コンストラクタ ※n! % M および nCr % M を使用する場合には必要
		/// </summary>
		private readonly int[] m_facs;
		public Modulo(int n) {
			// x が n までの、x! % M の結果を配列に保持する
			m_facs = new int[n + 1];
			m_facs[0] = 1;
			for (int i = 1; i <= n; ++i) {
				m_facs[i] = Mul(m_facs[i - 1], i);
			}
		}

		/// <summary>n! % M</summary>
		public int Fac(int n) {
			return m_facs[n];
		}
		/// <summary>
		/// 組み合わせ nCr % M
		/// </summary>
		/// <see cref="https://www.youtube.com/watch?v=gdQxKESnXKs のD問題"/>
		public int Ncr(int n, int r) {
			if (n < r) { return 0; }
			if (n == r) { return 1; }
			int res = Fac(n);
			res = Div(res, Fac(r));
			res = Div(res, Fac(n - r));
			return res;
		}

		/// <summary>
		/// 組み合わせ nCr % M を高速に計算する
		/// ※n!のテーブルを使用しない版（Nが大きくても計算できる）
		/// </summary>
		public static int CalcNcr(int n, int r) {
			if (n - r < r) return CalcNcr(n, n - r);

			long ansMul = 1;
			long ansDiv = 1;
			for (int i = 0; i < r; i++) {
				ansMul *= n - i;
				ansDiv *= i + 1;
				ansMul %= M;
				ansDiv %= M;
			}

			return Div((int)ansMul, (int)ansDiv);
		}
	}
}

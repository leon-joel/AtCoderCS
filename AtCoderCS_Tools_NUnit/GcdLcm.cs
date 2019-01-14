using System;
namespace AtCoderCS_Tools
{
	public static partial class Utils
	{
		/// <summary>
		/// 最大公約数 ※ユークリッドの互除法 
		/// ※a,bどちらかが0の場合は0じゃない方を、両方0の場合は0を返す。
		/// </summary>
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

		/// <summary>
		/// 最小公倍数
		/// ※a,bどちらかが0の場合は0を返す。
		/// ※両方0の場合はDivideByZeroExceptionがthrowされる。
		/// </summary>
		public static int Lcm(int a, int b) {
			return a * b / Gcd(a, b);
		}
	}
}

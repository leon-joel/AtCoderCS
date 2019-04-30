using System;
namespace Tools
{
	public static partial class Util
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
		public static int Gcd(params int[] nums) {
			if (nums == null || nums.Length < 1)
				throw new ArgumentException(nameof(nums));
			if (nums.Length == 1)
				return nums[0];

			var g = Gcd(nums[0], nums[1]);
			for (int i = 2; i < nums.Length; i++) {
				g = Gcd(g, nums[i]);
			}
			return g;
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

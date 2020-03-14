using System;
using System.Collections.Generic;

namespace AtCoderCS_Tools_NUnit
{
	class Sample
	{
		void BIT全探索のテンプレート() {
			var primes = new List<long>();

			// 全探索に使用する全bit数
			var n = primes.Count;

			// bit mask の生成
			// 2^n     = 1 << n
			for (ulong mask = 0; mask <= ((1ul << n) - 1); mask++) {

				// 各bitを下から舐める
				for (int i = 0; i < n; i++) {

					//iビット目が立っている？
					if (0 != (mask & (1ul << i))) {
						// 何か処理する

					}
				}
			}
		}
	}
}

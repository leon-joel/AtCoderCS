using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

// 典型的なDPコンテスト
// https://tdpc.contest.atcoder.jp/tasks/tdpc_contest
//
namespace TypicalDPContest
{
	public static class Util {
		public static string DumpToString<T>(IEnumerable<T> array) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var item in array) {
				sb.Append(item);
				sb.Append(", ");
			}
			return sb.ToString();
		}

		public static void InitDP<T>(T[,] dp, T value) {
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					dp[i, j] = value;
				}
			}
		}
	}

	public class SolverBase {
		virtual protected string ReadLine() => Console.ReadLine();
		virtual protected int ReadInt() => int.Parse(ReadLine());
		virtual protected long ReadLong() => long.Parse(ReadLine());
		virtual protected string[] ReadStringArray() => ReadLine().Split(' ');
		virtual protected int[] ReadIntArray() => ReadLine().Split(' ').Select<string, int>(s => int.Parse(s)).ToArray();
		virtual protected long[] ReadLongArray() => ReadLine().Split(' ').Select<string, long>(s => long.Parse(s)).ToArray();
		virtual protected void WriteLine(string line) => Console.WriteLine(line);
		virtual protected void WriteLine<T>(T value) where T : IFormattable => Console.WriteLine(value);
		virtual protected void WriteLine(double d) => Console.WriteLine($"{d:F9}");

		[Conditional("DEBUG")]
		protected void Dump(string s) => Console.WriteLine(s);
		[Conditional("DEBUG")]
		protected void Dump(char c) => Console.WriteLine(c);
		[Conditional("DEBUG")]
		protected void Dump(double d) => Console.WriteLine($"{d:F9}");
		[Conditional("DEBUG")]
		protected void Dump<T>(IEnumerable<T> array) where T : IFormattable {
			string s = Util.DumpToString(array);
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(s);
			//_writer.WriteLine(s);
		}
		[Conditional("DEBUG")]
		protected void DumpGrid<T>(IEnumerable<IEnumerable<T>> arrayOfArray) where T : IFormattable {
			var sb = new StringBuilder();
			foreach (var a in arrayOfArray) {
				sb.AppendLine(Util.DumpToString(a));
			}
			// Consoleに出力すると、UnitTestの邪魔をしないというメリットあり。
			Console.WriteLine(sb.ToString());
			//_writer.WriteLine(sb.ToString());
		}
		[Conditional("DEBUG")]
		protected void DumpDP<T>(T[,] dp) where T : IFormattable {
			var sb = new StringBuilder();
			for (int i = 0; i < dp.GetLength(0); i++) {
				for (int j = 0; j < dp.GetLength(1); j++) {
					sb.Append(dp[i, j]);
					sb.Append(", ");
				}
				sb.AppendLine();
			}
			Console.WriteLine(sb.ToString());
		}
	}

	// A: コンテスト 
	public class Solver : SolverBase {
		int N;
		int[] Nums;
		int UpperW = 100 * 100;

		public void Run() {
			N = ReadInt();
			Nums = ReadIntArray();

			//var ans = DpSolve();
			var ans = DistributeDP();
			WriteLine(ans);
		}

		// 動的計画法（DP）（＝漸化式＋ループ）での実装
		// 貰うDP
		int DpSolve() {
			var DP = new bool[N + 1, UpperW + 1];
			//for (int i = 0; i < N + 1; i++) {
			//	for (int j = 0; j < UpperW + 1; j++) {
			//		DP[i, j] = false;
			//	}
			//}
			DP[0, 0] = true;

			for (int i = 0; i < N; i++) {
				for (int w = 0; w < UpperW + 1; w++) {
					if (w < Nums[i]) {
						// 現在itemの重さより左側（現在itemが使えない部分）は上から下ろしてくるだけ
						DP[i + 1, w] = DP[i, w];
					} else {
						// 現在itemの重さ以上の部分（右側）は
						// 重さ分左のvalue | 現在value or 下ろしてくるだけ
						DP[i + 1, w] = DP[i, w] | DP[i, w - Nums[i]];
					}
				}
			}

			int cnt = 0;
			for (int i = 0; i < UpperW + 1; i++) {
				if (DP[N, i]) ++cnt;
			}

			return cnt;
		}

		// 配るDP ＋ 1次元配列DP
		int DistributeDP() {
			// 配るDPは、配布先のIndexOutOfRangeチェックが面倒なので、
			// DP配列の範囲を必要なだけ拡げておく
			var DP = new bool[UpperW + 1 + 100];
			DP[0] = true;

			// 配るDP --- dp[w] からの遷移先を更新する＝遷移元でループする
			foreach (var point in Nums) {
				// ※1次元DPなので、2度配りしないように末尾からループする
				for (int w = UpperW - 1; 0 <= w; w--) {
					// 配布元がfalse（＝配布元にくばる経路がない）はスキップ
					if (DP[w]) {
						// 点数リストでループし、配布先に配る
						DP[w + point] = true;
					}
				}
			}
			int cnt = 0;
			for (int i = 0; i < UpperW; i++) {
				if (DP[i]) ++cnt;
			}
			return cnt;
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	// B: ゲーム 2次元DP
	public class SolverB : SolverBase {
		public void Run() {
			var ary = ReadIntArray();
			var nA = ary[0];
			var nB = ary[1];

			var numsA = ReadIntArray();
			var numsB = ReadIntArray();

			// 2次元DP
			int[,] dp = new int[nA + 1, nB + 1];
			// DP[A山の残り数, B山の残り数] = すぬけが取りうる最大の価値
			// 初期化: DP[0, 0] = 0
			//
			// A山[1] B山[2, 10]の場合 ※先手は常にすぬけ君
			// ※^^はすぬけ君が取った手。無印は相手の手。
			//       10    2  ←B山
			//   0 ← 10 ← 10
			//   ↑   ^^   ↑
			// 1 1 ←  1   11
			//  ^^        ^^
			// A山

			// すぬけ君の手番 ※トータル数が奇数なら残数奇数が手番になる
			int myTurn = (nA + nB) % 2;

			// 左端を先に全部埋める ※余計なIndex境界判定を不要にできる
			for (int i = 1; i < nA + 1; i++) {
				if (i % 2 == myTurn) {
					// すぬけ番なので上のに現在価値を加算して格納
					dp[i, 0] = dp[i - 1, 0] + numsA[nA - i];
				} else {
					// 相手の番なので上のをそのまま格納
					dp[i, 0] = dp[i - 1, 0];
				}
			}
			// 同様に上端を全部埋める
			for (int j = 1; j < nB + 1; j++) {
				if (j % 2 == myTurn) {
					// すぬけ番なので上のに現在価値を加算して格納
					dp[0, j] = dp[0, j - 1] + numsB[nB - j];
				} else {
					// 相手の番なので上のをそのまま格納
					dp[0, j] = dp[0, j - 1];
				}
			}
			//DumpDP(dp);

			// 貰うDP
			for (int i = 1; i < nA + 1; i++) {
				for (int j = 1; j < nB + 1; j++) {
					if ((i + j) % 2 == myTurn) {
						// すぬけ君の手番（＝価値を最大化する）
						// 上＋A or 左＋B の大きい方を選択し、書き込み
						var a = dp[i - 1, j] + numsA[nA - i];
						var b = dp[i, j - 1] + numsB[nB - j];
						dp[i, j] = Math.Max(a, b);
					} else {
						// 相手の手番（＝すぬけの価値を最小化する）
						// 上 or 左 の小さい方を選択し、書き込み
						dp[i, j] = Math.Min(dp[i - 1, j], dp[i, j - 1]);
					}
				}
			}
			//DumpDP(dp);

			WriteLine(dp[nA, nB]);
		}

#if !MYHOME
		public static void Main(string[] args) {
			new Solver().Run();
		}
#endif
	}

	// C: トーナメント
	public class SolverC : SolverBase {
		public void Run() {
			var K = ReadInt();
			var playerNum = (int)Math.Pow(2, K);
			var Rates = new int[playerNum];

			for (int i = 0; i < playerNum; i++) {
				Rates[i] = ReadInt();
			}

			// 動的計画法（DP）（＝漸化式＋ループ）での実装
			// DP[i回戦, j番目の選手] = i回戦目まで勝ち上がってくる確率(~1)
			var DP = new double[K + 1, playerNum];
			// 0回戦目は全員勝ち上がり確率 1 とする
			for (int i = 0; i < playerNum; i++) {
				DP[0, i] = 1;
			}

			for (int i = 1; i < K + 1; i++) {
				for (int j = 0; j < playerNum; j++) {
					// i回戦目(1~)の対戦相手
					//   jのiビット目を反転
					//   その下位ビット（がもしあれば）をbit全探索
					//Dump($"{i}回戦: {j}");

					// 積算対戦勝率
					double sumRatio = 0;
					var prefix = j ^ (1 << (i - 1));    // iビットだけ反転
					prefix &= int.MaxValue << (i - 1);  // iビットより下位は全部0クリ
					for (int r = 0; r < (1 << (i - 1)); r++) {
						// 対戦相手
						var o = prefix | r;
						//Dump(o.ToString());

						// += 勝率 * 相手がそのラウンドまで勝ち上がってくる確率
						sumRatio += CalcWinRatioOfA(Rates[j], Rates[o]) * DP[i - 1, o];
					}

					// 自分が(i-1)回戦目まで勝ち上がる確率 * i回戦を勝ち上がる確率
					DP[i, j] = DP[i - 1, j] * sumRatio;
				}
			}

			for (int j = 0; j < playerNum; j++) {
				WriteLine(DP[K, j]);
				Dump(DP[K, j]);
			}
		}

		double CalcWinRatioOfA(int rA, int rB) {
			return 1 / (1 + Math.Pow(10, (rB - rA) / (double)400));
		}

#if !MYHOME
		public static void Main(string[] args) {
			new SolverC().Run();
		}
#endif
	}

	// D: サイコロ
	public class SolverD : SolverBase {
		private const double ZERO = default(double);

		public void Run() {
#pragma warning disable RECS0018 // 等値演算子による浮動小数点値の比較
			var ary = ReadLongArray();
			int N = (int)ary[0];
			var D = ary[1];
			if (D == 1) {
				// ※ほんとは、1は特別扱いしなくても大丈夫だけど
				WriteLine(1.0);
				return;
			}

			// Dを素因数分解
			int p2 = 0, p3 = 0, p5 = 0;
			var d = D;
			while (d % 2 == 0) {
				++p2;
				d /= 2;
			}
			while (d % 3 == 0) {
				++p3;
				d /= 3;
			}
			while (d % 5 == 0) {
				++p5;
				d /= 5;
			}

			// 素因数分解の結果2,3,5以外の因数が残った場合は絶対にその倍数にはならない
			if (1 < d) {
				WriteLine(0.0);
				return;
			}

			// 多次元DP
			// DP[ターン, 素因数2の個数, 同3, 同5] = この状況になる確率
			//   ※メモリー節約のため、ターンは2セット分の領域を使い回す
			double[,,,] DP = new double[2, p2 + 1, p3 + 1, p5 + 1];
			// 初期値
			DP[0, 0, 0, 0] = 1.0;

			Action<int> DumpTurn = (int n) => {
#if MYHOME
				Dump($"Turn {n} ==================");
				for (int x = 0; x < p2 + 1; x++) {
					for (int y = 0; y < p3 + 1; y++) {
						for (int z = 0; z < p5 + 1; z++) {
							Dump($"[{x},{y},{z}] = {DP[n % 2 ^ 1, x, y, z]}");
						}
					}
				}
#endif
			};

			// N回サイコロを振る
			for (int i = 0; i < N; i++) {
				// 使用するメモリー領域の切り替え
				int cur = (i % 2);
				int tar = 1 ^ cur;  // curが1だったら0、0だったら1。※ ^ は排他的論理和

				// target側の0クリ
				for (int x = 0; x < p2 + 1; x++) {
					for (int y = 0; y < p3 + 1; y++) {
						for (int z = 0; z < p5 + 1; z++) {
							DP[tar, x, y, z] = ZERO;
						}
					}
				}

				// 2,3,5の3重ループ
				for (int x = 0; x < p2 + 1; x++) {
					for (int y = 0; y < p3 + 1; y++) {
						for (int z = 0; z < p5 + 1; z++) {
							if (DP[cur, x, y, z] == ZERO) continue;

							// 配るDP
							// 配布元の確率を6等分して、1~6 の配布先に分配していく
							// 1が出るなら
							DP[tar, x, y, z] += DP[cur, x, y, z] / 6.0;
							// 2 ※所定数以上はすべて配列末尾に加算していく
							DP[tar, Math.Min(x + 1, p2), y, z] += DP[cur, x, y, z] / 6.0;
							// 3
							DP[tar, x, Math.Min(y + 1, p3), z] += DP[cur, x, y, z] / 6.0;
							// 4
							DP[tar, Math.Min(x + 2, p2), y, z] += DP[cur, x, y, z] / 6.0;
							// 5
							DP[tar, x, y, Math.Min(z + 1, p5)] += DP[cur, x, y, z] / 6.0;
							// 6
							DP[tar, Math.Min(x + 1, p2), Math.Min(y + 1, p3), z] += DP[cur, x, y, z] / 6.0;
						}
					}
				}
				DumpTurn(i);

#pragma warning restore RECS0018 // 等値演算子による浮動小数点値の比較
			}

			double ans = DP[N % 2, p2, p3, p5];
			WriteLine(ans);
		}

#if !MYHOME
		public static void Main(string[] args) {
			new SolverD().Run();
		}
#endif
	}

	// E: 数
	public class SolverE : SolverBase {
		const int MOD = 1000000007;
		public void Run() {
			var D = ReadInt();
			var S = ReadLine();
			//for (int i = 0; i < S.Length; i++) {
			//	// 文字列の先頭からダンプ
			//	Dump(S[i]);
			//}

			// DP[pos桁目][各桁の和sum][制約の有無rist] = パターン数
			//   posは文字列の先頭を1とする
			//   sumは mod D したもの
			//   rist は制約あり:1 制約なし:0 とする
			//   パターン数は mod MOD したもの
			int[,,] DP = new int[S.Length+1, D, 2];
			// 初期状態 ※1桁目は必ず制約あり
			DP[0, 0, 1] = 1;

			// 配るDP
			for (int pos = 0; pos < S.Length; pos++) {
				for (int sum = 0; sum < D; sum++) {
					for (int rist = 0; rist <= 1; rist++) {
						if (DP[pos, sum, rist] == 0) continue;

						if (rist == 0) {
							for (int d = 0; d < 10; d++) {
								// 配布
								DP[pos + 1, (sum + d) % D, rist] += DP[pos, sum, rist];
								DP[pos + 1, (sum + d) % D, rist] %= MOD;
							}
						} else {
							// 制約ありなので、Sから制約となる数字を取得する
							var upper = (char)S[pos] - (char)'0';

							for (int d = 0; d < upper; d++) {
								// 制約なしの部分なので、配布先は制約なしとなる
								DP[pos + 1, (sum + d) % D, 0] += DP[pos, sum, rist];
								DP[pos + 1, (sum + d) % D, 0] %= MOD;
							}
							// 制約ありの部分
							DP[pos + 1, (sum + upper) % D, 1] += DP[pos, sum, rist];
							DP[pos + 1, (sum + upper) % D, 1] %= MOD;
						}
					}
				}
			}
			// 制限なし ＋ 制限あり - オール0の分
			var ans = (DP[S.Length, 0, 0] + DP[S.Length, 0, 1] - 1) % MOD;
			WriteLine(ans);
		}
#if !MYHOME
		public static void Main(string[] args) {
			new SolverE().Run();
		}
#endif
	}
}

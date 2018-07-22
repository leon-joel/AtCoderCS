using System;
using System.Linq;

namespace AtCoderCS
{
    class MainClass
    {
		public static void Main(string[] args)
		{
			//// 整数の入力
			//int a = int.Parse(Console.ReadLine());
			//// スペース区切りの整数の入力
			//string[] input = Console.ReadLine().Split(' ');
			//int b = int.Parse(input[0]);
			//int c = int.Parse(input[1]);
			//// 文字列の入力
			//string s = Console.ReadLine();
			////出力
			//Console.WriteLine((a + b + c) + " " + s);

			var nums = Console.ReadLine().Split().Select(int.Parse).ToList();

			int max = nums.Max();
			int min = nums.Min();

			Console.WriteLine(max - min);
		}
    }
}

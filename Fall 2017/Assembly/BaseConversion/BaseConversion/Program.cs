using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseConversion
{
	class Program
	{
		static void Main(string[] args)
		{
			bool end = false;
			do
			{
				string output = "";

				Console.WriteLine("Enter Decimal Value:");
				long v = Convert.ToInt64(Console.ReadLine());
				Console.WriteLine("Enter Base:");
				long b = Convert.ToInt64(Console.ReadLine());
				string res = "";

				int i = 0;
				while (v > 0)
				{
					res += c(v % b);
					v /= b;
				}
				Console.WriteLine(res.ToCharArray().Reverse().ToArray());

				Console.WriteLine("Continue? Y/N");
				string e = Console.ReadLine();
				if (e == "n" || e == "N")
				{
					end = true;
				}
				Console.WriteLine("===================");
			} while (end == false);
		}

		public static string c(long input)
		{
			string ascii = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/";
			
			string output = ascii[Convert.ToInt32(input)].ToString();
			return output;
		}
		public static string revStr(string input)
		{

			return input;
		}
	}
}

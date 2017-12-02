using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace a5
{
	class UI
	{
		public UI()
		{

		}
		public string StartMenu()
		{
			while (true)
			{
				Console.WriteLine("Enter Search Term");
				Console.WriteLine("_________________");
				string input = Console.ReadLine();

				if (String.IsNullOrEmpty(input))
				{
					Console.WriteLine("=============");
					Console.WriteLine("INVALID INPUT");
					Console.WriteLine("=============");
				}
				else { return input; }
			}
		}
		public void TTHeader()
		{
			Console.WriteLine("".PadRight(70, '_'));
			Console.WriteLine("SEARCH RESULTS FOR 2-3 TREE");
			Console.WriteLine();
			Console.WriteLine("DEC".PadRight(10, ' ') + 
				"OCT".PadRight(10, ' ') + 
				"HEX".PadRight(10, ' ') + 
				"BIN".PadRight(10, ' ') + 
				"SYMBOL".PadRight(10, ' ') + 
				"SEARCH TIME (ms)");
			Console.WriteLine("".PadRight(70, '_'));
		}
		public void TTFHeader()
		{
			Console.WriteLine("".PadRight(70, '_'));
			Console.WriteLine("SEARCH RESULTS FOR 2-3-4 TREE");
			Console.WriteLine();
			Console.WriteLine("DEC".PadRight(10, ' ') + 
				"OCT".PadRight(10, ' ') + 
				"HEX".PadRight(10, ' ') + 
				"BIN".PadRight(10, ' ') + 
				"SYMBOL".PadRight(10, ' ') + 
				"SEARCH TIME (ms)");
			Console.WriteLine("".PadRight(70, '_'));
		}
		public void Output(string[] arr, double time)
		{
			// DECIMAL	OCTAL	HEX		BINARY	SYMBOL	
			Console.WriteLine(	arr[0].PadRight(10, ' ') +
								arr[1].PadRight(10, ' ') + 
								arr[2].PadRight(10, ' ') +
								arr[3].PadRight(10, ' ') + 
								arr[4].PadRight(10, ' ') +
								time);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace a5
{
	/// <summary>
	/// UI class used to control majority of program user input and program output.
	/// </summary>
	class UI
	{
		/// <summary>
		/// Called at start of program to prompt user for a search item.
		/// </summary>
		/// <returns>Returns a validated search string.</returns>
		public string StartMenu()
		{
			while (true)
			{
				Console.WriteLine("Enter Search Term");
				Console.WriteLine("".PadRight(100, '_'));
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

		/// <summary>
		/// Helper method used to print a large amount of text that acts
		/// as a header for search results for a 2-3 Tree.
		/// </summary>
		/// <param name="height">Height of tree.</param>
		/// <param name="leaves">Number of leaves in tree.</param>
		public void TTHeader(int height, int leaves)
		{
			Console.WriteLine();
			Console.WriteLine("".PadRight(100, '_'));
			Console.WriteLine("".PadLeft(30, ' ') + "SEARCH RESULTS FOR 2-3 TREE");
			Console.WriteLine("".PadLeft(30, ' ') + $"HEIGHT: {height}, LEAF COUNT: {leaves}");
			Console.WriteLine();
			Console.WriteLine("".PadRight(20, ' ') + 
				"DEC".PadRight(10, ' ') + 
				"OCT".PadRight(10, ' ') + 
				"HEX".PadRight(10, ' ') + 
				"BIN".PadRight(10, ' ') + 
				"SYMBOL".PadRight(10, ' ') + 
				"SEARCH TIME (ms)");
			Console.WriteLine("".PadRight(100, '_'));
		}

		/// <summary>
		/// Helper method used to print a large amount of text that acts
		/// as a header for search results for a 2-3-4 Tree.
		/// </summary>
		/// <param name="height">Height of tree.</param>
		/// <param name="leaves">Number of leaves in tree.</param>
		public void TTFHeader(int height, int leaves)
		{
			Console.WriteLine();
			Console.WriteLine("".PadRight(100, '_'));
			Console.WriteLine("".PadLeft(30, ' ') + "SEARCH RESULTS FOR 2-3-4 TREE");
			Console.WriteLine("".PadLeft(30, ' ') + $"HEIGHT: {height}, LEAF COUNT: {leaves}");
			Console.WriteLine();
			Console.WriteLine("".PadRight(20, ' ') +
				"DEC".PadRight(10, ' ') + 
				"OCT".PadRight(10, ' ') + 
				"HEX".PadRight(10, ' ') + 
				"BIN".PadRight(10, ' ') + 
				"SYMBOL".PadRight(10, ' ') + 
				"SEARCH TIME (ms)");
			Console.WriteLine("".PadRight(100, '_'));
		}

		/// <summary>
		/// Main output method that prints out search results passed in from Program class.
		/// </summary>
		/// <param name="arr">Array of strings containing search results.</param>
		/// <param name="time">Total time (ms) of search/</param>
		/// <param name="type">String specifying the tree used to search. 
		/// This is needed since some searches return multiple different results from different columns in the ASCII table.</param>
		public void Output(string[] arr, double time, string type)
		{
			// DECIMAL	OCTAL	HEX		BINARY	SYMBOL	
			Console.WriteLine(	type.PadRight(20, ' ') +
								arr[0].PadRight(10, ' ') +
								arr[1].PadRight(10, ' ') + 
								arr[2].PadRight(10, ' ') +
								arr[3].PadRight(10, ' ') + 
								arr[4].PadRight(10, ' ') +
								time);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace a1
{
	class Program
	{
		static void Main(string[] args)
		{
			Stopwatch sw = new Stopwatch();
			int mill = 0;

			Console.WriteLine("Enter First Name:");
			string name = Console.ReadLine();
			Console.WriteLine("Enter Array Dimension:");
			string d = Console.ReadLine();

			int n = Convert.ToInt32(d);

			name = name.ToLower();

			// Create 2D array
			char[,] a = randomize(n);

			// Copy array a to b after converting to 1D array
			char[] b = Conv(a, n);
			int r = b.Length - 1; // Right position of b
			
			// UNSORTED LINEAR SEARCH TIMING
			for (int i = 1; i <= 1000; i++)
			{
				sw.Start();
				string[] res = boringSearch(a, name);
				sw.Stop();
				TimeSpan ts = sw.Elapsed;
				mill += ts.Milliseconds;
			}
			Console.WriteLine($"Unsorted Linear Search (ms): {mill / 1000}");
			sw.Reset();
			mill = 0;

			// MERGE SORT TIMING
			for (int i = 1; i <= 1000; i++)
			{
				// Recreate a and convert to b so we can sort again.
				// This is purely for timing purposes.
				a = randomize(n);
				b = Conv(a, n);

				sw.Start(); 
				Array.Sort(b);
				sw.Stop();
				TimeSpan ts = sw.Elapsed;
				mill += ts.Milliseconds;
			}
			Console.WriteLine($"Merge Sort (ms): {mill / 1000}");
			sw.Reset();
			mill = 0;

			/*
			// CUSTOM BINARY SEARCH TIMING
			for (int i = 1; i <= 1000; i++)
			{
				foreach (char c in name)
				{
					sw.Start();
					BinSearch(b, c, 0, r);
					sw.Stop();
					TimeSpan ts = sw.Elapsed;
					mill += ts.Milliseconds;
				}
			}
			Console.WriteLine($"Custom Binary Search (ms): {mill / 1000}");
			sw.Reset();
			mill = 0;
			*/

			// STOCK BINARY SEARCH TIMING
			for (int i = 1; i <= 1000; i++)
			{
				// Recreate a and convert to b so we can sort again.
				// This is purely for timing purposes.
				a = randomize(n);
				b = Conv(a, n);
				Array.Sort(b);

				foreach (char c in name)
				{
					sw.Start();
					Array.BinarySearch(b, c);
					sw.Stop();
					TimeSpan ts = sw.Elapsed;
					mill += ts.Milliseconds;
				}
			}
			Console.WriteLine($"Stock Binary Search (ms): {mill / 1000}");
			sw.Reset();
			mill = 0;
			
			Console.ReadLine();
		}
		#region unsorted linear search
		static string[] boringSearch(char[,] input, string name)
		{
			int x = input.GetLength(0);
			int y = input.GetLength(1);
			string[] result = new string[name.Length];
			bool found = false;

			for (int k = 0; k < name.Length; k++)
			{
				for (int i = 0; i < x; i++)
				{
					for (int j = 0; j < y; j++)
					{
						found = false;
						if(name[k] == input[i,j])
						{
							result[k] = $"[{i},{j}]";
							input[i, j] = '0';
							i = x;
							j = y;
							found = true;
							break;
						}
						else
						{
							found = false;
							result[k] = "[-1,-1]";
						}
					}
				}
			}
			return result;
		}
		#endregion

		#region binary search
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		static char BinSearch(char[] input, char search, int l, int r)
		{
			int m = (r - l) / 2;

			if((m - 1) == -1)
			{
				return '0';
			}
			if (search < input[m])
			{
				BinSearch(input, search, l, m - 1);
				return '0';
			}
			else if (search > input[m])
			{
				BinSearch(input, search, m + 1, r);
				return '0';
			}
			else if (search == input[m])
			{
				return input[m];
			}
			else
				return '0';
		}
		#endregion

		#region helper methods
		static char[] Conv(char[,] input, int n)
		{
			char[] temp = new char[n * n];
			int i = 0;
			foreach (char c in input)
			{
				temp[i++] = c;
			}
			return temp;
		}

		static char[,] randomize(int n)
		{
			Random rnd = new Random();
			string ascii = "abcdefghijklmnopqrstuvwxyz";
			//int x = rnd.Next(2, 100);
			//int y = rnd.Next(2, 100);
			int x = n;
			int y = n;

			char[,] a = new char[x, y];

			for(int i = 0; i < x; i++)
			{
				for(int j = 0; j < y; j++)
				{
					a[i, j] = ascii[rnd.Next(0, 26)];
				}
			}

			return a;
		}

		static void printArray(char[] input)
		{
			for(int i = 0; i < input.Length -1; i++)
			{
				Console.WriteLine($" {input[i]}");
			}
		}
		#endregion
	}
}

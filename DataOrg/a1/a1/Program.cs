using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Enter First Name:");
			string name = Console.ReadLine();
			Console.WriteLine("Enter Array Dimension:");
			string d = Console.ReadLine();

			int n = Convert.ToInt32(d);

			name = name.ToLower();

			char[,] a = randomize(n);
			/*
			DateTime linearDT = DateTime.Now;
			string[] r = boringSearch(a, name);
			TimeSpan linearTS = DateTime.Now - linearDT;
			Console.WriteLine($"Linear Search Runtime: {linearTS}");
			*/
			/*
			int i = 0;
			foreach(object j in r)
			{
				Console.WriteLine($"{name[i++]}: {j}");
			}
			*/

			// MergeSort
			// Create new 1d array and assign with converted 2d array from Conv method.
			char[] b = Conv(a, n);
			int r = b.Length;
			MergeSort(b, 0, r, n);

			Array.BinarySearch(b, search);

			Console.ReadLine();
		}

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

		/// <summary>
		/// Recursive calling MergeSort method which uses Merge-
		/// sort to sort the newly converted 1d array.
		/// Called by Main.
		/// Calls Merge method and itself.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <param name="n"></param>
		static void MergeSort(char[] input, int l, int r, int n)
		{
			int m;
			if(l < r)
			{
				m = (l + r) / 2;
				MergeSort(input, l, m, n);
				MergeSort(input, m++, r, n);
				Merge(input, l, m++, r, n);
			}
		}

		/// <summary>
		/// Called by MergeSort.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="l"></param>
		/// <param name="m"></param>
		/// <param name="r"></param>
		/// <param name="n"></param>
		static void Merge(char[] input, int l, int m, int r, int n)
		{
			char[] temp = new char[n*n];
			int i, le, num_elements, temp_pos;

			le = (m--);
			temp_pos = l;
			num_elements = (r - l + 1);
			while ((l <= le) && (m <= r))
			{
				if (input[l] <= input[m])
					temp[temp_pos++] = input[l++];
				else
					temp[temp_pos++] = input[m++];
			}

			while (l <= le)
				temp[temp_pos++] = input[l++];

			while (m <= r)
				temp[temp_pos++] = input[m++];

			for (i = 0; i < num_elements; i++)
			{
				input[r] = temp[r];
				r--;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		static char BinSearch(char[] input, char search, int l, int r)
		{

			int m = (r - l) / 2;
			if(search < input[m])
			{
				BinSearch(input, search, l, m - 1);
				return '0';
			}
			else if(search > input[m])
			{
				BinSearch(input, search, m + 1, r);
				return '0';
			}
			else if(search == input[m])
			{
				return input[m];
			}
			else
			{
				return '0';
			}
		}

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
	}
}

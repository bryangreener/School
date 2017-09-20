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
			sort(a, n);

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

		static char[,] sort(char[,] input, int n)
		{
			char[] temp = new char[n*n];
			int i = 0;
			foreach(char c in input)
			{
				temp[i++] = c;
			}
			Array.Sort(temp);
			int l = 0;
			for(int k = 0; k < n; k++)
			{
				for (int j = 0; j < n; j++)
				{
					input[k, j] = temp[l++];
					Console.Write($" {input[k, j]}");
				}
			}

			return input;
		}

		static void binSearch(char[,] input)
		{

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

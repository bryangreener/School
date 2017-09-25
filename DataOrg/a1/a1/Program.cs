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
			// Create stopwatch item for timing use later on
			Stopwatch sw = new Stopwatch();
			int mill = 0;
		 
			// Get user input and assign to appropriate variables
			Console.WriteLine("Enter First Name:");
			string name = Console.ReadLine();
			name = name.ToLower(); // Helps prevent issues with bad input
			Console.WriteLine("Enter Array Dimension:");
			int n = Convert.ToInt32(Console.ReadLine());

			// Index arrays used to store array search results
			int[][] index = new int[name.Length][];
            int[] index2 = new int[name.Length];


            // Character arrays used to store randomized characters to be searched.
            char[,] a = new char[n, n];
            char[] b = new char[n*n];

			#region Unsorted Linear Search Timing
			// UNSORTED LINEAR SEARCH TIMING
			for (int i = 1; i <= 1000; i++) // 1000 runs for better average
			{
                // Randomize content of array a.
                a = randomize(n);

				// I want this to get recreated each loop to get wider range of searches.
				index = new int[name.Length][];

                // Start timer
				sw.Start();
                // Search array a for name and save in index
				index = boringSearch(a, name);
                // Stop timer
				sw.Stop();
                // Get time
				TimeSpan ts = sw.Elapsed;
                // Add resulting time to total time
				mill += ts.Milliseconds;
			}
            // Print average time
			Console.WriteLine($"Unsorted Linear Search (ms): {mill / 1000}");
            // Reset timer and total time for future use
            sw.Reset();
			mill = 0;

            // Print array and search results
            if (n <= 10)
            {
                PrintArray(a, n);
                PrintResult(index, name);
            }
            #endregion

            #region Stock Sort Timing
            // STOCK SORT TIMING
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
            Console.WriteLine();
			Console.WriteLine($"Merge Sort (ms): {mill / 1000}");
			sw.Reset();
			mill = 0;
			#endregion

			#region Stock Binary Search Timing
			// STOCK BINARY SEARCH TIMING
			for (int i = 1; i <= 1000; i++)
			{
				// Recreate a and convert to b so we can sort again.
				// This is purely for getting better average of runtime.
				a = randomize(n);
				b = Conv(a, n);
				Array.Sort(b);
				// I want this to get recreated each loop to get wider range of searches.
				index2 = new int[name.Length];

				// Int for stepping through index array
				int j = 0;

                // Loop through each character in name
				foreach (char c in name)
				{
					sw.Start();
                    // Built in binary search from Array.Search library. Takes in array and search item.
					index2[j] = Array.BinarySearch(b, c);
					sw.Stop();
					TimeSpan ts = sw.Elapsed;
					mill += ts.Milliseconds;
					j++;
				}
			}
            Console.WriteLine();
			Console.WriteLine($"Stock Binary Search (ms): {mill / 1000}");
			sw.Reset();
			mill = 0;
            // Revert back to jagged array for output.
			index = Revert(index2, name.Length);
            // Print Results
            if (n <= 10)
            {
                PrintArray(RevertChar(b,n),n);
                PrintResult(index, name);
            }
			#endregion

			// Prevent console from automatically closing
			Console.ReadLine();
		}

		#region unsorted linear search
        /// <summary>
        /// Searches through 2D char array and returns jagged array with
        /// resulting index in x,y format.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="name"></param>
        /// <returns></returns>
		static int[][] boringSearch(char[,] input, string name)
		{
            // Assign height and width of char array to x and y as int
			int x = input.GetLength(0);
			int y = input.GetLength(1);

            char[,] test = new char[x, y];
            test = input;

            // Jagged array used to store resulting x,y coord pairs
            int[][] result = new int[name.Length][];

			for (int k = 0; k < name.Length; k++)       // Loop through characters in name
			{
				for (int i = 0; i < x; i++)             // Loop through each x position in array
				{
					for (int j = 0; j < y; j++)         // Loop through each y position in array
					{
						if(name[k] == test[i,j])       // If matching chars
						{
                            // New entry in result array with coordinate pair
							result[k] = new int[] { i, j };

							// Prevent search from picking this result next time around
                            // WARNING: This is currently breaking program. Somehow this
                            // result saves to original array which isn't even being
                            // passed into this method. FIX ME!!!
							input[i, j] = '0';
							i = x;
							j = y;
							break;
						}
						else
						{
                            // If not found by end of loop...
							result[k] = new int[] { -1, -1 };
						}
					}
				}
			}
            // Send array back to call.
			return result;
		}
		#endregion
        
		#region helper methods
        /// <summary>
        /// Converts 2D character array to 1D array so that I can
        /// use built in Array.Search library.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="n"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Reverts a 1D array populated with index values
        /// corresponding to search result locations
        /// back to a jagged 2D array for easier output.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static int[][] Revert(int[] input, int n)
        {
            int[][] output = new int[n][];
            int x, y, j = 0;
            foreach (int i in input)
            {
                x = i % n;
                y = i / n;
                output[j] = new int[] { x, y };
                j++;
            }
            return output;
        }

        static char[,] RevertChar(char[] input, int n)
        {
            int k = 0;
            char[,] output = new char[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    output[i, j] = input[k];
                    k++;
                }
            }
            return output;
        }

        /// <summary>
        /// Populates 2D char array with random letters
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
		static char[,] randomize(int n)
		{
			Random rnd = new Random();
			string ascii = "abcdefghijklmnopqrstuvwxyz";
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

        /// <summary>
        /// Prints array of characters.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="n"></param>
		static void PrintArray(char[,] input, int n)
		{
            Console.WriteLine();
            for(int i = 0; i < n; i ++)
            {
                for(int j = 0; j < n; j++)
                {
                    Console.Write($"{input[i, j]} ");
                }
                Console.WriteLine();
            }
		}

        /// <summary>
        /// Prints name and index results found for each character in name.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="name"></param>
		static void PrintResult(int[][] input, string name)
		{
            Console.WriteLine();
            for(int i = 0; i < name.Length; i++)
            {
                Console.WriteLine($"{name[i]}: [ {input[i][1]}, {input[i][0]} ]");
            }
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace a1
{
	class Program
	{
		static void Main(string[] args)
		{
            string programContinue = "";
            int[] runN = new int[] { 5, 10, 100, 1000 };
            int[] runM = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256 };

            // Get user name and validate input
            Console.WriteLine("Enter First Name:");
            string name = Console.ReadLine();
            name = ValidateInput(name, 0);

            // UI crap
            Console.WriteLine();
            Console.WriteLine("Running...");

            // Items used to write to txt file
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream("./CS3310-A1.txt", FileMode.Create, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot open text file for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);

            do
            {
                Console.WriteLine("========================");
                Console.WriteLine("START VARIABLE DIMENSION");
                Console.WriteLine("========================");

                // VARIABLE DIMENSIONS
                foreach (int num in runN)
                {
                    Console.WriteLine();
                    Console.WriteLine($"NEW ARRAY SIZE: {num}x{num}");
                    Console.WriteLine();
                    Controller(name, num);
                }

                Console.WriteLine("========================");
                Console.WriteLine("  START VARIABLE NAME   ");
                Console.WriteLine("========================");

                // Pause file writer for user input
                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();
                try
                {
                    ostrm = new FileStream("./CS3310-A1.txt", FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(ostrm);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot open text file for writing");
                    Console.WriteLine(e.Message);
                    return;
                }

                // Get user input for dimension
                Console.WriteLine();
                Console.WriteLine("Enter Array Dimension:");
                string dimension = Console.ReadLine();
                dimension = ValidateInput(dimension, 1);
                int n = Convert.ToInt32(dimension);

                // Return output to writer
                Console.SetOut(writer);

                // VARIABLE NAME LETTERS
                foreach (int m in runM)
                {   
                    string newName = RandomLetter(name, m);
                    Console.WriteLine($"REPLACING {m} LETTERS");
                    Console.WriteLine($"NEW SEARCH: {newName}");
                    Console.WriteLine();
                    Controller(newName, n);

                }
                /*
                // Prompt user to continue or exit program
                Console.WriteLine();
                Console.WriteLine("-------------");
                Console.WriteLine("Continue? Y/N");
                programContinue = Console.ReadLine();
                programContinue = ValidateInput(programContinue, 2);
                */

                // Text file output code
                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();
                Console.WriteLine("Program completed successfully.");
                Console.WriteLine("Press enter to continue...");
                Console.Read();
            } while (programContinue == "Y");
		}

        #region main
        static void Controller(string name, int n )
        {
            // Create stopwatch item for timing use later on
            Stopwatch sw = new Stopwatch();
            TimeSpan ts = new TimeSpan();
            // int used to add milliseconds
            int mill = 0;

            // Index arrays used to store array search results
            int[] index = new int[name.Length];


            // Character arrays used to store randomized characters to be searched.
            char[,] a = new char[n, n];
            char[] b = new char[n * n];

            #region Unsorted Linear Search Timing
            // UNSORTED LINEAR SEARCH TIMING
            for (int i = 1; i <= 1000; i++) // 1000 runs for better average
            {
                // Randomize content of array a.
                a = randomize(n);
                b = Conv(a, n);

                // I want this to get recreated each loop to get wider range of searches.
                index = new int[name.Length];
                int j = 0;

                foreach (char c in name)
                {
                    // Start Timer
                    sw.Start();
                    // Get index of search result
                    index = BoringSearch(b, name);
                    // Stop Timer
                    sw.Stop();
                    // Get time difference
                    ts = sw.Elapsed;
                    // Add to total time
                    mill += ts.Milliseconds;
                    // Increment index counter
                    j++;
                }
            }
            // Print out resulting average milliseconds per search.
            Console.WriteLine($"Unsorted Search (ms): {mill / 1000}");

            // Reset timer and millisecond counter for future use.
            sw.Reset();
            mill = 0;

            // Only print if <= 10x10 array
            if (n <= 10)
            {
                // Revert array and print.
                PrintArray(RevertChar(b, n), n);
                // Revert result index array and print.
                PrintResult(Revert(index, name.Length, n), name);
            }
            #endregion

            #region Stock Sort Timing
            // STOCK SORT TIMING
            for (int i = 1; i <= 1000; i++)
            {
                a = randomize(n);
                b = Conv(a, n);

                sw.Start();
                Array.Sort(b);
                sw.Stop();
                ts = sw.Elapsed;
                mill += ts.Milliseconds;
            }
            Console.WriteLine($"Merge Sort (ms): {mill / 1000}");
            sw.Reset();
            mill = 0;
            #endregion

            #region Binary Search Timing
            // BINARY SEARCH TIMING
            for (int i = 1; i <= 1000; i++)
            {
                a = randomize(n);
                b = Conv(a, n);
                Array.Sort(b);

                index = new int[name.Length];

                foreach (char c in name)
                {
                    sw.Start();
                    index = SortedSearch(b, name);
                    sw.Stop();
                    ts = sw.Elapsed;
                    mill += ts.Milliseconds;
                }
            }
            Console.WriteLine($"Binary Search (ms): {mill / 1000}");
            Console.WriteLine();

            sw.Reset();
            mill = 0;

            if (n <= 10)
            {
                PrintArray(RevertChar(b, n), n);
                PrintResult(Revert(index, name.Length, n), name);
            }
            #endregion
        }
        #endregion

        #region unsorted linear search
        /// <summary>
        /// Searches through 2D char array and returns jagged array with
        /// resulting index in x,y format.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static int[] BoringSearch(char[] input, string name)
		{
			int[] result = new int[name.Length];
			int j = 0;
			foreach (char c in name)
			{
				for (int i = 0; i < input.Length; i++)
				{
					if (input[i] == c && !result.Contains(i))
					{
						result[j] = i;
						break;
					}
					else
					{
						result[j] = -1;
					}
				}
				j++;
			}

			// Send array back to call.
			return result;
		}
		#endregion

		#region sorted binary search
		/// <summary>
		/// Searches 1D sorted array using Binary Search algorithm.
		/// Skips duplicate items in result.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		static int[] SortedSearch(char[] input, string name)
		{
            // Result array stores index of found characters
			int[] result = new int[name.Length];
            // Preload result array with -1 to prevent issues with comparison later on
			for(int j = 0; j < result.Length; j++)
			{
				result[j] = -1;
			}

            // Counter for result index
			int i = 0;
			foreach(char c in name)
			{
				int l = 0, m = 0, r = (input.Length);

                // Standard binary search algorithm
				while(l < r)
				{
					m = l + (r - l) / 2;
                    // Added comparison to verify that we get new index and not a duplicate
					if(input[m] == c && !result.Contains(m))
					{
						result[i] = m;
						r = m;
					}
					else if(input[m] > c)
					{
						r = m;
					}
					else
					{
						l = m + 1;
					}
				}
				i++;
			}

			return result;
		}
		#endregion

		#region helper methods

        /// <summary>
        /// Method that validates all input to verify it won't cause errors.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ic"></param>
        /// <returns></returns>
		static string ValidateInput(string input, int ic)
		{
			bool valid = false;

            // No empty string allowed here
			while(input == "")
			{
				Console.WriteLine("No Blank Input Allowed");
				Console.WriteLine("Please Type New Input:");
				input = Console.ReadLine();
			}

			switch(ic)
			{
				case 0:
                    // Check if input is all letters then remove spaces and change to lowercase
					while (valid == false)
					{
						if (input.Any(char.IsDigit))
						{
							Console.WriteLine("Name Cannot Contain Any Digits");
							Console.WriteLine("Enter First Name:");
							input = Console.ReadLine();
						}
						else
						{
							input = input.ToLower();
                            input = input.Replace(" ", "");
							valid = true;
						}
					}
					break;
				case 1:
                    // Check if input has no letters and is >= 2
					while (valid == false)
					{
						int temp = 0;
						if(!int.TryParse(input, out temp))
						{
							Console.WriteLine("Dimension Cannot Contain Any Letters");
							Console.WriteLine("Enter Array Dimension:");
							input = Console.ReadLine();
						}
						if(temp < 2)
						{
							Console.WriteLine("Dimension Must Be >= 2");
							Console.WriteLine("Enter Array Dimension:");
							input = Console.ReadLine();
						}
						else
						{
                            valid = true;
						}
					}
					break;
                case 2:

                    // Remove case sensitive input and either close or restart program
                    if(input.ToUpper() == "Y")
                    {
                        Console.WriteLine();
                        input = "Y";
                    }
                    else if(input.ToUpper() == "N")
                    {
                        Console.WriteLine("!!   CLOSING  APPLICATION   !!");
                        Console.WriteLine("Press ENTER to close window...");
                        System.Environment.Exit(0);
                    }
                    break;
			}

			return input;
		}

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
		static int[][] Revert(int[] input, int name, int n)
		{
            // Create jagged array
			int[][] output = new int[name][];
			int x, y, j = 0;
			foreach (int i in input)
			{
				if(i < 0)
				{
                    // For failed search items
					output[j] = new int[] { -1, -1 };
				}
				else
				{
                    // Formula to convert from 1D array to 2D x,y coords
					y = i % n;
					x = i / n;
                    // Load values to array
					output[j] = new int[] { x, y };
				}
				j++;
			}
			return output;
		}
		
		/// <summary>
		/// Reverts 1D array to 2D array
		/// </summary>
		/// <param name="input"></param>
		/// <param name="n"></param>
		/// <returns></returns>
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
        /// Returns random character.
        /// Used for second part of assignment where we have to randomly
        /// replace letters in name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string RandomLetter(string name, int n)
        {
            StringBuilder sb = new StringBuilder(name);
            Random rnd = new Random();
            string ascii = "abcdefghijklmnopqrstuvwxyz";
            
            for(int i = 1; i <= n; i++)
            {
                char c = ascii[rnd.Next(0, ascii.Length)];
                int r = rnd.Next(0, name.Length);

                sb.Remove(r, 1);
                sb.Insert(r, c);
            }
            return sb.ToString();
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

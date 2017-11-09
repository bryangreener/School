using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace a4
{
	/// <summary>
	/// Contains only the Main method which calls a new instance of the Controller class.
	/// </summary>
    class Program
    {
		/// <summary>
		/// Handles txt file input and splits input into var.
		/// Also handles looping Controller call to allow program to repeat.
		/// </summary>
		/// <param name="args">default param</param>
        static void Main(string[] args)
        {
			UI ui = new UI();
			Tests tests = new Tests();
			File.Delete("./BENCHMARK.txt");	// Delete benchmark file if it exists.

			// Temp save namelist.txt into string.
			string text = File.ReadAllText("namelist.txt");
			// Split text string by line
			var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			// Create new jagged array. Number of elements is equal to number of elements in lines variable
			string[][] input = new string[lines.Length][];

			// For each lines in lines, split line at TAB and save into array
			for (int i = 0; i < lines.Length; i++)
			{
				input[i] = lines[i].Split('\t');
			}

			// Infinite loop that prevents program from closing unless user requests exit.
			while (true)
			{
				Console.Clear();
				// NOTE: LAST,FIRST order so input item1 is LAST NAME
				// Get search tuple from UI.
				Tuple<string, string> search = ui.Main();
				
				// If option 3 is selected...
				if (search != null && search.Item1 == "run" && search.Item2 == "tests")
				{
					// Run through tests class which calls all other classes in program to perform operations on trees.
					Console.WriteLine("Running benchmark tests. This will take a while...");
					tests.Main();
					Console.WriteLine("Saved benchmamrk results to bin/debug/BENCHMARK.txt");
					Console.WriteLine();
					Console.WriteLine("Press any key to continue...");
					// Wait for user input then loop back through program.
					while (Console.ReadKey() == null) { Console.ReadKey(); }
				}
				else
				{
					Controller c = new Controller(input, search);               // New instance of Controller class, passing in input array
					Console.WriteLine("Press any key to continue...");
					while (Console.ReadKey() == null) { Console.ReadKey(); }    // Prevents console from closing until key is pressed.
				}
			}
        }
		
    }

	/// <summary>
	/// Class used to control multiple loops of program using different size trees.
	/// Entirely used as a benchmark or test of the algorithms for analysis report.
	/// </summary>
	class Tests
	{
		/// <summary>
		/// Main method in tests class.
		/// Creates new instance of controller for each size of test string.
		/// </summary>
		public void Main()
		{
			// Different tree sizes saved in array
			int[] testCounts = new int[] { 1, 10, 100, 1000, 10000, 100000, 250000, 500000, 1000000 };
			foreach(int i in testCounts)	// For each tree size
			{
				string[][] testStrings = Main(i, i.ToString());	// Create new string with random values
				Controller c = new Controller(testStrings, Tuple.Create("run", "tests"));	// Pass into controller class
			}
		}

		public string[][] Main(int size, string filename)
		{
			// Create new file stream for saving random name lists.
			FileStream ostrm;
			StreamWriter writer;

			string ascii = "abcdefghijklmnopqrstuvwxyz";    // Ascii string used to generate random strings.
			string[][] randomWords = new string[size][];	// Create new jagged array of specified size.
			Random rnd = new Random();						// New random to be used to generate random string.	
			for (int i = 0; i < size; i++)
			{
				string tempWord = "";	// Create string to be randomly generated.
				for (int j = 0; j < 100; j++)	// String length will be 100 chars long.
				{
					tempWord += ascii[rnd.Next(0, 26)];	// Randomly generate character and append to tempWord string.
				}
				randomWords[i] = new string[] { tempWord, tempWord };	// Save tempword in jagged array. Duplicate for first and last name.
			}

			// Set up file stream to output string array to text file.
			TextWriter oldOut = Console.Out;
			ostrm = new FileStream($"./{filename}.txt", FileMode.Create, FileAccess.Write);
			writer = new StreamWriter(ostrm);
			Console.SetOut(writer);

			// Output names to text file.
			foreach (var v in randomWords)
			{
				Console.WriteLine($"{v[0]}\t{v[0]}");
			}

			Console.SetOut(oldOut);
			writer.Close();
			ostrm.Close();

			// Return string array.
			return randomWords;
		}
	}

	/// <summary>
	/// This class controls nearly all console and txt file outputs and user inputs.
	/// Called from Controller class methods and returns values to Controller methods.
	/// </summary>
	class UI
	{
		#region Input Methods
		/// <summary>
		/// Main menu method that prints the main menu and asks for user to select an option.
		/// This method can call a secondary menu method and an input validation method.
		/// </summary>
		/// <returns>Tuple of strings containing first and last name.</returns>
		public Tuple<string,string> Main()
		{
			HR();	// Call method to display horizontal rule. This is used often so I won't comment it every time.
			Console.WriteLine("ENTER OPTION NUMBER");
			Console.WriteLine("1) Print all items");
			Console.WriteLine("2) Search for Name");
			Console.WriteLine("3) Run Search Tests !LONG!");
			Console.WriteLine("4) Exit");

			// Switch that handles possible inputs after stripping input of escape chars and sending the user input to a validate method
			switch (ValidateMain(Regex.Escape(Console.ReadLine())))
			{
				case 1:	// Print all
					Console.WriteLine("Saving output to /bin/debug/output.txt");
					break;
				case 2:	// Search
					return SearchMenu();    // Call secondary menu method and return its result to method call
				case 3: // Tests
					return Tuple.Create("run", "tests");
				case 4:	// Exit
					Console.WriteLine("Exiting application. Enter c to cancel.");
					Console.WriteLine("Press enter to exit.");
					string response = Console.ReadLine();
					if (response.ToLower() == "c") { Main(); }  // If response is c, then we return to the main menu
					else { Environment.Exit(0); }               // Otherwise, exit the program
					break;
			}
			return null;
		}

		/// <summary>
		/// Secondary menu that asks for user to input first and last name to search for.
		/// </summary>
		/// <returns>Tuple of strings containing first and last name.</returns>
		private Tuple<string,string> SearchMenu()
		{
			HR();
			Console.WriteLine("ENTER FIRST NAME");
			string first = Regex.Escape(Console.ReadLine());
			Console.WriteLine("ENTER LAST NAME");
			string last = Regex.Escape(Console.ReadLine());
			// Return the result of validating the first and last names.
			return ValidateSearch(first, last);
		}

		/// <summary>
		/// Method that validates main menu input number to verify that it is an integer between 1 and 3.
		/// </summary>
		/// <param name="input">Input string from Main() method.</param>
		/// <returns>Integer with valid menu selection.</returns>
		private int ValidateMain(string input)
		{
			int n;
			// Try parsing input as an integer. If successful, save to int n. Also verify input isnt empty and it is between 1 and 3.
			if (!int.TryParse(input, out n) || n < 1 || n > 4 || input == "")
			{
				// If any of these conditions fails, print error and call Main menu method again to ask for correct input.
				Console.WriteLine("=========================================");
				Console.WriteLine("            ! INVALID INPUT !            ");
				Console.WriteLine("Input must be an integer between 1 and 3.");
				Console.WriteLine("=========================================");
				Main();
			}
			// If input is valid, return the menu option selected to the Main method.
			return n;
		}

		/// <summary>
		/// This method validates the first and last names to verify they don't contain numbers or null characters.
		/// </summary>
		/// <param name="first">First name passed in from SearchMenu</param>
		/// <param name="last">Last name passed in from SearchMenu</param>
		/// <returns>Tuple of strings containing first and last name.</returns>
		private Tuple<string, string> ValidateSearch(string first, string last)
		{
			// If any character in first or last name is a number, or if either are blank...
			if (first.Any(c => char.IsDigit(c)) || last.Any(c => char.IsDigit(c)) || first == "" || last == "")
			{
				// Write an error and call the SearchMenu method again to ask for correct first and last names.
				Console.WriteLine("=========================================");
				Console.WriteLine("            ! INVALID INPUT !            ");
				Console.WriteLine("Input must be nonempty string of letters.");
				Console.WriteLine("=========================================");
				SearchMenu();
			}
			// Remove all leading and trailing whitespace from first and last names then convert them to lowercase characters.
			first = first.Trim().ToLower();
			last = last.Trim().ToLower();
			return Tuple.Create(first, last);   // Return the new valid first and last name.
		}
		#endregion

		public void PrintBuildTimes(int items, double minHeapBuildtime, double maxHeapBuildTime, double bstBuildTime)
		{
			HR();
			Console.WriteLine($"TREE BUILD TIMES FOR {items} ITEMS");
			Console.WriteLine();
			Console.WriteLine("".PadRight(10, ' ') + "MINHEAP".PadRight(15, ' ') + "MAXHEAP".PadRight(15, ' ') + "BST");
			Console.WriteLine("TIME (ms)".PadRight(10, ' ') +
				Math.Round(minHeapBuildtime, 4).ToString().PadRight(15, ' ') +
				Math.Round(maxHeapBuildTime, 4).ToString().PadRight(15, ' ') +
				Math.Round(bstBuildTime, 4).ToString());
			HR();
		}
		/// <summary>
		/// This method takes in a lot of parameters from the Controller class and prints to console in a pretty table.
		/// </summary>
		/// <param name="name">Tuple of strings containing first and last name.</param>
		/// <param name="minPos">Tuple of integers containing X and Y coordinates in MinHeap.</param>
		/// <param name="maxPos">Tuple of integers contianing X and Y coordinates in MaxHeap.</param>
		/// <param name="bstPos">Tuple of integers containing X and Y coordinates in Binary Search Tree.</param>
		/// <param name="minDFS">Double containing total MinHeap Depth First Search time in milliseconds.</param>
		/// <param name="minBFS">Double containing total MinHeap Breadth First Search time in milliseconds.</param>
		/// <param name="maxDFS">Double containing total MaxHeap Depth First Search time in milliseconds.</param>
		/// <param name="maxBFS">Double containing total MaxHeap Breadth First Search time in milliseconds.</param>
		/// <param name="bts">Double containing total Binary Search Tree Search time in milliseconds.</param>
		public void PrintRndSearch(Tuple<string, string> name, Tuple<int, int> minPos, Tuple<int, int> maxPos, Tuple<int, int> bstPos, double minDFS, double minBFS, double maxDFS, double maxBFS, double bts)
		{
			HR();
			Console.WriteLine("AVERAGE SEARCH TIMES FOR UPWARD OF 1,000 RANDOM NAMES"); // Display first and last name being searched
			Console.WriteLine();
			// Header
			Console.WriteLine("".PadRight(10, ' ') + "MINHEAP".PadRight(15, ' ') + "MAXHEAP".PadRight(15, ' ') + "BST");
			// Print out search times and X Y coordinates for search item using string formatting to create a table.
			Console.WriteLine("BFS (ms)".PadRight(10, ' ') + Math.Round(minBFS, 4).ToString().PadRight(15, ' ') + Math.Round(maxBFS, 4).ToString().PadRight(15, ' ') + "n/a");
			Console.WriteLine("DFS (ms)".PadRight(10, ' ') + Math.Round(minDFS, 4).ToString().PadRight(15, ' ') + Math.Round(maxDFS, 4).ToString().PadRight(15, ' ') + "n/a");
			Console.WriteLine("BTS (ms)".PadRight(10, ' ') + "n/a".PadRight(15, ' ') + "n/a".PadRight(15, ' ') + Math.Round(bts, 4).ToString());
			HR();
		}

		/// <summary>
		/// This method takes in a lot of parameters from the Controller class and prints to console in a pretty table.
		/// </summary>
		/// <param name="name">Tuple of strings containing first and last name.</param>
		/// <param name="minPos">Tuple of integers containing X and Y coordinates in MinHeap.</param>
		/// <param name="maxPos">Tuple of integers contianing X and Y coordinates in MaxHeap.</param>
		/// <param name="bstPos">Tuple of integers containing X and Y coordinates in Binary Search Tree.</param>
		/// <param name="minDFS">Double containing total MinHeap Depth First Search time in milliseconds.</param>
		/// <param name="minBFS">Double containing total MinHeap Breadth First Search time in milliseconds.</param>
		/// <param name="maxDFS">Double containing total MaxHeap Depth First Search time in milliseconds.</param>
		/// <param name="maxBFS">Double containing total MaxHeap Breadth First Search time in milliseconds.</param>
		/// <param name="bts">Double containing total Binary Search Tree Search time in milliseconds.</param>
		public void PrintNameSearch(Tuple<string,string> name, Tuple<int,int> minPos, Tuple<int,int> maxPos, Tuple<int,int> bstPos, double minDFS, double minBFS, double maxDFS, double maxBFS, double bts)
		{
			HR();
			Console.WriteLine($"SEARCH FOR {name.Item1} {name.Item2}");	// Display first and last name being searched
			Console.WriteLine();
			// Header
			Console.WriteLine("".PadRight(10, ' ') + "MINHEAP".PadRight(15, ' ') + "MAXHEAP".PadRight(15, ' ') + "BST");
			// Print out search times and X Y coordinates for search item using string formatting to create a table.
			Console.WriteLine("  (X, Y)".PadRight(10, ' ') + minPos.ToString().PadRight(15, ' ') + maxPos.ToString().PadRight(15, ' ') + bstPos.ToString().PadRight(15, ' '));
			Console.WriteLine("BFS (ms)".PadRight(10, ' ') + Math.Round(minBFS, 4).ToString().PadRight(15, ' ') + Math.Round(maxBFS, 4).ToString().PadRight(15, ' ') + "n/a");
			Console.WriteLine("DFS (ms)".PadRight(10, ' ') + Math.Round(minDFS, 4).ToString().PadRight(15, ' ') + Math.Round(maxDFS, 4).ToString().PadRight(15, ' ') + "n/a");
			Console.WriteLine("BTS (ms)".PadRight(10, ' ') + "n/a".PadRight(15, ' ') + "n/a".PadRight(15, ' ') + Math.Round(bts, 4).ToString());
			HR();
		}

		#region Text Outputs
		/// <summary>
		/// Void method that simply prints a horizontal rule made up of 45 '_' characters.
		/// </summary>
		public void HR()
		{
			Console.WriteLine("".PadRight(45, '_'));
		}

		/// <summary>
		/// Console output header for min heap results
		/// </summary>
		public void MinHeapHeader()
		{
			Console.WriteLine("======================");
			Console.WriteLine("MIN HEAP RESULTS BELOW");
			Console.WriteLine("======================");
			Console.WriteLine();
			HR();
		}

		/// <summary>
		///  Console output header for max heap results
		/// </summary>
		public void MaxHeapHeader()
		{
			Console.WriteLine("======================");
			Console.WriteLine("MAX HEAP RESULTS BELOW");
			Console.WriteLine("======================");
			Console.WriteLine();
			HR();
		}

		/// <summary>
		/// Console output header for BST results
		/// </summary>
		public void BSTHeader()
		{
			Console.WriteLine("=================");
			Console.WriteLine("BST RESULTS BELOW");
			Console.WriteLine("=================");
			Console.WriteLine();
			HR();
		}

		/// <summary>
		/// Min/Max Heap search results header for when Main Menu option 1 is selected.
		/// </summary>
		public void SR()
		{
			Console.WriteLine("SEARCH RESULTS".PadRight(26, ' ') + "DFS       BFS");
			Console.WriteLine("".PadRight(25, ' ') + "(X, Y)    (X, Y)");
			HR();
		}

		/// <summary>
		/// BST search results header for when Main Menu option 1 is selected.
		/// </summary>
		public void SRBST()
		{
			Console.WriteLine("SEARCH RESULTS".PadRight(26, ' ') + "BTS");
			Console.WriteLine("".PadRight(25, ' ') + "(X, Y)");
			HR();
		}
		#endregion
	}

	/// <summary>
	/// This is the main class that contains methods that controls tree functions and searches.
	/// </summary>
	class Controller
	{
		public Controller() { }
		
		// Controller class variables. Input is a jagged array and ui is a new instance of the UI class which automatically calls Main() method.
		private string[][] Input { get; set; }
		UI ui = new UI();
		
		/// <summary>
		/// This method calls each tree method and handles filestream output and passes input array to each tree method.
		/// </summary>
		/// <param name="input">Jagged array containing all names.</param>
		/// <param name="search">Tuple of strings containing first and last name to search.</param>
		public Controller(string[][] input, Tuple<string,string> search)
		{
			// File items used to output to a text file and control console output.
			FileStream ostrm;
			StreamWriter writer;
			Stopwatch sw = new Stopwatch();
			Random rnd = new Random();
			double minHeapBuildTime = 0, maxHeapBuildTime = 0, bstBuildTime = 0;

			// Set input to the global Input object.
			Input = input;

			// Initialize each of the tree structures.
			sw.Start();
			MinHeap minHeap = MinHeapInit();
			sw.Stop();
			minHeapBuildTime = sw.Elapsed.TotalMilliseconds;
			sw.Reset();

			sw.Start();
			MaxHeap maxHeap = MaxHeapInit();
			sw.Stop();
			maxHeapBuildTime = sw.Elapsed.TotalMilliseconds;
			sw.Reset();

			sw.Start();
			BST bst = BSTInit();
			sw.Stop();
			bstBuildTime = sw.Elapsed.TotalMilliseconds;
			sw.Reset();

			
			if(search == null) // If user chose option 1 instead of 2 or 3 in menu (print all instead of search)...
			{
				// Save current console output for later use.
				TextWriter oldOut = Console.Out;
				// Try creating/writing to a text file in bin/debug/ folder for console output.
				try
				{
					// If file exists, edit. If not, create.
					ostrm = new FileStream("./output.txt", FileMode.Create, FileAccess.Write);
					writer = new StreamWriter(ostrm);
				}
				catch (Exception e)	// Throws error if file is locked or anything else that prevents FileStream from accessing it.
				{
					Console.WriteLine("Cannot open output.txt for editing.");
					Console.WriteLine(e.Message);
					return;
				}
				// Set console output to write to text file.
				Console.SetOut(writer);

				// Call each print method for each type of tree and add ui headers for extra prettiness points.
				ui.MinHeapHeader();
				MinHeapPrint(minHeap);
				ui.MaxHeapHeader();
				MaxHeapPrint(maxHeap);
				ui.BSTHeader();
				BSTPrint(bst);
				ui.HR();

				// Give output control back to console and close stream/writer.
				Console.SetOut(oldOut);
				writer.Close();
				ostrm.Close();
			}
			else if (search != null && search.Item1 == "run" && search.Item2 == "tests")
			{	// If running tests (option 3)
				// Change console output and create file names Benchmark.txt.
				TextWriter oldOut = Console.Out;
				ostrm = new FileStream($"./BENCHMARK.txt", FileMode.Append, FileAccess.Write);
				writer = new StreamWriter(ostrm);
				Console.SetOut(writer);

				// Print saved build times using method in ui class
				ui.PrintBuildTimes(input.Length, minHeapBuildTime, maxHeapBuildTime, bstBuildTime);
				// Search for random names to get an average.
				SearchName(minHeap, maxHeap, bst, search);

				// Set output back to console.
				Console.SetOut(oldOut);
				writer.Close();
				ostrm.Close();
			}
			else // OTherwise just search for name.
			{
				SearchName(minHeap, maxHeap, bst, search);
			}

			// Search each tree back to null just to prevent issues with it not being overwritten.
			minHeap = null;
			maxHeap = null;
			bst = null;
		}

		/// <summary>
		/// Initializes MinHeap by inserting first and last names.
		/// </summary>
		/// <returns>MinHeap sent back to Controller that is used in output.</returns>
		private MinHeap MinHeapInit()
		{
			// Create new MinHeap
			MinHeap heap = new MinHeap();
			// For each name in input...
			foreach (var v in Input)
			{
				// Insert a new Tuple<string,string> into heap. (v[1] is last name, v[0] is first name)
				heap.Insert(Tuple.Create(v[1].ToLower(), v[0].ToLower()));
			}
			
			// Call method in heap that traverses tree and assigns x,y coords to each.
			heap.AssignXY(heap.ReturnRoot(), 1, 1);

			// Return entire heap to controller.
			return heap;
		}

		/// <summary>
		/// Prints MinHeap in formatted style.
		/// Searches for last name.
		/// </summary>
		/// <param name="heap">This is the heap that was returned from initializing the MinHeap.</param>
		/// <returns>MinHeap sent back to Controller method.</returns>
		private MinHeap MinHeapPrint(MinHeap heap)
		{
			ui.SR();    // Print search result header.
			// For each name in Input array.
			foreach (var v in Input)
			{
				// Outputs a formatted string that uses $ to allow us to call searches from within the string.
				// This isn't offloaded to UI class since it is accessing the heap methods.
				Console.WriteLine($"{v[0].PadRight(25, ' ')}" + $"{heap.DFS(v[0].ToLower())}".PadRight(10, ' ') + $"{heap.BFS(v[0].ToLower())}");
			}

			ui.HR();
			heap.Traverse();	// Run through preorder, inorder, and postorder traversals in MinHeap class.
			return heap;		// Return heap to controller method.
		}

		/// <summary>
		/// Initializes MaxHeap by inserting first and last names.
		/// </summary>
		/// <returns>MaxHeap sent back to Controller that is used in output.</returns>
		private MaxHeap MaxHeapInit()
		{
			// Create new MaxHeap and send in Input array and max length possible with number of inputs in array.
			MaxHeap heap = new MaxHeap(Input, Input.Length*2 + 2);

			heap.AssignXY(0, 1, 1);	// Assign XY values to each node.

			return heap;			// Returns heap to controller method.
		}

		/// <summary>
		/// Prints MaxHeap in formatted style.
		/// This method is identical to the MinHeapPrint method except it is searching for first name instead of last.
		/// </summary>
		/// <param name="heap">This is the heap that was returned from initializing the MaxHeap.</param>
		/// <returns>MaxHeap sent back to Controller method.</returns>
		private MaxHeap MaxHeapPrint(MaxHeap heap)
		{
			ui.SR();
			foreach (var v in Input)
			{
				Console.WriteLine($"{v[1].PadRight(25, ' ')}" + $"{heap.DFS(v[1].ToLower())}".PadRight(10, ' ') + $"{heap.BFS(v[1].ToLower())}");
			}

			ui.HR();
			heap.Traverse();

			return heap;
		}

		/// <summary>
		/// Initializes BST by inserting first and last names.
		/// This is identical to the MinHeapInit method.
		/// </summary>
		/// <returns>BST sent back to Controller that is used in output.</returns>
		private BST BSTInit()
		{
			BST bst = new BST();
			foreach(var v in Input)
			{
				bst.Insert(Tuple.Create(v[1].ToLower(), v[0].ToLower()));
			}
			// Print results
			bst.AssignXY(bst.ReturnRoot(), 1, 1);

			return bst;
		}

		/// <summary>
		/// Prints BST in formatted style.
		/// This method is identical to the MinHeapPrint method except it only calls a single search type.
		/// </summary>
		/// <param name="bst">This is the bst that was returned from initializing the BST.</param>
		/// <returns>BST sent back to Controller method.</returns>
		private BST BSTPrint(BST bst)
		{
			ui.SRBST();	// Search Result header for BST
			foreach (var v in Input)
			{
				Console.WriteLine($"{v[0].PadRight(25, ' ')}" + $"{bst.Get(v[0].ToLower())}");
			}

			ui.HR();
			bst.Traverse();
			return bst;
		}

		/// <summary>
		/// Searches for a user specified name.
		/// </summary>
		/// <param name="min">MinHeap passed in by controller.</param>
		/// <param name="max">MaxHeap passed in by controller.</param>
		/// <param name="bst">BST passed in by controller.</param>
		/// <param name="name">Tuple of strings containing first and last name.</param>
		private void SearchName(MinHeap min, MaxHeap max, BST bst, Tuple<string,string> name)
		{
			// Declare stopwatch, timers, total runtime variables, and tuples to be used throughout this method.
			Random rnd = new Random();
			Stopwatch sw = new Stopwatch();
			double minDFS = 0, minBFS = 0, maxDFS = 0, maxBFS = 0, bts = 0;
			Tuple<int, int> minPos = null, maxPos = null, bstPos = null;

			// If running tests...
			if (name.Item1 == "run" && name.Item2 == "tests")
			{
				int loops = 100;    // Can set to 1000 but runs extremely slow.
				for (int i = 0; i < loops; i++)	
				{
					int randomIndex = rnd.Next(0, Input.Length);
					// Each of these blocks are nearly identical. They simply call different methods.

					sw.Start();                             // Start stopwatch
					minPos = min.DFS(name.Item2);           // Find last name in MinHeap using DFS.
					sw.Stop();                              // Stop stopwatch
					minDFS += sw.Elapsed.TotalMilliseconds; // Accumulate minDFS total.
					sw.Reset();                             // Reset stopwatch

					// MinHeap BFS (last name)
					sw.Start();
					min.BFS(name.Item2);
					sw.Stop();
					minBFS += sw.Elapsed.TotalMilliseconds;
					sw.Reset();

					// MaxHeap DFS (first name)
					sw.Start();
					maxPos = max.DFS(name.Item1);
					sw.Stop();
					maxDFS += sw.Elapsed.TotalMilliseconds;
					sw.Reset();

					// MaxHeap BFS (first name)
					sw.Start();
					max.BFS(name.Item1);
					sw.Stop();
					maxBFS += sw.Elapsed.TotalMilliseconds;
					sw.Reset();

					// BST BTS (last name)
					sw.Start();
					bstPos = bst.Get(name.Item2);
					sw.Stop();
					bts += sw.Elapsed.TotalMilliseconds;
					sw.Reset();
				}
				// Used for extremely precise measurement by ticks.
				//decimal frequency = Stopwatch.Frequency;
				//decimal nsPerTick = (1000 * 1000 * 1000) / frequency;

				// Call ui method that prints table with passed in search results, calculating average as well.
				ui.PrintRndSearch(name, minPos, maxPos, bstPos, minDFS/loops, minBFS/loops, maxDFS/loops, maxBFS/loops, bts/loops);
			}
			else	// Otherwise, search as normal for single key.
			{
				// Each of these blocks are nearly identical. They simply call different methods.

				sw.Start();                             // Start stopwatch
				minPos = min.DFS(name.Item2);           // Find last name in MinHeap using DFS.
				sw.Stop();                              // Stop stopwatch
				minDFS = sw.Elapsed.TotalMilliseconds;  // Save total milliseconds during search.
				sw.Reset();                             // Reset stopwatch

				// MinHeap BFS (last name)
				sw.Start();
				min.BFS(name.Item2);
				sw.Stop();
				minBFS = sw.Elapsed.TotalMilliseconds;
				sw.Reset();

				// MaxHeap DFS (first name)
				sw.Start();
				maxPos = max.DFS(name.Item1);
				sw.Stop();
				maxDFS = sw.Elapsed.TotalMilliseconds;
				sw.Reset();

				// MaxHeap BFS (first name)
				sw.Start();
				max.BFS(name.Item1);
				sw.Stop();
				maxBFS = sw.Elapsed.TotalMilliseconds;
				sw.Reset();

				// BST BTS (last name)
				sw.Start();
				bstPos = bst.Get(name.Item2);
				sw.Stop();
				bts = sw.Elapsed.TotalMilliseconds;
				sw.Reset();

				// Used for extremely precise measurement by ticks.
				//decimal frequency = Stopwatch.Frequency;
				//decimal nsPerTick = (1000 * 1000 * 1000) / frequency;

				// Call ui method that prints table with passed in search results.
				ui.PrintNameSearch(name, minPos, maxPos, bstPos, minDFS, minBFS, maxDFS, maxBFS, bts);
			}
		}
	}

	/// <summary>
	/// Class Object used to create nodes for trees.
	/// Used by both BST and MinHeap classes.
	/// </summary>
	class Node
	{
		/// <summary>
		/// Default constructor used to create a null node.
		/// </summary>
		public Node() { }

		/// <summary>
		/// Constructor that only takes in a name.
		/// </summary>
		/// <param name="value">Tuple of strings containing first and last name.</param>
		public Node(Tuple<string, string> value)
		{
			Value = value;
		}

		/// <summary>
		/// Constructor that takes in a name and a value n.
		/// </summary>
		/// <param name="value">Tuple of strings containing first and last name.</param>
		/// <param name="n">Integer referencing subtree size.</param>
		public Node(Tuple<string, string> value, int n)
		{
			Value = value;
			N = n;
		}

		/// <summary>
		/// Accessor/Mutator for Tuple of strings containing first and last name.
		/// </summary>
		public Tuple<string, string> Value { get; set; }
		/// <summary>
		/// Accessor/Mutator for integer refering to subtree size.
		/// </summary>
		public int N { get; set; }
		/// <summary>
		/// Accessor/Mutator for integer refering to X position of node in a row.
		/// </summary>
		public int X { get; set; }
		/// <summary>
		/// Accessor/Mutator for integer refering to Y height of node in a tree.
		/// </summary>
		public int Y { get; set; }
		/// <summary>
		/// Accessor/Mutator for Node object refering to parent of current node.
		/// </summary>
		public Node Parent { get; set; }
		/// <summary>
		/// Accessor/Mutator for Node object refering to left child of current node.
		/// </summary>
		public Node Left { get; set; }
		/// <summary>
		/// Accessor/Mutator for Node object refering to right child of current node.
		/// </summary>
		public Node Right { get; set; }

		#region RedBlack BST Code (UNUSED)
		// Following code is only used in RedBlack BST
		
		public Node(Tuple<string, string> value, int n, bool color)
		{
			Value = value;
			N = n;
			Color = color;
		}
		public bool Color { get; set; }
		
		#endregion
	}

	/// <summary>
	/// Class containing methods containing all operations of a min heap using a binary tree implementation.
	/// </summary>
	class MinHeap
	{
		private Node root;	// Create new Node object for root of tree.

		#region Public Methods
		/// <summary>
		/// Public method that gets called from outside classes.
		/// Calls InsertUtil private method which then inserts a value.
		/// </summary>
		/// <param name="val">Tuple of strings containing first and last name.</param>
		public void Insert(Tuple<string,string> val)
		{
			root = InsertUtil(root, val);	// Pass root node and val tuple into InsertUtil.
		}

		/// <summary>
		/// Public method that gets called from outside classes.
		/// Calls DFSUtil private method which uses Depth First Search to find a node with specified last name.
		/// </summary>
		/// <param name="last">Last name to search.</param>
		/// <returns>Tuple of ints returned to Controller class. Contains X,Y coordinate of search result. (-1,-1) if search miss.</returns>
		public Tuple<int, int> DFS(string last)
		{
			Node h = DFSUtil(root, last);	// Pass root and last name into DFSUtil. Save as a new temp node.
			if (h == null) { return Tuple.Create(-1, -1); }	// If search returns a null node, return new tuple with -1,-1 to indicate search miss.
			else { return Tuple.Create(h.X, h.Y); }			// Else, return the returned search hit X and Y positions as a new tuple.
		}

		/// <summary>
		/// Public method that gets called from outside classes.
		/// Calls BFSUtil private method which uses Breadth First Search to find a node with the specified last name.
		/// </summary>
		/// <param name="last">Last name to search.</param>
		/// <returns>Tuple of ints returned to Controller class. Contains X,Y coordinates of search result. (-1,-1) if search miss.</returns>
		public Tuple<int, int> BFS(string last)
		{
			return BFSUtil(root, last);	// Pass root and last name into BFSUtil and return the tuple<int,int> (coordinate) result.
		}

		// TRY TO IMPLEMENT THIS FUNCTIONALITY IN INSERT METHOD
		public void AssignXY(Node h, int x, int y)
		{
			if (h == null) { return; }
			h.X = x;
			h.Y = y;
			AssignXY(h.Left, 2 * x - 1, y + 1);
			AssignXY(h.Right, 2 * x, y + 1);
		}

		/// <summary>
		/// Public method called by outside classes.
		/// Calls each of the private traversal methods and separate with headers.
		/// </summary>
		public void Traverse()
		{
			Console.WriteLine("=== PREORDER  ===");
			PreOrder(root);
			Console.WriteLine();
			Console.WriteLine("=== INORDER   ===");
			InOrder(root);
			Console.WriteLine();
			Console.WriteLine("=== POSTORDER ===");
			PostOrder(root);
		}

		/// <summary>
		/// Helper method used to return root to call. This allows us to pass the root node into a method like BFS or DFS.
		/// </summary>
		/// <returns></returns>
		public Node ReturnRoot()
		{
			return root;
		}

		/// <summary>
		/// Helper method that calls private Size method.
		/// </summary>
		/// <returns>Returns root's N property.</returns>
		public int Size()
		{
			return Size(root);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Called by Insert public method.
		/// Inserts a given value into the next node in a size balanced tree.
		/// On insert, the node sifts up the tree until its parent is a smaller value.
		/// </summary>
		/// <param name="h">Node initially passed in as root then is changed during recursive call.</param>
		/// <param name="val">Tuple of strings containing first and last name.</param>
		/// <returns></returns>
		private Node InsertUtil(Node h, Tuple<string,string> val)
		{
			Node newNode = new Node();					// Create new node to help swap nodes around without overwriting one.
			if(h == null) { return new Node(val, 1); }	// If current node position doesnt exist, create new node with value and subtree size of 1.
			if(h.Left == null || h.Right == null)		// If either left or right child is null...
			{
				if(h.Left == null) { newNode = h.Left = InsertUtil(h.Left, val); newNode.Parent = h; }	// If left child is null, create new node and save as left child and newNode, maintaining links.
				else { newNode = h.Right = InsertUtil(h.Right, val); newNode.Parent = h; }				// If right child is null, create new node and save as right child and newNode, maintaining links.
			}
			else if(h.Left.N < h.Right.N) { h.Left = InsertUtil(h.Left, val); }	// If Left subtree is smaller than right subtree, move to left child and recursive call InsertUtil.
			else{ h.Right = InsertUtil(h.Right, val); }							// If left subtree is larger or equal to right subtree, move to right child and recursive call InsertUtil.

			// While not at root of tree and newNode (current) value is less than parent node...
			while(newNode.Parent != null && newNode.Value.Item2.CompareTo(h.Value.Item2) < 0)
			{
				Swap(newNode, h);	// Swap newNode and parent node (h).
				newNode = h;		// Set newNode to parent position.
			}

			// Increase h subtree size by adding left and right subtrees (+1 for root node).
			h.N = Size(h.Left) + Size(h.Right) + 1;
			return h;	// Return h to Insert public method.
		}

		/// <summary>
		/// Not used in this program.
		/// Deletes root from minheap.
		/// </summary>
		/// <returns>Returns Node to call.</returns>
		private Node DeleteMin()
		{
			Node temp = GoToLast(root);									// Pointer for last item in tree
			Swap(root, temp);											// Swap root and last node
			if(temp.Parent.Right != null) { temp.Parent.Right = null; }	// IF ELSE used to set last node to null (deleting min value).
			else { temp.Parent.Left = null; }							
			temp.Parent = null;											// Remove link to main tree.
			DownHeapify(root);											// Heapify
			root.N = Size(root.Left) + Size(root.Right) + 1;			// Update size of subtrees.
			return temp;
		}

		/// <summary>
		/// Depth First Search private method called by DFS public method.
		/// </summary>
		/// <param name="h">Node initially passed in as root then updated recursively.</param>
		/// <param name="last">Last name to search.</param>
		/// <returns>Node containing search result or null node if search miss.</returns>
		private Node DFSUtil(Node h, string last)
		{
			// If tree isn't empty...
			if (h != null)
			{
				if(h.Value.Item2 == last) { return h; }	// If search hit, return result node.
				else									// Otherwise, continue search
				{
					Node ret = DFSUtil(h.Left, last);					// Create temp node containing result of left subtree recursive call.
					if(ret == null) { ret = DFSUtil(h.Right, last); }	// If null is returned, search right and save result as ret.
					return ret;											// Return the result. This is allowed to be null indicating search miss.
				}
			}
			else { return null; }	// If tree is empty, return null indicating search miss.
		}

		/// <summary>
		/// Bredth First Search private method called by BFS public method.
		/// </summary>
		/// <param name="h">Node initially passed in as root then updated recursively.</param>
		/// <param name="last">Last name to search.</param>
		/// <returns>Tuple of ints referencing node.X and node.Y values (position).</returns>
		private Tuple<int, int> BFSUtil(Node h, string last)
		{
			Queue<Node> q = new Queue<Node>();				// Create new built-int C# queue.
			int x = 0, y = 0;								// X,Y values initialized.
			if (h == null) { return Tuple.Create(-1, -1); }	// If tree is empty, return new tuple containing -1,-1 indicating search miss.
			q.Enqueue(h);									// Enqueue current node.
			while (q.Count() != 0)							// While the queue isn't empty...
			{
				h = q.Dequeue();							// Save Node h as dequeued node.
				if (h.Value.Item2 == last)					// If search hit...
				{
					x = h.X;								// Set x to node's X value
					y = h.Y;								// Set y to node's Y value
				}
				else										// If search miss...
				{
					if (h.Left != null)						// If left child exists...
					{
						q.Enqueue(h.Left);					// Enqueue left child.
					}
					if (h.Right != null)					// If right child exists...
					{
						q.Enqueue(h.Right);					// Enqueue right child.
					}
				}
			}
			return Tuple.Create(x, y);						// Reached after search hit. Returns tuple of ints containing x,y coordinates.
		}

		/// <summary>
		/// Heapify from bottom to top.
		/// </summary>
		/// <param name="h">Node passed in originally as inserted node.</param>
		/// <returns>Node referencing inserted node.</returns>
		private Node UpHeapify(Node h)
		{
			if(h.Parent == null) { return h; }							// If at root, return root.
			int cmp = h.Value.Item2.CompareTo(h.Parent.Value.Item2);	// Set int cmp to result of comparing current node's last name to the paren't last name.
			if (cmp < 0) { return UpHeapify(Swap(h, h.Parent)); }		// If cmp < 0 (current last name < parent last name), return the result of recursive call of upheapify while moving up to parent.
			else { return h; }											// Otherwise, if can't move up heap then return current node.
		}

		/// <summary>
		/// Heapify from top to bottom.
		/// </summary>
		/// <param name="h">Node passed in originally as inserted node.</param>
		/// <returns>Node referencing inserted node.</returns>
		private Node DownHeapify(Node h)
		{
			int cmpN = h.Left.N.CompareTo(h.Right.N);					// Set int cmpN to result of comparing left child's subtree value to right child's subtree value.
			
			if (cmpN < 0)												// If left subtree is less than right subtree...
			{
				int cmpV = h.Value.Item2.CompareTo(h.Left.Value.Item2);	// Set int cmpV to result of comparing current node's last name to left child's last name.
				if(cmpV > 0) { return DownHeapify(Swap(h, h.Left)); }	// If left child's last name is less than current node's last name, downheapify recursively while moving to left child.
				else { return h; }										// Otherwise, return current node.
			}
			else if(cmpN > 0)											// If right subtree is larger...
			{
				int cmpV = h.Value.Item2.CompareTo(h.Right.Value.Item2);
				if (cmpV > 0) { return DownHeapify(Swap(h, h.Right)); }	// If right child val is less than current, move right.
				else { return h; }										// Otherwise, return current node.
			}
			else { return h; }											// Return current node when if subtrees are equal.
		}

		/// <summary>
		/// Helper method used to swap two node's values while keeping other object properties the same.
		/// </summary>
		/// <param name="h1">Current node.</param>
		/// <param name="h2">Node to swap with.</param>
		/// <returns>Returns new position of current node.</returns>
		private Node Swap(Node h1, Node h2)
		{
			var temp = h1.Value;	// Create temp var containing node 1's value.
			h1.Value = h2.Value;	// Set node 1's value to node 2's value.
			h2.Value = temp;		// Set node 2's value to temp var value.
			return h2;				// Return new position of passed in node 1.
		}

		/// <summary>
		/// Moves current node to last position in tree.
		/// </summary>
		/// <param name="h">Node initially passed in as root then recursively changed.</param>
		/// <returns>Returns node at last position in tree.</returns>
		private Node GoToLast(Node h)
		{
			if(h == null) { return null; }				// If tree is empty, return null;
			if(h.N == 1) { return h; }					// If tree contains a single node, return that node.
			int cmp = h.Left.N.CompareTo(h.Right.N);	// Set int cmp to result of comparing left and right subtrees.
			if(cmp > 0) { return GoToLast(h.Left); }	// If left is larger, move left recursively.
			else { return GoToLast(h.Right); }			// If right is larger or equal, move right recursively.
		}

		/// <summary>
		/// Preorder traversal method called by Traversal public method.
		/// </summary>
		/// <param name="h">Node passed in as root then updated recursively.</param>
		private void PreOrder(Node h)
		{
			if (h == null) { return; }			// If tree is empty, exit method.
			Console.WriteLine(h.Value.Item2);	// Print out current node's last name.
			PreOrder(h.Left);					// Move left recursively.
			PreOrder(h.Right);					// Move right recursively.
		}
		/// <summary>
		/// Same as Preorder traversal method but uses InOrder traversal.
		/// </summary>
		/// <param name="h">Node passed in as root then updated recursively.</param>
		private void InOrder(Node h)
		{
			if(h == null) { return; }
			InOrder(h.Left);
			Console.WriteLine(h.Value.Item2);
			InOrder(h.Right);
		}
		/// <summary>
		/// Same as Preorder and Inorder traversals but uses PostOrder traversal.
		/// </summary>
		/// <param name="h">Node passed in as root then updated recursively.</param>
		private void PostOrder(Node h)
		{
			if(h == null) { return; }
			PostOrder(h.Left);
			PostOrder(h.Right);
			Console.WriteLine(h.Value.Item2);
		}

		/// <summary>
		/// Private size method called from InsertUtil and from Size public method.
		/// </summary>
		/// <param name="x">Node passed in that will have its size changed.</param>
		/// <returns>Returns node's N property.</returns>
		private int Size(Node x)
		{
			if (x == null) { return 0; }
			else { return x.N; }
		}
		#endregion
	}

	/// <summary>
	/// Class object used to create array of people. Used only by MaxHeap class.
	/// </summary>
	class Person
	{
		/// <summary>
		/// Default constructor taking 0 parameters. Used to create null person.
		/// </summary>
		public Person() { }
		/// <summary>
		/// Constructor used to create new person with a first and last name.
		/// </summary>
		/// <param name="value">Tuple of strings containing first and last name.</param>
		public Person(Tuple<string,string> value)
		{
			Value = value;	// Set passed in value to Value property.
		}
		public int X { get; set; }						// X property used to store X position of node in a row.
		public int Y { get; set; }						// Y property used to store Y height of node in tree.
		public Tuple<string,string> Value { get; set; }	// Value property used to store first and last names of Person object in form of a tuple of strings.
	}

	/// <summary>
	/// Class containing methods containing all operations of a max heap using an array implementation.
	/// </summary>
	class MaxHeap
	{
		private Person[] Heap { get; set; } // Global property of array of People objects. Used throughout this class.
		private int size, n = 0;			// Integers used to keep track of size of array and current node pointer.

		#region Public Methods
		/// <summary>
		/// Main method of MaxHeap class.
		/// Takes in array of items and a max size of array and builds the array of People.
		/// </summary>
		/// <param name="items">Jagged array passed in by Controller class. Contains first and last names.</param>
		/// <param name="max">Max size of array allowed given number of items in the items array.</param>
		public MaxHeap(string[][] items, int max)
		{
			size = max;					// Sets global size integer to passed in max integer.
			Heap = new Person[size];    // Create new array of Person of size size and save as Heap.
			for (int i = 0; i < items.Length; i++)  // For each item in the items array...
			{
				// At position i in Heap, create new person with first and last names from items array.
				Heap[i] = new Person(Tuple.Create(items[i][1].ToLower(), items[i][0].ToLower()));
			}
			BuildHeap();	// Call buildheap private method to build the maxheap.
		}

		/// <summary>
		/// Insert method that inserts an item into the array then maintains the max heap.
		/// </summary>
		/// <param name="val">Tuple of strings containing first and last names.</param>
		public void Insert(Tuple<string,string> val)
		{
			if (n >= size)	// If at or past end of array...
			{
				Console.WriteLine("Heap is full");	// Print error.
				return;								// Exit method.
			}
			int curr = n++;			//Set current to next position in array.
			Heap[curr].Value = val; // Start at end of heap

			// While maxheap isn't maintained...
			while ((curr != 0) && (Heap[curr].Value.Item1.CompareTo(Heap[Parent(curr)].Value.Item1) > 0))
			{
				Swap(curr, Parent(curr));	// Swap current with parent of current.
				curr = Parent(curr);		// Set current to parent.
			}
		}
		
		/// <summary>
		/// Deletes root of array then sifts down to maintain max heap.
		/// </summary>
		/// <returns>Return value removed.</returns>
		public Person DeleteMax()
		{
			if (n == 0) { return null; }
			Swap(0, --n);
			if(n != 0) { SiftDown(0); }
			return Heap[n];
		}

		/// <summary>
		/// Deletes at specified position then sifts down to maintain heap.
		/// </summary>
		/// <param name="pos">Position of item in array to delete.</param>
		/// <returns>Return value removed.</returns>
		public Person Delete(int pos)
		{
			if((pos < 0) || (pos >= n)) { return null; }	// If outside the bounds of the array, return null.
			if(pos == (n - 1)) { n--; }						// If at last position in array, delete.
			else
			{
				Swap(pos, --n);	// Swap current and one position up.
				// While maxheap not maintained...
				while((pos > 0) && (Heap[pos].Value.Item1.CompareTo(Heap[Parent(pos)].Value.Item1) > 0))
				{
					Swap(pos, Parent(pos));	// Swap current and parent.
					pos = Parent(pos);		// Set current to parent of current.
				}
				if(n != 0) { SiftDown(pos); }	// If not at root, siftdown.
			}
			return Heap[n];	// Return deleted value.
		}

		public void AssignXY(int h, int x, int y)
		{
			if (Heap[h] == null) { return; }
			Heap[h].X = x;
			Heap[h].Y = y;
			AssignXY(LeftChild(h), 2 * x - 1, y + 1);
			AssignXY(RightChild(h), 2 * x, y + 1);
		}

		/// <summary>
		/// Public method that calls private Breadth First Search method.
		/// </summary>
		/// <param name="first">First name to search.</param>
		/// <returns>Returns X,Y position of item in "tree".</returns>
		public Tuple<int,int> BFS(string first)
		{
			return BFSUtil(0, first);	// Return result of BFSUtil when passing in root and search string.
		}

		/// <summary>
		/// Public method that calls private Depth First Search method.
		/// </summary>
		/// <param name="first">First name to search.</param>
		/// <returns>Returns X,Y position of item in "tree".</returns>
		public Tuple<int,int> DFS(string first)
		{
			Person h = DFSUtil(0, first);					// Create new Person containing result of DFSUtil when passing in root and search string.
			if (h == null) { return Tuple.Create(-1, -1); }	// If null node returned, return tuple with position -1,-1 indicating search miss.
			else { return Tuple.Create(h.X, h.Y); }			// Otherwise, return the returned search results by creating tuple of X,Y values.
		}

		/// <summary>
		/// Public method that calls private tree traversal methods when passing in root node and splits them up with headers.
		/// </summary>
		public void Traverse()
		{
			Console.WriteLine("=== PREORDER  ===");
			PreOrder(0);
			Console.WriteLine();
			Console.WriteLine("=== INORDER   ===");
			InOrder(0);
			Console.WriteLine();
			Console.WriteLine("=== POSTORDER ===");
			PostOrder(0);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Private method that sifts down maxheap.
		/// </summary>
		private void BuildHeap()
		{
			// For each item in heap starting at end and moving up, siftdown.
			for (int i = (n / 2) - 1; i >= 0; i--) { SiftDown(i); }
		}

		/// <summary>
		/// Method used to sift individual nodes down tree.
		/// </summary>
		/// <param name="pos">Current position index to sift down.</param>
		private void SiftDown(int pos)
		{
			if ((pos < 0) || (pos >= n)) { return; } // Shouldnt ever do this... bad position
			while (!IsLeaf(pos))	// While not at a leaf node...
			{
				int j = LeftChild(pos);																		// Temp int used to keep track of left child index.
				if ((j < (n - 1)) && (Heap[j].Value.Item1.CompareTo(Heap[j + 1].Value.Item1) < 0)) { j++; }	// If maxheap maintained, increment j.
				if (Heap[pos].Value.Item1.CompareTo(Heap[j].Value.Item1) >= 0) { return; }					// If maxheap maintained and complete. Exit method.
				Swap(pos, j);	// Swap current and j.
				pos = j;		// Set current to j.
			}
		}

		/// <summary>
		/// Breadth First Search method for array implementation of MaxHeap. Called from public BFS method.
		/// </summary>
		/// <param name="h">Int passed in as root then updated recursively.</param>
		/// <param name="first">First name to search.</param>
		/// <returns>Tuple of ints containing X,Y coordinates of search result.</returns>
		private Tuple<int, int> BFSUtil(int h, string first)
		{
			Queue<int> q = new Queue<int>();						// Create new queue to be used in this method. 
			int x = 0, y = 0;										// Counters for X,Y positions.
			if (Heap[h] == null) { return Tuple.Create(-1, -1); }	// If empty tree, return tuple with -1,-1 indicating search miss.
			q.Enqueue(h);											// Enqueue current position.
			while (q.Count() != 0)									// While queue isn't empty...
			{	
				h = q.Dequeue();									// Set h to dequeued value.
				if (Heap[h].Value.Item1 == first)					// If search hit...
				{	
					x = Heap[h].X;									// Set x to current position's X property.
					y = Heap[h].Y;									// Set y to current position's Y property.
				}
				else												// Otherwise if search miss...
				{
					if (Heap[LeftChild(h)] != null)					// If left child exists...
					{
						q.Enqueue(LeftChild(h));					// Enqueue left child.
					}
					if (Heap[RightChild(h)] != null)				// If right child exists...
					{
						q.Enqueue(RightChild(h));					// Enqueue right child.
					}
				}
			}
			return Tuple.Create(x, y);								// Return a tuple containing x y values from search result.
		}

		/// <summary>
		/// Depth First Search method for array implementation of MaxHeap. Called from public DFS method.
		/// </summary>
		/// <param name="h">Int passed in as root then updated recursively.</param>
		/// <param name="first">First name to search.</param>
		/// <returns>Person object returned to call.</returns>
		private Person DFSUtil(int h, string first)
		{
			if (Heap[h] != null)												// If heap isn't empty...
			{
				if (Heap[h].Value.Item1 == first) { return Heap[h]; }			// If search hit, return current Person.
				else															// Otherwise...
				{
					Person ret = DFSUtil(LeftChild(h), first);					// Create temp Person and set to result from recursive search of left child.
					if (ret == null) { ret = DFSUtil(RightChild(h), first); }	// If temp Person is null, set temp Person to result from recursive search of right child.
					return ret;													// Return the temp Person.
				}
			}
			else { return null; }												// Otherwise, if heap is empty, return null.
		}

		/// <summary>
		/// Preorder traversal method called by Traversal public method.
		/// </summary>
		/// <param name="pos">Index passed in as root then updated recursively.</param>
		private void PreOrder(int pos)
		{
			if(Heap[pos] == null) { return; }				// If current position is null, exit method.
			Console.WriteLine($"{Heap[pos].Value.Item1}");	// Output current position's first name.
			PreOrder(LeftChild(pos));						// Recursively call while moving to left child.
			PreOrder(RightChild(pos));						// Recursively call while moving to right child.
		}
		/// <summary>
		/// Same as Preorder traversal method but uses Inorder traversal instead.
		/// </summary>
		/// <param name="pos">Index passed in as root then updated recursively.</param>
		private void InOrder(int pos)
		{
			if (Heap[pos] == null) { return; }
			PreOrder(LeftChild(pos));
			Console.WriteLine($"{Heap[pos].Value.Item1}");
			PreOrder(RightChild(pos));
		}
		/// <summary>
		/// Same as Preorder and Inorder traversal methods but uses PostOrder traversal instead.
		/// </summary>
		/// <param name="pos">Index passed in as root then updated recursively.</param>
		private void PostOrder(int pos)
		{
			if (Heap[pos] == null) { return; }
			PreOrder(LeftChild(pos));
			PreOrder(RightChild(pos));
			Console.WriteLine($"{Heap[pos].Value.Item1}");
		}

		/// <summary>
		/// Helper method used to swap two node values while leaving other properties unchanged.
		/// </summary>
		/// <param name="p1">Current position.</param>
		/// <param name="p2">Position to swap with.</param>
		private void Swap(int p1, int p2) // Swap current and parent of current
		{
			Person temp = Heap[p1];	// Create temp Person containing current position's values.
			Heap[p1] = Heap[p2];	// Set current position value to position 2's value.
			Heap[p2] = temp;		// Set position 2's value to temp Person value.
		}

		/// <summary>
		/// Helper method that returns true if passed in index is a leaf.
		/// </summary>
		/// <param name="pos">Index of item in array.</param>
		/// <returns>True if pos is leaf. Otherwise false.</returns>
		private bool IsLeaf(int pos)
		{
			return (pos >= n / 2) && (pos < n);
		}

		/// <summary>
		/// Helper method that returns index of left child.
		/// </summary>
		/// <param name="pos">Position to check.</param>
		/// <returns>Int index of left child.</returns>
		private int LeftChild(int pos) { return (2 * pos) + 1; }
		/// <summary>
		/// Helper method that returns index of right child.
		/// </summary>
		/// <param name="pos">Position to check.</param>
		/// <returns>Int index of right child.</returns>
		private int RightChild(int pos) { return (2 * pos) + 2; }
		/// <summary>
		/// Helper method that returns index of parent.
		/// </summary>
		/// <param name="pos">Position to check.</param>
		/// <returns>Int index of parent.</returns>
		private int Parent(int pos) { return (int)Math.Floor((double)((pos - 1) / 2)); }
		/// <summary>
		/// Helper method that returns index of left sibling.
		/// </summary>
		/// <param name="pos">Position to check.</param>
		/// <returns>Int index of left sibling.</returns>
		private int LeftSibling(int pos)
		{
			if (pos % 2 == 0) { return pos - 1; }
			else { return -1; }
		}
		/// <summary>
		/// Helper method that returns index of right sibling.
		/// </summary>
		/// <param name="pos">Position to check.</param>
		/// <returns>Int index of right subling.</returns>
		private int RightSibling(int pos)
		{
			if (pos % 2 != 0) { return pos + 1; }
			else { return -1; }
		}
		#endregion
	}
	
	/// <summary>
	/// Class containing methods containing all operations of binary search tree using binary tree implementation.
	/// </summary>
	class BST
	{
		private Node root;	// Create new node and set to root. Used between methods in this class.

		#region Public Methods
		/// <summary>
		/// Insert method that calls a private InsertUtil method by passing in value and root.
		/// </summary>
		/// <param name="val">Tuple of strings containing first and last name.</param>
		public void Insert(Tuple<string,string> val)
		{
			root = InsertUtil(root, val);	// Save root as the return from InsertUtil when passing in current root and value.
		}

		/// <summary>
		/// Search method that calls GetUtil private method by passing in root and search string.
		/// </summary>
		/// <param name="last"></param>
		/// <returns></returns>
		public Tuple<int,int> Get(string last)
		{
			Node pos = GetUtil(root, last);						// Save new node as return from getUtil when passing in current root and search string.
			if (pos == null) { return Tuple.Create(-1, -1); }	// If pos is null, return a new tuple with -1,-1 indicating search miss.
			else { return Tuple.Create(pos.X, pos.Y); }			// Otherwise, return a new tuple containing X,Y coordinates of search item.
		}

		/// <summary>
		/// Method that calls private traversal methods with header outputs inbetween.
		/// </summary>
		public void Traverse()
		{
			Console.WriteLine("=== PREORDER  ===");
			PreOrder(root);
			Console.WriteLine();
			Console.WriteLine("=== INORDER   ===");
			InOrder(root);
			Console.WriteLine();
			Console.WriteLine("=== POSTORDER ===");
			PostOrder(root);
		}

		public void AssignXY(Node h, int x, int y)
		{
			if (h == null) { return; }
			h.X = x;
			h.Y = y;
			AssignXY(h.Left, 2 * x - 1, y + 1);
			AssignXY(h.Right, 2 * x, y + 1);
		}

		/// <summary>
		/// Helper method used to return root of tree. This is used in the Controller class when calling methods that require the root to be passed in.
		/// </summary>
		/// <returns></returns>
		public Node ReturnRoot()
		{
			return root;
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Insert utility private method that carries out a binary search through the BST.
		/// </summary>
		/// <param name="h">Node passed in as root then recursively updated.</param>
		/// <param name="val">Tuple of strings containing first and last name.</param>
		/// <returns>Node return to Insert public method.</returns>
		private Node InsertUtil(Node h, Tuple<string, string> val)
		{
			if (h == null) { return new Node(val); }					// If current node is empty, return a new node containing passed in value.
			int cmp = val.Item2.CompareTo(h.Value.Item2);				// Set int cmp to the result of comparing passed in val's last name to current node's last name.
			if (cmp < 0) { h.Left = InsertUtil(h.Left, val); }			// If passed in value is less than current node's value, recursive move left and insert.
			else if (cmp > 0) { h.Right = InsertUtil(h.Right, val); }	// Else If passed in value is greater, recursive move right and insert.
			else { h.Value = val; }										// Otherwise if it is equal. Set current value to passed in value.
			return h;													// Return new node.
		}

		/// <summary>
		/// Private search method called by public Get method.
		/// </summary>
		/// <param name="h">Node passed in as root then recursively updated.</param>
		/// <param name="last">Last name to search.</param>
		/// <returns>Returns search result node.</returns>
		private Node GetUtil(Node h, string last)
		{
			if (h == null) { return null; }							// If tree is null, return null node.
			int cmp = last.CompareTo(h.Value.Item2);				// Set int cmp to result of comparing search value with current node's value.
			if (cmp < 0) { return GetUtil(h.Left, last); }			// If search value is less than current node, return a recursive search while moving left.
			else if (cmp > 0) { return GetUtil(h.Right, last); }	// Else If search value is greater, return a recursive search while moving right.
			else { return h; }										// Otherwise, return current node.
		}

		/// <summary>
		/// Preorder traversal method called by Traversal public method.
		/// </summary>
		/// <param name="h">Node passed in as root then updated recursively.</param>
		private void PreOrder(Node h)
		{
			if (h == null) { return; }			// If tree is empty, exit method.
			Console.WriteLine(h.Value.Item2);	// Print current node's last name.
			PreOrder(h.Left);					// Recursively move left.
			PreOrder(h.Right);					// Recursively move right.
		}
		/// <summary>
		/// Same as Preorder traversal method but using an InOrder traversal.
		/// </summary>
		/// <param name="h">Node passed in as root then updated recursively.</param>
		private void InOrder(Node h)
		{
			if (h == null) { return; }
			InOrder(h.Left);
			Console.WriteLine(h.Value.Item2);
			InOrder(h.Right);
		}
		/// <summary>
		/// Same as Preorder and Inorder traversal but using a PostOrder traversal.
		/// </summary>
		/// <param name="h">Node passed in as root then updated recursively.</param>
		private void PostOrder(Node h)
		{
			if (h == null) { return; }
			PostOrder(h.Left);
			PostOrder(h.Right);
			Console.WriteLine(h.Value.Item2);
		}
		#endregion
	}

	#region RedBlack BST (NOT USED)
	/// <summary>
	/// Ignore this class. I used this as a test of a red black 2-3 tree.
	/// Leaving this in the program as it works with this program so I'd rather not create an
	/// entire new solution to save this.
	/// </summary>
	class RedBlackBST
	{
		private const bool RED = true;
		private const bool BLACK = false;

		private Node root;

		public void Insert(Tuple<string,string> val)
		{
			root = Insert(root, val);
			root.Color = BLACK;
		}
		private Node Insert(Node h, Tuple<string,string> val)
		{
			if(h == null) { return new Node(val, 1, RED); }

			int cmp = val.Item2.CompareTo(h.Value.Item2);

			if (cmp < 0) { h.Left = Insert(h.Left, val); }
			else if (cmp > 0) { h.Right = Insert(h.Right, val); }
			else { h.Value = val; }

			if(IsRed(h.Right) && !IsRed(h.Left)) { h = RotateLeft(h); }
			if(IsRed(h.Left) && IsRed(h.Left.Left)) { h = RotateRight(h); }
			if(IsRed(h.Left) && IsRed(h.Right)) { FlipColors(h); }

			h.N = Size(h.Left) + Size(h.Right) + 1;
			return h;
		}

		public Tuple<int,int> Get(string last)
		{
			var pos = Get(root, last, 0, 1);
			return Tuple.Create(pos.Item1 + 1, pos.Item2);
		}
		private Tuple<int,int> Get(Node h, string last, int x, int y)
		{
			if(h == null) { return Tuple.Create(-1,-1); }
			int cmp = last.CompareTo(h.Value.Item2);
			if (cmp < 0)
			{
				if(h.Left.Color == RED) { return Get(h.Left, last, (x << 1), y); }
				else { return Get(h.Left, last, (x << 1), y + 1); }
				
			}
			else if (cmp > 0)
			{
				if(h.Right.Color == RED) { return Get(h.Right, last, (x << 1) + 1, y); }
				else { return Get(h.Right, last, (x << 1) + 1, y + 1); }
				
			}
			else { return Tuple.Create(x,y); }
		}

		// Helper methods
		private bool IsRed(Node x)
		{
			if (x == null) { return false; }
			return (x.Color == RED);

		}
		public Node RotateLeft(Node h)
		{
			Node x = h.Right;
			h.Right = x.Left;
			x.Left = h;
			x.Color = h.Color;
			h.Color = RED;
			x.N = h.N;
			h.N = 1 + Size(h.Left) + Size(h.Right);
			return x;
		}
		public Node RotateRight(Node h)
		{
			Node x = h.Left;
			h.Left = x.Right;
			x.Right = h;
			x.Color = h.Color;
			h.Color = RED;
			x.N = h.N;
			h.N = 1 + Size(h.Left) + Size(h.Right);
			return x;
		}
		public void FlipColors(Node h)
		{
			h.Color = RED;
			h.Left.Color = BLACK;
			h.Right.Color = BLACK;
		}
		
		// Traversals
		public void Traverse()
		{
			Console.WriteLine("=== PREORDER  ===");
			PreOrder(root);
			Console.WriteLine();
			Console.WriteLine("=== INORDER   ===");
			InOrder(root);
			Console.WriteLine();
			Console.WriteLine("=== POSTORDER ===");
			PostOrder(root);
		}
		private void PreOrder(Node x)
		{
			if(x == null) { return; }
			Console.WriteLine(x.Value.Item2 + " " + x.Color);
			PreOrder(x.Left);
			PreOrder(x.Right);
		}
		private void InOrder(Node x)
		{
			if (x == null) { return; }
			InOrder(x.Left);
			Console.WriteLine(x.Value.Item2 + " " + x.Color);
			InOrder(x.Right);
		}   // Sorted traversal
		private void PostOrder(Node x)
		{
			if (x == null) { return; }
			PostOrder(x.Left);
			PostOrder(x.Right);
			Console.WriteLine(x.Value.Item2 + " " + x.Color);
		}

		private int Size()
		{
			return Size(root);
		}
		private int Size(Node x)
		{
			if (x == null) { return 0; }
			else { return x.N; }
		}
	}
	#endregion
}

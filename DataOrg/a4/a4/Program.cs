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

			// Infinite loop that prevents application from closing when program is done executing.
			while (true)
			{
				Console.Clear();									// Clear the console window
				Controller c = new Controller(input);               // New instance of Controller class, passing in input array
				Console.WriteLine("Press any key to continue...");
				while(Console.ReadKey() == null) { Console.ReadKey(); } // Prevents console from closing until key is pressed.
			}
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
			Console.WriteLine("3) Exit");

			// Switch that handles possible inputs after stripping input of escape chars and sending the user input to a validate method
			switch (ValidateMain(Regex.Escape(Console.ReadLine())))
			{
				case 1:	// Print all
					Console.WriteLine("Saving output to /bin/debug/output.txt");
					break;
				case 2:	// Search
					return SearchMenu();	// Call secondary menu method and return its result to method call
				case 3:	// Exit
					Console.WriteLine("Exiting application. Enter c to cancel.");
					Console.WriteLine("Press enter to exit.");
					string response = Console.ReadLine();
					if (response.ToLower() == "c") { Main(); }  // If response is c, then we return to the main menu
					else { Environment.Exit(0); }               // Otherwise, exit the program
					break;
			}
			return null;	// Shouldnt ever reach this line but it is a failsafe incase the program skipped user input.
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
		/// This method validates the first and last names to verify they don't contain numbers or null characters.
		/// </summary>
		/// <param name="first">First name passed in from SearchMenu</param>
		/// <param name="last">Last name passed in from SearchMenu</param>
		/// <returns>Tuple of strings containing first and last name.</returns>
		private Tuple<string,string> ValidateSearch(string first, string last)
		{
			// If any character in first or last name is a number, or if either are blank...
			if(first.Any(c => char.IsDigit(c)) || last.Any(c => char.IsDigit(c)) || first == "" || last == "")
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
			return Tuple.Create(first, last);	// Return the new valid first and last name.
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
			if(!int.TryParse(input, out n) || n < 1 || n > 3 || input == "")
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

		#endregion

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
		// Controller class variables. Input is a jagged array and ui is a new instance of the UI class which automatically calls Main() method.
		private string[][] Input { get; set; }
		UI ui = new UI();
		
		/// <summary>
		/// This method calls each tree method and handles filestream output and passes input array to each tree method.
		/// </summary>
		/// <param name="input"></param>
		public Controller(string[][] input)
		{
			// File items used to output to a text file and control console output.
			FileStream ostrm;
			StreamWriter writer;

			// Set input to the global Input object.
			Input = input;

			// Initialize each of the tree structures.
			MinHeap minHeap = MinHeapInit();
			MaxHeap maxHeap = MaxHeapInit();
			BST bst = BSTInit();

			// NOTE: LAST,FIRST order so input item1 is LAST NAME
			// Get search tuple from UI.
			Tuple<string,string> search = ui.Main();

			if(search == null) // If user chose option 1 instead of 2 in menu (print all instead of search)...
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
			else // OTherwise just search for name.
			{
				SearchName(minHeap, maxHeap, bst, search);
			}
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
		/// <param name="heap">This is the heap that was returned from initializing the BST.</param>
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
			Stopwatch sw = new Stopwatch();
			double minDFS = 0, minBFS = 0, maxDFS = 0, maxBFS = 0, bts = 0;
			Tuple<int, int> minPos = null, maxPos = null, bstPos = null;

			// Each of these blocks are nearly identical. They simply call different methods.

			sw.Start();								// Start stopwatch
			minPos = min.DFS(name.Item2);			// Find last name in MinHeap using DFS.
			sw.Stop();								// Stop stopwatch
			minDFS = sw.Elapsed.TotalMilliseconds;	// Save total milliseconds during search.
			sw.Reset();								// Reset stopwatch

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

	class MinHeapNode
	{
		public MinHeapNode() { }
		public MinHeapNode(Tuple<string, string> value, int n)
		{
			Value = value;
			N = n;
		}
		public Tuple<string, string> Value { get; set; }
		public int N { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public MinHeapNode Parent { get; set; }
		public MinHeapNode Left { get; set; }
		public MinHeapNode Right { get; set; }
	}
	class MinHeap
	{
		private MinHeapNode root;

		#region Public Methods

		public void Insert(Tuple<string,string> val)
		{
			root = InsertUtil(root, val);
		}


		public Tuple<int, int> DFS(string last)
		{
			MinHeapNode h = DFSUtil(root, last);
			if (h == null) { return Tuple.Create(-1, -1); }
			else { return Tuple.Create(h.X, h.Y); }
		}

		public Tuple<int, int> BFS(string last)
		{
			return BFSUtil(root, last);
		}

		public void AssignXY(MinHeapNode h, int x, int y)
		{
			if (h == null) { return; }
			h.X = x;
			h.Y = y;
			AssignXY(h.Left, 2 * x - 1, y + 1);
			AssignXY(h.Right, 2 * x, y + 1);
		}

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

		public MinHeapNode ReturnRoot()
		{
			return root;
		}
		#endregion

		#region Private Methods

		private MinHeapNode InsertUtil(MinHeapNode h, Tuple<string,string> val)
		{
			MinHeapNode newNode = new MinHeapNode();
			if(h == null) { return new MinHeapNode(val, 1); }
			if(h.Left == null || h.Right == null)
			{
				if(h.Left == null) { newNode = h.Left = InsertUtil(h.Left, val); newNode.Parent = h; }
				else { newNode = h.Right = InsertUtil(h.Right, val); newNode.Parent = h; }
			}
			else if(h.Left.N < h.Right.N) { h.Left = InsertUtil(h.Left, val); }
			else{ h.Right = InsertUtil(h.Right, val); }

			while(newNode.Parent != null && newNode.Value.Item2.CompareTo(h.Value.Item2) < 0)
			{
				Swap(newNode, h);
				newNode = h;
			}

			h.N = Size(h.Left) + Size(h.Right) + 1;
			return h;
		}

		private MinHeapNode DeleteMin()
		{
			MinHeapNode temp = GoToLast(root);
			Swap(root, temp);
			if(temp.Parent.Right != null) { temp.Parent.Right = null; }
			else { temp.Parent.Left = null; }
			temp.Parent = null;
			DownHeapify(root);
			root.N = Size(root.Left) + Size(root.Right) + 1;
			return temp;
		}

		private MinHeapNode DFSUtil(MinHeapNode h, string last)
		{
			if (h != null)
			{
				if(h.Value.Item2 == last) { return h; }
				else
				{
					MinHeapNode ret = DFSUtil(h.Left, last);
					if(ret == null) { ret = DFSUtil(h.Right, last); }
					return ret;
				}
			}
			else { return null; }
		}

		private Tuple<int, int> BFSUtil(MinHeapNode h, string last)
		{
			Queue<MinHeapNode> q = new Queue<MinHeapNode>();
			int x = 0, y = 0, counter = 0;
			if (h == null) { return Tuple.Create(-1, -1); }
			q.Enqueue(h);
			while (q.Count() != 0)
			{
				h = q.Dequeue();
				counter++;
				if (h.Value.Item2 == last)
				{
					x = h.X;
					y = h.Y;
				}
				else
				{
					if (h.Left != null)
					{
						q.Enqueue(h.Left);
					}
					if (h.Right != null)
					{
						q.Enqueue(h.Right);
					}
				}
			}
			return Tuple.Create(x, y);
		}

		private MinHeapNode UpHeapify(MinHeapNode h)
		{
			if(h.Parent == null) { return h; }
			int cmp = h.Value.Item2.CompareTo(h.Parent.Value.Item2);
			if (cmp < 0) { return UpHeapify(Swap(h, h.Parent)); }
			else { return h; }
		}

		private MinHeapNode DownHeapify(MinHeapNode h)
		{
			int cmpN = h.Left.N.CompareTo(h.Right.N);
			
			if (cmpN < 0)
			{
				int cmpV = h.Value.Item2.CompareTo(h.Left.Value.Item2);
				if(cmpV > 0) { return DownHeapify(Swap(h, h.Left)); }
				else { return h; }
			}
			else if(cmpN > 0)
			{
				int cmpV = h.Value.Item2.CompareTo(h.Right.Value.Item2);
				if (cmpV > 0) { return DownHeapify(Swap(h, h.Right)); }
				else { return h; }
			}
			else { return h; }
		}

		private MinHeapNode Swap(MinHeapNode h1, MinHeapNode h2)
		{
			var temp = h1.Value;
			h1.Value = h2.Value;
			h2.Value = temp;
			return h2;
		}

		private MinHeapNode GoToLast(MinHeapNode h)
		{
			if(h == null) { return null; }
			if(h.N == 1) { return h; }
			int cmp = h.Left.N.CompareTo(h.Right.N);
			if(cmp > 0) { return GoToLast(h.Left); }
			else { return GoToLast(h.Right); }
		}

		private void PreOrder(MinHeapNode h)
		{
			if (h == null) { return; }
			Console.WriteLine(h.Value.Item2);
			PreOrder(h.Left);
			PreOrder(h.Right);
		}
		private void InOrder(MinHeapNode h)
		{
			if(h == null) { return; }
			InOrder(h.Left);
			Console.WriteLine(h.Value.Item2);
			InOrder(h.Right);
		}
		private void PostOrder(MinHeapNode h)
		{
			if(h == null) { return; }
			PostOrder(h.Left);
			PostOrder(h.Right);
			Console.WriteLine(h.Value.Item2);
		}

		public int Size()
		{
			return Size(root);
		}
		private int Size(MinHeapNode x)
		{
			if (x == null) { return 0; }
			else { return x.N; }
		}
		#endregion
	}

	class Person
	{
		public Person() { }
		public Person(Tuple<string,string> value)
		{
			Value = value;
		}
		public int X { get; set; }
		public int Y { get; set; }
		public Tuple<string,string> Value { get; set; }
	}
	class MaxHeap
	{
		private Person[] Heap { get; set; }
		private int size, n = 0;

		#region Public Methods
		public MaxHeap(string[][] items, int max)
		{
			size = max;
			Heap = new Person[size];
			for(int i = 0; i < items.Length; i++)
			{
				Heap[i] = new Person(Tuple.Create(items[i][1].ToLower(), items[i][0].ToLower()));
			}
			BuildHeap();
		}

		public void Insert(Tuple<string,string> val)
		{
			if (n >= size)
			{
				Console.WriteLine("Heap is full");
				return;
			}
			int curr = n++;
			Heap[curr].Value = val;  // Start at end of heap
							   // Now sift up until curr's parent's key > curr's key
			while ((curr != 0) && (Heap[curr].Value.Item1.CompareTo(Heap[Parent(curr)].Value.Item1) > 0))
			{
				Swap(curr, Parent(curr));
				curr = Parent(curr);
			}
		}
		
		public Person DeleteMax()
		{
			if (n == 0) { return null; }
			Swap(0, --n);
			if(n != 0) { SiftDown(0); }
			return Heap[n];
		}

		public Person Delete(int pos)
		{
			if((pos < 0) || (pos >= n)) { return null; } // Bad pos
			if(pos == (n - 1)) { n--; } // Last element in array
			else
			{
				Swap(pos, --n);
				while((pos > 0) && (Heap[pos].Value.Item1.CompareTo(Heap[Parent(pos)].Value.Item1) > 0))
				{
					Swap(pos, Parent(pos));
					pos = Parent(pos);
				}
				if(n != 0) { SiftDown(pos); }
			}
			return Heap[n];
		}

		public void AssignXY(int h, int x, int y)
		{
			if (Heap[h] == null) { return; }
			Heap[h].X = x;
			Heap[h].Y = y;
			AssignXY(LeftChild(h), 2 * x - 1, y + 1);
			AssignXY(RightChild(h), 2 * x, y + 1);
		}

		public Tuple<int,int> BFS(string first)
		{
			return BFSUtil(0, first);
		}
		public Tuple<int,int> DFS(string first)
		{
			Person h = DFSUtil(0, first);
			if (h == null) { return Tuple.Create(-1, -1); }
			else { return Tuple.Create(h.X, h.Y); }
		}

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
		private void BuildHeap()
		{
			for (int i = (n / 2) - 1; i >= 0; i--) { SiftDown(i); }
		}

		private void SiftDown(int pos)
		{
			if ((pos < 0) || (pos >= n)) { return; } // Shouldnt ever do this... bad position
			while (!IsLeaf(pos))
			{
				int j = LeftChild(pos);
				if ((j < (n - 1)) && (Heap[j].Value.Item1.CompareTo(Heap[j + 1].Value.Item1) < 0)) { j++; }
				if (Heap[pos].Value.Item1.CompareTo(Heap[j].Value.Item1) >= 0) { return; }
				Swap(pos, j);
				pos = j;
			}
		}

		private Tuple<int, int> BFSUtil(int h, string first)
		{
			Queue<int> q = new Queue<int>();
			int x = 0, y = 0;
			if (Heap[h] == null) { return Tuple.Create(-1, -1); }
			q.Enqueue(h);
			while (q.Count() != 0)
			{
				h = q.Dequeue();
				if (Heap[h].Value.Item1 == first)
				{
					x = Heap[h].X;
					y = Heap[h].Y;
				}
				else
				{
					if (Heap[LeftChild(h)] != null)
					{
						q.Enqueue(LeftChild(h));
					}
					if (Heap[RightChild(h)] != null)
					{
						q.Enqueue(RightChild(h));
					}
				}
			}
			return Tuple.Create(x, y);
		}
		private Person DFSUtil(int h, string first)
		{
			if (Heap[h] != null)
			{
				if (Heap[h].Value.Item1 == first) { return Heap[h]; }
				else
				{
					Person ret = DFSUtil(LeftChild(h), first);
					if (ret == null) { ret = DFSUtil(RightChild(h), first); }
					return ret;
				}
			}
			else { return null; }
		}

		private void PreOrder(int pos)
		{
			if(Heap[pos] == null) { return; }
			Console.WriteLine($"{Heap[pos].Value.Item1}");
			PreOrder(LeftChild(pos));
			PreOrder(RightChild(pos));
		}
		private void InOrder(int pos)
		{
			if (Heap[pos] == null) { return; }
			PreOrder(LeftChild(pos));
			Console.WriteLine($"{Heap[pos].Value.Item1}");
			PreOrder(RightChild(pos));
		}
		private void PostOrder(int pos)
		{
			if (Heap[pos] == null) { return; }
			PreOrder(LeftChild(pos));
			PreOrder(RightChild(pos));
			Console.WriteLine($"{Heap[pos].Value.Item1}");
		}

		private void Swap(int p1, int p2) // Swap current and parent of current
		{
			Person temp = Heap[p1];
			Heap[p1] = Heap[p2];
			Heap[p2] = temp;
		}

		private bool IsLeaf(int pos)
		{
			return (pos >= n / 2) && (pos < n);
		}

		private int LeftChild(int pos) { return (2 * pos) + 1; }
		private int RightChild(int pos) { return (2 * pos) + 2; }
		private int Parent(int pos) { return (int)Math.Floor((double)((pos - 1) / 2)); }
		private int LeftSibling(int pos)
		{
			if (pos % 2 == 0) { return pos - 1; }
			else { return -1; }
		}
		private int RightSibling(int pos)
		{
			if (pos % 2 != 0) { return pos + 1; }
			else { return -1; }
		}
		#endregion
	}

	class BSTNode
	{
		public BSTNode(Tuple<string,string> value)
		{
			Value = value;
		}
		
		public Tuple<string,string> Value { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public BSTNode Left { get; set; }
		public BSTNode Right { get; set; }

		#region RedBlack BST Code (UNUSED)
		// Following code is only used in RedBlack BST
		public BSTNode(Tuple<string, string> value, int n, bool color)
		{
			Value = value;
			N = n;
			Color = color;
		}
		public int N { get; set; }
		public bool Color { get; set; }
		#endregion
	}
	class BST
	{
		private BSTNode root;

		#region Public Methods
		public void Insert(Tuple<string,string> val)
		{
			root = InsertUtil(root, val);
		}

		public Tuple<int,int> Get(string last)
		{
			BSTNode pos = GetUtil(root, last);
			if (pos == null) { return Tuple.Create(-1, -1); }
			else { return Tuple.Create(pos.X, pos.Y); }
		}

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

		public void AssignXY(BSTNode h, int x, int y)
		{
			if (h == null) { return; }
			h.X = x;
			h.Y = y;
			AssignXY(h.Left, 2 * x - 1, y + 1);
			AssignXY(h.Right, 2 * x, y + 1);
		}

		public BSTNode ReturnRoot()
		{
			return root;
		}
		#endregion

		#region Private Methods
		private BSTNode InsertUtil(BSTNode h, Tuple<string, string> val)
		{
			if (h == null) { return new BSTNode(val); }
			int cmp = val.Item2.CompareTo(h.Value.Item2);
			if (cmp < 0) { h.Left = InsertUtil(h.Left, val); }
			else if (cmp > 0) { h.Right = InsertUtil(h.Right, val); }
			else { h.Value = val; }
			return h;
		}

		private BSTNode GetUtil(BSTNode h, string last)
		{
			if (h == null) { return null; }
			int cmp = last.CompareTo(h.Value.Item2);
			if (cmp < 0) { return GetUtil(h.Left, last); }
			else if (cmp > 0) { return GetUtil(h.Right, last); }
			else { return h; }
		}

		private void PreOrder(BSTNode h)
		{
			if (h == null) { return; }
			Console.WriteLine(h.Value.Item2);
			PreOrder(h.Left);
			PreOrder(h.Right);
		}
		private void InOrder(BSTNode h)
		{
			if (h == null) { return; }
			InOrder(h.Left);
			Console.WriteLine(h.Value.Item2);
			InOrder(h.Right);
		}
		private void PostOrder(BSTNode h)
		{
			if (h == null) { return; }
			PostOrder(h.Left);
			PostOrder(h.Right);
			Console.WriteLine(h.Value.Item2);
		}
		#endregion
	}

	#region RedBlack BST (NOT USED)
	class RedBlackBST
	{
		private const bool RED = true;
		private const bool BLACK = false;

		private BSTNode root;

		public void Insert(Tuple<string,string> val)
		{
			root = Insert(root, val);
			root.Color = BLACK;
		}
		private BSTNode Insert(BSTNode h, Tuple<string,string> val)
		{
			if(h == null) { return new BSTNode(val, 1, RED); }

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
		private Tuple<int,int> Get(BSTNode h, string last, int x, int y)
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
		private bool IsRed(BSTNode x)
		{
			if (x == null) { return false; }
			return (x.Color == RED);

		}
		public BSTNode RotateLeft(BSTNode h)
		{
			BSTNode x = h.Right;
			h.Right = x.Left;
			x.Left = h;
			x.Color = h.Color;
			h.Color = RED;
			x.N = h.N;
			h.N = 1 + Size(h.Left) + Size(h.Right);
			return x;
		}
		public BSTNode RotateRight(BSTNode h)
		{
			BSTNode x = h.Left;
			h.Left = x.Right;
			x.Right = h;
			x.Color = h.Color;
			h.Color = RED;
			x.N = h.N;
			h.N = 1 + Size(h.Left) + Size(h.Right);
			return x;
		}
		public void FlipColors(BSTNode h)
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
		private void PreOrder(BSTNode x)
		{
			if(x == null) { return; }
			Console.WriteLine(x.Value.Item2 + " " + x.Color);
			PreOrder(x.Left);
			PreOrder(x.Right);
		}
		private void InOrder(BSTNode x)
		{
			if (x == null) { return; }
			InOrder(x.Left);
			Console.WriteLine(x.Value.Item2 + " " + x.Color);
			InOrder(x.Right);
		}   // Sorted traversal
		private void PostOrder(BSTNode x)
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
		private int Size(BSTNode x)
		{
			if (x == null) { return 0; }
			else { return x.N; }
		}
	}
	#endregion
}

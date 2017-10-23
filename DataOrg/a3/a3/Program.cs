using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace a3
{
	/// <summary>
	/// Assignment:	3
	/// By:			Bryan Greener
	/// Class:		CS3310
	///	Submitted:	2017-10-22
	///	
	///	This program's goal is to allow the coder to practice bubble, insertion, selection, and merge sort.
	///	It also needs to compare the complexities of using linked lists and arrays to sort.
	/// 
	/// I give permission to the instructor to share my solution(s) with the class.
	/// </summary>

	/// <summary>
	/// Main class. Only contains Main method.
	/// </summary>
	class Program
	{
		/// <summary>
		/// The only function of this method is to call the UI and Sort class constructors.
		/// It then prevents the program from automatically closing until the enter key is pressed.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			UI ui = new UI();
			// Pass ui.N and ui.Name return values into main constructor of Sort class.
			Sort s = new Sort(ui.N(), ui.Name());
			Console.WriteLine("COMPLETE. Press ENTER to close...");
			Console.ReadLine();
		}
	}

	/// <summary>
	/// This class handles all UI.
	/// Pretty much anything that writes to console gets put in here to prevent clutter in
	/// the other classes.
	/// </summary>
	class UI
	{
		/// <summary>
		/// Requests input for number of elements to sort.
		/// Input comes in as string then is trimmed of excess spaces,
		/// validated, then returned as an int.
		/// </summary>
		/// <returns>Returns validated int</returns>
		public int N()
		{
			Console.WriteLine("Please enter an integer N");
			string input = Console.ReadLine();
			// Make sure input isnt blank.
			while(input == "")
			{
				Console.WriteLine("INVALID INPUT - Please enter an integer for N");
				input = Console.ReadLine();
			}
			// Verify input is an integer.
			bool valid = false;
			while(valid == false)
			{
				int temp = 0;
				if (!int.TryParse(input, out temp))
				{
					Console.WriteLine("INVALID INPUT - Please enter an integer for N");
					input = Console.ReadLine();
				}
				else { valid = true; }
			}
			// Setting this any higher than 500 causes the program to run very slow when using large values of test loops.
			while(Convert.ToInt32(input) > 1000)
			{
				Console.WriteLine("Sorry but bublesort can't handle that big of a number without comparing into infinity.");
				Console.WriteLine("Please enter a lower integer N");
				input = Console.ReadLine();
			}
			return Convert.ToInt32(input);
		}

		/// <summary>
		/// Requests input for name to sort by. Input comes in as string, it trimmed of excess spaces, validated to
		/// verify no numbers exist, then returned.
		/// </summary>
		/// <returns>Returns validated string for name</returns>
		public string Name()
		{
			Console.WriteLine("Please Enter Last Name");
			string name = Console.ReadLine();
			// Check for whitespace and verify no numbers are in string.
			while (name.ToLower().Replace(" ", string.Empty) == "" || name.Any(char.IsDigit))
			{
				Console.WriteLine("INVALID NAME - Please enter a valid name");
				name = Console.ReadLine();
			}
			// Return and remove any whitespace.
			return name.ToLower().Replace(" ", "");
		}
		
		/// <summary>
		/// Simple output for UI to display a header before results.
		/// </summary>
		/// <param name="n">Number of elements to sort</param>
		/// <param name="name">User input name</param>
		/// <param name="search">New search order</param>
		public void LLHeader(int n, string name, string search)
		{
			Console.WriteLine();
			Console.WriteLine("-----------------------");
			Console.WriteLine("== LINKED LIST SORTS ==");
			Console.WriteLine("-----------------------");
			Console.WriteLine($"Number of chars: {n}");
			Console.WriteLine($"Sorting by name: {name}");
			Console.WriteLine($"Search order:    {search}");
		}

		/// <summary>
		/// Same as LLHeader but for array results.
		/// </summary>
		/// <param name="n">Number of elements to sort</param>
		/// <param name="name">User input name</param>
		/// <param name="search">New search order</param>
		public void ArrHeader(int n, string name, string search)
		{
			Console.WriteLine();
			Console.WriteLine("-----------------------");
			Console.WriteLine("== ARRAY BASED SORTS ==");
			Console.WriteLine("-----------------------");
			Console.WriteLine($"Number of chars: {n}");
			Console.WriteLine($"Sorting by name: {name}");
			Console.WriteLine($"Search order:    {search}");
		}

		/// <summary>
		/// Prints unsorted list when number of elements is less than 26
		/// </summary>
		/// <param name="list">Unsorted list passed in from Sort class</param>
		/// <param name="s">Type of sort</param>
		public void LLUnsorted(LL list, string s)
		{
			Console.WriteLine();
			Console.WriteLine($"{s.ToUpper()} SORT");
			Console.WriteLine("--------------");
			if (list.Count() <= 25)
			{
				Console.Write("Unsorted: ");
				list.Print();
			}
		}

		/// <summary>
		/// Prints sorted list when number of elements is less than 26
		/// </summary>
		/// <param name="list">Sorted list passed in from Sort class</param>
		public void LLSorted(LL list)
		{
			Console.WriteLine();
			if (list.Count() <= 25)
			{
				Console.Write("Sorted:   ");
				list.Print();
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Prints unsorted array when number of elements is less than 26
		/// </summary>
		/// <param name="arr">Unsorted array passed in from Sort class</param>
		/// <param name="s">Type of sort</param>
		public void ArrUnsorted(char[] arr, string s)
		{
			Console.WriteLine();
			Console.WriteLine($"{s.ToUpper()} SORT");
			Console.WriteLine("--------------");
			if(arr.Count() <= 25)
			{
				Console.Write("Unsorted: ");
				ArrPrint(arr);
			}
		}

		/// <summary>
		/// Prints sorted array when number of elements is less than 26
		/// </summary>
		/// <param name="arr">Sorted array passed in from Sort class</param>
		public void ArrSorted(char[] arr)
		{
			Console.WriteLine();
			if (arr.Count() <= 25)
			{
				Console.Write("Sorted:   ");
				ArrPrint(arr);
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Prints entire Array sorted or not.
		/// Used to offload work in ArrUnsorted and ArrSorted methods
		/// </summary>
		/// <param name="arr">Array passed in from ArrUnsorted/ArrSorted methods</param>
		private void ArrPrint(char[] arr)
		{
			foreach(char c in arr)
			{
				Console.Write($"{c} ");
			}
		}

		/// <summary>
		/// Method used to calculate and print total runtime.
		/// </summary>
		/// <param name="time">Accumulated milliseconds</param>
		/// <param name="loops">Number of loops. Used to divide time to get average runtime per loop</param>
		public void TotalTime(double time, int loops)
		{
			Console.WriteLine($"Elapsed Time: {time/loops}ms");
		}
	}

	/// <summary>
	/// This class contains methods that initialize lists/arrays, sort them, and handles communication to UI.
	/// </summary>
	class Sort
	{
		// Create new instances of UI, Random, and Stopwatch to be used in multiple methods.
		UI ui = new UI();
		Random rnd = new Random();
		Stopwatch sw = new Stopwatch();

		// Declare variables used between multiple classes.
		private int n, testLoops = 1; // SET testLoops TO 1000 FOR AVERAGE THEN UNCOMMENT BLOCK IN SORT METHOD
		private string name, sortString;
		private string ascii = "abcdefghijklmnopqrstuvwxyz";
		private double ms = 0; // Variable used to accumulate milliseconds

		/// <summary>
		/// Main constructor of Sort class.
		/// Calls sorting methods in order with headers above each.
		/// </summary>
		/// <param name="n">Number of elements to sort</param>
		/// <param name="name">User input name</param>
		public Sort(int n, string name)
		{
			// Set inputs to local private variables
			this.n = n;
			this.name = name;

			/* UNCOMMENT WHEN SETTING testLoops TO 1000
			if (n > 100) { testLoops = 100; }*/

			sortString = CharValues();			// sets sortString to newly generated sorting string

			// LINKED LIST SORTS w/ header
			ui.LLHeader(n, name, sortString);
			ListSorts();

			// ARRAY SORTS w/ header
			ui.ArrHeader(n, name, sortString);
			ArraySorts();
		}

		/// <summary>
		/// This method creates a new instance of LL(linked list) class.
		/// It then initializes this list, prints unsorted list, starts stopwatch, 
		/// sorts list, then calculate stopwatch time and prints sorted list.
		/// It does this process for each type of sort.
		/// </summary>
		private void ListSorts()
		{
			LL list = new LL();

			LLInitialize(list);	// Initializes list with new values
			ui.LLUnsorted(list, "Insertion");	// Prints list
			for (int i = 0; i < testLoops; i++)	// Runs sort for however many testLoops are specified
			{
				sw.Start();							// Start Stopwatch
				LLInsertion(list);					// Sort list
				sw.Stop();							// Stop Stopwatch
				ms += sw.Elapsed.TotalMilliseconds;	// Accumulate ms with total runtime of sort
				sw.Reset();							// Reset Stopwatch for next loop
			}
			ui.LLSorted(list);				// Print sorted list
			ui.TotalTime(ms, testLoops);	// Print average runtime of sort
			ms = 0;							// Reset ms variable for future use

			list = new LL();	// Starts entire previous series of instructions over with cleared list
			list = LLInitialize(list);
			ui.LLUnsorted(list, "Bubble");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start();
				LLBubble(list);
				sw.Stop();
				ms += sw.Elapsed.TotalMilliseconds;
				sw.Reset();
			}
			ui.LLSorted(list);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			list = new LL();
			list = LLInitialize(list);
			ui.LLUnsorted(list, "Selection");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start();
				LLSelection(list);
				sw.Stop();
				ms += sw.Elapsed.TotalMilliseconds;
				sw.Reset();
			}
			ui.LLSorted(list);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			list = new LL();
			list = LLInitialize(list);
			ui.LLUnsorted(list, "Merge");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start();
				LLMerge(list, new LL(), 1, list.Count());
				sw.Stop();
				ms += sw.Elapsed.TotalMilliseconds;
				sw.Reset();
			}
			ui.LLSorted(list);
			ui.TotalTime(ms, testLoops);
			ms = 0;
		}

		/// <summary>
		/// This method is similar to ListSorts with only a few variations.
		/// It creates a new char array, initializes it with new values, prints the unsorted list,
		/// starts stopwatch, sorts array, stops stopwatch, accumulates time, then prints sorted list.
		/// Again, this does these operations for each type of sort.
		/// </summary>
		private void ArraySorts()
		{
			char[] arr = new char[n];	// New char array of length n (specified num elements)

			ArrInitialize(arr);	// Populate array with new chars
			ui.ArrUnsorted(arr, "Insertion");	// Print unsorted array
			for (int i = 0; i < testLoops; i++)	// For specified number of test loops
			{
				if (testLoops != 1) { ArrInitialize(arr); }	// if looping more than once, initialize again
				sw.Start();									// Start Stopwatch
				ArrInsertion(arr);							// Sort array
				sw.Stop();									// Stop Stopwatch
				ms += sw.Elapsed.TotalMilliseconds;			// Accumulate total runtime
				sw.Reset();									// Reset Stopwatch
			}
			ui.ArrSorted(arr);				// Print sorted array
			ui.TotalTime(ms, testLoops);	// Print average runtime
			ms = 0;							// Reset ms variable for future use

			ArrInitialize(arr);	// Repeat steps above for each type of sort
			ui.ArrUnsorted(arr, "Bubble");
			for (int i = 0; i < testLoops; i++)
			{
				if (testLoops != 1) { ArrInitialize(arr); }
				sw.Start();
				ArrBubble(arr);
				sw.Stop();
				ms += sw.Elapsed.TotalMilliseconds;
				sw.Reset();
			}
			ui.ArrSorted(arr);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			ArrInitialize(arr);
			ui.ArrUnsorted(arr, "Selection");
			for (int i = 0; i < testLoops; i++)
			{
				if (testLoops != 1) { ArrInitialize(arr); }
				sw.Start();
				ArrSelection(arr);
				sw.Stop();
				ms += sw.Elapsed.TotalMilliseconds;
				sw.Reset();
			}
			ui.ArrSorted(arr);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			ArrInitialize(arr);
			ui.ArrUnsorted(arr, "Merge");
			for (int i = 0; i < testLoops; i++)
			{
				if (testLoops != 1) { ArrInitialize(arr); }
				sw.Start();
				ArrMerge(arr, new char[n], 0, arr.Count() - 1);
				sw.Stop();
				ms += sw.Elapsed.TotalMilliseconds;
				sw.Reset();
			}
			ui.ArrSorted(arr);
			ui.TotalTime(ms, testLoops);
			ms = 0;

		}

		/// <summary>
		/// This method inserts random characters into linked list.
		/// </summary>
		/// <param name="list">Passed in list from ListSorts method</param>
		/// <returns>Returns newly populated list</returns>
		private LL LLInitialize(LL list)
		{
			for(int i = 0; i < n; i++)
			{
				list.Insert(ascii[rnd.Next(0, 26)]);	// Insert random values from Ascii string into list
			}
			return list;
		}

		/// <summary>
		/// This method inserts random characters into array.
		/// This is nearly identical to LLInitialize.
		/// </summary>
		/// <param name="arr">Passed in array from ArraySorts method</param>
		/// <returns>Returns newly populated array</returns>
		private char[] ArrInitialize(char[] arr)
		{
			for(int i = 0; i < n; i++)
			{
				arr[i] = ascii[rnd.Next(0, 26)];
			}
			return arr;
		}

		/// <summary>
		/// This method takes the user input name and outputs a string with that name at the front
		/// to be used to sort values.
		/// </summary>
		/// <returns>Returns new sorting string</returns>
		private string CharValues()
		{
			string tempAscii = "abcdefghijklmnopqrstuvwxyz";			// Need to make this here because it gets manipulated.
			char[] chars = new char[26];								// New char array of length 26
			char[] nameArr = name.ToCharArray().Distinct().ToArray();	// Takes name, removes duplicate letters, then stores in char array

			for(int i = 0; i < nameArr.Count(); i++)	// For each item in nameArr
			{
				chars[i] = nameArr[i];	// Copy char to chars array
				tempAscii = tempAscii.Remove(tempAscii.IndexOf(nameArr[i]), 1);	// Remove newly added char from tempAscii
			}
			for(int i = nameArr.Count(); i < 26; i++)	// Starting where previous loop left off, finish adding ascii values to chars[]
			{
				chars[i] = tempAscii[i - nameArr.Count()];	// Append remaining ascii values to chars array
			}
			return new string(chars);	// Return chars array as a string
		}

		/// <summary>
		/// This method compares two chars based on the new sorting string created in CharValues() method.
		/// This uses the index of the item in the new sorting string and compares it to the int index of
		/// the second char in that sorting string. It compares the int to determine which is greater/less.
		/// </summary>
		/// <param name="x">Char 1</param>
		/// <param name="y">Char 2</param>
		/// <returns>Returns int indicating if input 1 is greater/less/equal to input 2</returns>
		private int CharCompare(char x, char y)
		{
			if (sortString.IndexOf(x) < sortString.IndexOf(y)) { return -1; }		// If 1 < 2
			else if (sortString.IndexOf(x) > sortString.IndexOf(y)) { return 1; }	// If 1 > 2
			else { return 0; }														// If 1 = 2
		}

		#region LinkedListSorts
		/// <summary>
		/// Linked List (LL) Insertion Sort
		/// </summary>
		/// <param name="list">list passed in from ListSorts method</param>
		private void LLInsertion(LL list)
		{
			for(int i = 1; i < list.Count(); i++)	// For each item in list
			{
				for(int j = i + 1; j > 1 && CharCompare(list.GetData(j), list.GetData(j - 1)) == -1; j--)
				{   // Move left through list from i, comparing as we go
					list.Swap(j, j - 1);	// If j < j - 1, swap these values
				}
			}
		}

		/// <summary>
		/// LL Bubble Sort
		/// </summary>
		/// <param name="list">list passed from ListSorts method</param>
		private void LLBubble(LL list)
		{
			for(int i = 0; i < list.Count(); i++)	// For each item in list
			{
				for(int j = 1; j < list.Count() - i + 1; j++)	// Second pointer that leads previous for loop pointer
				{
					if(CharCompare(list.GetData(j - 1), list.GetData(j)) == 1)	// If j-1 > j
					{
						list.Swap(j, j - 1);	// Swap j and j-1 values
					}
				}
			}
		}

		/// <summary>
		/// LL Selection Sort
		/// </summary>
		/// <param name="list">list passed from ListSorts method</param>
		private void LLSelection(LL list)
		{
			for(int i = 0; i < list.Count(); i++) // For each item in list
			{
				int bigindex = 0;	// int used to save location of largest known item
				for(int j = 1; j < list.Count() - i + 1; j++)	// Second pointer for list
				{
					if(CharCompare(list.GetData(j), list.GetData(bigindex)) == 1)	// If j is > bigindex
					{
						bigindex = j; // Set new bigindex
					}
				}
				list.Swap(bigindex, list.Count() - i);	// move bigindex to last position that isnt already sorted
			}
		}

		/// <summary>
		/// LL Merge Sort
		/// </summary>
		/// <param name="list">list passed in from ListSorts</param>
		/// <param name="temp">temp list used only in this method. Originally passed in as empty.</param>
		/// <param name="left">Left index. Originally passed in as 1.</param>
		/// <param name="right">Right index. Originally passed in as list.Count</param>
		private void LLMerge(LL list, LL temp, int left, int right)
		{
			if(left == right) { return; }			// If pointers have met, break.
			int mid = (left + right) / 2;			// Calculate mid
			LLMerge(list, temp, left, mid);			// Recursively call merge with new left/right pointers
			LLMerge(list, temp, mid + 1, right);	// Recursively call merge with new left/right pointers
			for(int i = left; i <= right; i++)	// For each item, insert in temp
			{
				temp.Insert(list.GetData(i), i);
			}

			int i1 = left, i2 = mid + 1;	// Declare new pointers
			for (int current = left; current <= right; current++)   // For each item in sublists
			{
				if (i1 == mid + 1) { list.Replace(temp.GetData(i2++), current); }	// If at mid+1, replace mid+2 with current
				else if (i2 > right) { list.Replace(temp.GetData(i1++), current); }	// past right pointer, replace left+1 with current
				else if (CharCompare(temp.GetData(i1), temp.GetData(i2)) == -1 || CharCompare(temp.GetData(i1), temp.GetData(i2)) == 0)
				{	// If value at i1 is less than value at i2 or if they are equal
					list.Replace(temp.GetData(i1++), current);	// replace left + 1 with current
				}
				else { list.Replace(temp.GetData(i2++), current); }	// Otherwise, if i1 is greater than i2, replace mid + 2 with current
			}
		}
		#endregion

		#region ArraySorts
		/// <summary>
		/// Array Insertion Sort
		/// Same as LL implementation.
		/// </summary>
		/// <param name="arr">array passed in from ArraySorts</param>
		private void ArrInsertion(char[] arr)
		{
			for (int i = 0; i < arr.Count() - 1; i++)
			{
				for (int j = i + 1; j > 0 && CharCompare(arr[j], arr[j - 1]) == -1; j--)
				{
					Swap(ref arr[j], ref arr[j - 1]);
				}
			}
		}

		/// <summary>
		/// Array Bubble Sort
		/// Same as LL implementation.
		/// </summary>
		/// <param name="arr">array passed in from ArraySorts</param>
		private void ArrBubble(char[] arr)
		{
			for (int i = 0; i < arr.Count() - 1; i++)
			{
				for (int j = 1; j < arr.Count() - i; j++)
				{
					if (CharCompare(arr[j - 1], arr[j]) == 1)
					{
						Swap(ref arr[j], ref arr[j - 1]);
					}
				}
			}
		}

		/// <summary>
		/// Array Selection Sort
		/// Same as LL implementation.
		/// </summary>
		/// <param name="arr">array passed in from ArraySorts</param>
		private void ArrSelection(char[] arr)
		{
			for (int i = 0; i < arr.Count() - 1; i++)
			{
				int bigindex = 0;
				for (int j = 1; j < arr.Count() - i; j++)
				{
					if (CharCompare(arr[j], arr[bigindex]) == 1)
					{
						bigindex = j;
					}
				}
				Swap(ref arr[bigindex], ref arr[arr.Count() - i - 1]);
			}
		}

		/// <summary>
		/// Array Merge Sort
		/// Same as LL implementation.
		/// </summary>
		/// <param name="arr">array passed in from ArraySorts</param>
		/// <param name="temp">Temp array used in this method. Originally passed in as new array.</param>
		/// <param name="left">Left index. Originally passed in as 0.</param>
		/// <param name="right">Right index. Originally passed in as arr.Count</param>
		private void ArrMerge(char[] arr, char[] temp, int left, int right)
		{
			if(left == right) { return; }
			int mid = (left + right) / 2;
			ArrMerge(arr, temp, left, mid);
			ArrMerge(arr, temp, mid + 1, right);
			for(int i = left; i <= right; i++)
			{
				temp[i] = arr[i];
			}

			int i1 = left;
			int i2 = mid + 1;
			for(int current = left; current <= right; current++)
			{
				if (i1 == mid + 1) { arr[current] = temp[i2++]; }
				else if(i2 > right) { arr[current] = temp[i1++]; }
				else if(CharCompare(temp[i1], temp[i2]) == -1 || CharCompare(temp[i1], temp[i2]) == 0)
				{
					arr[current] = temp[i1++];
				}
				else { arr[current] = temp[i2++]; }
			}
		}

		/// <summary>
		/// Swap method used to quickly swap two chars (A and B)
		/// </summary>
		/// <param name="a">Char A. Passed in as reference</param>
		/// <param name="b">Char B. Passed in as reference.</param>
		private void Swap(ref char a, ref char b)
		{
			char temp = a;	// Temp char to store swap char
			a = b;			// Set a to b
			b = temp;		// Set b to temp char
		}
		#endregion
	}

	/// <summary>
	/// Standard Node class used with LL class to created Singly Linked Lists
	/// </summary>
	class Node
	{
		/// <summary>
		/// Default constructor used to create null node
		/// </summary>
		public Node() { }

		/// <summary>
		/// Constructor used to create node with specified Next node
		/// </summary>
		/// <param name="n">Node's .Next parameter</param>
		public Node(Node n)
		{
			Next = n;	// Set input n to local Next value using Next setter
		}

		/// <summary>
		/// Constructor used to create node with specified data and Next node
		/// </summary>
		/// <param name="d">char d to be set in new node</param>
		/// <param name="n">Node's .Next parameter</param>
		public Node(char d, Node n)
		{
			Data = d;	// Set input d to local Data value using Data setter
			Next = n;	// Set input n to local Next value using Next setter
		}

		/// <summary>
		/// Getter/Setter for Data
		/// </summary>
		public char Data { get; set; }

		/// <summary>
		/// Getter/Setter for Next
		/// </summary>
		public Node Next { get; set; }
	}

	/// <summary>
	/// Linked List class containing methods for insert/delete/etc operations on Singly Linked List of Nodes
	/// </summary>
	class LL
	{
		// Local private variables/pointer for lists
		private Node head, tail, current;
		private int count;

		/// <summary>
		/// Default constructor used to create new empty list with head, current, and tail pointers
		/// </summary>
		public LL()
		{
			current = tail = new Node(null);	// Current and tail pointers are set to new null node
			head = new a3.Node(tail);			// Create new node for head with .Next set to tail pointer
			count = 0;							// Current count is 0
		}

		/// <summary>
		/// Method used to clear entire list by setting head's next node to null. Not currently used.
		/// </summary>
		public void Clear()
		{
			head = null;
		}

		/// <summary>
		/// Insert function that acts as an append since it only places new nodes at the tail.
		/// Faster than insert at x where x = list.count since it doesn't traverse through entire list.
		/// </summary>
		/// <param name="d">Data to be saved in new node</param>
		public void Insert(char d)	// APPEND
		{
			tail.Next = new a3.Node(null);	// Create new empty node with null pointer to next node
			tail.Data = d;					// Current tail's data set to new data
			tail = tail.Next;				// Tail pointer set to current tail.next, putting it at end of list
			count++;						// Increment count
		}

		/// <summary>
		/// Insert function allowing insertion at specified index.
		/// </summary>
		/// <param name="d">Data to be saved in new node</param>
		/// <param name="index">Index to place new node</param>
		public void Insert(char d, int index) // INSERT AT X
		{
			current = GoToIndex(current, index);					// Calls method that moves 'current' pointer to specified index
			current.Next = new a3.Node(current.Data, current.Next);	// Sets current's next pointer to a new node with old and current's current next pointer
			current.Data = d;										// Sets current's data to new data
			if(tail == current) { tail = current.Next; }			// If current is at tail, move tail over 1
			count++;												// Increment count
		}

		/// <summary>
		/// Replaces value at index with new value
		/// </summary>
		/// <param name="d">New value for node</param>
		/// <param name="index">Index to have value replaced</param>
		public void Replace(char d, int index)
		{
			current = GoToIndex(current, index);	// Move to index
			current.Data = d;						// Current data = new data
		}

		/// <summary>
		/// Method that swaps data between two nodes.
		/// </summary>
		/// <param name="index1">Index1 of node to swap</param>
		/// <param name="index2">Index2 of node to swap</param>
		public void Swap(int index1, int index2)
		{
			Node current2 = new Node(null);			// Create new null node to be used as pointer
			current = GoToIndex(current, index1);	// Move current pointer to index1
			current2 = GoToIndex(current2, index2);	// Move new pointer to index2
			char temp = current2.Data;				// Store new pointer's node data
			current2.Data = current.Data;			// Save current pointer's node data to new pointer's node
			current.Data = temp;					// Save stored data from other pointer to current node
		}


		/// <summary>
		/// Delete node at index x.
		/// </summary>
		/// <param name="index">Specified index to delete node from</param>
		/// <returns>Returns deleted char. Not currently used but is needed for stacks/queues</returns>
		public char Delete(int index)
		{
			current = head.Next;							// Set current to first node with data (i.e. not head/tail)
			current = GoToIndex(current, index);			// Move current pointer to specified index
			if (current == tail) { return '\0'; }			// If at tail, return empty since value not found
			char c = current.Data;							// Save current data to be returned
			current.Data = current.Next.Data;				// Set current data to next node's data
			if(current.Next == tail) { tail = current; }	// If next to tail, move tail pointer to current
			current.Next = current.Next.Next;				// Set pointer to skip a node, effectively deleting next node from list
			count--;										// Decrement count
			return c;										// Return stored data
		}

		/// <summary>
		/// Returns data at specified index
		/// </summary>
		/// <param name="index">Index of data needed</param>
		/// <returns>Return the char data at a specified index</returns>
		public char GetData(int index)
		{
			if (index > count) { return '\0'; }		// If index is out of bounds, return null
			current = GoToIndex(current, index);	// Move to specified index
			return current.Data;					// Return data at index
		}

		/// <summary>
		/// Method used to print entire list to console.
		/// </summary>
		public void Print()
		{
			current = head;	// Set pointer at head
			for(int i = 0; i < count; i++)	// For each item in list
			{
				current = current.Next;		// Move through list
				Console.Write($"{current.Data} ");	// Printing data at index as we go
			}
		}

		/// <summary>
		/// Method used just to move a specified pointer to a specific index.
		/// This method is used by nearly every method in the LL class
		/// as a way to offload the work of taversing the list and to
		/// make the code a bit cleaner.
		/// </summary>
		/// <param name="pointer">Node pointer that needs to be moved</param>
		/// <param name="index">Specified index to move to</param>
		/// <returns>Returns the pointer that is being moved</returns>
		public Node GoToIndex(Node pointer, int index)
		{
			pointer = head.Next;	//Set pointer to first node with actual data (i.e. not head/tail)
			for (int i = 1; i < index; i++)	// While not at index
			{
				pointer = pointer.Next;	// Move pointer to next node
			}
			return pointer;	// Return pointer now at specified index
		}

		/// <summary>
		/// Simple method that returns the number of elements in the list.
		/// </summary>
		/// <returns>Returns an integer count of number of elements in list.</returns>
		public int Count() { return count; }
	}
}

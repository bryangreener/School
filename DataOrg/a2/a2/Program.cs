using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;


/// <summary>
/// By:			Bryan Greener
/// Class:		CS3310
///	Submitted:	2017-10-15
///	This program's goal is to take in a txt file, read each line as a separate entry,
///	then check to see if parentheses are balanced. It does this by using both a stack
///	and a queue implementation of a singly linked list.
/// 
/// I give permission to the instructor to share my solution(s) with the class.
/// </summary>

namespace a2
{
	class Program
	{
		/// <summary>
		/// Main method only calls StackQueueDemo's SanitizeInput method.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			///Create new instance of StackQueueDemo class then call its main method.
			StackQueueDemo demo = new StackQueueDemo();
			demo.SanitizeInput();
		}
	}

	/// <summary>
	/// Called by StackQueueDemo class.
	/// This class performs stack functions on linked list.
	/// </summary>
	class StackCheckBalancedParentheses
	{
		// Instantiate CharStack class with new stack object.
		CharStack stack = new CharStack();

		/// <summary>
		/// Method that contains loop that calls push and pop methods from CharStack class
		/// in order to determine the degree of mismatching pairs of parentheses.
		/// </summary>
		/// <param name="input">Input is a single line of the input file passed in as a string.</param>
		/// <returns>Degree is the total degree of mismatched parentheses.</returns>
		public int CheckBalancedParentheses(string input)
		{
			// Initialize degree integer as 0
			int degree = 0;
			// Loop through each character in input string.
			foreach (char c in input)
			{
				// If open parentheses, call Push method in CharStack class and pass in a single (
				if (c == '(')
				{
					stack.Push(c);
				}
				// If closed parentheses, check stack...
				if (c == ')')
				{
					// If stack doesn't return an open parentheses then there is a mismatching pair of parens
					if (stack.Pop() != '(')
					{
						// Increment degree.
						degree++;
					}
				}
			}
			// Return degree and remaining number of items on stack that didn't get popped.
			return degree += stack.Count();
		}
	}
	/// <summary>
	/// Called by StackCheckBalancedParenthesis class.
	/// </summary>
	/// <remarks>This class contains Push and Pop methods which access
	/// the CharList class which contains a Linked List.</remarks>
	class CharStack
	{
		// Instantiate CharList with new list.
		CharList list = new CharList();

		/// <summary>
		/// Stack operation that adds item before head in linked list.
		/// </summary>
		/// <param name="data">Single character from input string to be pushed on stack.</param>
		public void Push(char data)
		{
			list.AddHead(data);
		}
		/// <summary>
		/// Stack operation that removes head from linked list.
		/// </summary>
		/// <returns>Returns single character from top of stack.</returns>
		public char Pop()
		{
			return list.DeleteHead();
		}
		/// <summary>
		/// Used to get number of items in list while in StackCheckBalancedParentheses class.
		/// </summary>
		/// <returns>Integer length of linked list.</returns>
		public int Count()
		{
			return list.Count();
		}
	}


	/// <summary>
	/// Called by StackQueueDemo class.
	/// </summary>
	/// <remarks>This class performs stack functions on linked list
	/// by using two queues to imitate a stack
	/// and returns a degree of mismatched parenthesis.</remarks>
	class QueueCheckBalancedParentheses
	{
		// Instantiate new queue.
		CharQueue queue = new CharQueue();

		/// <summary>
		/// Method identical to method of same name in StackCheckBalancedParentheses class.
		/// </summary>
		/// <param name="input">Input is a single line of the input file passed in as a string.</param>
		/// <returns>Degree is the total degree of mismatched parentheses.</returns>
		public int CheckBalancedParentheses(string input)
		{
			int degree = 0;
			foreach (char c in input)
			{
				if (c == '(')
				{
					Push(c); // this calls the Push method in this class, not in the Stack class
				}
				if (c == ')')
				{
					if (Pop() != '(') // this calls the Pop method in this class, not in the Stack class
					{
						degree++;
					}
				}
			}
			return degree += queue.Count();
		}
		/// <summary>
		/// Push method used to simulate push operation on a stack.
		/// </summary>
		/// <remarks>By using two queues and rotating the first to pop the final element off to a different queue,
		/// this method simulates a stack.</remarks>
		/// <param name="c">Single character input to be sent to "stack".</param>
		private void Push(char c)
		{
			// Instantiate new queue.
			CharQueue queue2 = new CharQueue();

			// Immediately enqueue the input character.
			queue.Enqueue(c);

			// For each item in the original queue...
			for (int i = 0; i <= queue.Count(); i++)
			{
				// Loop through entire original queue until at very last element.
				for (int j = 0; j <= queue.Count() - 1; j++)
				{
					// For each shift left, dequeue and enqueue the returned char from dequeue operation.
					// This is effectively roating the linked list queue.
					queue.Enqueue(queue.Dequeue());
				}
				// Once we get to the last item in the original queue, dequeue that item and enqueue it in new queue.
				// By doing this, we now have the items in order as if they were pushed onto a stack.
				queue2.Enqueue(queue.Dequeue());
			}
			// Return queue2 items to original queue for use in other methods.
			for(int i = 0; i <= queue2.Count(); i++)
			{
				queue.Enqueue(queue2.Dequeue());
			}
		}
		/// <summary>
		/// Pop method using the previously converted queue to simulate pop operation on a stack.
		/// </summary>
		/// <returns>Return single character from front of queue.</returns>
		private char Pop()
		{
			return queue.Dequeue();
		}
	}

	/// <summary>
	/// Called by QueueCheckBalancedParenthesis class.
	/// This class contains Enqueue and Dequeue methods which access
	/// the CharList class which contains a Linked List.
	/// </summary>
	class CharQueue
	{
		// Instantiate new list.
		CharList list = new CharList();

		/// <summary>
		/// Adds input to tail of linked list.
		/// </summary>
		/// <param name="data">Single character input.</param>
		public void Enqueue(char data)
		{
			list.AddTail(data);
		}
		/// <summary>
		/// Removes head of linked list queue.
		/// </summary>
		/// <returns>Returns single character value from head of queue.</returns>
		public char Dequeue()
		{
			return list.DeleteHead();
		}
		/// <summary>
		/// Used to get length of linked list queue.
		/// </summary>
		/// <returns>Returns integer value.</returns>
		public int Count()
		{
			return list.Count();
		}
	}

	/// <summary>
	/// Contains SanitizeInput method which is called by Main method in Program class.
	/// </summary>
	class StackQueueDemo
	{
		/// <summary>
		/// Called by Main method in Program class.
		/// This class reads the balancedParenCheckInputs.txt text file
		/// inside the /bin/debug program folder. It then splits this input,
		/// sanitizes it, calls StackCheckBalancedParentheses and
		/// QueueCheckBalancedParentheses class methods. The return
		/// from these calls gets saved to a single list then outputs in
		/// a nice format.
		/// </summary>
		public void SanitizeInput()
		{
			// int used to keep track of length of longest line in input file for padding.
			int longest = 0;
			// Read entire text file in /bin/debug into single string.
			string text = File.ReadAllText("balancedParenCheckInputs.txt");
			// Split text string into separate lines and save in var lines
			var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			// New list which will store separate input lines.
			List<string> input = new List<string>();
			// For each item in var lines
			foreach (string line in lines)
			{
				// keep track of longest line in input
				if (line.Length > longest)
				{
					longest = line.Length;
				}
				// Add line as new list element after setting all escape characters to string literals.
				input.Add(Regex.Unescape(line));
			}
			// Foreach string in input
			foreach (string s in input)
			{
				// Instantiate StackCheckBalancedParentheses
				StackCheckBalancedParentheses stackCheck = new StackCheckBalancedParentheses();
				// Output string s, pad so all outputs are same length, then output the return from the CheckBalancedParentheses method in stackCheck when passing in s.
				Console.WriteLine($"STACK: {s.PadRight((longest - s.Length) + s.Length + 3, ' ')}Degree: {stackCheck.CheckBalancedParentheses(s)}");
			}
			// Line break for neater output.
			Console.WriteLine("-".PadRight(longest + 20, '-'));

			// Same exact system as previous foreach but using the queue classes.
			foreach (string s in input)
			{
				QueueCheckBalancedParentheses queueCheck = new QueueCheckBalancedParentheses();
				Console.WriteLine($"QUEUE: {s.PadRight((longest - s.Length) + s.Length + 3, ' ')}Degree: {queueCheck.CheckBalancedParentheses(s)}");
			}

			// Prevent console from closing automatically.
			Console.ReadLine();
		}
	}

	/// <summary>
	/// This class contains the accessors and mutators which
	/// control individual nodes in the Linked List.
	/// </summary>
	public class CharNode
	{
		private CharNode next;
		private char myData;

		/// <summary>
		/// Individual node object.
		/// </summary>
		/// <param name="c">Input character to be saved as node's value. Saved as local myData variable.</param>
		/// <param name="n">Input CharNode to be used to link current and next node. Saved as local next variable.</param>
		public CharNode(char c, CharNode n)
		{
			myData = c;
			next = n;
		}
		/// <summary>
		/// Accessor used to get value for specified node.
		/// </summary>
		/// <returns>Returns char value saved at specific node.</returns>
		public char GetData()
		{
			return myData;
		}
		/// <summary>
		/// Accessor used to get link to next node for current node.
		/// </summary>
		/// <returns>Returns CharNode next link at specific node.</returns>
		public CharNode GetNext()
		{
			return next;
		}
		/// <summary>
		/// Mutator used to save value at specified node.
		/// </summary>
		/// <param name="newData">Single character input saved as local myData variable.</param>
		public void SetData(char newData)
		{
			myData = newData;
		}
		/// <summary>
		/// Mutator used to save link to next element for specific node.
		/// </summary>
		/// <param name="newNext">CharNode input saved as local next variable.</param>
		public void SetNext(CharNode newNext)
		{
			next = newNext;
		}
	}

	/// <summary>
	/// This class contains the insert/delete/count functions of
	/// the program's singly linked list.
	/// This class gets called by CharStack and CharQueue class methods.
	/// </summary>
	public class CharList
	{
		// Create CharNodes for head, tail, and newNode which will be used throughout this class.
		protected CharNode head;
		protected CharNode tail;
		protected CharNode newNode;
		// Count is incremented/decremented during each add/delete operation.
		protected int count;
		
		/// <summary>
		/// Initializes a new linked list with head and tail and with a count of 0 elements.
		/// </summary>
		public CharList()
		{
			head = null;
			tail = head;
			count = 0;
		}

		/// <summary>
		/// Inserts new node as head.
		/// </summary>
		/// <param name="c">Single character input.</param>
		public void AddHead(char c)
		{
			// Creates new node with value=c and next=head and assigns to head.
			head = new CharNode(c, head);
			// Increment count since linked list has grown.
			count++;
		}
		/// <summary>
		/// Inserts new node as tail.
		/// </summary>
		/// <param name="c">Single charatcer input.</param>
		public void AddTail(char c)
		{
			// Creates new node with value=c and next=null
			newNode = new CharNode(c, null);
			// If the linked list is empty...
			if(head == null)
			{
				// Set head pointer to the new node.
				head = newNode;
			}
			else
			{
				// Add newNode after tail.
				tail.SetNext(newNode);
			}
			// Set tail pointer to newNode.
			tail = newNode;
			// Increment count as a new item was added to linked list.
			count++;
		}
		/// <summary>
		/// Deletes head node from linked list.
		/// </summary>
		/// <returns>Returns single character value from deleted node.</returns>
		public char DeleteHead()
		{
			// If list is empty, return 0. Any character return would work other than '(' or ')'
			if (head == null)
			{
				return '0';
			}
			// Create char variable and save value from head before deleting
			char c = head.GetData();
			// Set newNode pointer to head.
			newNode = head;
			// Set head pointer to next element.
			head = head.GetNext();
			// Set newNode.next to null to effectively dismember this node from the linked list.
			newNode.SetNext(null);
			// Decrement count since an item has been removed from the list.
			count--;
			// Return the saved character from removed node.
			return c;
		}
		/// <summary>
		/// Method used to get current count of items in linked list.
		/// </summary>
		/// <returns>Returns integer count of number of items in linked list.</returns>
		public int Count()
		{
			return count;
		}
	}
}

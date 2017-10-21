using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a3
{
	class Program
	{
		static void Main(string[] args)
		{
			Sort s = new Sort(10);
			Console.ReadLine();
		}
	}

	class Sort
	{
		// IMPORTANT: Last index of bubble and selection sort are not being sorted.
		LL list = new LL();
		public Sort(int n)
		{
			Initialize(n);
			Console.WriteLine(list.Count());
			//Console.WriteLine("Insertion Sort");
			//LLInsertion();
			//Console.WriteLine("Bubble Sort");
			//LLBubble();
			//Console.WriteLine("Selection Sort");
			LLSelection();
		}

		public void Initialize(int n)
		{
			Random rnd = new Random();
			string ascii = "abcdefghijklmnopqrstuvwxyz";
			for(int i = 0; i < n; i++)
			{
				list.Insert(ascii[rnd.Next(0, 26)]);
			}
			Console.WriteLine("initialized");
		}

		#region LinkedListSorts
		public void LLInsertion()
		{
			Console.WriteLine();
			Console.WriteLine("Unsorted:");
			list.Print();
			for(int i = 1; i < list.Count(); i++)
			{
				for(int j = i + 1; j > 1 && list.GetData(j) < list.GetData(j - 1); j--)
				{
					list.Swap(j, j - 1);
				}
			}
			Console.WriteLine();
			Console.WriteLine("Sorted:");
			list.Print();
		}

		public void LLBubble()
		{
			Console.WriteLine();
			Console.WriteLine("Unsorted:");
			list.Print();
			for(int i = 0; i < list.Count(); i++)
			{
				for(int j = 1; j < list.Count() - i; j++)
				{
					if(list.GetData(j - 1) > list.GetData(j))
					{
						list.Swap(j - 1, j);
					}
				}
			}
			Console.WriteLine();
			Console.WriteLine("Sorted:");
			list.Print();
		}

		public void LLSelection()
		{
			Console.WriteLine();
			Console.WriteLine("Unsorted:");
			list.Print();
			for(int i = 0; i < list.Count(); i++)
			{
				int bigindex = 0;
				for(int j = 1; j < list.Count() - i; j++)
				{
					if(list.GetData(j) > list.GetData(bigindex))
					{
						bigindex = j;
					}
					list.Swap(bigindex, list.Count() - i - 1);
				}
			}
			Console.WriteLine();
			Console.WriteLine("Sorted:");
			list.Print();
		}

		public void LLMerge()
		{

		}
		#endregion
		#region ArraySorts
		public void ArrInsertion()
		{

		}
		public void ArrBubble()
		{

		}
		public void ArrSelection()
		{

		}
		public void ArrMerge()
		{

		}
		#endregion
	}
	class Node
	{
		public Node() { }
		public Node(Node n)
		{
			Next = n;
		}
		public Node(char d, Node n)
		{
			Data = d;
			Next = n;
		}

		public char Data { get; set; }
		public Node Next { get; set; }
	}
	class LL
	{
		private Node head, tail, current;
		private int count;

		public LL()
		{ // clear list and initialize
			current = tail = new Node(null);
			head = new a3.Node(tail);
			count = 0;
		}

		public void Insert(char d)	// APPEND
		{
			tail.Next = new a3.Node(null);
			tail.Data = d;
			tail = tail.Next;
			count++;
		}
		public void Insert(char d, int index) // INSERT AT X
		{
			current = GoToIndex(current, index);
			current.Next = new a3.Node(current.Data, current.Next);
			current.Data = d;
			if(tail == current) { tail = current.Next; }
			count++;
		}

		public void Swap(int index1, int index2)
		{
			Node current2 = new Node(null);
			current = GoToIndex(current, index1);
			current2 = GoToIndex(current2, index2);
			char temp = current2.Data;
			current2.Data = current.Data;
			current.Data = temp;
		}

		public char Delete(int index)
		{
			current = head.Next;
			current = GoToIndex(current, index);
			if (current == tail) { return '\0'; }
			char c = current.Data;
			current.Data = current.Next.Data;
			if(current.Next == tail) { tail = current; }
			current.Next = current.Next.Next;
			count--;
			return c;
		}

		public char GetData(int index)
		{
			if (index > count) { return '\0'; }
			current = GoToIndex(current, index);
			return current.Data;
		}

		public void Print()
		{
			current = head;
			for(int i = 0; i < count; i++)
			{
				current = current.Next;
				Console.Write($"{current.Data} ");
			}
		}

		public Node GoToIndex(Node pointer, int index)
		{
			pointer = head.Next;
			for (int i = 1; i < index; i++)
			{
				pointer = pointer.Next;
			}
			return pointer;
		}
		public int Count() { return count; }
	}
}

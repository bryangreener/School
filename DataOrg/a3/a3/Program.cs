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

	class UI
	{
		public void Unsorted(LL list, string s)
		{
			Console.WriteLine();
			Console.WriteLine($"{s} Sort");
			Console.Write("Unsorted: ");
			list.Print();
		}
		public void Sorted(LL list)
		{
			Console.WriteLine();
			Console.Write("Sorted:   ");
			list.Print();
			Console.WriteLine();
		}
	}

	class Sort
	{
		UI ui = new UI();
		LL list = new LL();
		private int n;

		public Sort(int n)
		{
			this.n = n;
			ListSorts();
			ArraySorts();
		}

		public void ListSorts()
		{
			Initialize();
			ui.Unsorted(list, "Insertion");
			LLInsertion(list);
			ui.Sorted(list);

			list = new LL();
			Initialize();
			ui.Unsorted(list, "Bubble");
			LLBubble(list);
			ui.Sorted(list);

			list = new LL();
			Initialize();
			ui.Unsorted(list, "Selection");
			LLSelection(list);
			ui.Sorted(list);

			list = new LL();
			Initialize();
			ui.Unsorted(list, "Merge");
			LLMerge(list, new LL(), 1, list.Count());
			ui.Sorted(list);
		}

		public void ArraySorts()
		{

		}

		private void Initialize()
		{
			Random rnd = new Random();
			string ascii = "abcdefghijklmnopqrstuvwxyz";
			for(int i = 0; i < n; i++)
			{
				list.Insert(ascii[rnd.Next(0, 26)]);
			}
		}

		#region LinkedListSorts
		private void LLInsertion(LL list)
		{
			for(int i = 1; i < list.Count(); i++)
			{
				for(int j = i + 1; j > 1 && list.GetData(j) < list.GetData(j - 1); j--)
				{
					list.Swap(j, j - 1);
				}
			}
		}

		private void LLBubble(LL list)
		{
			for(int i = 0; i < list.Count(); i++)
			{
				for(int j = 1; j < list.Count() - i + 1; j++)
				{
					if(list.GetData(j - 1) > list.GetData(j))
					{
						list.Swap(j, j - 1);
					}
				}
			}
		}

		private void LLSelection(LL list)
		{
			for(int i = 0; i < list.Count(); i++)
			{
				int bigindex = 0;
				for(int j = 1; j < list.Count() - i + 1; j++)
				{
					if(list.GetData(j) > list.GetData(bigindex))
					{
						bigindex = j;
					}
				}
				list.Swap(bigindex, list.Count() - i);
			}
		}

		private void LLMerge(LL list, LL temp, int left, int right)
		{
			if(left == right) { return; }
			int mid = (left + right) / 2;
			LLMerge(list, temp, left, mid);
			LLMerge(list, temp, mid + 1, right);
			for(int i = left; i <= right; i++)
			{
				temp.Insert(list.GetData(i), i);
			}

			int i1 = left, i2 = mid + 1;
			for (int current = left; current <= right; current++)
			{
				if (i1 == mid + 1) { list.Replace(temp.GetData(i2++), current); }
				else if (i2 > right) { list.Replace(temp.GetData(i1++), current); }
				else if (temp.GetData(i1) <= temp.GetData(i2)) { list.Replace(temp.GetData(i1++), current); }
				else { list.Replace(temp.GetData(i2++), current); }
			}
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

		public void Replace(char d, int index)
		{
			current = GoToIndex(current, index);
			current.Data = d;
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

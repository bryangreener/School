using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace a3
{
	class Program
	{
		static void Main(string[] args)
		{
			UI ui = new UI();
			Sort s = new Sort(ui.N(), ui.Name());
			Console.ReadLine();
		}
	}

	class UI
	{
		public int N()
		{
			Console.WriteLine("Please enter an integer N");
			string input = Console.ReadLine();
			while(input == "")
			{
				Console.WriteLine("INVALID INPUT - Please enter an integer for N");
				input = Console.ReadLine();
			}
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
			while(Convert.ToInt32(input) > 500)
			{
				Console.WriteLine("Sorry but bublesort can't handle that big of a number without comparing into infinity.");
				Console.WriteLine("Please enter a lower integer N");
				input = Console.ReadLine();
			}
			return Convert.ToInt32(input);
		}
		public string Name()
		{
			Console.WriteLine("Please Enter Last Name");
			string name = Console.ReadLine();
			while (name.ToLower().Replace(" ", string.Empty) == "" || name.Any(char.IsDigit))
			{
				Console.WriteLine("INVALID NAME - Please enter a valid name");
				name = Console.ReadLine();
			}
			return name.ToLower().Replace(" ", "");
		}
		
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

		private void ArrPrint(char[] arr)
		{
			foreach(char c in arr)
			{
				Console.Write($"{c} ");
			}
		}

		public void TotalTime(double time, int loops)
		{
			Console.WriteLine($"Elapsed Time: {time/loops}ms");
		}
	}

	class Sort
	{
		UI ui = new UI();
		Random rnd = new Random();
		Stopwatch sw = new Stopwatch();

		private int n, testLoops = 1000;
		private string name, sortString;
		private string ascii = "abcdefghijklmnopqrstuvwxyz";
		private double ms = 0;

		public Sort(int n, string name)
		{
			this.n = n;
			this.name = name;

			if (n > 100)
			{
				testLoops = 100;
			}

			sortString = CharValues();

			ui.LLHeader(n, name, sortString);
			ListSorts();

			ui.ArrHeader(n, name, sortString);
			ArraySorts();
		}

		private void ListSorts()
		{
			LL list = new LL();

			LLInitialize(list);
			ui.LLUnsorted(list, "Insertion");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); LLInsertion(list); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.LLSorted(list);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			list = new LL();
			LLInitialize(list);
			ui.LLUnsorted(list, "Bubble");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); LLBubble(list); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.LLSorted(list);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			list = new LL();
			LLInitialize(list);
			ui.LLUnsorted(list, "Selection");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); LLSelection(list); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.LLSorted(list);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			list = new LL();
			LLInitialize(list);
			ui.LLUnsorted(list, "Merge");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); LLMerge(list, new LL(), 1, list.Count()); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.LLSorted(list);
			ui.TotalTime(ms, testLoops);
			ms = 0;
		}
		private void ArraySorts()
		{
			char[] arr = new char[n];

			ArrInitialize(arr);
			ui.ArrUnsorted(arr, "Insertion");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); ArrInsertion(arr); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.ArrSorted(arr);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			ArrInitialize(arr);
			ui.ArrUnsorted(arr, "Bubble");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); ArrBubble(arr); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.ArrSorted(arr);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			ArrInitialize(arr);
			ui.ArrUnsorted(arr, "Selection");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); ArrSelection(arr); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.ArrSorted(arr);
			ui.TotalTime(ms, testLoops);
			ms = 0;

			ArrInitialize(arr);
			ui.ArrUnsorted(arr, "Merge");
			for (int i = 0; i < testLoops; i++)
			{
				sw.Start(); ArrMerge(arr, new char[n], 0, arr.Count() - 1); sw.Stop(); ms += sw.Elapsed.TotalMilliseconds; sw.Reset();
			}
			ui.ArrSorted(arr);
			ui.TotalTime(ms, testLoops);
			ms = 0;

		}

		private void LLInitialize(LL list)
		{
			for(int i = 0; i < n; i++)
			{
				list.Insert(ascii[rnd.Next(0, 26)]);
			}
		}
		private char[] ArrInitialize(char[] arr)
		{
			for(int i = 0; i < n; i++)
			{
				arr[i] = ascii[rnd.Next(0, 26)];
			}
			return arr;
		}

		private string CharValues()
		{
			string tempAscii = "abcdefghijklmnopqrstuvwxyz";
			char[] chars = new char[26];
			char[] nameArr = name.ToCharArray().Distinct().ToArray();

			for(int i = 0; i < nameArr.Count(); i++)
			{
				chars[i] = nameArr[i];
				tempAscii = tempAscii.Remove(tempAscii.IndexOf(nameArr[i]), 1);
			}
			for(int i = nameArr.Count(); i < 26; i++)
			{
				chars[i] = tempAscii[i - nameArr.Count()];
			}
			return new string(chars);
		}
		private int CharCompare(char x, char y)
		{
			if (sortString.IndexOf(x) < sortString.IndexOf(y)) { return -1; }
			else if (sortString.IndexOf(x) > sortString.IndexOf(y)) { return 1; }
			else { return 0; }
		}

		#region LinkedListSorts
		private void LLInsertion(LL list)
		{
			for(int i = 1; i < list.Count(); i++)
			{
				for(int j = i + 1; j > 1 && CharCompare(list.GetData(j), list.GetData(j - 1)) == -1; j--)
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
					if(CharCompare(list.GetData(j - 1), list.GetData(j)) == 1)
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
					if(CharCompare(list.GetData(j), list.GetData(bigindex)) == 1)
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
				else if (CharCompare(temp.GetData(i1), temp.GetData(i2)) == -1 || CharCompare(temp.GetData(i1), temp.GetData(i2)) == 0)
				{
					list.Replace(temp.GetData(i1++), current);
				}
				else { list.Replace(temp.GetData(i2++), current); }
			}
		}

		#endregion
		#region ArraySorts
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

		private void Swap(ref char a, ref char b)
		{
			char temp = a;
			a = b;
			b = temp;
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
		{
			current = tail = new Node(null);
			head = new a3.Node(tail);
			count = 0;
		}

		public void Clear()
		{
			head = null;
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

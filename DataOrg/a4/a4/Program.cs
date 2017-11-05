using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace a4
{
    class Program
    {
        static void Main(string[] args)
        {
			string text = File.ReadAllText("namelist.txt");
			var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			string[][] input = new string[lines.Length][];

			for (int i = 0; i < lines.Length; i++)
			{
				input[i] = lines[i].Split('\t');
			}

			Controller c = new Controller(input);

			Console.ReadLine();
        }

		
    }

	class Controller
	{
		private string[][] Input { get; set; }
		
		public Controller(string[][] input)
		{
			Input = input;

			MinHeapControl();

			Console.WriteLine();
			Console.WriteLine("========================================");

			MaxHeapControl();
		}

		private void MinHeapControl()
		{
			MinHeap heap = new MinHeap();
			foreach (var r in Input)
			{
				heap.Insert(r[1].ToLower(), r[0].ToLower());
			}
			Console.WriteLine("========================================");
			Console.WriteLine("SEARCHES".PadRight(20,' ') + "DFS       BFS");
			foreach(var s in Input)
			{
				Console.WriteLine($"{s[0].PadRight(20, ' ')}" + $"{heap.DFS(s[0].ToLower())}".PadRight(10, ' ') + $"{heap.BFS(s[0].ToLower())}");
			}

			Console.WriteLine("========================================");
			Console.WriteLine("==== PREORDER  ====");
			heap.PreOrder(heap.ReturnRoot());
			Console.WriteLine("====  INORDER  ====");
			heap.InOrder(heap.ReturnRoot());
			Console.WriteLine("==== POSTORDER ====");
			heap.PostOrder(heap.ReturnRoot());
		}
		private void MaxHeapControl()
		{
			MaxHeap heap = new MaxHeap(Input.Length);
			foreach (var r in Input)
			{
				heap.Insert(r[1].ToLower(), r[0].ToLower());
			}

			Console.WriteLine("SEARCHES".PadRight(20, ' ') + "DFS       BFS");
			foreach (var s in Input)
			{
				Console.WriteLine($"{s[1].PadRight(20, ' ')}" + $"{heap.DFS(s[1].ToLower())}".PadRight(10, ' ') + $"{heap.BFS(s[1].ToLower())}");
			}


			Console.WriteLine("========================================");
			Console.WriteLine("==== PREORDER  ====");
			heap.PreOrder(0);
			Console.WriteLine("====  INORDER  ====");
			heap.InOrder(0);
			Console.WriteLine("==== POSTORDER ====");
			heap.PostOrder(0);
		}
		private void BSTControl()
		{

		}
	}

	class MinHeapNode
	{
		public MinHeapNode() { }

		public MinHeapNode(MinHeapNode p, MinHeapNode l, MinHeapNode r)
		{
			Parent = p;
			Left = l;
			Right = r;
		}

		public MinHeapNode(string f, string l, int s, MinHeapNode parent, MinHeapNode left, MinHeapNode right)
		{
			First = f;
			Last = l;
			Subtree = s;
			Parent = parent;
			Left = left;
			Right = right;
		}

		public string First { get; set; }
		public string Last { get; set; }
		public int Subtree { get; set; }

		public MinHeapNode Parent { get; set; }
		public MinHeapNode Left { get; set; }
		public MinHeapNode Right { get; set; }
	}
	class MinHeap
	{
		private MinHeapNode root, current;
		private int count;

		public MinHeap()
		{
			root = current = new MinHeapNode(null, null, null);
			root.Subtree = 0;
			count = 0;
		}

		public void Insert(string first, string last)
		{
			current = root;
			if (current.Last == null)
			{
				current.First = first;
				current.Last = last;
				current.Subtree++;
				count++;
				return;
			}
			while(current != null)
			{
				if(current.Left == null)
				{
					current.Subtree++;
					current.Left = new MinHeapNode(first, last, 1, current, null, null);
					current = current.Left;
					count++;
					Heapify();
					return;
				}
				if(current.Right == null)
				{
					current.Subtree++;
					current.Right = new MinHeapNode(first, last, 1, current, null, null);
					current = current.Right;
					count++;
					Heapify();
					return;
				}
				if (current.Left.Subtree > current.Right.Subtree)
				{
					if(current.Right == null)
					{
						current.Right = new MinHeapNode(first, last, 1, current, null, null);
						current = current.Right;
						count++;
						Heapify();
						return;
					}
					else
					{
						current.Subtree++;
						current = current.Right;
					}
				}
				else if(current.Left.Subtree <= current.Right.Subtree)
				{
					if (current.Left == null)
					{
						current.Left = new MinHeapNode(first, last, 1, current, null, null);
						current = current.Left;
						count++;
						Heapify();
						return;
					}
					else
					{
						current.Subtree++;
						current = current.Left;
					}
				}
			}
		}
		
		public Tuple<string,string> Delete(string last)
		{
			current = root;
			if(current == null) { return null; }
			while (current != null)
			{
				if (current.Left == null && current.Right == null)
				{
					var ret = Tuple.Create(root.First, root.Last);
					root.First = current.First;
					root.Last = current.Last;
					current = current.Parent;
					if (current.Right != null) { current.Right = null; }
					else if (current.Left != null) { current.Left = null; }
					Heapify();
					return ret;
				}
				else if (current.Right != null && current.Left != null)
				{
					current = current.Right;
				}
				else if (current.Left != null && current.Right == null)
				{
					current = current.Left;
				}
				else if (current.Left.Subtree < current.Right.Subtree)
				{
					current = current.Right;
				}
				else
				{
					current = current.Left;
				}
			}
			return Tuple.Create("Not", "Found");
		}

		public Tuple<int,int> DFS(string last)
		{
			var pos = DFSUtil(root, last, 0, 1);
			return Tuple.Create(pos.Item1 + 1, pos.Item2);
		}
		public Tuple<int,int> DFSUtil(MinHeapNode r, string last, int x, int y)
		{
			if (r == null) { return Tuple.Create(0,0); }
			// Preorder DFS
			if(r.Last == last) { return Tuple.Create(x, y); }
			var pos = DFSUtil(r.Left, last, (x << 1), y+1);
			if(pos.Item2 != 0) { return pos; }
			pos = DFSUtil(r.Right, last, (x << 1) + 1, y+1);
			return pos;
		}

		public Tuple<int, int> BFS(string last)
		{
			var pos = BFSUtil(root, last);
			return Tuple.Create(pos.Item1, pos.Item2);
		}
		private Tuple<int, int> BFSUtil(MinHeapNode r, string last)
		{
			Queue<MinHeapNode> q = new Queue<MinHeapNode>();
			int x = 0, y = 0, counter = 0;
			if (r == null) { return Tuple.Create(-1, -1); }
			q.Enqueue(r);
			while (q.Count() != 0)
			{
				r = q.Dequeue();
				counter++;
				if (r.Last == last)
				{
					// X calc
					if (counter == 1 || PowerOfTwo(counter)) { x = 1; }
					else if (counter == 3) { x = 2; } // Weird case where this doesnt work...
					else { x = counter - (int)Math.Ceiling(Math.Log(counter) / Math.Log(2)); }

					//Y calc
					if (counter == 1) { y = 1; }
					else if (PowerOfTwo(counter)) { y = (int)(Math.Log(counter) / Math.Log(2)) + 1; }
					else { y = (int)(Math.Ceiling(Math.Log(counter) / Math.Log(2))); }

					return Tuple.Create(x, y);
				}
				else
				{
					if (r.Left != null)
					{
						q.Enqueue(r.Left);
					}
					if (r.Right != null)
					{
						q.Enqueue(r.Right);
					}
				}
			}
			return Tuple.Create(-1, -1);
		}
		private bool PowerOfTwo(int x)
		{
			while (((x % 2) == 0) && x > 1) { x /= 2; }
			return (x == 1);
		}

		private void Heapify()
		{
			while(current.Parent != null)
			{
				if(string.Compare(current.Parent.Last, current.Last) == 1)
				{
					Swap(current, current.Parent);
				}
				else
				{
					return;
				}
			}
			Console.WriteLine("Error in Heapify(). Exited WHILE loop. Parent == null");
			return;
		}

		public void Swap(MinHeapNode c, MinHeapNode p)
		{
			string tempFirst = p.First;
			string tempLast = p.Last;
			p.First = c.First;
			p.Last = c.Last;
			c.First = tempFirst;
			c.Last = tempLast;
		}

		public void PreOrder(MinHeapNode r)
		{
			if (r == null) { return; }
			Console.WriteLine(r.Last);
			PreOrder(r.Left);
			PreOrder(r.Right);
		}
		public void InOrder(MinHeapNode r)
		{
			if(r == null) { return; }
			InOrder(r.Left);
			Console.WriteLine(r.Last);
			InOrder(r.Right);
		}
		public void PostOrder(MinHeapNode r)
		{
			if(r == null) { return; }
			PostOrder(r.Left);
			PostOrder(r.Right);
			Console.WriteLine(r.Last);
		}

		public MinHeapNode ReturnRoot()
		{
			return root;
		}

		public int Height(MinHeapNode r)
		{
			if(r == null){ return 0; }
			else
			{
				int lHeight = Height(r.Left);
				int rHeight = Height(r.Right);
				if(lHeight > rHeight) { return lHeight + 1; }
				else { return rHeight + 1; }
			}
		}
		public int Count()
		{
			return count;
		}
	}


	class Person
	{
		public Person() { }
		public Person(string first, string last)
		{
			First = first;
			Last = last;
		}
		public bool Visited { get; set; }
		public string First { get; set; }
		public string Last { get; set; }
	}
	class MaxHeap
	{
		private Person[] Heap { get; set; }
		private int current, n = 0;

		public MaxHeap(int length)
		{
			Heap = new Person[length*2 + 1];
			current = -1;
		}

		public void Insert(string first, string last)
		{
			if(n >= Heap.Length) { return; } // Shouldnt ever reach this line since size is known in advance.
			current = n++;
			Heap[current] = new Person(first, last);
			while ((current != 0) && (Heap[current].First.CompareTo(Heap[Parent(current)].First) > 0))
			{
				Swap(current, Parent(current));
				current = Parent(current);
			}
		}

		private void Heapify()
		{
			for(int i = (n / 2) - 1; i >= 0; i--) { SiftDown(i); }
		}

		private void SiftDown(int pos)
		{
			if((pos < 0) || (pos >= n)) { return; } // Shouldnt ever do this... bad position
			while(!IsLeaf(pos))
			{
				int j = LeftChild(pos);
				if ((j < (n - 1)) && (Heap[j].First.CompareTo(Heap[j + 1].First) < 0)) { j++; }
				if(Heap[pos].First.CompareTo(Heap[j].First) >= 0) { return; }
				Swap(pos, j);
				pos = j;
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
				while((pos > 0) && (Heap[pos].First.CompareTo(Heap[Parent(pos)].First) > 0))
				{
					Swap(pos, Parent(pos));
					pos = Parent(pos);
				}
				if(n != 0) { SiftDown(pos); }
			}
			return Heap[n];
		}

		public Tuple<int,int> BFS(string first)
		{
			var pos = BFSUtil(0, first);
			return Tuple.Create(pos.Item1, pos.Item2);
		}
		private Tuple<int,int> BFSUtil(int r, string first)
		{
			Queue<int> q = new Queue<int>();
			int x = 0, y = 0, counter = 0;
			if(Heap[r] == null) { return Tuple.Create(-1, -1); }
			q.Enqueue(r);
			while(q.Count() != 0)
			{
				int n = q.Dequeue();
				counter++;
				if(Heap[n].First == first)
				{
					// X calc
					if(counter == 1 || PowerOfTwo(counter)) { x = 1; }
					else if(counter == 3) { x = 2; } // Weird case where this doesnt work...
					else { x = counter - (int)Math.Ceiling(Math.Log(counter) / Math.Log(2)); }

					//Y calc
					if(counter == 1) { y = 1; }
					else if (PowerOfTwo(counter)) { y = (int)(Math.Log(counter) / Math.Log(2)) + 1; }
					else { y = (int)(Math.Ceiling(Math.Log(counter) / Math.Log(2))); }

					return Tuple.Create(y, x); }
				else
				{
					if(Heap[LeftChild(n)] != null)
					{
						q.Enqueue(LeftChild(n));
					}
					if (Heap[RightChild(n)] != null)
					{
						q.Enqueue(RightChild(n));
					}
				}
			}
			return Tuple.Create(-1, -1);
		}
		private bool PowerOfTwo(int x)
		{
			while(((x % 2) == 0) && x > 1) { x /= 2; }
			return (x == 1);
		}

		public Tuple<int,int> DFS(string first)
		{
			var pos = DFSUtil(0, first, 0, 1);
			return Tuple.Create(pos.Item2, pos.Item1 + 1);
		}
		private Tuple<int,int> DFSUtil(int r, string first, int x, int y)
		{
			if (Heap[r] == null) { return Tuple.Create(0, 0); }

			if (Heap[r].First == first) { return Tuple.Create(x, y); }
			var pos = DFSUtil(LeftChild(r), first, (x << 1), y + 1);
			if (pos.Item2 != 0) { return pos; }
			pos = DFSUtil(RightChild(r), first, (x << 1) + 1, y + 1);
			return pos;
		}

		public void PreOrder(int pos)
		{
			if(Heap[pos] == null) { return; }
			Console.WriteLine($"{Heap[pos].First}");
			PreOrder(LeftChild(pos));
			PreOrder(RightChild(pos));
		}
		public void InOrder(int pos)
		{
			if (Heap[pos] == null) { return; }
			PreOrder(LeftChild(pos));
			Console.WriteLine($"{Heap[pos].First}");
			PreOrder(RightChild(pos));
		}
		public void PostOrder(int pos)
		{
			if (Heap[pos] == null) { return; }
			PreOrder(LeftChild(pos));
			PreOrder(RightChild(pos));
			Console.WriteLine($"{Heap[pos].First}");
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
	}
}

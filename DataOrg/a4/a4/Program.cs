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
			// NOTE: LAST,FIRST order so input item1 is LAST NAME
			Input = input;

			MinHeapControl();

			//Console.WriteLine();
			//Console.WriteLine("========================================");

			//MaxHeapControl();

			//BSTControl();
		}

		private void MinHeapControl()
		{
			MinHeap heap = new MinHeap();
			foreach (var r in Input)
			{
				heap.Insert(Tuple.Create(r[1].ToLower(), r[0].ToLower()));
			}

			heap.AssignXY(heap.ReturnRoot(), 0, 0);

			Console.WriteLine("========================================");
			Console.WriteLine("SEARCHES".PadRight(20,' ') + "DFS       BFS");
			foreach(var s in Input)
			{
				Console.WriteLine($"{s[0].PadRight(20, ' ')}" + $"{heap.DFS(s[0].ToLower())}".PadRight(10, ' ') + $"{heap.BFS(s[0].ToLower())}");
			}

			Console.WriteLine("========================================");
			heap.Traverse();
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
			BST bst = new BST();
			foreach(var v in Input)
			{
				bst.Insert(Tuple.Create(v[1].ToLower(), v[0].ToLower()));
			}
			// Print results
			Console.WriteLine("SEARCHES".PadRight(20, ' ') + "DFS       BFS");
			foreach (var s in Input)
			{
				Console.WriteLine($"{s[0].PadRight(20, ' ')}" + $"{bst.Get(s[0].ToLower())}".PadRight(10, ' '));
				//Console.WriteLine($"{s[1].PadRight(20, ' ')}" + $"{bst.DFS(s[1].ToLower())}".PadRight(10, ' ') + $"{bst.BFS(s[1].ToLower())}");
			}
			Console.WriteLine("========================================");
			bst.Traverse();
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
		// WORKING
		public void Insert(Tuple<string,string> val)
		{
			root = Insert(root, val);
		}

		// WORKING
		public Tuple<int, int> DFS(string last)
		{
			MinHeapNode h = DFSUtil(root, last);
			if (h == null) { return Tuple.Create(-1, -1); }
			else { return Tuple.Create(h.X, h.Y); }
		}
		// WORKING
		public Tuple<int, int> BFS(string last)
		{
			return BFSUtil(root, last);
		}

		// NEW
		public int Height(MinHeapNode h)
		{
			if (h == null) { return 0; }
			else
			{
				int l = Height(h.Left);
				int r = Height(h.Right);
				if (l > r) { return l + 1; }
				else { return r + 1; }
			}
		}

		// WORKING oh my god it finally works
		public int AssignXY(MinHeapNode h, int x, int y)
		{
			if (h == null) { return 0; }
			h.X = x;
			h.Y = y;
			AssignXY(h.Left, x, y + 1);
			return AssignXY(h.Right, x + 1, y + 1);
		}

		// WORKING
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

		// NEW
		public MinHeapNode ReturnRoot()
		{
			return root;
		}
		#endregion

		#region Private Methods
		// WORKING
		private MinHeapNode Insert(MinHeapNode h, Tuple<string,string> val)
		{
			MinHeapNode newNode = new MinHeapNode();
			if(h == null) { return new MinHeapNode(val, 1); }
			if(h.Left == null || h.Right == null)
			{
				if(h.Left == null) { newNode = h.Left = Insert(h.Left, val); newNode.Parent = h; }
				else { newNode = h.Right = Insert(h.Right, val); newNode.Parent = h; }
			}
			else if(h.Left.N < h.Right.N) { h.Left = Insert(h.Left, val); }
			else{ h.Right = Insert(h.Right, val); }

			while(newNode.Parent != null && newNode.Value.Item2.CompareTo(h.Value.Item2) < 0)
			{
				Swap(newNode, h);
				newNode = h;
			}

			h.N = Size(h.Left) + Size(h.Right) + 1;
			return h;
		}

		// NEW
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

		// WORKING
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
		// WORKING
		private Tuple<int, int> BFSUtil(MinHeapNode h, string last)
		{
			Queue<MinHeapNode> q = new Queue<MinHeapNode>();
			int x = 0, y = 0, counter = 0;
			if (h == null) { return Tuple.Create(0, 0); }
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

		// PROBABLY WORKING
		private MinHeapNode UpHeapify(MinHeapNode h)
		{
			if(h.Parent == null) { return h; }
			int cmp = h.Value.Item2.CompareTo(h.Parent.Value.Item2);
			if (cmp < 0) { return UpHeapify(Swap(h, h.Parent)); }
			else { return h; }
		}
		// NEW
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

		// WORKING
		private MinHeapNode Swap(MinHeapNode h1, MinHeapNode h2)
		{
			var temp = h1.Value;
			h1.Value = h2.Value;
			h2.Value = temp;
			return h2;
		}

		// NEW
		private MinHeapNode GoToLast(MinHeapNode h)
		{
			if(h == null) { return null; }
			if(h.N == 1) { return h; }
			int cmp = h.Left.N.CompareTo(h.Right.N);
			if(cmp > 0) { return GoToLast(h.Left); }
			else { return GoToLast(h.Right); }
		}


		// WORKING
		private void PreOrder(MinHeapNode h)
		{
			if (h == null) { return; }
			Console.WriteLine(h.Value.Item2);
			PreOrder(h.Left);
			PreOrder(h.Right);
		}
		// WORKING
		private void InOrder(MinHeapNode h)
		{
			if(h == null) { return; }
			InOrder(h.Left);
			Console.WriteLine(h.Value.Item2);
			InOrder(h.Right);
		}
		// WORKING
		private void PostOrder(MinHeapNode h)
		{
			if(h == null) { return; }
			PostOrder(h.Left);
			PostOrder(h.Right);
			Console.WriteLine(h.Value.Item2);
		}

		// WORKING
		private int Size()
		{
			return Size(root);
		}
		// WORKING
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

	class BSTNode
	{
		public BSTNode(Tuple<string,string> value, int n, bool color)
		{
			Value = value;
			N = n;
			Color = color;
		}
		public Tuple<string,string> Value { get; set; }
		public int N { get; set; }
		public bool Color { get; set; }
		public BSTNode Left { get; set; }
		public BSTNode Right { get; set; }
	}
	class BST
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
}

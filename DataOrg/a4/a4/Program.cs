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
			string text = File.ReadAllText("namelistSMALL.txt");
			var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			string[][] input = new string[lines.Length][];

			for (int i = 0; i < lines.Length; i++)
			{
				input[i] = lines[i].Split('\t');
			}

			MinHeap heap = new MinHeap();

			foreach(var r in input)
			{
				heap.Insert(r[1].ToLower(), r[0].ToLower());
			}

			Console.WriteLine($"D	(3,3):	{heap.DFS("d")} -- {heap.BFS("d")}");
			Console.WriteLine($"CA	(2,2):	{heap.DFS("ca")} -- {heap.BFS("ca")}");
			Console.WriteLine($"BA	(1,2):	{heap.DFS("ba")} -- {heap.BFS("ba")}");
			Console.WriteLine($"BB	(2,3):	{heap.DFS("bb")} -- {heap.BFS("bb")}");
			Console.WriteLine($"CB	(1,3):	{heap.DFS("cb")} -- {heap.BFS("cb")}");
			Console.WriteLine($"A	(1,1):	{heap.DFS("a")} -- {heap.BFS("a")}");

			/*
			Console.WriteLine("INORDER TRAVERSAL");
			heap.InOrder(heap.ReturnRoot());
			Console.WriteLine("PREORDER TRAVERSAL");
			heap.PreOrder(heap.ReturnRoot());*/
			Console.ReadLine();
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

		public Tuple<int,int> BFS(string last)
		{
			int h = Height(root);
			var pos = Tuple.Create(0,0,false);
			for(int i = 0; i < h; i++)
			{
				pos = BFSUtil(root, last, i, 0);
				if (pos.Item3 == true)
				{
					break;
				}
			}
			return Tuple.Create(pos.Item1, pos.Item2);
		}
		public Tuple<int,int,bool> BFSUtil(MinHeapNode r, string last, int level, int x)
		{
			if(r == null) { return Tuple.Create(-1,-1,false); }
			if (r.Last == last) { return Tuple.Create(x, level, true); }
			if(level == 1)
			{
				if (r.Last == last) { return Tuple.Create(x, level, true); }
			}
			if(level > 1)
			{
				BFSUtil(r.Left, last, level - 1, (x << 1));
				BFSUtil(r.Right, last, level - 1, (x << 1) + 1);
			}
			return Tuple.Create(-1, -1, false);

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
}

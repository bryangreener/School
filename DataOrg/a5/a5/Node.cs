using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	public class Node<Z>
	{
		public Node() { }
		public Node(int degree)
		{
			Degree = degree;
			Children = new Node<Z>[degree + 1];
			Keys = new Z[degree - 1];
		}
		public Node(int degree, Z value, Node<Z> parent)
		{
			Degree = degree;
			Value = value;
			Parent = parent;
			Children = new Node<Z>[degree + 1];
			Keys = new Z[degree - 1];
		}
		public bool IsLeaf { get { return ChildCount == 0; } }
		public Z MaxValue { get; set; }
		public int Degree { get; set; }
		public Node<Z>[] Children { get; set; }
		public Z[] Keys { get; set; }
		public Node<Z> Parent { get; set; }
		public Z Value { get; set; }
		public int N { get; set; }
		public int ChildCount { get; set; }
		public int KeyCount { get; set; }
	}
}

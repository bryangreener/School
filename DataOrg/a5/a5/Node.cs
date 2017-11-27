using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	public class Node<Z>
	{
		public Node(int degree)
		{
			Degree = degree;
			Children = new List<Node<Z>>(degree);
			Keys = new List<Z>(degree);
		}
		public Node(Z value, Node<Z> parent)
		{
			Value = value;
			Parent = parent;
		}

		public int ChildCount
		{
			get { return this.Children.Count(); }
		}
		public bool IsLeaf
		{
			get { return (this.Children.Count() == 0); }
		}

		private int Degree { get; set; }
		public List<Node<Z>> Children { get; set; }
		public List<Z> Keys { get; set; }
		public Node<Z> Parent { get; set; }
		public Z Value { get; set; }
	}
}

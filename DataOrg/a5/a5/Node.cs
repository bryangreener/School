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
		public Node(Z value)
		{
			Value = value;
		}

		public Node<Z> Parent { get; set; }
		public Node<Z> A { get; set; }	// Left
		public Node<Z> B { get; set; }	// Left Mid
		public Node<Z> C { get; set; }	// Right Mid
		public Node<Z> D { get; set; }	// Right
		public Z KeyA { get; set; }
		public Z KeyB { get; set; }
		public Z KeyC { get; set; }
		public Z Value { get; set; }
		public int N { get; set; }
	}
}

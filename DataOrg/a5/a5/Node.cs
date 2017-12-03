using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	/// <summary>
	/// Generic node class used as object in BTree class.
	/// </summary>
	/// <typeparam name="Z">Generic parameter needed due to varying insert data types.</typeparam>
	public class Node<Z>
	{
		/// <summary>
		/// Default constructor used to create completely null node.
		/// </summary>
		public Node() { }

		/// <summary>
		/// Basic node constructor only used to initialize a node with no data associated with it.
		/// </summary>
		/// <param name="degree">Integer passed in to set Children and Key array lengths.</param>
		public Node(int degree)
		{
			Degree = degree;
			Children = new Node<Z>[degree + 1];
			Keys = new Z[degree - 1];
		}

		/// <summary>
		/// Main node constructor used both to initialize a node and to assign data/parent.
		/// </summary>
		/// <param name="degree">Integer passed in to set Children and Key array lengths.</param>
		/// <param name="value">Generic value containing the actual data stored in the node.</param>
		/// <param name="parent">Pointer to parent node in the tree.</param>
		public Node(int degree, Z value, Node<Z> parent)
		{
			Degree = degree;
			Value = value;
			Parent = parent;
			Children = new Node<Z>[degree + 1];
			Keys = new Z[degree - 1];
		}

		/// <summary>
		/// Accessor used to return a boolean status of whether or not a node is a leaf.
		/// </summary>
		public bool IsLeaf { get { return ChildCount == 0; } }
		/// <summary>
		/// MaxValue used to help search for items and insert new items.
		/// </summary>
		public Z MaxValue { get; set; }
		/// <summary>
		/// Degree of tree (max children per node).
		/// </summary>
		public int Degree { get; set; }
		/// <summary>
		/// Array of pointers to nodes that are the children of the current node.
		/// </summary>
		public Node<Z>[] Children { get; set; }
		/// <summary>
		/// Array of values used to traverse tree.
		/// </summary>
		public Z[] Keys { get; set; }
		/// <summary>
		/// Pointer to parent of current node.
		/// </summary>
		public Node<Z> Parent { get; set; }
		/// <summary>
		/// Data package for node.
		/// </summary>
		public Z Value { get; set; }
		/// <summary>
		/// Integer used to keep track of number of children.
		/// </summary>
		public int ChildCount { get; set; }
		/// <summary>
		/// Integer used to keep track of number of keys.
		/// </summary>
		public int KeyCount { get; set; }
	}
}

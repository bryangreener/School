using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	class BTree<Z> where Z : IComparable<Z>
	{
		private Node<Z> root;
		private int degree; // min degree
		public BTree(int degree)
		{
			root = null;
			this.degree = degree;
		}

		public Node<Z> Search(Z value)
		{
			return (root == null) ? null : root = SearchUtil(root, value);
		}

		public void Insert(Z value)
		{

		}

		private Node<Z> SearchUtil(Node<Z> n, Z value)
		{
			int i = 0;
			while(i < n.KeyCount && value.CompareTo(n.Keys[i]) > 0) { i++; }
			if(value.CompareTo(n.Keys[i]) == 0) { return n; }
			if (n.IsLeaf) { return null; }
			return n = SearchUtil(n.Children[i], value);
		}

		private Node<Z> InsertUtil(Node<Z> n, Z value)
		{
			if(root == null)
			{
				root = new Node<Z>(degree, value, null);
				root.Keys[0] = value;
				root.N = 1;
			}
			else
			{
				if(root.N == 2*degree-1)
				{
					Node<Z> newNode = new Node<Z>(degree);
					newNode.Children[0] = root;
					
				}
			}
		}

		private Node<Z> Split(int i, Node<Z> n)
		{
			Node<Z> m = new Node<Z>(n.Degree);
			m.N = degree - 1;

			for(int j = 0; j < degree - 1; j++)
			{
				m.Keys[j] = n.Keys[degree + j];
			}

			if(!n.IsLeaf)
			{
				for(int j = 0; j < degree; j++)
				{
					m.Children[j] = n.Children[degree + j];
				}
			}

			n.N = degree - 1;

			for(int j = n.ChildCount; j > i + 1; j++)
			{
				n.Children[j + 1] = n.Children[j];
			}

			n.Children[i + 1] = m;

			for(int j = n.ChildCount - 1; j >= i; j--)
			{
				n.Keys[j + 1] = n.Keys[j];
			}
			n.Keys[i] = n.Keys[degree - 1];
			n.N++;

			return n;
		}



		/*
		private Node<Z> root;
		private int Degree { get; set; }
		public BTree(int degree)
		{
			Degree = degree;
		}


		public Tuple<Node<Z>, int> Search(Z value)
		{
			return SearchUtil(root, root, 0, value);
		}

		public void Insert(Z value)
		{
			if (root == null)
			{
				root = new Node<Z>(Degree, value, null);
				return;
			}
			else if(root.IsLeaf)
			{
				Node<Z> newNode = new Node<Z>(Degree);
				if(value.CompareTo(root.Value) < 0)
				{
					newNode.Keys[0] = value;
					newNode.Children[0] = new Node<Z>(Degree, value, newNode);
					newNode.Keys[1] = root.Value;
					newNode.Children[1] = root;
					root.Parent = newNode;
					root = newNode;
				}
				else
				{
					newNode.Keys[0] = root.Value;
					newNode.Children[0] = root;
					root.Parent = newNode;
					newNode.Keys[1] = value;
					newNode.Children[1] = new Node<Z>(Degree, value, newNode);
					root = newNode;
				}
				return;
			}

			Tuple<Node<Z>,int> searchResult = Search(value);
			Insert(new Node<Z>(Degree, value, null), searchResult.Item1, searchResult.Item2);
		}

		private Tuple<Node<Z>, int> SearchUtil(Node<Z> n, Node<Z> m, int index, Z value)
		{
			int retVal = 0;
			if(n == null) { return Tuple.Create(m, index); }
			else
			{
				if(value.CompareTo(n.Keys[n.Keys.Count() - 1]) > 0) { return SearchUtil(n.Children[n.ChildCount - 1], n, Degree - 1, value); }
				else
				{
					for(int i = 0; i < Degree; i++)
					{
						if(value.CompareTo(n.Keys[i]) < 0) { retVal = i; break; }
					}
					return SearchUtil(n.Children[retVal], n, retVal, value);
				}
			}
		}

		private void Insert(Node<Z> n, Node<Z> m, int index) // Insert n into m at index m.Children[index]
		{
			
			if (m.ChildCount < Degree)
			{
				n.Parent = m;
				for(int i = m.ChildCount; i > index; i--)
				{
					m.Children[i] = m.Children[i - 1];
					if (i < Degree - 1)
					{

						if (m.Children[i].IsLeaf) { m.Keys[i] = m.Children[i].Value; }
						else { m.Keys[i] = m.Children[i].Keys[m.Children[i].Keys.Count()]; }
					}
				}
				m.Children[index] = n;
				if(index < m.Keys.Count())
				{
					if (n.IsLeaf) { m.Keys[index] = n.Value; }
					else { m.Keys[index] = n.Keys[Degree - 1]; }
				}
			}
			else
			{
				Node<Z> newNode = new Node<Z>(Degree);
				if (m.Parent == null)
				{
					newNode.Children[0] = m;
					m.Parent = newNode;
					newNode.Children[1] = Split(m);
					newNode.Children[1].Parent = newNode;
				}
				else
				{

					Insert(Split(m), m.Parent, index);
				}
			}
		}

		private Node<Z> Split(Node<Z> n)
		{
			int split = Convert.ToInt32(Math.Floor(Convert.ToDouble(Degree / 2)));
			Node<Z> newNode = new Node<Z>(Degree);
			for (int i = split, j = 0; i < n.ChildCount; i++, j++)
			{
				newNode.Children[j] = n.Children[i];
				n.Children[i] = null;
				newNode.Children[j].Parent = newNode;
				if(newNode.Children[j].IsLeaf)
				{
					newNode.Keys[j] = newNode.Children[j].Value;
				}
				else
				{
					newNode.Keys[j] = newNode.Children[j].Keys[newNode.Children[j].Keys.Count()];
				}
			}
			newNode.Parent = n.Parent;
			return newNode;
		}
		*/
	}
	
}

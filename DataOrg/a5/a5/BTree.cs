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
		private int degree, keyLength, childLength;
		public BTree(int degree)
		{
			root = null;
			this.degree = degree;
			keyLength = degree - 1;
			childLength = degree + 1;
		}
		public Node<Z> Search(Z value)
		{
			return SearchUtil(root, value);
		}

		public void Insert(Z value)
		{
			Node<Z> n = new Node<Z>(degree, value, null);
			n.MaxValue = value;
			if (root == null)
			{
				root = n;
			}
			else if (root.IsLeaf)
			{
				Node<Z> newRoot = new Node<Z>(degree);
				if (n.Value.CompareTo(root.Value) < 0)
				{
					newRoot.Children[0] = n;
					newRoot.Children[1] = root;
					root = newRoot;
					root.Keys[0] = root.Children[0].MaxValue;
					root.Keys[1] = root.Children[1].MaxValue;
					root.MaxValue = root.Keys[1];
					root.Children[0].Parent = root;
					root.Children[1].Parent = root;

					root.KeyCount = 2;
					root.ChildCount = 2;
				}
				else
				{
					newRoot.Children[0] = root;
					newRoot.Children[1] = n;
					root = newRoot;
					root.Keys[0] = root.Children[0].MaxValue;
					root.Keys[1] = root.Children[1].MaxValue;
					root.MaxValue = root.Keys[1];
					root.Children[0].Parent = root;
					root.Children[1].Parent = root;

					root.KeyCount = 2;
					root.ChildCount = 2;
				}
			}
			else
			{
				InsertUtil(n, root);
			}
		}

		private Node<Z> SearchUtil(Node<Z> n, Z value)
		{
			// if rightmost child exists AND value > all keys
			if(n.ChildCount > n.KeyCount && value.CompareTo(n.Children[n.KeyCount - 1].MaxValue) > 0 )
			{
				if(n.Children[n.KeyCount - 1].IsLeaf)
				{
					if(value.CompareTo(n.Children[n.KeyCount - 1].Value) == 0) { return n.Children[n.KeyCount - 1]; } // search hit
					else { return null; } // search midd
				}
				else { return SearchUtil(n.Children[n.KeyCount - 1], value); } // continue searching
			}
			for (int i = 0; i < n.KeyCount; i++) // for each key given (max keys = degree - 1)
			{
				if(n.Children[i].IsLeaf && value.CompareTo(n.Children[i].Value) == 0) { return n.Children[i]; } // search hit
				if (value.CompareTo(n.Keys[i]) <= 0) // search node's max < m.key[i]
				{
					if (n.Children[i].IsLeaf) { return null; } // search miss
					else { return SearchUtil(n.Children[i], value); } // continue searching
				}
			}
			return n;
		}

		private void InsertUtil(Node<Z> n, Node<Z> m)
		{
			m = InsertPos(n, m);
			FixKeys(m);
			if (m.ChildCount > degree)
			{
				Node<Z> newInternal = new Node<Z>(degree);
				newInternal = Split(m);
				if (m.Parent == null)
				{
					Node<Z> newRoot = new Node<Z>(degree);
					newRoot.Children[0] = m;
					newRoot.Children[1] = newInternal;
					root = newRoot;
					root.Keys[0] = root.Children[0].MaxValue;
					root.Keys[1] = root.Children[1].MaxValue;
					root.MaxValue = root.Keys[1];
					root.Children[0].Parent = root;
					root.Children[1].Parent = root;

					root.KeyCount = 2;
					root.ChildCount = 2;
				}
				else
				{
					InsertUtil(newInternal, m.Parent);
				}
			}
		}


		private Node<Z> InsertPos(Node<Z> n, Node<Z> m)
		{
			for (int i = 0; i < m.KeyCount; i++) // for each key given (max keys = degree - 1)
			{
				if (n.MaxValue.CompareTo(m.Keys[i]) < 0) // insert node's max < m.key[i]
				{
					if (m.Children[i].IsLeaf) // if new position is going to be a leaf
					{
						for (int j = m.ChildCount; j > i; j--) // shift m's children over to make room. Not Childcount-1 because it starts at first open place
						{
							m.Children[j] = m.Children[j - 1];
						}
						m.Children[i] = n; // insert
						n.Parent = m; // maintain links
						m.ChildCount++; // update child count
						return m;
					}
					else { return InsertPos(n, m.Children[i]); }
				}
			}

			// If not at leaf, AND last child isnt leaf AND node to be inserted is leaf
			if(m.ChildCount != 0 && !m.Children[m.ChildCount - 1].IsLeaf && m.Children[m.ChildCount - 1].KeyCount < degree + 1 && n.IsLeaf)
			{
				return InsertPos(n, m.Children[m.ChildCount - 1]); // move to far right
			}
			else if(m.Children[m.KeyCount] == null)
			{
				m.Children[m.KeyCount] = n;
				n.Parent = m;
			}
			else if(n.MaxValue.CompareTo(m.Children[m.KeyCount].MaxValue) < 0)
			{
				m.Children[m.KeyCount + 1] = m.Children[m.KeyCount];
				m.Children[m.KeyCount] = n;
				n.Parent = m;
			}
			else if(n.MaxValue.CompareTo(m.Children[m.KeyCount].MaxValue) > 0)
			{
				m.Children[m.KeyCount + 1] = n;
			}
			m.ChildCount++; // update childcount
			return m;
		}

		private Node<Z> Split(Node<Z> m)
		{
			int splitIndex = Convert.ToInt32(Math.Floor(Convert.ToDouble(m.ChildCount / 2)));
			Node<Z> newNode = new Node<Z>(degree);
			int oldCount = m.ChildCount;
			for (int i = splitIndex, j = 0; i < oldCount; i++, j++)
			{
				newNode.Children[j] = m.Children[i];
				newNode.Children[j].Parent = newNode;
				newNode.ChildCount++; // update childcount
				newNode.Keys[j] = newNode.Children[j].MaxValue;
				newNode.KeyCount++; // update keycount
				m.Children[i] = null;
				if(i < keyLength) { Array.Clear(m.Keys, i, 1); m.KeyCount--; }
			}
			m.MaxValue = m.Children[splitIndex - 1].MaxValue;
			newNode.MaxValue = newNode.Children[splitIndex - 1].MaxValue;
			m.ChildCount = splitIndex;
			FixKeys(m);
			FixKeys(newNode);
			return newNode;
		}

		private void FixKeys(Node<Z> n)
		{
			n.KeyCount = 0;
			for(int i = 0; i < childLength; i++)
			{
				if(n.Children[i] != null && i < keyLength)
				{
					n.Keys[i] = n.Children[i].MaxValue;
					n.KeyCount++;
				}
				if(i == n.ChildCount)
				{
					/*
					if( i < keyLength)
					{
						Array.Clear(n.Keys, n.ChildCount, keyLength - n.ChildCount);
						n.KeyCount -= (keyLength - n.ChildCount);
					}
					*/
					n.MaxValue = n.Children[i - 1].MaxValue;
					break;
				}
			}
			if (n.Parent != null) { FixKeys(n.Parent); }
			else { return; }
		}
	}

}

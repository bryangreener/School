using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace a5
{
	/// <summary>
	/// Generic BTree class that uses Node class to create BTree of
	/// arbitrary degree.
	/// i.e. 2-3 Tree, 2-3-4, etc.
	/// </summary>
	/// <typeparam name="Z">Generic used to handle different data type inputs.</typeparam>
	class BTree<Z> where Z : IComparable<Z>
	{
		private Node<Z> root;
		private int degree, keyLength, childLength, totalHeight;

		/// <summary>
		/// Default constructor. Initializes root, sets degree and key/child lengths.
		/// </summary>
		/// <param name="degree">The degree is the max number of children per node. Passed in by Program class.</param>
		public BTree(int degree)
		{
			root = null;
			this.degree = degree;
			keyLength = degree - 1;
			childLength = degree + 1;
		}

		/// <summary>
		/// Public method that calls a search utility private class by passing in the root node of the btree.
		/// </summary>
		/// <param name="value">Generic value to be searched</param>
		/// <returns>Returns a result node (null if search miss) from search.</returns>
		public Node<Z> Search(Z value)
		{
			return SearchUtil(root, value);
		}

		/// <summary>
		/// Public method that checks for base cases for inserting and handles these bases cases.
		/// Otherwise, it calls a private insert utility method.
		/// </summary>
		/// <param name="value">Generic value to be inserted into a new node.</param>
		public void Insert(Z value)
		{
			Node<Z> n = new Node<Z>(degree, value, null);
			n.MaxValue = value;
			if (root == null)
			{
				root = n;
				totalHeight = 1;
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
				totalHeight++;
			}
			else
			{
				InsertUtil(n, root);
			}
		}

		/// <summary>
		/// Helper method used to send tree height back to Program class.
		/// </summary>
		/// <returns>Integer height value of tree.</returns>
		public int GetHeight() { return totalHeight; }

		/// <summary>
		/// Public method that calls private Traverse utility method by passing in the root.
		/// </summary>
		public void Traverse()
		{
			TraverseUtil(root);
		}

		/// <summary>
		/// Private traverse utility method that uses level-by-level traversal using a queue
		/// to print parent keys, current node keys, and all children keys.
		/// </summary>
		/// <param name="n">Generic node passed in as starting point for traversal. Initially passed in as root.</param>
		private void TraverseUtil(Node<Z> n)
		{
			Queue<Node<Z>> q = new Queue<Node<Z>>();
			q.Enqueue(n);

			Console.WriteLine("LEVEL BY LEVEL TRAVERSAL ");
			if (degree == 3) { Console.Write("OF 2-3 TREE"); }
			if (degree == 4) { Console.Write("OF 2-3-4 TREE"); }

			while(q.Count != 0)
			{
				Node<Z> current = q.Dequeue();
				if (!current.IsLeaf)
				{
					Console.WriteLine();
					Console.Write("INTERNAL: ".PadRight(10, ' '));
					for (int i = 0; i < current.KeyCount; i++)
					{
						Console.Write($"KEY[{i}]: {Regex.Escape(current.Keys[i].ToString())}".PadRight(15, ' '));
					}
					Console.Write("PARENT: ");
					if (current.Parent == null) { Console.Write("NULL".PadRight(15, ' ')); }
					else
					{
						for (int i = 0; i < current.Parent.KeyCount; i++)
						{
							Console.Write($"KEY[{i}]: {Regex.Escape(current.Parent.Keys[i].ToString())}".PadRight(15, ' '));
						}
					}

					for (int i = 0; i < current.ChildCount; i++)
					{
						Console.WriteLine();

						if (current.Children[i].IsLeaf)
						{
							Console.Write("".PadLeft(13, ' ') + $"LEAF[{i}]:".PadRight(10, ' ') + $"VALUE: {Regex.Escape(current.Children[i].Value.ToString())}");
						}
						else
						{
							Console.Write("".PadRight(13, ' ') + $"CHILD[{i}]: ");
							for (int j = 0; j < current.Children[i].KeyCount; j++)
							{
								Console.Write($"KEY[{j}]: {Regex.Escape(current.Children[i].Keys[j].ToString())}".PadRight(15, ' '));
							}
						}
						q.Enqueue(current.Children[i]);
					}
				}
			}
			
		}

		/// <summary>
		/// Private search utility method.
		/// Used only when searching for a value, not used during the insert process.
		/// Uses standard binary tree style search based on key values of current node.
		/// </summary>
		/// <param name="n">Current generic node. Initially passed in as root.</param>
		/// <param name="value">Generic value being searched.</param>
		/// <returns>Generic node returned to be handled by Program class.</returns>
		private Node<Z> SearchUtil(Node<Z> n, Z value)
		{
			// if rightmost child exists AND value > all keys
			if(n.ChildCount > n.KeyCount && value.CompareTo(n.Children[n.KeyCount - 1].MaxValue) > 0 )
			{
				if(n.Children[n.KeyCount - 1].IsLeaf)
				{
					if(value.CompareTo(n.Children[n.KeyCount].Value) == 0) { return n.Children[n.KeyCount]; } // search hit // CHNAGES TO KEYCOUNT NOT KEYCOUNT -1
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

		/// <summary>
		/// Private insert utility method.
		/// Used to handle inserts beyond base cases.
		/// Searches for new node position and adds height to tree if
		/// needed. Recursively adds and splits nodes up tree.
		/// </summary>
		/// <param name="n">New generic node to be inserted.</param>
		/// <param name="m">Current generic node that is being inserted into.</param>
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

					totalHeight++;
				}
				else
				{
					InsertUtil(newInternal, m.Parent);
				}
			}
		}

		/// <summary>
		/// Private insert helper method.
		/// Recursively searches for position to insert a new node, inserts, then handles key updates as needed.
		/// </summary>
		/// <param name="n">Generic node being inserted.</param>
		/// <param name="m">Current generic node in recursive call.</param>
		/// <returns>Generic node returned to InsertUtil method for further operation.</returns>
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

		/// <summary>
		/// Private insert helper method called by InsertUtil method.
		/// Used to split a full node into two half nodes based on degree of tree.
		/// </summary>
		/// <param name="m">Generic node to be split. Remains partially in tact including parent pointer.</param>
		/// <returns>Returns a free floating node wih half of m's old children. Will be inserted back into tree.</returns>
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

		/// <summary>
		/// Private insert helper method called by multiple other methods.
		/// Recursively traverses up tree to root, fixing keys/maxValue on the way up.
		/// </summary>
		/// <param name="n">Generic node to start travelling up from.</param>
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
					n.MaxValue = n.Children[i - 1].MaxValue;
					break;
				}
			}
			if (n.Parent != null) { FixKeys(n.Parent); }
			else { return; }
		}
	}

}

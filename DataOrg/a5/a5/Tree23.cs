using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	public class Tree23<Z> where Z : IComparable<Z>
	{
		private Node<Z> root;

		public void Insert(Z value)
		{
			if(root == null)
			{
				root = new Node<Z>(value);
			}
			else if(ChildCount(root) == 0)
			{
				Node<Z> newInternal = new Node<Z>();
				if (value.CompareTo(root.Value) <= 0)
				{
					newInternal.KeyA = value;
					newInternal.A = new Node<Z>(value);
					newInternal.KeyB = root.Value;
					newInternal.B = root;
					root = newInternal;
				}
				else
				{
					newInternal.KeyA = root.Value;
					newInternal.A = root;
					newInternal.KeyB = value;
					newInternal.B = new Node<Z>(value);
					root = newInternal;

				}
				root.A.Parent = root;
				root.B.Parent = root;
			}
			else
			{
				root = InsertUtil(root, value);
			}
			
		}

		private Node<Z> InsertUtil(Node<Z> n, Z value)
		{
			if(n.A == null) { return n; }
			if(IsLeaf(n.A))
			{
				if(ChildCount(n) == 2)
				{
					if (value.CompareTo(n.A.Value) <= 0)
					{
						n.C = n.B;
						n.B = n.A;
						n.A = new Node<Z>(value);
						n.A.Parent = n.B.Parent = n.C.Parent = n;
						n.KeyA = n.A.Value;	// Left Max
						n.KeyB = n.B.Value;	// Mid Max
					}
					else if (value.CompareTo(n.B.Value) <= 0)
					{
						n.C = n.B;
						n.B = new Node<Z>(value);
						n.B.Parent = n.C.Parent = n;
						n.KeyB = n.B.Value;
					}
					else if(value.CompareTo(n.B.Value) > 0)
					{
						n.C = new Node<Z>(value);
						n.C.Parent = n;
					}
					else
					{
						Console.WriteLine("ERROR INSERTING IN 2NODE");
					}
				}
				else if(ChildCount(n) == 3)
				{
					if (value.CompareTo(n.A.Value) <= 0)
					{
						n.D = n.C;
						n.C = n.B;
						n.B = n.A;
						n.A = new Node<Z>(value);
						n.A.Parent = n.B.Parent = n.C.Parent = n.D.Parent = n;
						n.KeyA = n.A.Value; // Left Max
						n.KeyB = n.B.Value; // Mid Max
					}
					else if (value.CompareTo(n.B.Value) <= 0)
					{
						n.D = n.C;
						n.C = n.B;
						n.B = new Node<Z>(value);
						n.B.Parent = n.C.Parent = n.D.Parent = n;
						n.KeyB = n.B.Value;
					}
					else if (value.CompareTo(n.C.Value) <= 0)
					{
						n.D = n.C;
						n.C = new Node<Z>(value);
						n.C.Parent = n.D.Parent = n;
					}
					else if(value.CompareTo(n.C.Value) > 0)
					{
						n.D = new Node<Z>(value);
						n.D.Parent = n;
					}
					else
					{
						Console.WriteLine("ERROR INSERTING IN 3NODE");
					}

					Node<Z> m = new Node<Z>();
					m.A = n.C;
					m.B = n.D;
					n.C = null;
					n.D = null;
					m.A.Parent = m.B.Parent = m;
					m.KeyA = m.A.Value;
					m.KeyB = m.B.Value;
					FixParent(n, m);
				}
				else
				{
					Console.WriteLine("ERROR AT LEAF");
				}
			}

			if (value.CompareTo(n.KeyA) <= 0)
			{
				n.A = InsertUtil(n.A, value);

			}
			else if (value.CompareTo(n.KeyB) <=0 || ChildCount(n) == 2)
			{
				n.B = InsertUtil(n.B, value);
			}
			else if(value.CompareTo(n.KeyB) > 0 && ChildCount(n) == 3)
			{
				n.C = InsertUtil(n.C, value);
			}
			else
			{
				Console.WriteLine("ERROR TRAVERSING");
			}

			return n;
		}
		private Node<Z> FixParent(Node<Z> n, Node<Z> m)
		{
			if(n.Parent == null) { return n; }
			if(ChildCount(n.Parent) == 2)
			{
				if (n == n.Parent.A)
				{
					n.Parent.C = n.Parent.B;
					n.Parent.B = m;
					n.Parent.B.Parent = n.Parent.C.Parent = n.Parent;
					n.Parent.KeyB = n.Parent.B.Value; // Mid Max
				}
				else
				{
					n.Parent.C = m;
					n.Parent.C.Parent = n.Parent;
				}
			}
			else if (ChildCount(n.Parent) == 3)
			{
				if (n == n.Parent.A)
				{
					n.Parent.D = n.Parent.C;
					n.Parent.C = n.Parent.B;
					n.Parent.B = m;
					n.Parent.B.Parent = n.Parent.C.Parent = n.Parent.D.Parent = n.Parent;
					n.Parent.KeyB = n.Parent.B.Value; // mid max
				}
				else if (n == n.Parent.B)
				{
					n.Parent.D = n.Parent.C;
					n.Parent.C = m;
					n.Parent.C.Parent = n.Parent.D.Parent = n.Parent;
				}
				else
				{
					n.Parent.D = m;
					n.Parent.D.Parent = n.Parent;
				}
			}
			m = new Node<Z>();
			m.A = n.Parent.C;
			m.B = n.Parent.D;
			m.A.Parent = m.B.Parent = m;
			m.KeyA = m.A.Value;
			m.KeyB = m.B.Value;
			return FixParent(n.Parent, m);
		}

		#region Helper Methods
		private int ChildCount(Node<Z> n)
		{
			int count = 0;
			if(n.A != null) { count++; }
			if(n.B != null) { count++; }
			if(n.C != null) { count++; }
			return count;
		}
		private bool IsLeaf(Node<Z> n)
		{
			return (n.A == null && n.B == null && n.C == null);
		}
		private int Size()
		{
			return Size(root);
		}
		private int Size(Node<Z> n)
		{
			if (n == null) { return 0; }
			else { return n.N; }
		}
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace a5
{
	public class Tree23<Z> where Z : IComparable<Z>
	{
		private Node<Z> root;

		public void Insert(Z value)
		{
			if (root == null) { root = new Node<Z>(value); }
			else if (IsLeaf(root))
			{
				Node<Z> newInternal = new Node<Z>();
				if (value.CompareTo(root.Value) <= 0)
				{
					newInternal.KeyA = value;
					newInternal.A = new Node<Z>(value);
					newInternal.KeyB = root.Value;
					newInternal.B = new Node<Z>(root.Value);
					CopyNode(newInternal, root);
				}
				else
				{
					newInternal.KeyA = root.Value;
					newInternal.A = new Node<Z>(root.Value);
					newInternal.KeyB = value;
					newInternal.B = new Node<Z>(value);
					CopyNode(newInternal, root);
				}
				root.A.Parent = root;
				root.B.Parent = root;
			}
			else
			{
				Node<Z> n = new Node<Z>();
				n = FindUtil(root, value);
				if (n.C == null)
				{
					if(value.CompareTo(n.A.Value) < 0)
					{
						CopyNode(n.B, n.C);
						CopyNode(n.A, n.B);
						n.A = new Node<Z>(value, n);
						n.KeyB = n.KeyA;
						n.KeyA = value;
					}
					else if(value.CompareTo(n.B.Value) < 0)
					{
						CopyNode(n.B, n.C);
						n.B = new Node<Z>(value, n);
						n.KeyB = value;
					}
					else
					{
						n.C = new Node<Z>(value, n);
					}
				}
				else if (n.D == null)
				{
					if (value.CompareTo(n.A.Value) < 0)
					{
						CopyNode(n.C, n.D);
						CopyNode(n.B, n.C);
						CopyNode(n.A, n.B);
						n.A = new Node<Z>(value, n);
						n.KeyB = n.KeyA;
						n.KeyA = value;
					}
					else if (value.CompareTo(n.B.Value) < 0)
					{
						CopyNode(n.C, n.D);
						CopyNode(n.B, n.C);
						n.B = new Node<Z>(value, n);
						n.KeyB = value;
					}
					else if (value.CompareTo(n.C.Value) < 0)
					{
						CopyNode(n.C, n.D);
						n.C = new Node<Z>(value, n);
					}
					else
					{
						n.D = new Node<Z>(value, n);
					}
					Node<Z> m = new Node<Z>();
					m.A = new Node<Z>();
					m.B = new Node<Z>();
					CopyNode(n.C, m.A);
					CopyNode(n.D, m.B);
					n.C = null;
					n.D = null;
					m.KeyA = m.A.Value;
					m.KeyB = m.B.Value;
					m.A.Parent = m;
					m.B.Parent = m;
					m.Parent = n;
					FixUp(n, m);
				}
			}
		}
		private Node<Z> FindUtil(Node<Z> n, Z value)
		{
			if (n.A == null) { return n.Parent; } // At leaf
			else if(value.CompareTo(n.KeyA) < 0) { return FindUtil(n.A, value); }
			else if(value.CompareTo(n.KeyB) < 0) { return FindUtil(n.B, value); }
			else
			{
				if (n.C == null) { return FindUtil(n.B, value); }
				else { return FindUtil(n.C, value); }
			}

		}
		private void FixUp(Node<Z> n, Node<Z> m)
		{
			if (n.Parent == null)
			{
				root = new Node<Z>();
				root.A = new Node<Z>();
				root.B = new Node<Z>();
				CopyNode(n, root.A);
				CopyNode(m, root.B);
				n.Parent = root;
				m.Parent = root;
				root.KeyA = n.KeyB;
				root.KeyB = m.KeyB;
			}
			else if (n.Parent.C == null)
			{
				n = n.Parent;
				if (m.KeyB.CompareTo(n.B.Value) < 0)
				{
					CopyNode(n.B, n.C);
					CopyNode(m, n.B);
					n.B.Parent = n;
					n.KeyB = n.B.KeyB;
				}
				else
				{
					n.C = new Node<Z>();
					CopyNode(m, n.C);
					n.C.Parent = n;
				}
			}
			else if(n.Parent.D == null)
			{
				n = n.Parent;
				if (m.KeyB.CompareTo(n.B.Value) < 0)
				{
					CopyNode(n.C, n.D);
					CopyNode(n.B, n.C);
					n.B = new Node<Z>();
					CopyNode(m, n.B);
					n.B.Parent = n;
					n.KeyB = n.B.KeyB;
				}
				else if(m.KeyB.CompareTo(n.C.Value) < 0)
				{
					CopyNode(n.C, n.D);
					n.C = new Node<Z>();
					CopyNode(n.C, m);
					n.C.Parent = n;
				}
				else
				{
					n.D = new Node<Z>();
					CopyNode(m, n.D);
					n.D.Parent = n;
				}

				CopyNode(n.C, m.A);
				CopyNode(n.D, m.B);
				m.A.Parent = m.B.Parent = m;
				m.Parent = n;
				m.KeyA = m.A.KeyB;
				m.KeyB = m.B.KeyB;
				n.C = null;
				n.D = null;
				FixUp(n, m);
			}
		}

		#region Helper Methods
		private Node<Z> CopyNode(Node<Z> n, Node<Z> m)
		{
			if (n.Parent != null) { m.Parent = n.Parent; }
			if (n.A != null) { m.A = n.A; }
			if (n.B != null) { m.B = n.B; }
			if (n.C != null) { m.C = n.C; }
			if (n.D != null) { m.D = n.D; }
			m.Value = n.Value;
			m.KeyA = n.KeyA;
			m.KeyB = n.KeyB;
			return m;
		}

		private int ChildCount(Node<Z> n)
		{
			int count = 0;
			if(n.A != null) { count++; }
			if(n.B != null) { count++; }
			if(n.C != null) { count++; }
			if(n.D != null) { count++; }
			return count;
		}
		private bool IsLeaf(Node<Z> n)
		{
			return (ChildCount(n) == 0);
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

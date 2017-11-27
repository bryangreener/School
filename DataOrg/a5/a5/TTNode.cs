using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	class TTNode<Z> where Z : IComparable<Z>
	{
		private Z lval;
		private Z lkey;
		private Z rval;
		private Z rkey;
		private TTNode<Z> left;
		private TTNode<Z> mid;
		private TTNode<Z> right;

		public TTNode()
		{
			mid = left = right = null;
		}
		public TTNode(Z lk, Z lv, TTNode<Z> p1, TTNode<Z> p2, TTNode<Z> p3)
		{
			lkey = lk;
			lval = lv;
			left = p1;
			mid = p2;
			right = p3;
		}
		public TTNode(Z lk, Z lv, Z rk, Z rv, TTNode<Z> p1, TTNode<Z> p2, TTNode<Z> p3)
		{
			lkey = lk;
			rkey = rk;
			lval = lv;
			rval = rv;
			left = p1;
			mid = p2;
			right = p3;
		}

		public bool IsLeaf { get { return left == null; } }
		public TTNode<Z> lchild { get { return left; } }
		public TTNode<Z> mchild { get { return mid; } }
		public TTNode<Z> rchild { get { return right; } }
		public Z LKey { get { return lkey; } }
		public Z RKey { get { return rkey; } }
		public Z LVal { get { return lval; } }
		public Z RVal { get { return rval; } }
		public void SetLeft(Z k, Z v) { lkey = k; lval = v; }
		public void SetRight(Z k, Z v) { rkey = k; lval = v; }
		public void SetLeftChild(TTNode<Z> n) { left = n; }
		public void SetMidChild(TTNode<Z> n) { mid = n; }
		public void SetRightChild(TTNode<Z> n) { right = n; }

	}
}

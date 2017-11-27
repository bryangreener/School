using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	class TTTree<Z> where Z : IComparable<Z>
	{
		private TTNode<Z> FindUtil(TTNode<Z> root, Z k)
		{
			if (root == null) { return null; }
			if(k.CompareTo(root.LKey) == 0) { return root; }
			if(root.RKey != null && k.CompareTo(root.RKey) == 0) { return root; }
			if(k.CompareTo(root.LKey) < 0) { return FindUtil(root.lchild, k); }
			else if(root.RKey == null) { return FindUtil(root.mchild, k); }
			else if(k.CompareTo(root.RKey) < 0) { return FindUtil(root.mchild, k); }
			else { return FindUtil(root.rchild, k); }
		}

		private TTNode<Z> InsertUtil(TTNode<Z> rt, Z k, Z v)
		{
			TTNode<Z> retval;
			if(rt == null) { return new TTNode<Z>(k, v, null, null, null); }
			if (rt.IsLeaf) { return rt.}
		}

		public TTNode<Z> Add(TTNode<Z> n)
		{
			TTNode<Z> m = new TTNode<Z>();
			if()
		}
	}
}

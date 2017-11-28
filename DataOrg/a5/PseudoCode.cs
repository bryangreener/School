void Insert(value)
{
	Node n = new Node(value);
}

void InsertUtil(n, m) // insert new node n as child of m
{
	int pos = InsertPos(n, m);
	if(m.ChildCount > degree)
	{
		newInternal = Split(m);
		if(m.Parent = null)
		{
			Node newRoot = new Node();
			newRoot.Children[0] = m;
			newRoot.Children[1] = newInternal;
			root = newRoot;
			root.Keys[0] = root.Children[0].MaxValue;
			root.Keys[1] = root.Children[1].MaxValue;
			root.Children[0].Parent = root;
			root.Children[1].Parent = root;
		}
		else
		{
			InsertUtil(newInternal, m.Parent)
		}
	}
}
int InsertPos(n, m)
{
	private int retVal = -1;
	for(int i = 0; i < m.KeyCount; i++)	// For each key in m
	{
		if(n.MaxValue < m.Keys[i])	// If at insert pos
		{
			for(int j = m.ChildCount - 1; j > i; j--)	// From right to left child in m up to i
			{
				m.Children[j] = m.Children[j-1];	// shift child right to make space for new node
			}
			m.Children[i] = n;	// insert new node
			n.Parent = m;
			for(int j = 0; j < m.ChildCount - 1; j++) // for each child in m
			{	//update keys
				if(m.Children[j].IsLeaf)
				{
					m.Keys[j] = m.Children[j].Value;
				}
				else
				{
					m.Keys[j] = m.Children[j].MaxValue;
				}
			}
			retVal = i; // can be used. not used at the moment. gives insert index
			break;
		}
	}
	return retVal;
}

Node Split(m) // returns a new internal node and strips old node of half its children and leaves its parent in tact
{
	int splitIndex = Floor(m.ChildCount / 2); // this is start index for new node
	Node newNode = new Node();
	int oldCount = m.ChildCount;
	for(int i = splitIndex, j=0; i < oldCount; i++, j++) // assign items right of split point to new node children
	{
		newNode.Children[j] = m.Children[i];
		newNode.Children[j].Parent = newNode;
		if(newNode.Children[j].IsLeaf)
		{
			newNode.Keys[j] = newNode.Children[j].Value;
		}
		else
		{
			newNode.Keys[j] = newNode.Children[j].MaxValue;
		}
		m.Children[i] = null;
	}
	return newNode;
}
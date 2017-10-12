using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace a2
{
	class Program
	{
		static void Main(string[] args)
		{
			string text = File.ReadAllText("balancedParenCheckInputs.txt");
			var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			List<string> input = new List<string>();
			foreach (string line in lines)
			{
				input.Add(Regex.Unescape(line));
			}


			foreach (string s in input)
			{
				StackCheckBalancedParentheses stackCheck = new StackCheckBalancedParentheses();
				Console.WriteLine($"STACK: {s} --- Degree: {stackCheck.CheckBalancedParentheses(s)}");
			}

			foreach (string s in input)
			{
				QueueCheckBalancedParentheses queueCheck = new QueueCheckBalancedParentheses();
				Console.WriteLine($"QUEUE: {s} --- Degree: {queueCheck.CheckBalancedParentheses(s)}");
			}
			Console.ReadLine();
		}
	}

	class StackCheckBalancedParentheses
	{
		CharStack stack = new CharStack();

		public int CheckBalancedParentheses(string input)
		{
			int degree = 0;
			foreach (char c in input)
			{
				if (c == '(')
				{
					stack.Push(c);
				}
				if (c == ')')
				{
					if (stack.Pop() != '(')
					{
						degree++;
					}
				}
			}
			return degree += stack.Count();
		}
	}
	class CharStack
	{
		CharList list = new CharList();

		public void Push(char data)
		{
			list.AddHead(data);
		}
		public char Pop()
		{
			return list.DeleteHead();
		}
		public int Count()
		{
			return list.Count();
		}
	}

	class QueueCheckBalancedParentheses
	{
		CharQueue queue = new CharQueue();

		public int CheckBalancedParentheses(string input)
		{
			int degree = 0;
			foreach (char c in input)
			{
				if (c == '(')
				{
					Push(c); // this calls the Push method in this class, not in the Stack class
				}
				if (c == ')')
				{
					if (Pop() != '(') // this calls the Pop method in this class, not in the Stack class
					{
						degree++;
					}
				}
			}
			return degree += queue.Count();
		}
		private void Push(char c)
		{
			CharQueue queue2 = new CharQueue();
			queue.Enqueue(c);
			for (int i = 0; i < queue.Count() - 1; i++)
			{
				for (int j = 0; j < queue.Count() - 2; j++)
				{
					queue.Enqueue(queue.Dequeue());
				}
				queue2.Enqueue(queue.Dequeue());
			}
		}
		private char Pop()
		{
			return queue.Dequeue();
		}
	}
	class CharQueue
	{
		CharList list = new CharList();

		public void Enqueue(char data)
		{
			list.AddTail(data);
		}
		public char Dequeue()
		{
			return list.DeleteHead();
		}
		public int Count()
		{
			return list.Count();
		}
	}

	class StackQueueDemo
	{

	}

	public class CharNode
	{
		private CharNode next;
		private char myData;

		public CharNode(char c, CharNode n)
		{
			myData = c;
			next = n;
		}
		// accessors
		public char GetData()
		{
			return myData;
		}
		public CharNode GetNext()
		{
			return next;
		}
		// mutators
		public void SetData(char newData)
		{
			myData = newData;
		}
		public void SetNext(CharNode newNext)
		{
			next = newNext;
		}
	}

	public class CharList
	{
		protected CharNode head;
		protected int count;
		
		public CharList()
		{
<<<<<<< HEAD
			head = current = tail = new CharNode();
			head.Next = null;
			count = 0;
		}
		public void Insert(int index, char data)
=======
			head = new a2.CharNode('\0', null);
			count = 0;	
		}

		public void AddHead(char c)
		{
			head = new CharNode(c, head);
			count++;
		}
		public void AddTail(char c)
>>>>>>> 89ed3128a0153053d84906718ba0dd57fb8ba5c7
		{
			CharNode temp = new CharNode(c, null);
			CharNode tail = new CharNode('\0', null);
			temp.SetNext(null);
			if(head == null)
			{
<<<<<<< HEAD
				if(count == 0)
				{
					head = tail = newNode;
					count++;
					return;
				}
				head = newNode;
				newNode.Next = oldHead;
				count++;
				return;
			}
			if(index >= count)
=======
				head = temp;
			}
			else
>>>>>>> 89ed3128a0153053d84906718ba0dd57fb8ba5c7
			{
				tail.SetNext(temp);
			}
			tail = temp;
			count++;
		}

		public char DeleteHead()
		{
			CharNode temp = new CharNode('\0', null);
			if(head == null)
			{
				return '0';
			}
			char c = head.GetData();
			temp = head;
			head = head.GetNext();
			temp.SetNext(null);
			count--;
			return c;
		}
<<<<<<< HEAD
		private CharNode Find(int index)
		{
			current = head;
			for(int i = 1; i < index; i++)
			{
				current = current.Next;
			}
			return current;
		}
=======
>>>>>>> 89ed3128a0153053d84906718ba0dd57fb8ba5c7
		public int Count()
		{
			return count;
		}
	}
}

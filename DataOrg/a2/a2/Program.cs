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
			list.Insert(0, data);
		}
		public char Pop()
		{
			return list.Delete(0);
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
			list.Insert(list.Count(), data);
		}
		public char Dequeue()
		{
			return list.Delete(0);
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

		public char MyData { get; set; }
		public CharNode Next { get; set; }
	}

	public class CharList
	{
		private CharNode head;
		private CharNode tail;
		private CharNode current;
		private int count;
		
		public CharList()
		{
			head = new CharNode();
			current = tail = new CharNode();
			head.Next = tail;
			count = 0;
		}
		/*
		public void Insert(int index, char data)
		{
			CharNode newNode = new a2.CharNode();
			newNode.MyData = data;
			if (index == 1)
			{
				newNode.Next = head;
				head = newNode;
				count++;
				return;
			}
			current = head;
			for(int i = 1; i < index - 1; i++)
			{
				current = current.Next;
			}
			newNode.Next = current.Next;
			current.Next = newNode;
			count++;
		}
		*/
		public void Insert(int index, char data)
		{
			CharNode newNode = new a2.CharNode();
			newNode.MyData = data;
			CharNode oldHead = head;
			if(index == 0)
			{
				if(count == 0)
				{
					head = newNode;
					count++;
					return;
				}
				head = newNode;
				newNode.Next = oldHead;
				count++;
				return;
			}
			if(index >= count -1)
			{
				tail.Next = newNode;
				tail = newNode;
				count++;
				return;
			}
			CharNode previous = head;
			for (int i = 1; i < index; i++)
			{
				previous = previous.Next;
			}
			newNode.Next = previous.Next;
			previous.Next = newNode;
			count++;
		}
		public char Delete(int index)
		{
			CharNode previous = head;
			char c;
			if(index == 0)
			{
				if(head == null)
				{
					return '\0';
				}
				c = head.MyData;
				head = head.Next;
				count--;
				return c;
			}
			for(int i = 1; i < index; i++)
			{
				previous = previous.Next;
			}
			if(previous.Next == tail)
			{
				c = tail.MyData;
				tail = previous;
				tail.Next = null;
				count--;
				return c;
			}
			c = previous.Next.MyData;
			previous.Next = previous.Next.Next;
			count--;
			return c;
		}
		/*
		public char Delete(int index)
		{
			char result;
			current = head;
			for (int i = 1; i < index - 1; i++)
			{
				current = current.Next;
			}
			result = current.MyData;
			current.Next = current.Next.Next;
			count--;
			return result;
		}
		*/
		private CharNode Find(int index)
		{
			current = head;
			for(int i = 1; i < index; i++)
			{
				current = current.Next;
			}
			return current;
		}
		public int Count()
		{
			return count;
		}
	}
}

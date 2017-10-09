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
			foreach(string line in lines)
			{
				input.Add(Regex.Unescape(line));
			}

			
			foreach (string s in input)
			{
				StackCheckBalancedParentheses stackCheck = new StackCheckBalancedParentheses();
				Console.WriteLine($"STACK: {s} --- Degree: {stackCheck.CheckBalancedParentheses(s)}");
			}
			
			foreach(string s in input)
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
			return degree += stack.GetLength();
		}
	}
	class CharStack
	{
		CharList list = new CharList();
		public CharStack()
		{
			list.Clear();
		}
		public void Push(char data)
		{
			list.Insert(data);
			list.MoveToHead();
		}
		public char Pop()
		{
			while (list.CurrentPos() != 0)
			{
				list.MoveLeft();
			}
			return list.Delete();
		}
		public int GetLength()
		{
			return list.GetLength();
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
			return degree += queue.GetLength();
		}
		private void Push(char c)
		{
			CharQueue queue2 = new CharQueue();
			queue.Enqueue(c);
			for(int i = 0; i < queue.GetLength() - 1; i++)
			{
				queue.Enqueue(queue.Dequeue());
			}
			queue2.Enqueue(queue.Dequeue());
			for(int i = 0; i < queue2.GetLength(); i++)
			{
				queue.Enqueue(queue2.Dequeue());
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
		public CharQueue()
		{
			list.Clear();
		}
		public void Enqueue(char data)
		{
			list.Append(data);
		}
		public char Dequeue()
		{
			list.MoveToHead();
			return list.Delete();
		}
		public int GetLength()
		{
			return list.GetLength();
		}
		public void Clear()
		{
			list.Clear();
		}
	}

	class StackQueueDemo
	{

	}

	public class CharNode
	{
		public CharNode Next;
		public char myData;
	}

	public class CharList
	{
		private CharNode head;
		private CharNode tail;
		private CharNode current;
		public int Count;

		public CharList()
		{
			Clear();
		}

		public void Clear()
		{
			current = tail = new CharNode();
			head = new CharNode();
			head.Next = tail;
			Count = 0;
		}

		public void Insert(char data) // ins current
		{
			CharNode newNode = new CharNode();
			newNode.myData = current.myData;
			newNode.Next = current.Next;
			current.Next = newNode;
			current.myData = data;
			if(tail == current)
			{
				tail = current.Next;
			}
			Count++;
		}

		public char Delete() // del current
		{
			if (current == tail)
			{
				return current.myData;
			}
			char value = current.myData;
			current.myData = current.Next.myData;
			if (current.Next == tail)
			{
				tail = current;
			}
			current.Next = current.Next.Next;
			Count--;
			return value;
		}

		public int CurrentPos()
		{
			CharNode temp = head.Next;
			int i;
			for(i = 0; current != temp; i++)
			{
				temp = temp.Next;
			}
			return i;
		}

		public void Append(char data)
		{
			CharNode tempNode = new CharNode();
			tempNode.myData = tail.myData;
			tail.myData = data;
			tail.Next = tempNode;
			tempNode.Next = null;
			Count++;
		}

		public void MoveToHead()
		{
			current = head;
		}

		public void MoveToTail()
		{
			current = tail;
		}

		public void MoveLeft()
		{
			if(head.Next == current)
			{
				return;
			}
			CharNode temp = head;
			while(temp.Next != current)
			{
				temp = temp.Next;
			}
			current = temp;
		}

		public void MoveRight()
		{
			if(current != tail)
			{
				current = current.Next;
			}
		}

		public int GetLength()
		{
			return Count;
		}

		public char GetValue()
		{
			return current.myData;
		}

		public void PrintNodes()
		{
			Console.Write("HEAD");
			CharNode current = head;
			while (current.Next != null)
			{
				Console.Write($" -> {current.myData}");
				current = current.Next;
			}
			Console.Write(" -> NULL");
		}
	}
}

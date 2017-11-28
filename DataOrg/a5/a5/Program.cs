using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace a5
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = File.ReadAllLines("ASCII.txt").Select(x => x.Split('\t').ToList()).ToList();
			for (int i = 0; i < input.Count(); i++)
			{
				input[i].RemoveRange(5, 3);
			}
			// input[i] format = DEC,OCT,HEX,BIN,SYMBOL
			int degree = 3;
			BTree<string> dec = new BTree<string>(degree);
			BTree<int> oct = new BTree<int>(degree);
			BTree<string> hex = new BTree<string>(degree);
			BTree<int> bin = new BTree<int>(degree);
			BTree<string> sym = new BTree<string>(degree);

			for (int i = 0; i < input.Count(); i++)
			{
				dec.Insert(input[i][0]);
			}
			Console.ReadLine();
			/*
			for (int i = 0; i < input.Count(); i++)
			{
				oct.Insert((Z)(object)input[i][1]);
			}
			for (int i = 0; i < input.Count(); i++)
			{
				hex.Insert((Z)(object)input[i][2]);
			}
			for (int i = 0; i < input.Count(); i++)
			{
				bin.Insert((Z)(object)input[i][3]);
			}
			for (int i = 0; i < input.Count(); i++)
			{
				sym.Insert((Z)(object)input[i][4]);
			}*/
			Console.ReadLine();
		}
	}
}

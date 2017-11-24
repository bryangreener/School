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

			Tree23<string> dec = new Tree23<string>();
			Tree23<int> oct = new Tree23<int>();
			Tree23<string> hex = new Tree23<string>();
			Tree23<int> bin = new Tree23<int>();
			Tree23<string> sym = new Tree23<string>();

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

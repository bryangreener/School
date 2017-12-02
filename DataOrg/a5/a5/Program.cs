using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace a5
{
	class Program
	{
		static void Main(string[] args)
		{
			UI ui = new UI();
			Controller controller = new Controller();
			string searchTerm = ui.StartMenu();

			Stopwatch sw = new Stopwatch();

			var input = File.ReadAllLines("ASCII.txt").Select(x => x.Split('\t').ToList()).ToList();
			for (int i = 0; i < input.Count(); i++)
			{
				input[i].RemoveRange(5, 3);
			}

			// input[i] format = DEC,OCT,HEX,BIN,SYMBOL

			for (int degree = 4; degree < 6; degree++)
			{
				BTree<int> dec = new BTree<int>(degree);
				BTree<int> oct = new BTree<int>(degree);
				BTree<string> hex = new BTree<string>(degree);
				BTree<int> bin = new BTree<int>(degree);
				BTree<string> sym = new BTree<string>(degree);

				for (int i = 0; i < input.Count(); i++)
				{
					dec.Insert(Convert.ToInt32(input[i][0]));
				}
				for (int i = 0; i < input.Count(); i++)
				{
					oct.Insert(Convert.ToInt32(input[i][1]));
				}
				for (int i = 0; i < input.Count(); i++)
				{
					hex.Insert(input[i][2]);
				}
				for (int i = 0; i < input.Count(); i++)
				{
					bin.Insert(Convert.ToInt32(input[i][3]));
				}
				for (int i = 0; i < input.Count(); i++)
				{
					sym.Insert(input[i][4]);
				}
				Node<int> decResult = new Node<int>();
				Node<int> octResult = new Node<int>();
				Node<int> binResult = new Node<int>();
				Node<string> hexResult = new Node<string>();
				Node<string> symResult = new Node<string>();

				double dt = 0, ot = 0, bt = 0, ht = 0, st = 0;

				bool result = int.TryParse(searchTerm, out int search);
				if (result)
				{
					sw.Start();
					decResult = dec.Search(search);
					sw.Stop();
					dt += sw.Elapsed.TotalMilliseconds;
					sw.Reset();

					sw.Start();
					octResult = oct.Search(search);
					sw.Stop();
					ot += sw.Elapsed.TotalMilliseconds;
					sw.Reset();

					sw.Start();
					binResult = bin.Search(search);
					sw.Stop();
					bt += sw.Elapsed.TotalMilliseconds;
					sw.Reset();
				}
				else
				{
					sw.Start();
					hexResult = hex.Search(searchTerm.PadLeft(2, '0'));
					sw.Stop();
					ht += sw.Elapsed.TotalMilliseconds;
					sw.Reset();

					sw.Start();
					symResult = sym.Search(searchTerm);
					sw.Stop();
					st += sw.Elapsed.TotalMilliseconds;
					sw.Reset();
				}

				if(degree == 4) { ui.TTHeader(); }
				else { ui.TTFHeader(); }

				if (decResult.Children != null && decResult.Parent != null) { ui.Output(controller.ConvertFromDec(decResult.Value), dt); }
				if (octResult.Children != null && octResult.Parent != null) { ui.Output(controller.ConvertFromOct(octResult.Value), ot); }
				if (binResult.Children != null && binResult.Parent != null) { ui.Output(controller.ConvertFromBin(binResult.Value), bt); }
				if (hexResult != null && hexResult.Children != null && hexResult.Parent != null) { ui.Output(controller.ConvertFromHex(hexResult.Value), ht); }
				if (symResult != null && symResult.Children != null && symResult.Parent != null) { ui.Output(controller.ConvertFromSym(symResult.Value), st); }
			}
			Console.ReadLine();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace a5
{
	/// <summary>
	/// Main class that instantiates BTree, UI, and Controller classes.
	/// Also handles txt file input and sanitization as well as inserting into trees.
	/// </summary>
	class Program
	{
		/// <summary>
		/// This class handles communication between the BTree, the input dataset, UI, and Controller classes 
		/// </summary>
		/// <param name="args">default params</param>
		static void Main(string[] args)
		{
			UI ui = new UI();
			Controller controller = new Controller();
			// user search string
			string searchTerm = ui.StartMenu();

			Stopwatch sw = new Stopwatch();

			// Read ascii txt file and split into separate columns then remove unnecessary columns.
			var input = File.ReadAllLines("ASCII.txt").Select(x => x.Split('\t').ToList()).ToList();
			for (int i = 0; i < input.Count(); i++)
			{
				input[i].RemoveRange(5, 3);
			}

			// input[i] format = DEC,OCT,HEX,BIN,SYMBOL
			for (int degree = 3; degree < 5; degree++)
			{
				BTree<int> dec = new BTree<int>(degree);
				BTree<int> oct = new BTree<int>(degree);
				BTree<string> hex = new BTree<string>(degree);
				BTree<int> bin = new BTree<int>(degree);
				BTree<string> sym = new BTree<string>(degree);

				// Build each type of tree
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
					sym.Insert((input[i][4]));
				}

				Node<int> decResult = new Node<int>();
				Node<int> octResult = new Node<int>();
				Node<int> binResult = new Node<int>();
				Node<string> hexResult = new Node<string>();
				Node<string> symResult = new Node<string>();

				// Doubles used to keep track of search times.
				double dt = 0, ot = 0, bt = 0, ht = 0, st = 0;

				// Using timer to keep track of search times,
				// call the appropriate tree searches.
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
					if(search >= 0 && search <= 10)
					{
						sw.Start();
						hexResult = hex.Search(search.ToString().PadLeft(2, '0'));
						sw.Stop();
						ht += sw.Elapsed.TotalMilliseconds;
						sw.Reset();

						sw.Start();
						symResult = sym.Search(search.ToString());
						sw.Stop();
						st += sw.Elapsed.TotalMilliseconds;
						sw.Reset();
					}
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

				// Set output to txt file
				FileStream ostrm;
				StreamWriter writer;
				TextWriter oldOut = Console.Out;
				try
				{
					if(degree == 3)
					{
						ostrm = new FileStream("./output23.txt", FileMode.Create, FileAccess.Write);
						writer = new StreamWriter(ostrm);
					}
					else
					{
						ostrm = new FileStream("./output234.txt", FileMode.Create, FileAccess.Write);
						writer = new StreamWriter(ostrm);
					}
				}catch(Exception e)
				{
					Console.WriteLine("Cannot open file for editing");
					Console.WriteLine(e.Message);
					return;
				}
				Console.SetOut(writer);

				// Output travsersals to text file
				Console.WriteLine("".PadRight(100, '_'));
				Console.WriteLine("DECIMAL TREE TRAVERSAL");
				Console.WriteLine("".PadRight(100, '_'));
				dec.Traverse();
				Console.WriteLine();
				Console.WriteLine("".PadRight(100, '_'));
				Console.WriteLine("OCTAL TREE TRAVERSAL");
				Console.WriteLine("".PadRight(100, '_'));
				oct.Traverse();
				Console.WriteLine();
				Console.WriteLine("".PadRight(100, '_'));
				Console.WriteLine("HEXIDECIMAL TREE TRAVERSAL");
				Console.WriteLine("".PadRight(100, '_'));
				hex.Traverse();
				Console.WriteLine();
				Console.WriteLine("".PadRight(100, '_'));
				Console.WriteLine("BINARY TREE TRAVERSAL");
				Console.WriteLine("".PadRight(100, '_'));
				bin.Traverse();
				Console.WriteLine();
				Console.WriteLine("".PadRight(100, '_'));
				Console.WriteLine("SYMBOL TREE TRAVERSAL");
				Console.WriteLine("".PadRight(100, '_'));
				sym.Traverse();

				// Return output to console window
				Console.SetOut(oldOut);
				writer.Close();
				ostrm.Close();

				// Choose either 2-3 or 2-3-4 header for output
				if (degree == 3) { ui.TTHeader(dec.GetHeight(), input.Count()); }
				else { ui.TTFHeader(dec.GetHeight(), input.Count()); }

				// Convert search results and send to UI to be displayed.
				if (decResult != null &&decResult.Children != null && decResult.Parent != null) { ui.Output(controller.ConvertFromDec(decResult.Value), dt, "DEC SEARCH"); }
				if (octResult != null && octResult.Children != null && octResult.Parent != null) { ui.Output(controller.ConvertFromOct(octResult.Value), ot, "OCT SEARCH"); }
				if (binResult != null && binResult.Children != null && binResult.Parent != null) { ui.Output(controller.ConvertFromBin(binResult.Value), bt, "BIN SEARCH"); }
				if (hexResult != null && hexResult.Children != null && hexResult.Parent != null) { ui.Output(controller.ConvertFromHex(hexResult.Value), ht, "HEX SEARCH"); }
				if (symResult != null && symResult.Children != null && symResult.Parent != null) { ui.Output(controller.ConvertFromSym(symResult.Value), st, "SYM SEARCH"); }
			}
			Console.ReadLine();
		}
	}
}

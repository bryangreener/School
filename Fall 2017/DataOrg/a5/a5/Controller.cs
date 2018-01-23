using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	/// <summary>
	/// Controller class used to convert between different number bases and associate
	/// search results with items in the ascii table.
	/// </summary>
	class Controller
	{
		/// <summary>
		/// Default constructor to instantiate class.
		/// </summary>
		public Controller() { }

		public string[] ASCII = new string[]
		{
			"NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL", "BS", "HT", "LF",
			"VT", "FF", "CR", "SO", "SI", "DLE", "DC1", "DC2", "DC3", "DC4",
			"NAK", "SYN", "ETB", "CAN", "EM", "SUB", "ESC", "FS", "GS", "RS",
			"US", " ", "!", "\"", "#", "$", "%", "&", "\'", "(",
			")", "*", "+", ",", "-", ".", "/", "0", "1", "2",
			"3", "4", "5", "6", "7", "8", "9", ":", ";", "<",
			"=", ">", "?", "@", "A", "B", "C", "D", "E", "F",
			"G", "H", "I", "J", "K", "L", "M", "N", "O", "P",
			"Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
			"[", "\\", "]", "^", "_", "`", "a", "b", "c", "d",
			"e", "f", "g", "h", "i", "j", "k", "l", "m", "n",
			"o", "p", "q", "r", "s", "t", "u", "v", "w", "x",
			"y", "z", "{", "|", "}", "~", "DEL"
		};

		/// <summary>
		/// Helper method that converts from decimal to octal, hex, binary, and symbol in ascii table.
		/// </summary>
		/// <param name="n">decimal search result</param>
		/// <returns>returns search result string containing converted values</returns>
		public string[] ConvertFromDec(int n)
		{
			string oct, hex, bin, sym;
			oct = Convert.ToString(n, 8).PadLeft(3, '0');
			hex = n.ToString("X");
			bin = Convert.ToString(n, 2).PadLeft(8, '0');
			sym = ASCII[n];
			return new string[] { n.ToString(), oct, hex, bin, sym };
		}

		/// <summary>
		/// Helper method that converts from Octal to decimal, hex, binary, and symbol in ascii table.
		/// </summary>
		/// <param name="n">octal search result</param>
		/// <returns>returns search result string containing converted values</returns>
		public string[] ConvertFromOct(int n)
		{
			string dec, hex, bin, sym;
			dec = Convert.ToInt32(n.ToString(), 8).ToString();
			hex = Convert.ToInt32(dec).ToString("X");
			bin = Convert.ToInt32(n.ToString(), 8).ToString().PadLeft(8, '0');
			sym = ASCII[Convert.ToInt32(dec)];
			return new string[] { dec, n.ToString(), hex, bin, sym };
		}

		/// <summary>
		/// Helper method that converts from binary to decimal, octal, hex, and symbol in ascii table.
		/// </summary>
		/// <param name="n">binary search result</param>
		/// <returns>returns search result string containing converted values</returns>
		public string[] ConvertFromBin(int n)
		{
			string dec, oct, hex, sym;
			dec = Convert.ToInt32(n.ToString(), 2).ToString();
			oct = Convert.ToString(Convert.ToInt32(dec), 8).ToString().PadLeft(3, '0');
			hex = Convert.ToInt32(dec).ToString("X");
			sym = ASCII[Convert.ToInt32(dec)];
			return new string[] { dec, oct, hex, n.ToString().PadLeft(8, '0'), sym };
		}

		/// <summary>
		/// Helper method that converts from hexidecimal to decimal, octal, binary, and symbol in ascii table.
		/// </summary>
		/// <param name="n">hexideximal search result</param>
		/// <returns>returns search result string containing converted values</returns>
		public string[] ConvertFromHex(string n)
		{
			string dec, oct, bin, sym;
			int temp = int.Parse(n, System.Globalization.NumberStyles.HexNumber);
			dec = temp.ToString();
			oct = Convert.ToString(temp, 8).PadLeft(3, '0');
			bin = Convert.ToString(temp, 2).PadLeft(8, '0');
			sym = ASCII[temp];
			return new string[] { dec, oct, n, bin, sym };

		}

		/// <summary>
		/// Helper method that converts from a symbol in the ascii table to decinmal, octal, hex, and binary.
		/// </summary>
		/// <param name="n">ascii symbol search result</param>
		/// <returns>returns search result string containing converted values</returns>
		public string[] ConvertFromSym(string n)
		{
			string dec, oct, hex, bin;
			int temp = Array.IndexOf(ASCII, n);
			dec = temp.ToString();
			oct = Convert.ToString(temp, 8).PadLeft(3, '0');
			hex = temp.ToString("X");
			bin = Convert.ToString(temp, 2).PadLeft(8, '0');
			return new string[] { dec, oct, hex, bin, n };
		}
	}
}

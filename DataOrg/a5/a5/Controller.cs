using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5
{
	class Controller
	{
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

		public string[] ConvertFromDec(int n)
		{
			string oct, hex, bin, sym;
			oct = Convert.ToString(n, 8).PadLeft(3, '0');
			hex = n.ToString("X");
			bin = Convert.ToString(n, 2).PadLeft(8, '0');
			sym = ASCII[n];
			return new string[] { n.ToString(), oct, hex, bin, sym };
		}
		public string[] ConvertFromOct(int n)
		{
			string dec, hex, bin, sym;
			dec = Convert.ToString(n, 10);
			hex = Convert.ToInt32(dec).ToString("X");
			bin = Convert.ToString(n, 2).PadLeft(8, '0');
			sym = ASCII[Convert.ToInt32(dec)];
			return new string[] { dec, n.ToString(), hex, bin, sym };
		}
		public string[] ConvertFromBin(int n)
		{
			string dec, oct, hex, sym;
			dec = Convert.ToString(n, 10);
			oct = Convert.ToString(n, 8).PadLeft(3, '0');
			hex = Convert.ToInt32(dec).ToString("X");
			sym = ASCII[Convert.ToInt32(dec)];
			return new string[] { dec, oct, hex, n.ToString().PadLeft(8, '0'), sym };
		}
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

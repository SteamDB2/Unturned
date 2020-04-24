using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class Parser
	{
		public static bool trySplitStart(string serial, out string start, out string end)
		{
			start = string.Empty;
			end = string.Empty;
			int num = serial.IndexOf(' ');
			if (num == -1)
			{
				return false;
			}
			start = serial.Substring(0, num);
			end = serial.Substring(num + 1, serial.Length - num - 1);
			return true;
		}

		public static bool trySplitEnd(string serial, out string start, out string end)
		{
			start = string.Empty;
			end = string.Empty;
			int num = serial.LastIndexOf(' ');
			if (num == -1)
			{
				return false;
			}
			start = serial.Substring(0, num);
			end = serial.Substring(num + 1, serial.Length - num - 1);
			return true;
		}

		public static string[] getComponentsFromSerial(string serial, char delimiter)
		{
			List<string> list = new List<string>();
			int num;
			for (int i = 0; i < serial.Length; i = num + 1)
			{
				num = serial.IndexOf(delimiter, i);
				if (num == -1)
				{
					list.Add(serial.Substring(i, serial.Length - i));
					break;
				}
				list.Add(serial.Substring(i, num - i));
			}
			return list.ToArray();
		}

		public static string getSerialFromComponents(char delimiter, params object[] components)
		{
			string text = string.Empty;
			for (int i = 0; i < components.Length; i++)
			{
				text += components[i].ToString();
				if (i < components.Length - 1)
				{
					text += delimiter;
				}
			}
			return text;
		}

		public static bool checkIP(string ip)
		{
			int num = ip.IndexOf('.');
			if (num == -1)
			{
				return false;
			}
			int num2 = ip.IndexOf('.', num + 1);
			if (num2 == -1)
			{
				return false;
			}
			int num3 = ip.IndexOf('.', num2 + 1);
			if (num3 == -1)
			{
				return false;
			}
			int num4 = ip.IndexOf('.', num3 + 1);
			return num4 == -1;
		}

		public static uint getUInt32FromIP(string ip)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(ip, '.');
			return uint.Parse(componentsFromSerial[0]) << 24 | uint.Parse(componentsFromSerial[1]) << 16 | uint.Parse(componentsFromSerial[2]) << 8 | uint.Parse(componentsFromSerial[3]);
		}

		public static string getIPFromUInt32(uint ip)
		{
			return string.Concat(new object[]
			{
				ip >> 24 & 255u,
				".",
				ip >> 16 & 255u,
				".",
				ip >> 8 & 255u,
				".",
				ip & 255u
			});
		}
	}
}

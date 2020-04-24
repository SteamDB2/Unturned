using System;
using System.Globalization;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	public static class PaletteUtility
	{
		public static string toRGB(Color color)
		{
			Color32 color2 = color;
			return "#" + color2.r.ToString("X2") + color2.g.ToString("X2") + color2.b.ToString("X2");
		}

		public static string toRGBA(Color color)
		{
			Color32 color2 = color;
			return string.Concat(new string[]
			{
				"#",
				color2.r.ToString("X2"),
				color2.g.ToString("X2"),
				color2.b.ToString("X2"),
				color2.a.ToString("X2")
			});
		}

		public static bool tryParse(string value, out Color color)
		{
			color = Color.white;
			if (!string.IsNullOrEmpty(value))
			{
				switch (value.Length)
				{
				case 6:
				{
					uint num;
					if (uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out num))
					{
						color.r = (float)((byte)(num >> 16 & 255u));
						color.g = (float)((byte)(num >> 8 & 255u));
						color.b = (float)((byte)(num & 255u));
						color.a = 255f;
						return true;
					}
					break;
				}
				case 7:
				{
					uint num;
					if (uint.TryParse(value.Substring(1, value.Length - 1), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out num))
					{
						color.r = (float)((byte)(num >> 16 & 255u));
						color.g = (float)((byte)(num >> 8 & 255u));
						color.b = (float)((byte)(num & 255u));
						color.a = 255f;
						return true;
					}
					break;
				}
				case 8:
				{
					uint num;
					if (uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out num))
					{
						color.r = (float)((byte)(num >> 24 & 255u));
						color.g = (float)((byte)(num >> 16 & 255u));
						color.b = (float)((byte)(num >> 8 & 255u));
						color.a = (float)((byte)(num & 255u));
						return true;
					}
					break;
				}
				case 9:
				{
					uint num;
					if (uint.TryParse(value.Substring(1, value.Length - 1), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out num))
					{
						color.r = (float)((byte)(num >> 24 & 255u));
						color.g = (float)((byte)(num >> 16 & 255u));
						color.b = (float)((byte)(num >> 8 & 255u));
						color.a = (float)((byte)(num & 255u));
						return true;
					}
					break;
				}
				}
			}
			return false;
		}
	}
}

using System;
using System.Globalization;
using UnityEngine;

namespace SDG.Unturned
{
	public class Palette
	{
		public static string hex(Color32 color)
		{
			return "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}

		public static Color hex(string color)
		{
			uint num;
			if (!string.IsNullOrEmpty(color) && color.Length == 7 && uint.TryParse(color.Substring(1, color.Length - 1), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out num))
			{
				uint num2 = num >> 16 & 255u;
				uint num3 = num >> 8 & 255u;
				uint num4 = num & 255u;
				return new Color32((byte)num2, (byte)num3, (byte)num4, byte.MaxValue);
			}
			return Color.white;
		}

		public static readonly Color SERVER = Color.green;

		public static readonly Color ADMIN = Color.cyan;

		public static readonly Color PRO = new Color(0.8235294f, 0.7490196f, 0.13333334f);

		public static readonly Color COLOR_W = new Color(0.7058824f, 0.7058824f, 0.7058824f);

		public static readonly Color COLOR_R = new Color(0.7490196f, 0.121568628f, 0.121568628f);

		public static readonly Color COLOR_G = new Color(0.121568628f, 0.5294118f, 0.121568628f);

		public static readonly Color COLOR_B = new Color(0.196078435f, 0.596078455f, 0.784313738f);

		public static readonly Color COLOR_O = new Color(0.670588255f, 0.5019608f, 0.09803922f);

		public static readonly Color COLOR_Y = new Color(0.8627451f, 0.7058824f, 0.07450981f);

		public static readonly Color COLOR_P = new Color(0.41568628f, 0.274509817f, 0.427450985f);

		public static readonly Color AMBIENT = new Color(0.7f, 0.7f, 0.7f);

		public static readonly Color MYTHICAL = new Color(0.980392158f, 0.196078435f, 0.09803922f);
	}
}

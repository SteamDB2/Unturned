using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Customization
	{
		public static bool checkSkin(Color color)
		{
			for (int i = 0; i < Customization.SKINS.Length; i++)
			{
				if (Mathf.Abs(color.r - Customization.SKINS[i].r) < 0.01f && Mathf.Abs(color.g - Customization.SKINS[i].g) < 0.01f && Mathf.Abs(color.b - Customization.SKINS[i].b) < 0.01f)
				{
					return true;
				}
			}
			return false;
		}

		public static bool checkColor(Color color)
		{
			for (int i = 0; i < Customization.COLORS.Length; i++)
			{
				if (Mathf.Abs(color.r - Customization.COLORS[i].r) < 0.01f && Mathf.Abs(color.g - Customization.COLORS[i].g) < 0.01f && Mathf.Abs(color.b - Customization.COLORS[i].b) < 0.01f)
				{
					return true;
				}
			}
			return false;
		}

		public static readonly byte FREE_CHARACTERS = 1;

		public static readonly byte PRO_CHARACTERS = 4;

		public static readonly byte FACES_FREE = 10;

		public static readonly byte HAIRS_FREE = 5;

		public static readonly byte BEARDS_FREE = 5;

		public static readonly byte FACES_PRO = 22;

		public static readonly byte HAIRS_PRO = 18;

		public static readonly byte BEARDS_PRO = 11;

		public static readonly Color[] SKINS = new Color[]
		{
			new Color(0.956862748f, 0.9019608f, 0.8235294f),
			new Color(0.8509804f, 0.7921569f, 0.7058824f),
			new Color(0.745098054f, 0.647058845f, 0.509803951f),
			new Color(0.6156863f, 0.533333361f, 0.419607848f),
			new Color(0.5803922f, 0.4627451f, 0.294117659f),
			new Color(0.4392157f, 0.3764706f, 0.286274523f),
			new Color(0.3254902f, 0.2784314f, 0.211764708f),
			new Color(0.294117659f, 0.239215687f, 0.192156866f),
			new Color(0.2f, 0.172549024f, 0.145098045f),
			new Color(0.137254909f, 0.121568628f, 0.109803922f)
		};

		public static readonly Color[] COLORS = new Color[]
		{
			new Color(0.843137264f, 0.843137264f, 0.843137264f),
			new Color(0.75686276f, 0.75686276f, 0.75686276f),
			new Color(0.8039216f, 0.7529412f, 0.549019635f),
			new Color(0.6745098f, 0.41568628f, 0.223529413f),
			new Color(0.4f, 0.3137255f, 0.215686277f),
			new Color(0.34117648f, 0.270588249f, 0.184313729f),
			new Color(0.2784314f, 0.223529413f, 0.156862751f),
			new Color(0.20784314f, 0.172549024f, 0.13333334f),
			new Color(0.215686277f, 0.215686277f, 0.215686277f),
			new Color(0.09803922f, 0.09803922f, 0.09803922f)
		};

		public static readonly byte SKILLSETS = 11;
	}
}

using System;
using SDG.Framework.Debug;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Config
	{
		[TerminalCommandProperty("sleek2.header_font_size", "big text font size", 30)]
		public static uint headerFontSize
		{
			get
			{
				return Sleek2Config._headerFontSize;
			}
			set
			{
				Sleek2Config._headerFontSize = value;
				TerminalUtility.printCommandPass("Set header_font_size to: " + Sleek2Config.headerFontSize);
			}
		}

		[TerminalCommandProperty("sleek2.header_height", "height of big boxes", 40)]
		public static uint headerHeight
		{
			get
			{
				return Sleek2Config._headerHeight;
			}
			set
			{
				Sleek2Config._headerHeight = value;
				TerminalUtility.printCommandPass("Set header_height to: " + Sleek2Config.headerHeight);
			}
		}

		[TerminalCommandProperty("sleek2.body_font_size", "small text font size", 16)]
		public static int bodyFontSize
		{
			get
			{
				return Sleek2Config._bodyFontSize;
			}
			set
			{
				Sleek2Config._bodyFontSize = value;
				TerminalUtility.printCommandPass("Set body_font_size to: " + Sleek2Config.bodyFontSize);
			}
		}

		[TerminalCommandProperty("sleek2.body_height", "height of small boxes", 20)]
		public static int bodyHeight
		{
			get
			{
				return Sleek2Config._bodyHeight;
			}
			set
			{
				Sleek2Config._bodyHeight = value;
				TerminalUtility.printCommandPass("Set body_height to: " + Sleek2Config.bodyHeight);
			}
		}

		[TerminalCommandProperty("sleek2.tab_width", "width of tabs", 150)]
		public static int tabWidth
		{
			get
			{
				return Sleek2Config._tabWidth;
			}
			set
			{
				Sleek2Config._tabWidth = value;
				TerminalUtility.printCommandPass("Set tab_width to: " + Sleek2Config.tabWidth);
			}
		}

		[TerminalCommandProperty("sleek2.light_text_color", "hex color of white text", "#e1e1e1")]
		public static string lightTextColorHex
		{
			get
			{
				return Sleek2Config._lightTextColorHex;
			}
			set
			{
				Sleek2Config._lightTextColorHex = value;
				PaletteUtility.tryParse(Sleek2Config.lightTextColorHex, out Sleek2Config.lightTextColor);
				TerminalUtility.printCommandPass("Set light_text_color to: " + Sleek2Config.lightTextColorHex);
			}
		}

		[TerminalCommandProperty("sleek2.dark_text_color", "hex color of black text", "#191919")]
		public static string darkTextColorHex
		{
			get
			{
				return Sleek2Config._darkTextColorHex;
			}
			set
			{
				Sleek2Config._darkTextColorHex = value;
				PaletteUtility.tryParse(Sleek2Config.darkTextColorHex, out Sleek2Config.darkTextColor);
				TerminalUtility.printCommandPass("Set dark_text_color to: " + Sleek2Config.darkTextColorHex);
			}
		}

		[TerminalCommandProperty("sleek2.dock_color", "hex color of docking points", "#f3c50f")]
		public static string dockColorHex
		{
			get
			{
				return Sleek2Config._dockColorHex;
			}
			set
			{
				Sleek2Config._dockColorHex = value;
				PaletteUtility.tryParse(Sleek2Config.dockColorHex, out Sleek2Config.dockColor);
				TerminalUtility.printCommandPass("Set dock_color to: " + Sleek2Config.dockColorHex);
			}
		}

		private static uint _headerFontSize = 30u;

		private static uint _headerHeight = 40u;

		private static int _bodyFontSize = 16;

		private static int _bodyHeight = 20;

		private static int _tabWidth = 150;

		public static Color lightTextColor = new Color32(225, 225, 225, byte.MaxValue);

		private static string _lightTextColorHex = "#e1e1e1";

		public static Color darkTextColor = new Color32(25, 25, 25, byte.MaxValue);

		private static string _darkTextColorHex = "#191919";

		public static Color dockColor = new Color32(243, 197, 15, byte.MaxValue);

		private static string _dockColorHex = "#f3c50f";
	}
}

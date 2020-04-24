using System;
using System.Collections.Generic;

namespace SDG.Framework.Devkit
{
	public class DevkitBrowserSelection
	{
		public static List<string> selectedPaths
		{
			get
			{
				return DevkitBrowserSelection._selectedPaths;
			}
		}

		private static List<string> _selectedPaths = new List<string>();
	}
}

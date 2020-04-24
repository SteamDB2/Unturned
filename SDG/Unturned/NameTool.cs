using System;

namespace SDG.Unturned
{
	public class NameTool
	{
		public static bool checkNames(string input, string name)
		{
			return input.Length <= name.Length && name.ToLower().IndexOf(input.ToLower()) != -1;
		}

		public static bool isValid(string name)
		{
			foreach (int num in name.ToCharArray())
			{
				if (num <= 31)
				{
					return false;
				}
				if (num >= 126)
				{
					return false;
				}
				if (num == 47 || num == 92 || num == 96)
				{
					return false;
				}
			}
			return true;
		}
	}
}

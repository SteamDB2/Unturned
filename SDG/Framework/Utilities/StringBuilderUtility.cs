using System;
using System.Text;

namespace SDG.Framework.Utilities
{
	public class StringBuilderUtility
	{
		public static StringBuilder instance
		{
			get
			{
				if (StringBuilderUtility._instance == null)
				{
					StringBuilderUtility._instance = new StringBuilder();
				}
				StringBuilderUtility._instance.Length = 0;
				return StringBuilderUtility._instance;
			}
		}

		private static StringBuilder _instance;
	}
}

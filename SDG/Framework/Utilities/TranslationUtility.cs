using System;
using SDG.Framework.Translations;

namespace SDG.Framework.Utilities
{
	public static class TranslationUtility
	{
		public static bool tryParse(string value, out TranslationReference text)
		{
			string newNamespace;
			string newToken;
			bool result = TranslationUtility.tryParse(value, out newNamespace, out newToken);
			text = new TranslationReference(newNamespace, newToken);
			return result;
		}

		public static bool tryParse(string value, out string ns, out string token)
		{
			if (string.IsNullOrEmpty(value))
			{
				ns = null;
				token = null;
				return false;
			}
			string[] array = value.Split(TranslationUtility.DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				ns = array[0];
				token = array[1];
				return true;
			}
			ns = null;
			token = null;
			return false;
		}

		private static readonly string[] DELIMITERS = new string[]
		{
			"#",
			"::"
		};
	}
}

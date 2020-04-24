using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;

namespace SDG.Framework.Translations
{
	public class Translator
	{
		[TerminalCommandProperty("language", "name of language to load translations for", "english")]
		public static string language
		{
			get
			{
				return Translator._language;
			}
			set
			{
				if (Translator.language == value)
				{
					return;
				}
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				string language = Translator.language;
				Translator._language = value.ToLower();
				TerminalUtility.printCommandPass("Set language to: " + Translator.language);
				Translator.triggerLanguageChanged(language, Translator.language);
			}
		}

		public static event LanguageChangedHandler languageChanged;

		public static event TranslationRegisteredHandler translationRegistered;

		public static Dictionary<string, Dictionary<string, Translation>> languages
		{
			get
			{
				return Translator._languages;
			}
		}

		public static HashSet<TranslationReference> misses
		{
			get
			{
				return Translator._misses;
			}
		}

		public static bool isOriginLanguage(string language)
		{
			return !string.IsNullOrEmpty(language) && language.Equals(Translator.ORIGIN_LANGUAGE, StringComparison.InvariantCultureIgnoreCase);
		}

		public static bool isCurrentLanguage(string language)
		{
			return !string.IsNullOrEmpty(language) && language.Equals(Translator.language, StringComparison.InvariantCultureIgnoreCase);
		}

		public static TranslationLeaf addLeaf(TranslationReference reference)
		{
			if (!reference.isValid)
			{
				return null;
			}
			Dictionary<string, Translation> dictionary;
			if (!Translator.languages.TryGetValue(Translator.ORIGIN_LANGUAGE, out dictionary))
			{
				return null;
			}
			Translation translation;
			if (!dictionary.TryGetValue(reference.ns, out translation))
			{
				return null;
			}
			return translation.addLeaf(reference.token);
		}

		public static TranslationLeaf getLeaf(TranslationReference reference)
		{
			return Translator.getLeaf(Translator.language, reference);
		}

		public static TranslationLeaf getLeaf(string language, TranslationReference reference)
		{
			if (string.IsNullOrEmpty(language) || !reference.isValid)
			{
				return null;
			}
			Dictionary<string, Translation> dictionary;
			TranslationLeaf translationLeaf;
			Translation translation;
			if (!Translator.languages.TryGetValue(language, out dictionary))
			{
				translationLeaf = null;
			}
			else if (!dictionary.TryGetValue(reference.ns, out translation))
			{
				translationLeaf = null;
			}
			else
			{
				translationLeaf = translation.getLeaf(reference.token);
			}
			if (translationLeaf != null)
			{
				return translationLeaf;
			}
			if (language == Translator.ORIGIN_LANGUAGE)
			{
				Translator.misses.Add(reference);
				return null;
			}
			return Translator.getLeaf(Translator.ORIGIN_LANGUAGE, reference);
		}

		public static string translate(TranslationReference reference)
		{
			TranslationLeaf leaf = Translator.getLeaf(reference);
			if (leaf != null)
			{
				return leaf.text;
			}
			return reference.ToString();
		}

		public static void registerTranslationDirectory(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			if (!Directory.Exists(path))
			{
				return;
			}
			foreach (string path2 in Directory.GetFiles(path, "*.translation"))
			{
				Translator.registerTranslationFile(path2);
			}
		}

		public static void registerTranslationFile(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			if (!File.Exists(path))
			{
				return;
			}
			string text = null;
			string text2 = null;
			bool flag = false;
			using (StreamReader streamReader = new StreamReader(path))
			{
				IFormattedFileReader formattedFileReader = new LimitedKeyValueTableReader("Metadata", streamReader);
				formattedFileReader.readKey("Metadata");
				IFormattedFileReader formattedFileReader2 = formattedFileReader.readObject();
				if (formattedFileReader2 != null)
				{
					formattedFileReader2.readKey("Language");
					text = formattedFileReader2.readValue();
					formattedFileReader2.readKey("Namespace");
					text2 = formattedFileReader2.readValue();
					if (text != null && text2 != null)
					{
						text = text.ToLower();
						if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text))
						{
							Dictionary<string, Translation> dictionary;
							if (!Translator.languages.TryGetValue(text, out dictionary))
							{
								dictionary = new Dictionary<string, Translation>();
								Translator.languages.Add(text, dictionary);
							}
							if (!dictionary.ContainsKey(text2))
							{
								Translation value = new Translation(path, text, text2);
								dictionary.Add(text2, value);
								flag = true;
							}
						}
					}
				}
			}
			if (flag)
			{
				Translator.triggerTranslationRegistered(text, text2);
			}
		}

		public static void loadTranslations(string language)
		{
			Dictionary<string, Translation> dictionary;
			if (!Translator.languages.TryGetValue(language, out dictionary))
			{
				return;
			}
			foreach (KeyValuePair<string, Translation> keyValuePair in dictionary)
			{
				keyValuePair.Value.load();
			}
			Terminal.print("Loaded \"" + language + "\" language", null, "Translations", "<color=#d58718>Translations</color>", true);
		}

		public static void loadTranslation(string language, string ns)
		{
			Dictionary<string, Translation> dictionary;
			if (!Translator.languages.TryGetValue(language, out dictionary))
			{
				return;
			}
			Translation translation;
			if (!dictionary.TryGetValue(ns, out translation))
			{
				return;
			}
			translation.load();
			Terminal.print(string.Concat(new string[]
			{
				"Loaded \"",
				language,
				"\" language \"",
				ns,
				"\" namespace"
			}), null, "Translations", "<color=#d58718>Translations</color>", true);
		}

		public static void unloadTranslations(string language)
		{
			Dictionary<string, Translation> dictionary;
			if (!Translator.languages.TryGetValue(language, out dictionary))
			{
				return;
			}
			foreach (KeyValuePair<string, Translation> keyValuePair in dictionary)
			{
				keyValuePair.Value.unload();
			}
			Terminal.print("Unloaded \"" + language + "\" language", null, "Translations", "<color=#d58718>Translations</color>", true);
		}

		public static void unloadTranslation(string language, string ns)
		{
			Dictionary<string, Translation> dictionary;
			if (!Translator.languages.TryGetValue(language, out dictionary))
			{
				return;
			}
			Translation translation;
			if (!dictionary.TryGetValue(ns, out translation))
			{
				return;
			}
			translation.unload();
			Terminal.print(string.Concat(new string[]
			{
				"Unloaded \"",
				language,
				"\" language \"",
				ns,
				"\" namespace"
			}), null, "Translations", "<color=#d58718>Translations</color>", true);
		}

		protected static void triggerLanguageChanged(string oldLanguage, string newLanguage)
		{
			if (Translator.languageChanged != null)
			{
				Translator.languageChanged(oldLanguage, newLanguage);
			}
		}

		protected static void triggerTranslationRegistered(string language, string ns)
		{
			if (Translator.translationRegistered != null)
			{
				Translator.translationRegistered(language, ns);
			}
		}

		public static readonly string ORIGIN_LANGUAGE = "english";

		protected static string _language = Translator.ORIGIN_LANGUAGE;

		protected static Dictionary<string, Dictionary<string, Translation>> _languages = new Dictionary<string, Dictionary<string, Translation>>();

		protected static HashSet<TranslationReference> _misses = new HashSet<TranslationReference>();
	}
}

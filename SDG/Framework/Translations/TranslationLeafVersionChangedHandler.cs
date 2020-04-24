using System;

namespace SDG.Framework.Translations
{
	public delegate void TranslationLeafVersionChangedHandler(TranslationLeaf leaf, int oldVersion, int newVersion);
}

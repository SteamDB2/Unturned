using System;

namespace SDG.Framework.Translations
{
	public delegate void TranslationBranchKeyChangedHandler(TranslationBranch branch, string oldKey, string newKey);
}

using System;
using SDG.Framework.Translations;

namespace SDG.Framework.Debug
{
	public interface IInspectableListElement
	{
		string inspectableListIndexInternalName { get; }

		TranslationReference inspectableListIndexDisplayName { get; }
	}
}

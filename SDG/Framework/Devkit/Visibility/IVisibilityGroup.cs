using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Translations;

namespace SDG.Framework.Devkit.Visibility
{
	public interface IVisibilityGroup : IInspectableListElement, IFormattedFileReadable, IFormattedFileWritable
	{
		string internalName { get; set; }

		TranslationReference displayName { get; set; }

		bool isVisible { get; set; }

		event VisibilityGroupIsVisibleChangedHandler isVisibleChanged;
	}
}

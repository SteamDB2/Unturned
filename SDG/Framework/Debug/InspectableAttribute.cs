using System;
using SDG.Framework.Translations;

namespace SDG.Framework.Debug
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class InspectableAttribute : Attribute
	{
		public InspectableAttribute(string namePath, string tooltipPath = null)
		{
			this.name = new TranslationReference(namePath);
			this.tooltip = new TranslationReference(tooltipPath);
		}

		public TranslationReference name;

		public TranslationReference tooltip;
	}
}

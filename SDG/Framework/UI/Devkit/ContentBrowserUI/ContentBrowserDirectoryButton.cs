using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.ContentBrowserUI
{
	public class ContentBrowserDirectoryButton : Sleek2ImageButton
	{
		public ContentBrowserDirectoryButton(ContentDirectory newDirectory)
		{
			this.directory = newDirectory;
			Sleek2Label sleek2Label = new Sleek2Label();
			sleek2Label.transform.reset();
			sleek2Label.textComponent.text = "/" + this.directory.name;
			sleek2Label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2Label);
		}

		public ContentDirectory directory { get; protected set; }
	}
}

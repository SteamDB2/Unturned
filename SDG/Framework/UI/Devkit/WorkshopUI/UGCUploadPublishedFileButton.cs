using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class UGCUploadPublishedFileButton : Sleek2ImageButton
	{
		public UGCUploadPublishedFileButton(SteamPublished newFile)
		{
			this.file = newFile;
			this.label = new Sleek2Label();
			this.label.transform.anchorMin = new Vector2(0f, 0f);
			this.label.transform.anchorMax = new Vector2(1f, 1f);
			this.label.transform.offsetMin = new Vector2(5f, 5f);
			this.label.transform.offsetMax = new Vector2(-5f, -5f);
			this.label.textComponent.text = string.Concat(new object[]
			{
				this.file.name,
				" [",
				this.file.id,
				"]"
			});
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(this.label);
		}

		public Sleek2Label label { get; protected set; }

		public SteamPublished file { get; protected set; }
	}
}

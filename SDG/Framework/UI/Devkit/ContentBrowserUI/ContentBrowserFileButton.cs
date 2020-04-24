using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.ContentBrowserUI
{
	public class ContentBrowserFileButton : Sleek2ImageButton
	{
		public ContentBrowserFileButton(ContentFile newFile)
		{
			this.file = newFile;
			Sleek2Label sleek2Label = new Sleek2Label();
			sleek2Label.transform.reset();
			sleek2Label.textComponent.text = this.file.name;
			sleek2Label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2Label);
			Type guessedType = this.file.guessedType;
			if (guessedType == null)
			{
				return;
			}
			Type type = typeof(ContentReference<>).MakeGenericType(new Type[]
			{
				guessedType
			});
			this.dragable = base.gameObject.AddComponent<DragableSystemObject>();
			this.dragable.target = base.transform;
			this.dragable.source = Activator.CreateInstance(type, new object[]
			{
				this.file.rootDirectory.name,
				this.file.path
			});
		}

		public ContentFile file { get; protected set; }

		public DragableSystemObject dragable { get; protected set; }
	}
}

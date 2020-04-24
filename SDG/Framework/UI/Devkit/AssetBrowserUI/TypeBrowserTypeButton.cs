using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.AssetBrowserUI
{
	public class TypeBrowserTypeButton : Sleek2ImageButton
	{
		public TypeBrowserTypeButton(Type newType)
		{
			this.type = newType;
			Sleek2Label sleek2Label = new Sleek2Label();
			sleek2Label.transform.reset();
			sleek2Label.textComponent.text = this.type.Name;
			sleek2Label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2Label);
			Type type = typeof(TypeReference<>).MakeGenericType(new Type[]
			{
				this.type
			});
			this.dragable = base.gameObject.AddComponent<DragableSystemObject>();
			this.dragable.target = base.transform;
			this.dragable.source = Activator.CreateInstance(type, new object[]
			{
				this.type.AssemblyQualifiedName
			});
		}

		public Type type { get; protected set; }

		public DragableSystemObject dragable { get; protected set; }
	}
}

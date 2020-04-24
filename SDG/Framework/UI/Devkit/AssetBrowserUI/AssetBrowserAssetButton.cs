using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.AssetBrowserUI
{
	public class AssetBrowserAssetButton : Sleek2ImageButton
	{
		public AssetBrowserAssetButton(Asset newAsset)
		{
			this.asset = newAsset;
			this.field = new Sleek2Field();
			this.field.transform.anchorMin = new Vector2(0f, 0.5f);
			this.field.transform.anchorMax = new Vector2(1f, 1f);
			this.field.transform.offsetMin = new Vector2(5f, 0f);
			this.field.transform.offsetMax = new Vector2(-5f, -5f);
			this.field.fieldComponent.text = this.asset.name;
			this.field.fieldComponent.textComponent.text = this.field.fieldComponent.text;
			this.field.submitted += this.handleFieldSubmitted;
			this.addElement(this.field);
			this.label = new Sleek2Label();
			this.label.transform.anchorMin = new Vector2(0f, 0f);
			this.label.transform.anchorMax = new Vector2(1f, 0.5f);
			this.label.transform.offsetMin = new Vector2(5f, 5f);
			this.label.transform.offsetMax = new Vector2(-5f, 0f);
			this.label.textComponent.text = '[' + this.asset.GetType().Name + ']';
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(this.label);
			Type type = this.asset.GetType();
			Type type2 = typeof(AssetReference<>).MakeGenericType(new Type[]
			{
				type
			});
			this.dragable = base.gameObject.AddComponent<DragableSystemObject>();
			this.dragable.target = base.transform;
			this.dragable.source = Activator.CreateInstance(type2, new object[]
			{
				this.asset.GUID
			});
		}

		public Asset asset { get; protected set; }

		public DragableSystemObject dragable { get; protected set; }

		public Sleek2Field field { get; protected set; }

		public Sleek2Label label { get; protected set; }

		protected virtual void handleFieldSubmitted(Sleek2Field field, string value)
		{
			Assets.rename(this.asset, value);
		}
	}
}

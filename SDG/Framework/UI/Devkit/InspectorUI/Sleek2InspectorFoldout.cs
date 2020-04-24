using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class Sleek2InspectorFoldout : Sleek2Element
	{
		public Sleek2InspectorFoldout()
		{
			base.name = "Foldout";
			this.title = new Sleek2Image();
			this.title.name = "Title";
			this.title.transform.pivot = new Vector2(0f, 1f);
			this.title.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Toolbar_Background");
			this.title.imageComponent.type = 1;
			this.addElement(this.title);
			this.button = new Sleek2ImageButton();
			this.button.transform.anchorMin = new Vector2(0f, 1f);
			this.button.transform.anchorMax = new Vector2(0f, 1f);
			this.button.transform.pivot = new Vector2(0f, 1f);
			this.button.transform.sizeDelta = new Vector2((float)Sleek2Config.bodyHeight, (float)Sleek2Config.bodyHeight);
			this.button.imageComponent.color = new Color(0.5f, 0.5f, 0.5f);
			this.button.clicked += this.handleButtonClicked;
			this.title.addElement(this.button);
			this.label = new Sleek2TranslatedLabel();
			this.label.transform.anchorMin = new Vector2(0f, 0f);
			this.label.transform.anchorMax = new Vector2(1f, 1f);
			this.label.transform.pivot = new Vector2(0f, 0f);
			this.label.transform.offsetMin = new Vector2((float)(Sleek2Config.bodyHeight + 5), 0f);
			this.label.transform.offsetMax = new Vector2(0f, 0f);
			this.label.textComponent.alignment = 3;
			this.title.addElement(this.label);
			this.contents = new Sleek2Element();
			this.contents.name = "Contents";
			this.contents.transform.pivot = new Vector2(0f, 1f);
			this.addElement(this.contents);
			this.layoutComponent = this.title.gameObject.AddComponent<LayoutElement>();
			this.layoutComponent.minHeight = (float)Sleek2Config.bodyHeight;
			this.groupComponent = base.gameObject.AddComponent<VerticalLayoutGroup>();
			this.groupComponent.childForceExpandWidth = true;
			this.groupComponent.childForceExpandHeight = false;
			this.foldoutComponent = this.contents.gameObject.AddComponent<VerticalLayoutGroup>();
			this.foldoutComponent.padding.left = 25;
			this.foldoutComponent.padding.top = 5;
			this.foldoutComponent.spacing = 5f;
			this.foldoutComponent.childForceExpandWidth = true;
			this.foldoutComponent.childForceExpandHeight = false;
			this.isOpen = true;
		}

		public Sleek2Image title { get; protected set; }

		public Sleek2ImageButton button { get; protected set; }

		public Sleek2TranslatedLabel label { get; protected set; }

		public Sleek2Element contents { get; protected set; }

		public LayoutElement layoutComponent { get; protected set; }

		public VerticalLayoutGroup groupComponent { get; protected set; }

		public VerticalLayoutGroup foldoutComponent { get; protected set; }

		public virtual bool isOpen
		{
			get
			{
				return this.contents.gameObject.activeSelf;
			}
			set
			{
				this.contents.gameObject.SetActive(value);
				if (value)
				{
					this.button.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Fold_In");
				}
				else
				{
					this.button.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Fold_Out");
				}
			}
		}

		protected virtual void handleButtonClicked(Sleek2ImageButton button)
		{
			this.isOpen = !this.isOpen;
		}
	}
}

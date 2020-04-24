using System;
using SDG.Framework.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Separator : Sleek2Element
	{
		public Sleek2Separator()
		{
			base.gameObject.name = "Separator";
			this.handle = base.gameObject.AddComponent<Separator>();
			this.handle.min = 0.1f;
			this.handle.max = 0.9f;
			this.handle.padding = 5f;
			this.handle.width = 8f;
			this.image = base.gameObject.AddComponent<Image>();
			this.image.type = 1;
			this.direction = Separator.EDirection.HORIZONTAL;
			base.gameObject.AddComponent<Selectable>();
		}

		public Separator handle { get; protected set; }

		public Image image { get; protected set; }

		public Separator.EDirection direction
		{
			get
			{
				return this.handle.direction;
			}
			set
			{
				this.handle.direction = value;
				Separator.EDirection direction = this.direction;
				if (direction != Separator.EDirection.HORIZONTAL)
				{
					if (direction == Separator.EDirection.VERTICAL)
					{
						this.image.sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Vertical");
					}
				}
				else
				{
					this.image.sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Horizontal");
				}
			}
		}
	}
}

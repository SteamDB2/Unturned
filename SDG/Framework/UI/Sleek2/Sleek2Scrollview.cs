using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Scrollview : Sleek2Element
	{
		public Sleek2Scrollview()
		{
			base.gameObject.name = "Scrollview";
			this.scrollrectComponent = base.gameObject.AddComponent<ScrollRect>();
			this.scrollrectComponent.horizontalScrollbarVisibility = 2;
			this.scrollrectComponent.horizontalScrollbarSpacing = 5f;
			this.scrollrectComponent.horizontal = false;
			this.scrollrectComponent.verticalScrollbarVisibility = 2;
			this.scrollrectComponent.verticalScrollbarSpacing = 5f;
			this.scrollrectComponent.vertical = false;
			this.mask = new Sleek2Mask();
			this.mask.transform.reset();
			this.mask.transform.pivot = new Vector2(0f, 1f);
			this.addElement(this.mask);
			this.scrollrectComponent.viewport = this.mask.transform;
			this.horizontalScrollbar = new Sleek2Scrollbar();
			this.horizontalScrollbar.transform.anchorMin = new Vector2(0f, 0f);
			this.horizontalScrollbar.transform.anchorMax = new Vector2(1f, 0f);
			this.horizontalScrollbar.transform.pivot = new Vector2(0f, 0f);
			this.horizontalScrollbar.transform.offsetMin = new Vector2(0f, 0f);
			this.horizontalScrollbar.transform.offsetMax = new Vector2(0f, 20f);
			this.horizontalScrollbar.scrollbarComponent.direction = 0;
			this.horizontalScrollbar.gameObject.SetActive(false);
			this.addElement(this.horizontalScrollbar);
			this.verticalScrollbar = new Sleek2Scrollbar();
			this.verticalScrollbar.transform.anchorMin = new Vector2(1f, 0f);
			this.verticalScrollbar.transform.anchorMax = new Vector2(1f, 1f);
			this.verticalScrollbar.transform.pivot = new Vector2(1f, 1f);
			this.verticalScrollbar.transform.offsetMin = new Vector2(-20f, 0f);
			this.verticalScrollbar.transform.offsetMax = new Vector2(0f, 0f);
			this.verticalScrollbar.scrollbarComponent.direction = 2;
			this.verticalScrollbar.gameObject.SetActive(false);
			this.addElement(this.verticalScrollbar);
		}

		public ScrollRect scrollrectComponent { get; protected set; }

		public Sleek2Mask mask { get; protected set; }

		public Sleek2Element panel
		{
			get
			{
				return this._panel;
			}
			set
			{
				if (this.panel != null)
				{
					this.mask.removeElement(this.panel);
				}
				this._panel = value;
				if (this.panel != null)
				{
					this.mask.addElement(this.panel);
					this.scrollrectComponent.content = this.panel.transform;
				}
				else
				{
					this.scrollrectComponent.content = null;
				}
			}
		}

		public bool horizontal
		{
			get
			{
				return this.scrollrectComponent.horizontalScrollbar != null;
			}
			set
			{
				this.scrollrectComponent.horizontalScrollbar = ((!value) ? null : this.horizontalScrollbar.scrollbarComponent);
			}
		}

		public bool vertical
		{
			get
			{
				return this.scrollrectComponent.verticalScrollbar != null;
			}
			set
			{
				this.scrollrectComponent.verticalScrollbar = ((!value) ? null : this.verticalScrollbar.scrollbarComponent);
			}
		}

		public Sleek2Scrollbar horizontalScrollbar { get; protected set; }

		public Sleek2Scrollbar verticalScrollbar { get; protected set; }

		protected Sleek2Element _panel;
	}
}

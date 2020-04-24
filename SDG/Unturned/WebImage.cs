using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	public class WebImage : MonoBehaviour
	{
		private IEnumerator Download()
		{
			WWW request = new WWW(this.url);
			yield return request;
			if (string.IsNullOrEmpty(request.error))
			{
				this.texture = request.texture;
				this.texture.name = "WebImage";
				this.texture.filterMode = 2;
				if (this.texture != null)
				{
					this.sprite = Sprite.Create(this.texture, new Rect(0f, 0f, (float)this.texture.width, (float)this.texture.height), new Vector2(0.5f, 0.5f), 100f);
					this.sprite.name = "WebSprite";
					this.targetImage.sprite = this.sprite;
					this.isExpanded = (this.texture.width <= 360 && this.texture.height <= 360);
					this.updateLayout();
				}
			}
			yield break;
		}

		public void Refresh()
		{
			if (this.targetImage == null || string.IsNullOrEmpty(this.url))
			{
				return;
			}
			base.StartCoroutine("Download");
		}

		protected void updateLayout()
		{
			if (this.texture == null)
			{
				return;
			}
			this.targetLayout.preferredHeight = (float)((!this.isExpanded) ? Mathf.Min(this.texture.height / 2, 360) : -1);
		}

		protected void onClick()
		{
			this.isExpanded = !this.isExpanded;
			this.updateLayout();
		}

		protected virtual void Start()
		{
			this.Refresh();
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		public Image targetImage;

		public Button targetButton;

		public LayoutElement targetLayout;

		public string url;

		public bool isExpanded;

		protected Texture2D texture;

		protected Sprite sprite;
	}
}

using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekImageMaterial : SleekImageTexture
	{
		public SleekImageMaterial()
		{
			base.init();
		}

		public SleekImageMaterial(Texture newTexture, Material newMaterial)
		{
			base.init();
			this.texture = newTexture;
			this.material = newMaterial;
		}

		public override void draw(bool ignoreCulling)
		{
			if (!this.isHidden)
			{
				SleekRender.drawImageMaterial(base.frame, this.texture, this.material);
			}
			base.drawChildren(ignoreCulling);
		}

		public Material material;
	}
}

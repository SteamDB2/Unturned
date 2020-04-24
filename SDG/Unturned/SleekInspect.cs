using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekInspect : SleekBox
	{
		public SleekInspect(string path)
		{
			base.init();
			RenderTexture renderTexture = (RenderTexture)Resources.Load(path);
			this.renderImage = new SleekImageMaterial();
			this.renderImage.sizeScale_X = 1f;
			this.renderImage.sizeScale_Y = 1f;
			this.renderImage.constraint = ESleekConstraint.X;
			this.renderImage.constrain_Y = renderTexture.height;
			this.renderImage.texture = renderTexture;
			this.renderImage.material = (Material)Resources.Load("Materials/RenderTexture");
			base.add(this.renderImage);
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
		}

		public SleekImageMaterial renderImage;
	}
}

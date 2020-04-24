using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekTile : Sleek
	{
		public SleekTile()
		{
			base.init();
		}

		public override void draw(bool ignoreCulling)
		{
			SleekRender.drawTile(base.frame, this.texture, base.backgroundColor);
			base.drawChildren(ignoreCulling);
		}

		public Texture texture;
	}
}

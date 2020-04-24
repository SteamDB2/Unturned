using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekGrid : Sleek
	{
		public SleekGrid()
		{
			base.init();
		}

		public override void draw(bool ignoreCulling)
		{
			if (SleekRender.drawGrid(base.frame, this.texture, base.backgroundColor) && this.onClickedGrid != null)
			{
				this.onClickedGrid(this);
			}
			base.drawChildren(ignoreCulling);
		}

		public Texture texture;

		public ClickedGrid onClickedGrid;
	}
}

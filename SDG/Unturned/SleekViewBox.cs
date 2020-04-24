using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekViewBox : Sleek
	{
		public SleekViewBox()
		{
			this.state = new Vector2(0f, 0f);
			this.local = true;
			this.backgroundTint = ESleekTint.BACKGROUND;
			base.init();
		}

		public override Rect getCullingRect()
		{
			return new Rect(this.state.x, this.state.y, base.frame.width, base.frame.height);
		}

		public override void draw(bool ignoreCulling)
		{
			GUI.backgroundColor = base.backgroundColor;
			this.state = GUI.BeginScrollView(base.frame, this.state, this.area);
			base.drawChildren(ignoreCulling);
			GUI.EndScrollView(false);
		}

		public Rect area;

		public Vector2 state;
	}
}

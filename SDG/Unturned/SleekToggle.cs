using System;

namespace SDG.Unturned
{
	public class SleekToggle : Sleek
	{
		public SleekToggle()
		{
			base.init();
		}

		public override void draw(bool ignoreCulling)
		{
			bool flag = SleekRender.drawToggle(base.frame, base.backgroundColor, this.state);
			if (flag != this.state && this.onToggled != null)
			{
				this.onToggled(this, flag);
			}
			this.state = flag;
			base.drawChildren(ignoreCulling);
		}

		public Toggled onToggled;

		public bool state;
	}
}

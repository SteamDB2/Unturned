using System;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;

namespace SDG.Framework.UI.Devkit.LoadUI
{
	public class Sleek2Loadable : Sleek2ImageLabelButton
	{
		public Sleek2Loadable(LevelInfo newLevelInfo)
		{
			this.levelInfo = newLevelInfo;
			base.label.textComponent.text = this.levelInfo.name;
		}

		public LevelInfo levelInfo { get; protected set; }
	}
}

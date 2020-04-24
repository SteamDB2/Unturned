using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekNew : SleekLabel
	{
		public SleekNew(bool isUpdate = false)
		{
			base.init();
			base.positionOffset_X = -105;
			base.positionScale_X = 1f;
			base.sizeOffset_X = 100;
			base.sizeOffset_Y = 30;
			this.fontAlignment = 5;
			base.text = Provider.localization.format((!isUpdate) ? "New" : "Updated");
			this.foregroundTint = ESleekTint.NONE;
			base.foregroundColor = Color.green;
		}
	}
}

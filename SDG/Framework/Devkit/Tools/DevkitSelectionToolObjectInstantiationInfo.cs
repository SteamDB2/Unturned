using System;
using SDG.Unturned;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitSelectionToolObjectInstantiationInfo : DevkitSelectionToolInstantiationInfoBase
	{
		public virtual ObjectAsset asset { get; set; }

		public override void instantiate()
		{
			DevkitObjectFactory.instantiate(this.asset, this.position, this.rotation, this.scale);
		}
	}
}

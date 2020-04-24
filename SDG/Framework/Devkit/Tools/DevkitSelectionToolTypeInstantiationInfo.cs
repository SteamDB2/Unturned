using System;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitSelectionToolTypeInstantiationInfo : DevkitSelectionToolInstantiationInfoBase
	{
		public virtual Type type { get; set; }

		public override void instantiate()
		{
			DevkitTypeFactory.instantiate(this.type, this.position, this.rotation, this.scale);
		}
	}
}

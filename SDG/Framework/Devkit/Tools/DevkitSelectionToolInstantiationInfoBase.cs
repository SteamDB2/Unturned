using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public abstract class DevkitSelectionToolInstantiationInfoBase : IDevkitSelectionToolInstantiationInfo
	{
		public DevkitSelectionToolInstantiationInfoBase()
		{
			this.scale = Vector3.one;
		}

		public virtual Vector3 position { get; set; }

		public virtual Quaternion rotation { get; set; }

		public virtual Vector3 scale { get; set; }

		public abstract void instantiate();
	}
}

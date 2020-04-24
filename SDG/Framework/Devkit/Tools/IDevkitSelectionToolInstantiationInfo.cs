using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public interface IDevkitSelectionToolInstantiationInfo
	{
		Vector3 position { get; set; }

		Quaternion rotation { get; set; }

		Vector3 scale { get; set; }

		void instantiate();
	}
}

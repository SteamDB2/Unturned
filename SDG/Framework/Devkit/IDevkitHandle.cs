using System;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public interface IDevkitHandle
	{
		void suggestTransform(Vector3 position, Quaternion rotation);
	}
}

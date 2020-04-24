using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class LightLODTool
	{
		public static void applyLightLOD(Transform transform)
		{
			if (transform == null)
			{
				return;
			}
			LightLODTool.lightsInChildren.Clear();
			transform.GetComponentsInChildren<Light>(true, LightLODTool.lightsInChildren);
			for (int i = 0; i < LightLODTool.lightsInChildren.Count; i++)
			{
				Light light = LightLODTool.lightsInChildren[i];
				if (light.type != 3 && light.type != 1)
				{
					LightLOD lightLOD = light.gameObject.AddComponent<LightLOD>();
					lightLOD.targetLight = light;
				}
			}
		}

		private static List<Light> lightsInChildren = new List<Light>();
	}
}

using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class GrassDisplacement : MonoBehaviour
	{
		private void Update()
		{
			Shader.SetGlobalVector(this._Grass_Displacement_Point, new Vector4(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z, 0f));
		}

		private void OnEnable()
		{
			if (this._Grass_Displacement_Point == -1)
			{
				this._Grass_Displacement_Point = Shader.PropertyToID("_Grass_Displacement_Point");
			}
		}

		private int _Grass_Displacement_Point = -1;
	}
}

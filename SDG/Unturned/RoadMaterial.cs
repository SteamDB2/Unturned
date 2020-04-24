using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class RoadMaterial
	{
		public RoadMaterial(Texture2D texture)
		{
			this._material = new Material(Shader.Find("Standard/Diffuse"));
			this.material.name = "Road";
			this.material.mainTexture = texture;
			this.width = 4f;
			this.height = 1f;
			this.depth = 0.5f;
			this.offset = 0f;
			this.isConcrete = true;
		}

		public Material material
		{
			get
			{
				return this._material;
			}
		}

		private Material _material;

		public float width;

		public float height;

		public float depth;

		public float offset;

		public bool isConcrete;
	}
}

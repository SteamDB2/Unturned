using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	public sealed class MaterialFactory : IDisposable
	{
		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = 52
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		public void Dispose()
		{
			foreach (KeyValuePair<string, Material> keyValuePair in this.m_Materials)
			{
				Material value = keyValuePair.Value;
				GraphicsUtils.Destroy(value);
			}
			this.m_Materials.Clear();
		}

		private Dictionary<string, Material> m_Materials;
	}
}

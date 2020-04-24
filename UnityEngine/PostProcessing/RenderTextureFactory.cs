using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	public sealed class RenderTextureFactory : IDisposable
	{
		public RenderTextureFactory()
		{
			this.m_TemporaryRTs = new HashSet<RenderTexture>();
		}

		public RenderTexture Get(RenderTexture baseRenderTexture)
		{
			return this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, (!baseRenderTexture.sRGB) ? 1 : 2, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");
		}

		public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = 2, RenderTextureReadWrite rw = 0, FilterMode filterMode = 1, TextureWrapMode wrapMode = 1, string name = "FactoryTempTexture")
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format);
			temporary.filterMode = filterMode;
			temporary.wrapMode = wrapMode;
			temporary.name = name;
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		public void Release(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				throw new ArgumentException(string.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt));
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		public void ReleaseAll()
		{
			foreach (RenderTexture renderTexture in this.m_TemporaryRTs)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			this.m_TemporaryRTs.Clear();
		}

		public void Dispose()
		{
			this.ReleaseAll();
		}

		private HashSet<RenderTexture> m_TemporaryRTs;
	}
}

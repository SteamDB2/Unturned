using System;

namespace UnityEngine.PostProcessing
{
	public class PostProcessingContext
	{
		public bool interrupted { get; private set; }

		public void Interrupt()
		{
			this.interrupted = true;
		}

		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == 3;
			}
		}

		public bool isHdr
		{
			get
			{
				return this.camera.hdr;
			}
		}

		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		public PostProcessingProfile profile;

		public Camera camera;

		public MaterialFactory materialFactory;

		public RenderTextureFactory renderTextureFactory;
	}
}

using System;

namespace UnityEngine.PostProcessing
{
	public abstract class PostProcessingComponentBase
	{
		public virtual DepthTextureMode GetCameraFlags()
		{
			return 0;
		}

		public abstract bool active { get; }

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}

		public abstract PostProcessingModel GetModel();

		public PostProcessingContext context;
	}
}

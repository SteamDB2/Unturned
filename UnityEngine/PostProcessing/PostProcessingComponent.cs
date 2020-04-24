using System;

namespace UnityEngine.PostProcessing
{
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		public T model { get; internal set; }

		public void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}

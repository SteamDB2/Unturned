using System;

namespace UnityEngine.PostProcessing
{
	public sealed class TrackballAttribute : PropertyAttribute
	{
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		public readonly string method;
	}
}

using System;

namespace UnityEngine.PostProcessing
{
	public sealed class MinAttribute : PropertyAttribute
	{
		public MinAttribute(float min)
		{
			this.min = min;
		}

		public readonly float min;
	}
}

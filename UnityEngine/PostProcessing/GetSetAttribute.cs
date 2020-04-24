using System;

namespace UnityEngine.PostProcessing
{
	public sealed class GetSetAttribute : PropertyAttribute
	{
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		public readonly string name;

		public bool dirty;
	}
}

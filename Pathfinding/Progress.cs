using System;

namespace Pathfinding
{
	public struct Progress
	{
		public Progress(float p, string d)
		{
			this.progress = p;
			this.description = d;
		}

		public float progress;

		public string description;
	}
}

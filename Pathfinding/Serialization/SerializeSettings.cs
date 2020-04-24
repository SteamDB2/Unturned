using System;

namespace Pathfinding.Serialization
{
	public class SerializeSettings
	{
		public static SerializeSettings Settings
		{
			get
			{
				return new SerializeSettings
				{
					nodes = false
				};
			}
		}

		public static SerializeSettings All
		{
			get
			{
				return new SerializeSettings
				{
					nodes = true
				};
			}
		}

		public bool nodes = true;

		public bool prettyPrint;

		public bool editorSettings;
	}
}

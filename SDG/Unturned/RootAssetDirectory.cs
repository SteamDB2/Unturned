using System;

namespace SDG.Unturned
{
	public class RootAssetDirectory : AssetDirectory
	{
		public RootAssetDirectory(string newPath, string newName) : base(newName, null)
		{
			this.path = newPath;
		}

		public string path { get; protected set; }

		public override string getPath(string path)
		{
			return this.path + path;
		}
	}
}

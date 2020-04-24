using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class AssetDirectory
	{
		public AssetDirectory(string newName, AssetDirectory newParent)
		{
			this.name = newName;
			this.parent = newParent;
			this.assets = new List<Asset>();
			this.directories = new List<AssetDirectory>();
		}

		public string name { get; protected set; }

		public AssetDirectory parent { get; protected set; }

		public List<Asset> assets { get; protected set; }

		public List<AssetDirectory> directories { get; protected set; }

		public virtual string getPath(string path)
		{
			path = '/' + this.name + path;
			if (this.parent != null)
			{
				path = this.parent.getPath(path);
			}
			return path;
		}
	}
}

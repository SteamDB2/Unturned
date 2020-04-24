using System;

namespace SDG.Unturned
{
	public struct ScannedFileInfo
	{
		public ScannedFileInfo(string name, string assetPath, string dataPath, string bundlePath, bool dataUsePath, bool bundleUsePath, bool loadFromResources, AssetDirectory directory, EAssetOrigin origin)
		{
			this.name = name;
			this.assetPath = assetPath;
			this.dataPath = dataPath;
			this.bundlePath = bundlePath;
			this.dataUsePath = dataUsePath;
			this.bundleUsePath = bundleUsePath;
			this.loadFromResources = loadFromResources;
			this.directory = directory;
			this.origin = origin;
		}

		public string name;

		public string assetPath;

		public string dataPath;

		public string bundlePath;

		public bool dataUsePath;

		public bool bundleUsePath;

		public bool loadFromResources;

		public AssetDirectory directory;

		public EAssetOrigin origin;
	}
}

using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class RootContentDirectory : ContentDirectory
	{
		public RootContentDirectory(AssetBundle newAssetBundle, string newName) : base(newName, null)
		{
			this.assetBundle = newAssetBundle;
			string[] allAssetNames = this.assetBundle.GetAllAssetNames();
			for (int i = 0; i < allAssetNames.Length; i++)
			{
				ContentDirectory contentDirectory = this;
				string[] array = allAssetNames[i].Split(new char[]
				{
					'/'
				});
				for (int j = 0; j < array.Length; j++)
				{
					if (j == array.Length - 1)
					{
						string newFile = array[j];
						contentDirectory.files.Add(new ContentFile(this, contentDirectory, allAssetNames[i], newFile));
					}
					else
					{
						string text = array[j];
						ContentDirectory contentDirectory2;
						if (!contentDirectory.directories.TryGetValue(text, out contentDirectory2))
						{
							contentDirectory2 = new ContentDirectory(text, contentDirectory);
							contentDirectory.directories.Add(text, contentDirectory2);
						}
						contentDirectory = contentDirectory2;
					}
				}
			}
		}

		public AssetBundle assetBundle { get; protected set; }
	}
}

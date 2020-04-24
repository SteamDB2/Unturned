using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class Bundle
	{
		public Bundle(string path) : this(path, true, false)
		{
		}

		public Bundle(string path, bool usePath, bool loadFromResources)
		{
			this.loadFromResources = loadFromResources;
			if (!loadFromResources && ReadWrite.fileExists(path, false, usePath))
			{
				this.asset = AssetBundle.LoadFromFile((!usePath) ? path : (ReadWrite.PATH + path));
			}
			else
			{
				this.asset = null;
			}
			this.name = ReadWrite.fileName(path);
			if (this.asset == null)
			{
				this.resource = ReadWrite.folderPath(path).Substring(1);
			}
		}

		public Bundle()
		{
			this.asset = null;
			this.name = "#NAME";
		}

		public AssetBundle asset { get; protected set; }

		public string resource { get; protected set; }

		public string name { get; protected set; }

		public bool hasResource
		{
			get
			{
				return this.asset == null;
			}
		}

		public Object load(string name)
		{
			if (!(this.asset != null))
			{
				return Resources.Load(this.resource + "/" + name);
			}
			if (this.asset.Contains(name))
			{
				Object @object = this.asset.LoadAsset(name);
				if (this.convertShadersToStandard && @object.GetType() == typeof(GameObject))
				{
					if (Bundle.shader == null)
					{
						Bundle.shader = Shader.Find("Standard");
					}
					Bundle.renderers.Clear();
					((GameObject)@object).GetComponentsInChildren<Renderer>(true, Bundle.renderers);
					for (int i = 0; i < Bundle.renderers.Count; i++)
					{
						Renderer renderer = Bundle.renderers[i];
						if (!(renderer == null))
						{
							Material sharedMaterial = renderer.sharedMaterial;
							if (!(sharedMaterial == null))
							{
								sharedMaterial.shader = Bundle.shader;
							}
						}
					}
				}
				return @object;
			}
			return null;
		}

		public Object[] load()
		{
			if (this.asset != null)
			{
				return this.asset.LoadAllAssets();
			}
			return null;
		}

		public Object[] load(Type type)
		{
			if (this.asset != null)
			{
				return this.asset.LoadAllAssets(type);
			}
			return null;
		}

		public void unload()
		{
			if (this.asset != null)
			{
				this.asset.Unload(false);
			}
		}

		private static Shader shader;

		private static List<Renderer> renderers = new List<Renderer>();

		public bool convertShadersToStandard;

		public bool loadFromResources;
	}
}

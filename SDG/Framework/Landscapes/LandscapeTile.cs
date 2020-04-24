using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.Foliage;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public class LandscapeTile : IFormattedFileReadable, IFormattedFileWritable, IFoliageSurface
	{
		public LandscapeTile(LandscapeCoord newCoord)
		{
			this.gameObject = new GameObject();
			this.gameObject.name = "Tile";
			this.gameObject.tag = "Ground";
			this.gameObject.layer = LayerMasks.GROUND;
			this.gameObject.transform.parent = Landscape.instance.transform;
			this.gameObject.transform.rotation = MathUtility.IDENTITY_QUATERNION;
			this.gameObject.transform.localScale = Vector3.one;
			this.coord = newCoord;
			this.sourceHeightmap = new float[Landscape.HEIGHTMAP_RESOLUTION, Landscape.HEIGHTMAP_RESOLUTION];
			this.sourceSplatmap = new float[Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_LAYERS];
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					this.sourceHeightmap[i, j] = 0.5f;
				}
			}
			for (int k = 0; k < Landscape.SPLATMAP_RESOLUTION; k++)
			{
				for (int l = 0; l < Landscape.SPLATMAP_RESOLUTION; l++)
				{
					this.sourceSplatmap[k, l, 0] = 1f;
				}
			}
			this.materials = new InspectableList<AssetReference<LandscapeMaterialAsset>>(Landscape.SPLATMAP_LAYERS);
			for (int m = 0; m < Landscape.SPLATMAP_LAYERS; m++)
			{
				this.materials.Add(AssetReference<LandscapeMaterialAsset>.invalid);
			}
			this.materials.canInspectorAdd = false;
			this.materials.canInspectorRemove = false;
			this.materials.inspectorChanged += this.handleMaterialsInspectorChanged;
			this.prototypes = new SplatPrototype[Landscape.SPLATMAP_LAYERS];
			for (int n = 0; n < this.prototypes.Length; n++)
			{
				SplatPrototype splatPrototype = new SplatPrototype();
				splatPrototype.texture = Texture2D.blackTexture;
				this.prototypes[n] = splatPrototype;
			}
			this.data = new TerrainData();
			this.data.splatPrototypes = this.prototypes;
			this.data.heightmapResolution = Landscape.HEIGHTMAP_RESOLUTION;
			this.data.alphamapResolution = Landscape.SPLATMAP_RESOLUTION;
			this.data.baseMapResolution = Landscape.BASEMAP_RESOLUTION;
			this.data.size = new Vector3(Landscape.TILE_SIZE, Landscape.TILE_HEIGHT, Landscape.TILE_SIZE);
			this.data.SetHeightsDelayLOD(0, 0, this.sourceHeightmap);
			this.data.SetAlphamaps(0, 0, this.sourceSplatmap);
			this.data.wavingGrassTint = Color.white;
			this.terrain = this.gameObject.AddComponent<Terrain>();
			this.terrain.terrainData = this.data;
			this.terrain.heightmapPixelError = 200f;
			this.terrain.materialType = 3;
			this.terrain.reflectionProbeUsage = 0;
			this.terrain.castShadows = false;
			this.terrain.drawHeightmap = !Dedicator.isDedicated;
			this.terrain.drawTreesAndFoliage = false;
			this.terrain.collectDetailPatches = false;
			this.terrain.Flush();
			this.collider = this.gameObject.AddComponent<TerrainCollider>();
			this.collider.terrainData = this.data;
		}

		public GameObject gameObject { get; protected set; }

		public LandscapeCoord coord
		{
			get
			{
				return this._coord;
			}
			protected set
			{
				this._coord = value;
				this.updateTransform();
			}
		}

		public Bounds localBounds
		{
			get
			{
				return new Bounds(new Vector3(Landscape.TILE_SIZE / 2f, 0f, Landscape.TILE_SIZE / 2f), new Vector3(Landscape.TILE_SIZE, Landscape.TILE_HEIGHT, Landscape.TILE_SIZE));
			}
		}

		public Bounds worldBounds
		{
			get
			{
				Bounds localBounds = this.localBounds;
				localBounds.center += new Vector3((float)this.coord.x * Landscape.TILE_SIZE, 0f, (float)this.coord.y * Landscape.TILE_SIZE);
				return localBounds;
			}
		}

		[Inspectable("#SDG::Tile.Materials", null)]
		public InspectableList<AssetReference<LandscapeMaterialAsset>> materials { get; protected set; }

		public SplatPrototype[] prototypes { get; protected set; }

		public TerrainData data { get; protected set; }

		public Terrain terrain { get; protected set; }

		public TerrainCollider collider { get; protected set; }

		public virtual void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.coord = reader.readValue<LandscapeCoord>("Coord");
			int num = reader.readArrayLength("Materials");
			for (int i = 0; i < num; i++)
			{
				this.materials[i] = reader.readValue<AssetReference<LandscapeMaterialAsset>>(i);
			}
			this.updatePrototypes();
			this.readHeightmaps();
			this.readSplatmaps();
		}

		public virtual void readHeightmaps()
		{
			this.readHeightmap("_Source", this.sourceHeightmap);
			this.data.SetHeightsDelayLOD(0, 0, this.sourceHeightmap);
		}

		protected virtual void readHeightmap(string suffix, float[,] heightmap)
		{
			string path = string.Concat(new object[]
			{
				Level.info.path,
				"/Landscape/Heightmaps/Tile_",
				this.coord.x,
				'_',
				this.coord.y,
				suffix,
				".heightmap"
			});
			if (!File.Exists(path))
			{
				return;
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
				{
					for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
					{
						ushort num = (ushort)(fileStream.ReadByte() << 8 | fileStream.ReadByte());
						float num2 = (float)num / 65535f;
						heightmap[i, j] = num2;
					}
				}
			}
		}

		public virtual void readSplatmaps()
		{
			this.readSplatmap("_Source", this.sourceSplatmap);
			this.data.SetAlphamaps(0, 0, this.sourceSplatmap);
		}

		protected virtual void readSplatmap(string suffix, float[,,] splatmap)
		{
			string path = string.Concat(new object[]
			{
				Level.info.path,
				"/Landscape/Splatmaps/Tile_",
				this.coord.x,
				'_',
				this.coord.y,
				suffix,
				".splatmap"
			});
			if (!File.Exists(path))
			{
				return;
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
				{
					for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
					{
						for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
						{
							byte b = (byte)fileStream.ReadByte();
							float num = (float)b / 255f;
							splatmap[i, j, k] = num;
						}
					}
				}
			}
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<LandscapeCoord>("Coord", this.coord);
			writer.beginArray("Materials");
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				writer.writeValue<AssetReference<LandscapeMaterialAsset>>(this.materials[i]);
			}
			writer.endArray();
			writer.endObject();
			this.writeHeightmaps();
			this.writeSplatmaps();
		}

		public virtual void writeHeightmaps()
		{
			this.writeHeightmap("_Source", this.sourceHeightmap);
		}

		protected virtual void writeHeightmap(string suffix, float[,] heightmap)
		{
			string path = string.Concat(new object[]
			{
				Level.info.path,
				"/Landscape/Heightmaps/Tile_",
				this.coord.x,
				'_',
				this.coord.y,
				suffix,
				".heightmap"
			});
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
			{
				for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
				{
					for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
					{
						float num = heightmap[i, j];
						ushort num2 = (ushort)Mathf.RoundToInt(num * 65535f);
						fileStream.WriteByte((byte)(num2 >> 8 & 255));
						fileStream.WriteByte((byte)(num2 & 255));
					}
				}
			}
		}

		public virtual void writeSplatmaps()
		{
			this.writeSplatmap("_Source", this.sourceSplatmap);
		}

		protected virtual void writeSplatmap(string suffix, float[,,] splatmap)
		{
			string path = string.Concat(new object[]
			{
				Level.info.path,
				"/Landscape/Splatmaps/Tile_",
				this.coord.x,
				'_',
				this.coord.y,
				suffix,
				".splatmap"
			});
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
			{
				for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
				{
					for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
					{
						for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
						{
							float num = splatmap[i, j, k];
							byte value = (byte)Mathf.RoundToInt(num * 255f);
							fileStream.WriteByte(value);
						}
					}
				}
			}
		}

		public void updatePrototypes()
		{
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				LandscapeMaterialAsset landscapeMaterialAsset = Assets.find<LandscapeMaterialAsset>(this.materials[i]);
				if (landscapeMaterialAsset == null)
				{
					this.prototypes[i].texture = Texture2D.blackTexture;
					this.prototypes[i].normalMap = Texture2D.blackTexture;
				}
				else
				{
					this.prototypes[i].texture = Assets.load<Texture2D>(landscapeMaterialAsset.texture);
					if (this.prototypes[i].texture == null)
					{
						this.prototypes[i].texture = Texture2D.blackTexture;
					}
					this.prototypes[i].normalMap = Assets.load<Texture2D>(landscapeMaterialAsset.mask);
					if (this.prototypes[i].normalMap == null)
					{
						this.prototypes[i].normalMap = Texture2D.blackTexture;
					}
				}
			}
			this.data.splatPrototypes = this.prototypes;
		}

		protected void updateTransform()
		{
			this.gameObject.transform.position = new Vector3((float)this.coord.x * Landscape.TILE_SIZE, -Landscape.TILE_HEIGHT / 2f, (float)this.coord.y * Landscape.TILE_SIZE);
		}

		public void convertLegacyHeightmap()
		{
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					HeightmapCoord heightmapCoord = new HeightmapCoord(i, j);
					Vector3 worldPosition = Landscape.getWorldPosition(this.coord, heightmapCoord, this.sourceHeightmap[i, j]);
					float num = LevelGround.getConversionHeight(worldPosition);
					num /= Landscape.TILE_HEIGHT;
					num += 0.5f;
					this.sourceHeightmap[i, j] = num;
				}
			}
			this.data.SetHeights(0, 0, this.sourceHeightmap);
		}

		public void convertLegacySplatmap()
		{
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					SplatmapCoord splatmapCoord = new SplatmapCoord(i, j);
					Vector3 worldPosition = Landscape.getWorldPosition(this.coord, splatmapCoord);
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						float conversionWeight = LevelGround.getConversionWeight(worldPosition, k, true);
						this.sourceSplatmap[i, j, k] = conversionWeight;
					}
				}
			}
			this.data.SetAlphamaps(0, 0, this.sourceSplatmap);
		}

		public void resetHeightmap()
		{
			for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					this.sourceHeightmap[i, j] = 0.5f;
				}
			}
			Landscape.reconcileNeighbors(this);
			this.terrain.ApplyDelayedHeightmapModification();
		}

		public void resetSplatmap()
		{
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					this.sourceSplatmap[i, j, 0] = 1f;
					for (int k = 1; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						this.sourceSplatmap[i, j, k] = 0f;
					}
				}
			}
			this.data.SetAlphamaps(0, 0, this.sourceSplatmap);
		}

		public void normalizeSplatmap()
		{
			for (int i = 0; i < Landscape.SPLATMAP_RESOLUTION; i++)
			{
				for (int j = 0; j < Landscape.SPLATMAP_RESOLUTION; j++)
				{
					float num = 0f;
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						num += this.sourceSplatmap[i, j, k];
					}
					for (int l = 0; l < Landscape.SPLATMAP_LAYERS; l++)
					{
						this.sourceSplatmap[i, j, l] /= num;
					}
				}
			}
			this.data.SetAlphamaps(0, 0, this.sourceSplatmap);
		}

		public void applyGraphicsSettings()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (GraphicsSettings.blend)
			{
				ERenderMode renderMode = GraphicsSettings.renderMode;
				if (renderMode != ERenderMode.FORWARD)
				{
					if (renderMode != ERenderMode.DEFERRED)
					{
						this.terrain.materialTemplate = null;
						Debug.LogError("Unknown render mode: " + GraphicsSettings.renderMode);
					}
					else
					{
						this.terrain.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Deferred");
					}
				}
				else
				{
					this.terrain.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Forward");
				}
				this.terrain.basemapDistance = 512f;
			}
			else
			{
				this.terrain.materialTemplate = Resources.Load<Material>("Materials/Landscapes/Landscape_Classic");
				this.terrain.basemapDistance = 256f;
			}
			switch (GraphicsSettings.terrainQuality)
			{
			case EGraphicQuality.LOW:
				this.terrain.heightmapPixelError = 64f;
				break;
			case EGraphicQuality.MEDIUM:
				this.terrain.heightmapPixelError = 32f;
				break;
			case EGraphicQuality.HIGH:
				this.terrain.heightmapPixelError = 16f;
				break;
			case EGraphicQuality.ULTRA:
				this.terrain.heightmapPixelError = 8f;
				break;
			}
		}

		public FoliageBounds getFoliageSurfaceBounds()
		{
			return new FoliageBounds(new FoliageCoord(this.coord.x * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT, this.coord.y * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT), new FoliageCoord((this.coord.x + 1) * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT - 1, (this.coord.y + 1) * Landscape.TILE_SIZE_INT / FoliageSystem.TILE_SIZE_INT - 1));
		}

		public bool getFoliageSurfaceInfo(Vector3 position, out Vector3 surfacePosition, out Vector3 surfaceNormal)
		{
			surfacePosition = position;
			surfacePosition.y = this.terrain.SampleHeight(position) - Landscape.TILE_HEIGHT / 2f;
			surfaceNormal = this.data.GetInterpolatedNormal((position.x - (float)this.coord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE, (position.z - (float)this.coord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE);
			return !LandscapeHoleUtility.isPointInsideHoleVolume(surfacePosition);
		}

		public void bakeFoliageSurface(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			int num = (foliageTile.coord.y * FoliageSystem.TILE_SIZE_INT - this.coord.y * Landscape.TILE_SIZE_INT) / FoliageSystem.TILE_SIZE_INT * FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			int num2 = num + FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			int num3 = (foliageTile.coord.x * FoliageSystem.TILE_SIZE_INT - this.coord.x * Landscape.TILE_SIZE_INT) / FoliageSystem.TILE_SIZE_INT * FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			int num4 = num3 + FoliageSystem.SPLATMAP_RESOLUTION_PER_TILE;
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					SplatmapCoord splatmapCoord = new SplatmapCoord(i, j);
					float num5 = (float)this.coord.x * Landscape.TILE_SIZE + (float)splatmapCoord.y * Landscape.SPLATMAP_WORLD_UNIT;
					float num6 = (float)this.coord.y * Landscape.TILE_SIZE + (float)splatmapCoord.x * Landscape.SPLATMAP_WORLD_UNIT;
					Bounds bounds = default(Bounds);
					bounds.min = new Vector3(num5, 0f, num6);
					bounds.max = new Vector3(num5 + Landscape.SPLATMAP_WORLD_UNIT, 0f, num6 + Landscape.SPLATMAP_WORLD_UNIT);
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						float num7 = this.sourceSplatmap[i, j, k];
						if (num7 >= 0.01f)
						{
							LandscapeMaterialAsset landscapeMaterialAsset = Assets.find<LandscapeMaterialAsset>(this.materials[k]);
							if (landscapeMaterialAsset != null)
							{
								FoliageInfoCollectionAsset foliageInfoCollectionAsset = Assets.find<FoliageInfoCollectionAsset>(landscapeMaterialAsset.foliage);
								if (foliageInfoCollectionAsset != null)
								{
									foliageInfoCollectionAsset.bakeFoliage(bakeSettings, this, bounds, num7);
								}
							}
						}
					}
				}
			}
		}

		protected virtual void handleMaterialsInspectorChanged(IInspectableList list)
		{
			this.updatePrototypes();
		}

		public virtual void enable()
		{
			FoliageSystem.addSurface(this);
		}

		public virtual void disable()
		{
			FoliageSystem.removeSurface(this);
		}

		protected LandscapeCoord _coord;

		public float[,] sourceHeightmap;

		public float[,,] sourceSplatmap;
	}
}

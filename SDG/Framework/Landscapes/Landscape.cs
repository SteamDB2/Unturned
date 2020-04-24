using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public class Landscape : DevkitHierarchyItemBase, IDevkitHierarchyAutoSpawnable, IDevkitHierarchySpawnable
	{
		public static Landscape instance { get; protected set; }

		public static event LandscapeLoadedHandler loaded;

		public static bool getWorldHeight(Vector3 position, out float height)
		{
			LandscapeCoord coord = new LandscapeCoord(position);
			LandscapeTile tile = Landscape.getTile(coord);
			if (tile != null)
			{
				height = tile.terrain.SampleHeight(position) - Landscape.TILE_HEIGHT / 2f;
				return true;
			}
			height = 0f;
			return false;
		}

		public static bool getWorldHeight(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, out float height)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile != null)
			{
				height = tile.sourceHeightmap[heightmapCoord.x, heightmapCoord.y] * Landscape.TILE_HEIGHT - Landscape.TILE_HEIGHT / 2f;
				return true;
			}
			height = 0f;
			return false;
		}

		public static bool getHeight01(Vector3 position, out float height)
		{
			LandscapeCoord coord = new LandscapeCoord(position);
			LandscapeTile tile = Landscape.getTile(coord);
			if (tile != null)
			{
				height = tile.terrain.SampleHeight(position) / Landscape.TILE_HEIGHT;
				return true;
			}
			height = 0f;
			return false;
		}

		public static bool getHeight01(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, out float height)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile != null)
			{
				height = tile.sourceHeightmap[heightmapCoord.x, heightmapCoord.y];
				return true;
			}
			height = 0f;
			return false;
		}

		public static bool getNormal(Vector3 position, out Vector3 normal)
		{
			LandscapeCoord coord = new LandscapeCoord(position);
			LandscapeTile tile = Landscape.getTile(coord);
			if (tile != null)
			{
				normal = tile.data.GetInterpolatedNormal((position.x - (float)coord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE, (position.z - (float)coord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE);
				return true;
			}
			normal = Vector3.up;
			return false;
		}

		public static bool getSplatmapMaterial(Vector3 position, out AssetReference<LandscapeMaterialAsset> materialAsset)
		{
			LandscapeCoord tileCoord = new LandscapeCoord(position);
			SplatmapCoord splatmapCoord = new SplatmapCoord(tileCoord, position);
			return Landscape.getSplatmapMaterial(tileCoord, splatmapCoord, out materialAsset);
		}

		public static bool getSplatmapMaterial(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, out AssetReference<LandscapeMaterialAsset> materialAsset)
		{
			int index;
			if (Landscape.getSplatmapLayer(tileCoord, splatmapCoord, out index))
			{
				materialAsset = Landscape.getTile(tileCoord).materials[index];
				return true;
			}
			materialAsset = AssetReference<LandscapeMaterialAsset>.invalid;
			return false;
		}

		public static bool getSplatmapLayer(Vector3 position, out int layer)
		{
			LandscapeCoord tileCoord = new LandscapeCoord(position);
			SplatmapCoord splatmapCoord = new SplatmapCoord(tileCoord, position);
			return Landscape.getSplatmapLayer(tileCoord, splatmapCoord, out layer);
		}

		public static bool getSplatmapLayer(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, out int layer)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile != null)
			{
				layer = Landscape.getSplatmapHighestWeightLayerIndex(splatmapCoord, tile.sourceSplatmap, -1);
				return true;
			}
			layer = -1;
			return false;
		}

		public static int getSplatmapHighestWeightLayerIndex(SplatmapCoord splatmapCoord, float[,,] currentWeights, int ignoreLayer = -1)
		{
			float num = -1f;
			int result = -1;
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				if (i != ignoreLayer)
				{
					if (currentWeights[splatmapCoord.x, splatmapCoord.y, i] > num)
					{
						num = currentWeights[splatmapCoord.x, splatmapCoord.y, i];
						result = i;
					}
				}
			}
			return result;
		}

		public static int getSplatmapHighestWeightLayerIndex(float[] currentWeights, int ignoreLayer = -1)
		{
			float num = -1f;
			int result = -1;
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				if (i != ignoreLayer)
				{
					if (currentWeights[i] > num)
					{
						num = currentWeights[i];
						result = i;
					}
				}
			}
			return result;
		}

		public static void clearHeightmapTransactions()
		{
			Landscape.heightmapTransactions.Clear();
		}

		public static void clearSplatmapTransactions()
		{
			Landscape.splatmapTransactions.Clear();
		}

		public static bool isPointerInTile(Vector3 worldPosition)
		{
			return Landscape.getTile(worldPosition) != null;
		}

		public static Vector3 getWorldPosition(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, float height)
		{
			float num = (float)tileCoord.x * Landscape.TILE_SIZE + (float)heightmapCoord.y / (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE * Landscape.TILE_SIZE;
			num = (float)Mathf.RoundToInt(num);
			float num2 = -Landscape.TILE_HEIGHT / 2f + height * Landscape.TILE_HEIGHT;
			float num3 = (float)tileCoord.y * Landscape.TILE_SIZE + (float)heightmapCoord.x / (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE * Landscape.TILE_SIZE;
			num3 = (float)Mathf.RoundToInt(num3);
			return new Vector3(num, num2, num3);
		}

		public static Vector3 getWorldPosition(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord)
		{
			float num = (float)tileCoord.x * Landscape.TILE_SIZE + (float)splatmapCoord.y / (float)Landscape.SPLATMAP_RESOLUTION * Landscape.TILE_SIZE;
			num = (float)Mathf.RoundToInt(num) + Landscape.HALF_SPLATMAP_WORLD_UNIT;
			float num2 = (float)tileCoord.y * Landscape.TILE_SIZE + (float)splatmapCoord.x / (float)Landscape.SPLATMAP_RESOLUTION * Landscape.TILE_SIZE;
			num2 = (float)Mathf.RoundToInt(num2) + Landscape.HALF_SPLATMAP_WORLD_UNIT;
			Vector3 vector;
			vector..ctor(num, 0f, num2);
			float y;
			Landscape.getWorldHeight(vector, out y);
			vector.y = y;
			return vector;
		}

		public static void readHeightmap(Bounds worldBounds, Landscape.LandscapeReadHeightmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						HeightmapBounds heightmapBounds = new HeightmapBounds(landscapeCoord, worldBounds);
						for (int k = heightmapBounds.min.x; k < heightmapBounds.max.x; k++)
						{
							for (int l = heightmapBounds.min.y; l < heightmapBounds.max.y; l++)
							{
								HeightmapCoord heightmapCoord = new HeightmapCoord(k, l);
								float num = tile.sourceHeightmap[k, l];
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, heightmapCoord, num);
								callback(landscapeCoord, heightmapCoord, worldPosition, num);
							}
						}
					}
				}
			}
		}

		public static void readSplatmap(Bounds worldBounds, Landscape.LandscapeReadSplatmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
						for (int k = splatmapBounds.min.x; k < splatmapBounds.max.x; k++)
						{
							for (int l = splatmapBounds.min.y; l < splatmapBounds.max.y; l++)
							{
								SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
								for (int m = 0; m < Landscape.SPLATMAP_LAYERS; m++)
								{
									Landscape.SPLATMAP_LAYER_BUFFER[m] = tile.sourceSplatmap[k, l, m];
								}
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
								callback(landscapeCoord, splatmapCoord, worldPosition, Landscape.SPLATMAP_LAYER_BUFFER);
							}
						}
					}
				}
			}
		}

		public static void writeHeightmap(Bounds worldBounds, Landscape.LandscapeWriteHeightmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						if (!Landscape.heightmapTransactions.ContainsKey(landscapeCoord))
						{
							LandscapeHeightmapTransaction landscapeHeightmapTransaction = new LandscapeHeightmapTransaction(tile);
							DevkitTransactionManager.recordTransaction(landscapeHeightmapTransaction);
							Landscape.heightmapTransactions.Add(landscapeCoord, landscapeHeightmapTransaction);
						}
						HeightmapBounds heightmapBounds = new HeightmapBounds(landscapeCoord, worldBounds);
						for (int k = heightmapBounds.min.x; k <= heightmapBounds.max.x; k++)
						{
							for (int l = heightmapBounds.min.y; l <= heightmapBounds.max.y; l++)
							{
								HeightmapCoord heightmapCoord = new HeightmapCoord(k, l);
								float num = tile.sourceHeightmap[k, l];
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, heightmapCoord, num);
								tile.sourceHeightmap[k, l] = Mathf.Clamp01(callback(landscapeCoord, heightmapCoord, worldPosition, num));
							}
						}
						tile.data.SetHeightsDelayLOD(0, 0, tile.sourceHeightmap);
					}
				}
			}
		}

		public static void writeSplatmap(Bounds worldBounds, Landscape.LandscapeWriteSplatmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						if (!Landscape.splatmapTransactions.ContainsKey(landscapeCoord))
						{
							LandscapeSplatmapTransaction landscapeSplatmapTransaction = new LandscapeSplatmapTransaction(tile);
							DevkitTransactionManager.recordTransaction(landscapeSplatmapTransaction);
							Landscape.splatmapTransactions.Add(landscapeCoord, landscapeSplatmapTransaction);
						}
						SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
						for (int k = splatmapBounds.min.x; k <= splatmapBounds.max.x; k++)
						{
							for (int l = splatmapBounds.min.y; l <= splatmapBounds.max.y; l++)
							{
								SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
								for (int m = 0; m < Landscape.SPLATMAP_LAYERS; m++)
								{
									Landscape.SPLATMAP_LAYER_BUFFER[m] = tile.sourceSplatmap[k, l, m];
								}
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
								callback(landscapeCoord, splatmapCoord, worldPosition, Landscape.SPLATMAP_LAYER_BUFFER);
								for (int n = 0; n < Landscape.SPLATMAP_LAYERS; n++)
								{
									tile.sourceSplatmap[k, l, n] = Mathf.Clamp01(Landscape.SPLATMAP_LAYER_BUFFER[n]);
								}
							}
						}
						tile.data.SetAlphamaps(0, 0, tile.sourceSplatmap);
					}
				}
			}
		}

		public static void getHeightmapVertices(Bounds worldBounds, Landscape.LandscapeGetHeightmapVerticesHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						HeightmapBounds heightmapBounds = new HeightmapBounds(landscapeCoord, worldBounds);
						for (int k = heightmapBounds.min.x; k <= heightmapBounds.max.x; k++)
						{
							for (int l = heightmapBounds.min.y; l <= heightmapBounds.max.y; l++)
							{
								HeightmapCoord heightmapCoord = new HeightmapCoord(k, l);
								float height = tile.sourceHeightmap[k, l];
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, heightmapCoord, height);
								callback(landscapeCoord, heightmapCoord, worldPosition);
							}
						}
					}
				}
			}
		}

		public static void getSplatmapVertices(Bounds worldBounds, Landscape.LandscapeGetSplatmapVerticesHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					if (Landscape.getTile(landscapeCoord) != null)
					{
						SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
						for (int k = splatmapBounds.min.x; k <= splatmapBounds.max.x; k++)
						{
							for (int l = splatmapBounds.min.y; l <= splatmapBounds.max.y; l++)
							{
								SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
								callback(landscapeCoord, splatmapCoord, worldPosition);
							}
						}
					}
				}
			}
		}

		public static void applyLOD()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				value.terrain.ApplyDelayedHeightmapModification();
			}
		}

		public static void linkNeighbors()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				LandscapeTile tile = Landscape.getTile(new LandscapeCoord(value.coord.x - 1, value.coord.y));
				LandscapeTile tile2 = Landscape.getTile(new LandscapeCoord(value.coord.x, value.coord.y + 1));
				LandscapeTile tile3 = Landscape.getTile(new LandscapeCoord(value.coord.x + 1, value.coord.y));
				LandscapeTile tile4 = Landscape.getTile(new LandscapeCoord(value.coord.x, value.coord.y - 1));
				Terrain terrain = (tile != null) ? tile.terrain : null;
				Terrain terrain2 = (tile2 != null) ? tile2.terrain : null;
				Terrain terrain3 = (tile3 != null) ? tile3.terrain : null;
				Terrain terrain4 = (tile4 != null) ? tile4.terrain : null;
				value.terrain.SetNeighbors(terrain, terrain2, terrain3, terrain4);
				value.terrain.Flush();
			}
		}

		public static void reconcileNeighbors(LandscapeTile tile)
		{
			LandscapeTile tile2 = Landscape.getTile(new LandscapeCoord(tile.coord.x - 1, tile.coord.y));
			if (tile2 != null)
			{
				for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
				{
					tile.sourceHeightmap[i, 0] = tile2.sourceHeightmap[i, Landscape.HEIGHTMAP_RESOLUTION - 1];
				}
			}
			LandscapeTile tile3 = Landscape.getTile(new LandscapeCoord(tile.coord.x, tile.coord.y - 1));
			if (tile3 != null)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					tile.sourceHeightmap[0, j] = tile3.sourceHeightmap[Landscape.HEIGHTMAP_RESOLUTION - 1, j];
				}
			}
			LandscapeTile tile4 = Landscape.getTile(new LandscapeCoord(tile.coord.x + 1, tile.coord.y));
			if (tile4 != null)
			{
				for (int k = 0; k < Landscape.HEIGHTMAP_RESOLUTION; k++)
				{
					tile.sourceHeightmap[k, Landscape.HEIGHTMAP_RESOLUTION - 1] = tile4.sourceHeightmap[k, 0];
				}
			}
			LandscapeTile tile5 = Landscape.getTile(new LandscapeCoord(tile.coord.x, tile.coord.y + 1));
			if (tile5 != null)
			{
				for (int l = 0; l < Landscape.HEIGHTMAP_RESOLUTION; l++)
				{
					tile.sourceHeightmap[Landscape.HEIGHTMAP_RESOLUTION - 1, l] = tile5.sourceHeightmap[0, l];
				}
			}
			tile.data.SetHeightsDelayLOD(0, 0, tile.sourceHeightmap);
		}

		public static LandscapeTile addTile(LandscapeCoord coord)
		{
			if (Landscape.instance == null)
			{
				return null;
			}
			if (Landscape.tiles.ContainsKey(coord))
			{
				return null;
			}
			LandscapeTile landscapeTile = new LandscapeTile(coord);
			landscapeTile.enable();
			landscapeTile.applyGraphicsSettings();
			Landscape.tiles.Add(coord, landscapeTile);
			return landscapeTile;
		}

		protected static void clearTiles()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				value.disable();
			}
			Landscape.tiles.Clear();
		}

		public static LandscapeTile getOrAddTile(Vector3 worldPosition)
		{
			LandscapeCoord coord = new LandscapeCoord(worldPosition);
			return Landscape.getOrAddTile(coord);
		}

		public static LandscapeTile getTile(Vector3 worldPosition)
		{
			LandscapeCoord coord = new LandscapeCoord(worldPosition);
			return Landscape.getTile(coord);
		}

		public static LandscapeTile getOrAddTile(LandscapeCoord coord)
		{
			LandscapeTile result;
			if (!Landscape.tiles.TryGetValue(coord, out result))
			{
				result = Landscape.addTile(coord);
			}
			return result;
		}

		public static LandscapeTile getTile(LandscapeCoord coord)
		{
			LandscapeTile result;
			Landscape.tiles.TryGetValue(coord, out result);
			return result;
		}

		public static bool removeTile(LandscapeCoord coord)
		{
			LandscapeTile landscapeTile;
			if (!Landscape.tiles.TryGetValue(coord, out landscapeTile))
			{
				return false;
			}
			landscapeTile.disable();
			Object.Destroy(landscapeTile.gameObject);
			Landscape.tiles.Remove(coord);
			return true;
		}

		public void devkitHierarchySpawn()
		{
			Landscape.addTile(new LandscapeCoord(0, 0));
			Landscape.linkNeighbors();
		}

		public override void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			int num = reader.readArrayLength("Tiles");
			for (int i = 0; i < num; i++)
			{
				reader.readArrayIndex(i);
				LandscapeTile landscapeTile = new LandscapeTile(LandscapeCoord.ZERO);
				landscapeTile.enable();
				landscapeTile.applyGraphicsSettings();
				landscapeTile.read(reader);
				if (Landscape.tiles.ContainsKey(landscapeTile.coord))
				{
					Debug.LogError("Duplicate landscape coord read: " + landscapeTile.coord);
				}
				else
				{
					Landscape.tiles.Add(landscapeTile.coord, landscapeTile);
				}
			}
			Landscape.linkNeighbors();
			Landscape.applyLOD();
		}

		public override void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.beginArray("Tiles");
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				writer.writeValue<LandscapeTile>(value);
			}
			writer.endArray();
			writer.endObject();
		}

		protected void triggerLandscapeLoaded()
		{
			if (Landscape.loaded != null)
			{
				Landscape.loaded();
			}
		}

		protected void handleGraphicsSettingsApplied()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				value.applyGraphicsSettings();
			}
		}

		protected void handlePlanarReflectionPreRender()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				value.terrain.basemapDistance = 0f;
			}
		}

		protected void handlePlanarReflectionPostRender()
		{
			float basemapDistance = (float)((!GraphicsSettings.blend) ? 256 : 512);
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				value.terrain.basemapDistance = basemapDistance;
			}
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
		}

		protected void OnDisable()
		{
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Landscape";
			base.gameObject.layer = LayerMasks.GROUND;
			if (Landscape.instance == null)
			{
				Landscape.instance = this;
				Landscape.clearTiles();
				if (Level.isEditor)
				{
					LandscapeHeightmapCopyPool.warmup(DevkitTransactionManager.historyLength);
					LandscapeSplatmapCopyPool.warmup(DevkitTransactionManager.historyLength);
				}
				GraphicsSettings.graphicsSettingsApplied += this.handleGraphicsSettingsApplied;
				PlanarReflection.preRender += this.handlePlanarReflectionPreRender;
				PlanarReflection.postRender += this.handlePlanarReflectionPostRender;
			}
		}

		protected void Start()
		{
			if (Landscape.instance == this)
			{
				this.triggerLandscapeLoaded();
			}
		}

		protected void OnDestroy()
		{
			if (Landscape.instance == this)
			{
				GraphicsSettings.graphicsSettingsApplied -= this.handleGraphicsSettingsApplied;
				PlanarReflection.preRender -= this.handlePlanarReflectionPreRender;
				PlanarReflection.postRender -= this.handlePlanarReflectionPostRender;
				Landscape.instance = null;
				Landscape.clearTiles();
				LandscapeHeightmapCopyPool.empty();
				LandscapeSplatmapCopyPool.empty();
			}
		}

		public static readonly float TILE_SIZE = 1024f;

		public static readonly int TILE_SIZE_INT = 1024;

		public static readonly float TILE_HEIGHT = 2048f;

		public static readonly int TILE_HEIGHT_INT = 2048;

		public static readonly int HEIGHTMAP_RESOLUTION = 257;

		public static readonly int HEIGHTMAP_RESOLUTION_MINUS_ONE = 256;

		public static readonly float HEIGHTMAP_WORLD_UNIT = 4f;

		public static readonly float HALF_HEIGHTMAP_WORLD_UNIT = 2f;

		public static readonly int SPLATMAP_RESOLUTION = 256;

		public static readonly int SPLATMAP_RESOLUTION_MINUS_ONE = 255;

		public static readonly float SPLATMAP_WORLD_UNIT = 4f;

		public static readonly float HALF_SPLATMAP_WORLD_UNIT = 2f;

		public static readonly int BASEMAP_RESOLUTION = 64;

		public static readonly int SPLATMAP_COUNT = 2;

		public static readonly int SPLATMAP_CHANNELS = 4;

		public static readonly int SPLATMAP_LAYERS = Landscape.SPLATMAP_COUNT * Landscape.SPLATMAP_CHANNELS;

		protected static readonly float[] SPLATMAP_LAYER_BUFFER = new float[Landscape.SPLATMAP_LAYERS];

		protected static Dictionary<LandscapeCoord, LandscapeTile> tiles = new Dictionary<LandscapeCoord, LandscapeTile>();

		protected static Dictionary<LandscapeCoord, LandscapeHeightmapTransaction> heightmapTransactions = new Dictionary<LandscapeCoord, LandscapeHeightmapTransaction>();

		protected static Dictionary<LandscapeCoord, LandscapeSplatmapTransaction> splatmapTransactions = new Dictionary<LandscapeCoord, LandscapeSplatmapTransaction>();

		public delegate void LandscapeReadHeightmapHandler(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight);

		public delegate void LandscapeReadSplatmapHandler(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights);

		public delegate float LandscapeWriteHeightmapHandler(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight);

		public delegate void LandscapeWriteSplatmapHandler(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights);

		public delegate void LandscapeGetHeightmapVerticesHandler(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition);

		public delegate void LandscapeGetSplatmapVerticesHandler(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition);
	}
}

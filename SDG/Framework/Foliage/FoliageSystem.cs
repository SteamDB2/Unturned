using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Translations;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Framework.Foliage
{
	public class FoliageSystem : DevkitHierarchyItemBase, IDevkitHierarchyAutoSpawnable, IDevkitHierarchySpawnable
	{
		static FoliageSystem()
		{
			FoliageSystem.foliageVisibilityGroup = VisibilityManager.registerVisibilityGroup<VisibilityGroup>(FoliageSystem.foliageVisibilityGroup);
		}

		public static FoliageSystem instance { get; private set; }

		public static List<IFoliageSurface> surfaces { get; private set; } = new List<IFoliageSurface>();

		public static VisibilityGroup foliageVisibilityGroup { get; private set; } = new VisibilityGroup("foliage_instanced_meshes", new TranslationReference("#SDG::Devkit.Visibility.Foliage_Instanced_Meshes"), true);

		public static event FoliageSystemPreBakeHandler preBake;

		public static event FoliageSystemPreBakeTileHandler preBakeTile;

		public static event FoliageSystemPostBakeTileHandler postBakeTile;

		public static event FoliageSystemGlobalBakeHandler globalBake;

		public static event FoliageSystemLocalBakeHandler localBake;

		public static event FoliageSystemPostBakeHandler postBake;

		public static int bakeQueueProgress
		{
			get
			{
				return FoliageSystem.bakeQueueTotal - FoliageSystem.bakeQueue.Count;
			}
		}

		public static int bakeQueueTotal { get; private set; }

		public static FoliageBakeSettings bakeSettings { get; private set; }

		public static void addSurface(IFoliageSurface surface)
		{
			FoliageSystem.surfaces.Add(surface);
		}

		public static void removeSurface(IFoliageSurface surface)
		{
			FoliageSystem.surfaces.Remove(surface);
		}

		public static void addCut(IShapeVolume cut)
		{
			Bounds worldBounds = cut.worldBounds;
			FoliageBounds foliageBounds = new FoliageBounds(worldBounds);
			for (int i = foliageBounds.min.x; i <= foliageBounds.max.x; i++)
			{
				for (int j = foliageBounds.min.y; j <= foliageBounds.max.y; j++)
				{
					FoliageCoord tileCoord = new FoliageCoord(i, j);
					FoliageTile orAddTile = FoliageSystem.getOrAddTile(tileCoord);
					orAddTile.addCut(cut);
				}
			}
		}

		private static Dictionary<FoliageTile, List<IFoliageSurface>> getTileSurfacePairs()
		{
			Dictionary<FoliageTile, List<IFoliageSurface>> dictionary = new Dictionary<FoliageTile, List<IFoliageSurface>>();
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair in FoliageSystem.tiles)
			{
				FoliageTile value = keyValuePair.Value;
				if (FoliageVolumeUtility.isTileBakeable(value))
				{
					dictionary.Add(value, new List<IFoliageSurface>());
				}
			}
			foreach (IFoliageSurface foliageSurface in FoliageSystem.surfaces)
			{
				FoliageBounds foliageSurfaceBounds = foliageSurface.getFoliageSurfaceBounds();
				for (int i = foliageSurfaceBounds.min.x; i <= foliageSurfaceBounds.max.x; i++)
				{
					for (int j = foliageSurfaceBounds.min.y; j <= foliageSurfaceBounds.max.y; j++)
					{
						FoliageCoord tileCoord = new FoliageCoord(i, j);
						FoliageTile orAddTile = FoliageSystem.getOrAddTile(tileCoord);
						if (FoliageVolumeUtility.isTileBakeable(orAddTile))
						{
							List<IFoliageSurface> list;
							if (!dictionary.TryGetValue(orAddTile, out list))
							{
								list = new List<IFoliageSurface>();
								dictionary.Add(orAddTile, list);
							}
							list.Add(foliageSurface);
						}
					}
				}
			}
			return dictionary;
		}

		private static void bakePre()
		{
			if (FoliageSystem.preBake != null)
			{
				FoliageSystem.preBake();
			}
			FoliageSystem.bakeQueue.Clear();
		}

		public static void bakeGlobal(FoliageBakeSettings bakeSettings)
		{
			FoliageSystem.bakeSettings = bakeSettings;
			FoliageSystem.bakePre();
			FoliageSystem.bakeGlobalBegin();
		}

		private static void bakeGlobalBegin()
		{
			Dictionary<FoliageTile, List<IFoliageSurface>> tileSurfacePairs = FoliageSystem.getTileSurfacePairs();
			foreach (KeyValuePair<FoliageTile, List<IFoliageSurface>> item in tileSurfacePairs)
			{
				FoliageSystem.bakeQueue.Enqueue(item);
			}
			FoliageSystem.bakeQueueTotal = FoliageSystem.bakeQueue.Count;
			if (FoliageSystem.<>f__mg$cache0 == null)
			{
				FoliageSystem.<>f__mg$cache0 = new FoliageSystemPostBakeHandler(FoliageSystem.bakeGlobalEnd);
			}
			FoliageSystem.bakeEnd = FoliageSystem.<>f__mg$cache0;
			FoliageSystem.bakeEnd();
		}

		private static void bakeGlobalEnd()
		{
			if (FoliageSystem.globalBake != null)
			{
				FoliageSystem.globalBake();
			}
			FoliageSystem.bakePost();
		}

		public static void bakeLocal(FoliageBakeSettings bakeSettings)
		{
			FoliageSystem.bakeSettings = bakeSettings;
			FoliageSystem.bakePre();
			FoliageSystem.bakeLocalBegin();
		}

		private static void bakeLocalBegin()
		{
			FoliageSystem.bakeLocalPosition = MainCamera.instance.transform.position;
			int num = 6;
			int num2 = num * num;
			FoliageCoord foliageCoord = new FoliageCoord(FoliageSystem.bakeLocalPosition);
			Dictionary<FoliageTile, List<IFoliageSurface>> tileSurfacePairs = FoliageSystem.getTileSurfacePairs();
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					int num3 = i * i + j * j;
					if (num3 <= num2)
					{
						FoliageCoord tileCoord = new FoliageCoord(foliageCoord.x + i, foliageCoord.y + j);
						FoliageTile tile = FoliageSystem.getTile(tileCoord);
						if (tile != null)
						{
							List<IFoliageSurface> value;
							if (tileSurfacePairs.TryGetValue(tile, out value))
							{
								KeyValuePair<FoliageTile, List<IFoliageSurface>> item = new KeyValuePair<FoliageTile, List<IFoliageSurface>>(tile, value);
								FoliageSystem.bakeQueue.Enqueue(item);
							}
						}
					}
				}
			}
			FoliageSystem.bakeQueueTotal = FoliageSystem.bakeQueue.Count;
			if (FoliageSystem.<>f__mg$cache1 == null)
			{
				FoliageSystem.<>f__mg$cache1 = new FoliageSystemPostBakeHandler(FoliageSystem.bakeLocalEnd);
			}
			FoliageSystem.bakeEnd = FoliageSystem.<>f__mg$cache1;
			FoliageSystem.bakeEnd();
		}

		private static void bakeLocalEnd()
		{
			if (FoliageSystem.localBake != null)
			{
				FoliageSystem.localBake(FoliageSystem.bakeLocalPosition);
			}
			FoliageSystem.bakePost();
		}

		public static void bakeCancel()
		{
			if (FoliageSystem.bakeQueue.Count == 0)
			{
				return;
			}
			FoliageSystem.bakeQueue.Clear();
			FoliageSystem.bakeEnd();
		}

		private static void bakePreTile(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			if (!bakeSettings.bakeInstancesMeshes)
			{
				return;
			}
			if (!foliageTile.hasInstances)
			{
				foliageTile.readInstancesOnThread();
				foliageTile.clearPostBake = true;
			}
			if (foliageTile.hasInstances)
			{
				if (bakeSettings.bakeApplyScale)
				{
					foliageTile.applyScale();
				}
				else
				{
					foliageTile.clearGeneratedInstances();
				}
			}
		}

		private static void bakePostTile(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			if (!bakeSettings.bakeInstancesMeshes)
			{
				return;
			}
			if (foliageTile.hasInstances)
			{
				foliageTile.writeInstances();
				if (foliageTile.clearPostBake)
				{
					foliageTile.clearInstances();
				}
			}
		}

		private static void bake(FoliageTile tile, List<IFoliageSurface> list)
		{
			FoliageSystem.bakePreTile(FoliageSystem.bakeSettings, tile);
			if (FoliageSystem.preBakeTile != null)
			{
				FoliageSystem.preBakeTile(FoliageSystem.bakeSettings, tile);
			}
			if (!FoliageSystem.bakeSettings.bakeApplyScale)
			{
				foreach (IFoliageSurface foliageSurface in list)
				{
					foliageSurface.bakeFoliageSurface(FoliageSystem.bakeSettings, tile);
				}
			}
			FoliageSystem.bakePostTile(FoliageSystem.bakeSettings, tile);
			if (FoliageSystem.postBakeTile != null)
			{
				FoliageSystem.postBakeTile(FoliageSystem.bakeSettings, tile);
			}
		}

		private static void bakePost()
		{
			if (LevelHierarchy.instance != null)
			{
				LevelHierarchy.instance.isDirty = true;
			}
			if (FoliageSystem.postBake != null)
			{
				FoliageSystem.postBake();
			}
		}

		public static void addInstance(AssetReference<FoliageInstancedMeshInfoAsset> assetReference, Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked)
		{
			FoliageTile orAddTile = FoliageSystem.getOrAddTile(position);
			Matrix4x4 newMatrix = Matrix4x4.TRS(position, rotation, scale);
			orAddTile.addInstance(new FoliageInstanceGroup(assetReference, newMatrix, clearWhenBaked));
		}

		protected static void clearTiles()
		{
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair in FoliageSystem.tiles)
			{
				FoliageTile value = keyValuePair.Value;
				if (value.hasInstances)
				{
					value.clearInstances();
				}
			}
			FoliageSystem.tiles.Clear();
		}

		public static void drawTiles(Vector3 position, int drawDistance, Camera camera, Plane[] frustumPlanes)
		{
			int num = drawDistance * drawDistance;
			FoliageCoord foliageCoord = new FoliageCoord(position);
			for (int i = -drawDistance; i <= drawDistance; i++)
			{
				for (int j = -drawDistance; j <= drawDistance; j++)
				{
					FoliageCoord foliageCoord2 = new FoliageCoord(foliageCoord.x + i, foliageCoord.y + j);
					if (!FoliageSystem.activeTiles.ContainsKey(foliageCoord2))
					{
						FoliageTile tile = FoliageSystem.getTile(foliageCoord2);
						if (tile != null)
						{
							int num2 = i * i + j * j;
							if (num2 <= num)
							{
								float density = 1f;
								float num3 = Mathf.Sqrt((float)num2);
								if (num3 > 2f && drawDistance > 3)
								{
									density = 1f - (num3 - 2f) / (float)(drawDistance - 1);
								}
								FoliageSystem.drawTileCullingChecks(tile, num2, density, camera, frustumPlanes);
								FoliageSystem.activeTiles.Add(foliageCoord2, tile);
							}
						}
					}
				}
			}
		}

		public static void drawTileCullingChecks(FoliageTile tile, int sqrDistance, float density, Camera camera, Plane[] frustumPlanes)
		{
			if (tile == null)
			{
				return;
			}
			if (!GeometryUtility.TestPlanesAABB(frustumPlanes, tile.worldBounds))
			{
				return;
			}
			FoliageSystem.drawTile(tile, sqrDistance, density, camera);
		}

		public static void drawTile(FoliageTile tile, int sqrDistance, float density, Camera camera)
		{
			if (tile == null)
			{
				return;
			}
			if (tile.hasInstances)
			{
				foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in tile.instances)
				{
					FoliageInstanceList value = keyValuePair.Value;
					value.loadAsset();
					Mesh mesh = value.mesh;
					if (!(mesh == null))
					{
						Material material = value.material;
						if (!(material == null))
						{
							bool castShadows = value.castShadows;
							if (!value.tileDither)
							{
								density = 1f;
							}
							density *= FoliageSettings.instanceDensity;
							if (value.sqrDrawDistance == -1 || sqrDistance <= value.sqrDrawDistance)
							{
								if (FoliageSettings.forceInstancingOff || !SystemInfo.supportsInstancing)
								{
									foreach (List<Matrix4x4> list in value.matrices)
									{
										int num = Mathf.RoundToInt((float)list.Count * density);
										for (int i = 0; i < num; i++)
										{
											Graphics.DrawMesh(mesh, list[i], material, LayerMasks.ENVIRONMENT, camera, 0, null, castShadows, true);
										}
									}
								}
								else
								{
									ShadowCastingMode shadowCastingMode = (!castShadows) ? 0 : 1;
									foreach (List<Matrix4x4> list2 in value.matrices)
									{
										int num2 = Mathf.RoundToInt((float)list2.Count * density);
										Graphics.DrawMeshInstanced(mesh, 0, material, list2.GetInternalArray<Matrix4x4>(), num2, null, shadowCastingMode, true, LayerMasks.ENVIRONMENT, camera);
									}
								}
							}
						}
					}
				}
			}
			else
			{
				tile.readInstancesJob();
			}
		}

		public static FoliageTile getOrAddTile(Vector3 worldPosition)
		{
			FoliageCoord tileCoord = new FoliageCoord(worldPosition);
			return FoliageSystem.getOrAddTile(tileCoord);
		}

		public static FoliageTile getTile(Vector3 worldPosition)
		{
			FoliageCoord tileCoord = new FoliageCoord(worldPosition);
			return FoliageSystem.getTile(tileCoord);
		}

		public static FoliageTile getOrAddTile(FoliageCoord tileCoord)
		{
			FoliageTile foliageTile;
			if (!FoliageSystem.tiles.TryGetValue(tileCoord, out foliageTile))
			{
				foliageTile = new FoliageTile(tileCoord);
				FoliageSystem.tiles.Add(tileCoord, foliageTile);
			}
			return foliageTile;
		}

		public static FoliageTile getTile(FoliageCoord tileCoord)
		{
			FoliageTile result;
			FoliageSystem.tiles.TryGetValue(tileCoord, out result);
			return result;
		}

		public void devkitHierarchySpawn()
		{
		}

		public override void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			int num = reader.readArrayLength("Tiles");
			for (int i = 0; i < num; i++)
			{
				reader.readArrayIndex(i);
				FoliageTile foliageTile = new FoliageTile(FoliageCoord.ZERO);
				foliageTile.read(reader);
				FoliageSystem.tiles.Add(foliageTile.coord, foliageTile);
			}
		}

		public override void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.beginArray("Tiles");
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair in FoliageSystem.tiles)
			{
				FoliageTile value = keyValuePair.Value;
				writer.writeValue<FoliageTile>(value);
			}
			writer.endArray();
			writer.endObject();
		}

		protected void Update()
		{
			if (MainCamera.instance == null)
			{
				return;
			}
			FoliageSystem.activeTiles.Clear();
			if (FoliageSettings.enabled && FoliageSystem.foliageVisibilityGroup.isVisible)
			{
				FoliageSystem.mainCameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(MainCamera.instance);
				FoliageSystem.drawTiles(MainCamera.instance.transform.position, FoliageSettings.drawDistance, null, FoliageSystem.mainCameraFrustumPlanes);
				if (FoliageSettings.drawFocus && FoliageSystem.isFocused && FoliageSystem.focusCamera != null)
				{
					if (MainCamera.instance == FoliageSystem.focusCamera)
					{
						FoliageSystem.focusCameraFrustumPlanes = FoliageSystem.mainCameraFrustumPlanes;
					}
					else
					{
						FoliageSystem.focusCameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(FoliageSystem.focusCamera);
					}
					FoliageSystem.drawTiles(FoliageSystem.focusPosition, FoliageSettings.drawFocusDistance, FoliageSystem.focusCamera, FoliageSystem.focusCameraFrustumPlanes);
				}
			}
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair in FoliageSystem.prevTiles)
			{
				if (!FoliageSystem.activeTiles.ContainsKey(keyValuePair.Key))
				{
					if (keyValuePair.Value != null && keyValuePair.Value.hasInstances)
					{
						if (keyValuePair.Value.canSafelyClear)
						{
							keyValuePair.Value.clearInstances();
						}
					}
				}
			}
			FoliageSystem.prevTiles.Clear();
			foreach (KeyValuePair<FoliageCoord, FoliageTile> keyValuePair2 in FoliageSystem.activeTiles)
			{
				FoliageSystem.prevTiles.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			if (FoliageSystem.bakeQueue.Count > 0)
			{
				KeyValuePair<FoliageTile, List<IFoliageSurface>> keyValuePair3 = FoliageSystem.bakeQueue.Dequeue();
				FoliageSystem.bake(keyValuePair3.Key, keyValuePair3.Value);
				if (FoliageSystem.bakeQueue.Count == 0)
				{
					FoliageSystem.bakeEnd();
				}
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
			base.name = "Foliage_System";
			base.gameObject.layer = LayerMasks.GROUND;
			if (FoliageSystem.instance == null)
			{
				FoliageSystem.instance = this;
				FoliageSystem.prevTiles.Clear();
				FoliageSystem.activeTiles.Clear();
				FoliageSystem.bakeQueue.Clear();
				FoliageSystem.clearTiles();
			}
		}

		protected void OnDestroy()
		{
			if (FoliageSystem.instance == this)
			{
				FoliageSystem.instance = null;
				FoliageSystem.prevTiles.Clear();
				FoliageSystem.activeTiles.Clear();
				FoliageSystem.bakeQueue.Clear();
				FoliageSystem.clearTiles();
			}
		}

		public static float TILE_SIZE = 32f;

		public static int TILE_SIZE_INT = 32;

		public static int SPLATMAP_RESOLUTION_PER_TILE = 8;

		protected static Dictionary<FoliageCoord, FoliageTile> prevTiles = new Dictionary<FoliageCoord, FoliageTile>();

		protected static Dictionary<FoliageCoord, FoliageTile> activeTiles = new Dictionary<FoliageCoord, FoliageTile>();

		protected static Dictionary<FoliageCoord, FoliageTile> tiles = new Dictionary<FoliageCoord, FoliageTile>();

		protected static Queue<KeyValuePair<FoliageTile, List<IFoliageSurface>>> bakeQueue = new Queue<KeyValuePair<FoliageTile, List<IFoliageSurface>>>();

		protected static FoliageSystemPostBakeHandler bakeEnd;

		protected static Vector3 bakeLocalPosition;

		protected static Plane[] mainCameraFrustumPlanes;

		protected static Plane[] focusCameraFrustumPlanes;

		public static Vector3 focusPosition;

		public static bool isFocused;

		public static Camera focusCamera;

		[CompilerGenerated]
		private static FoliageSystemPostBakeHandler <>f__mg$cache0;

		[CompilerGenerated]
		private static FoliageSystemPostBakeHandler <>f__mg$cache1;
	}
}

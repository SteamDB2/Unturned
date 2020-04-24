using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Landscapes;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public class FoliageTile : IFormattedFileReadable, IFormattedFileWritable
	{
		public FoliageTile(FoliageCoord newCoord)
		{
			this.coord = newCoord;
			this.hasInstances = false;
			this.canSafelyClear = true;
			this.cuts = new List<IShapeVolume>();
		}

		public FoliageCoord coord
		{
			get
			{
				return this._coord;
			}
			protected set
			{
				this._coord = value;
				this.updateBounds();
			}
		}

		public bool hasInstances { get; protected set; }

		public Bounds worldBounds { get; protected set; }

		public bool canSafelyClear { get; protected set; }

		public void addCut(IShapeVolume cut)
		{
			this.cuts.Add(cut);
			if (!this.hasInstances)
			{
				return;
			}
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
			{
				FoliageInstanceList value = keyValuePair.Value;
				for (int i = 0; i < value.matrices.Count; i++)
				{
					List<Matrix4x4> list = value.matrices[i];
					List<bool> list2 = value.clearWhenBaked[i];
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (cut.containsPoint(list[j].GetPosition()))
						{
							list.RemoveAt(j);
							list2.RemoveAt(j);
						}
					}
				}
			}
		}

		public bool isInstanceCut(Vector3 point)
		{
			foreach (IShapeVolume shapeVolume in this.cuts)
			{
				if (shapeVolume.containsPoint(point))
				{
					return true;
				}
			}
			return false;
		}

		protected FoliageInstanceList getOrAddList(Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> source, AssetReference<FoliageInstancedMeshInfoAsset> assetReference)
		{
			FoliageInstanceList foliageInstanceList;
			if (!source.TryGetValue(assetReference, out foliageInstanceList))
			{
				object typeFromHandle = typeof(PoolablePool<FoliageInstanceList>);
				lock (typeFromHandle)
				{
					foliageInstanceList = PoolablePool<FoliageInstanceList>.claim();
				}
				foliageInstanceList.assetReference = assetReference;
				source.Add(assetReference, foliageInstanceList);
			}
			return foliageInstanceList;
		}

		public void addInstance(FoliageInstanceGroup instance)
		{
			if (!this.hasInstances)
			{
				return;
			}
			FoliageInstanceList orAddList = this.getOrAddList(this.instances, instance.assetReference);
			orAddList.addInstanceRandom(instance);
			this.updateBounds();
			this.canSafelyClear = false;
		}

		public void removeInstance(FoliageInstanceList list, int matricesIndex, int matrixIndex)
		{
			if (!this.hasInstances)
			{
				return;
			}
			list.removeInstance(matricesIndex, matrixIndex);
			this.canSafelyClear = false;
		}

		public void clearInstances()
		{
			this.hasInstances = false;
			if (this.instances.Count > 0)
			{
				object typeFromHandle = typeof(PoolablePool<FoliageInstanceList>);
				lock (typeFromHandle)
				{
					foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
					{
						FoliageInstanceList value = keyValuePair.Value;
						PoolablePool<FoliageInstanceList>.release(value);
					}
				}
			}
			this.instances = null;
			this.isReadingInstances = false;
		}

		public void clearGeneratedInstances()
		{
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
			{
				FoliageInstanceList value = keyValuePair.Value;
				value.clearGeneratedInstances();
			}
		}

		public void applyScale()
		{
			foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
			{
				FoliageInstanceList value = keyValuePair.Value;
				value.applyScale();
			}
		}

		public virtual void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.coord = reader.readValue<FoliageCoord>("Coord");
		}

		public virtual void readInstancesJob()
		{
			object obj = this.thisLock;
			lock (obj)
			{
				if (!this.hasInstances && !this.isReadingInstances)
				{
					this.isReadingInstances = true;
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.readInstances));
				}
			}
		}

		public virtual void readInstancesOnThread()
		{
			object obj = this.thisLock;
			lock (obj)
			{
				if (!this.hasInstances && !this.isReadingInstances)
				{
					this.isReadingInstances = true;
					this.readInstances(null);
				}
			}
		}

		protected virtual void readInstances(object stateInfo)
		{
			Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> source = new Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList>();
			string path = string.Concat(new object[]
			{
				Level.info.path,
				"/Foliage/Tile_",
				this.coord.x,
				"_",
				this.coord.y,
				".foliage"
			});
			if (File.Exists(path))
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open))
				{
					BinaryReader binaryReader = new BinaryReader(fileStream);
					int num = binaryReader.ReadInt32();
					int num2 = binaryReader.ReadInt32();
					for (int i = 0; i < num2; i++)
					{
						GuidBuffer guidBuffer = default(GuidBuffer);
						object guid_BUFFER = GuidBuffer.GUID_BUFFER;
						lock (guid_BUFFER)
						{
							fileStream.Read(GuidBuffer.GUID_BUFFER, 0, 16);
							guidBuffer.Read(GuidBuffer.GUID_BUFFER, 0);
						}
						AssetReference<FoliageInstancedMeshInfoAsset> assetReference = new AssetReference<FoliageInstancedMeshInfoAsset>(guidBuffer.GUID);
						FoliageInstanceList orAddList = this.getOrAddList(source, assetReference);
						int num3 = binaryReader.ReadInt32();
						for (int j = 0; j < num3; j++)
						{
							Matrix4x4 matrix4x = default(Matrix4x4);
							for (int k = 0; k < 16; k++)
							{
								matrix4x[k] = binaryReader.ReadSingle();
							}
							bool newClearWhenBaked = num <= 2 || binaryReader.ReadBoolean();
							if (!this.isInstanceCut(matrix4x.GetPosition()))
							{
								orAddList.addInstanceAppend(new FoliageInstanceGroup(assetReference, matrix4x, newClearWhenBaked));
							}
						}
					}
				}
			}
			object obj = this.thisLock;
			lock (obj)
			{
				if (!this.hasInstances)
				{
					this.instances = source;
					this.updateBounds();
					this.hasInstances = true;
					this.isReadingInstances = false;
				}
			}
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue<FoliageCoord>("Coord", this.coord);
			writer.endObject();
			if (this.hasInstances)
			{
				this.writeInstances();
			}
		}

		public virtual void writeInstances()
		{
			string path = string.Concat(new object[]
			{
				Level.info.path,
				"/Foliage/Tile_",
				this.coord.x,
				"_",
				this.coord.y,
				".foliage"
			});
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
			{
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				binaryWriter.Write(FoliageTile.FOLIAGE_FILE_VERSION);
				binaryWriter.Write(this.instances.Count);
				foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
				{
					GuidBuffer guidBuffer = new GuidBuffer(keyValuePair.Key.GUID);
					object guid_BUFFER = GuidBuffer.GUID_BUFFER;
					lock (guid_BUFFER)
					{
						guidBuffer.Write(GuidBuffer.GUID_BUFFER, 0);
						fileStream.Write(GuidBuffer.GUID_BUFFER, 0, 16);
					}
					int num = 0;
					foreach (List<Matrix4x4> list in keyValuePair.Value.matrices)
					{
						num += list.Count;
					}
					binaryWriter.Write(num);
					for (int i = 0; i < keyValuePair.Value.matrices.Count; i++)
					{
						List<Matrix4x4> list2 = keyValuePair.Value.matrices[i];
						List<bool> list3 = keyValuePair.Value.clearWhenBaked[i];
						for (int j = 0; j < list2.Count; j++)
						{
							Matrix4x4 matrix4x = list2[j];
							for (int k = 0; k < 16; k++)
							{
								binaryWriter.Write(matrix4x[k]);
							}
							bool value = list3[j];
							binaryWriter.Write(value);
						}
					}
				}
			}
			this.canSafelyClear = true;
		}

		protected void updateBounds()
		{
			if (this.hasInstances)
			{
				float num = Landscape.TILE_HEIGHT;
				float num2 = -Landscape.TILE_HEIGHT;
				foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in this.instances)
				{
					FoliageInstanceList value = keyValuePair.Value;
					foreach (List<Matrix4x4> list in value.matrices)
					{
						foreach (Matrix4x4 matrix4x in list)
						{
							float m = matrix4x.m13;
							if (m < num)
							{
								num = m;
							}
							if (m > num2)
							{
								num2 = m;
							}
						}
					}
				}
				float num3 = num2 - num;
				this.worldBounds = new Bounds(new Vector3((float)this.coord.x * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f, num + num3 / 2f, (float)this.coord.y * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f), new Vector3(FoliageSystem.TILE_SIZE, num3, FoliageSystem.TILE_SIZE));
			}
			else
			{
				this.worldBounds = new Bounds(new Vector3((float)this.coord.x * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f, 0f, (float)this.coord.y * FoliageSystem.TILE_SIZE + FoliageSystem.TILE_SIZE / 2f), new Vector3(FoliageSystem.TILE_SIZE, Landscape.TILE_HEIGHT, FoliageSystem.TILE_SIZE));
			}
		}

		public static readonly int FOLIAGE_FILE_VERSION = 3;

		protected FoliageCoord _coord;

		public Dictionary<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> instances;

		public bool clearPostBake;

		protected List<IShapeVolume> cuts;

		protected bool isReadingInstances;

		protected object thisLock = new object();
	}
}

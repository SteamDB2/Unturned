using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public class FoliageInstanceList : IPoolable
	{
		public FoliageInstanceList()
		{
			this.matrices = new List<List<Matrix4x4>>(1);
			this.clearWhenBaked = new List<List<bool>>(1);
		}

		public List<List<Matrix4x4>> matrices { get; protected set; }

		public List<List<bool>> clearWhenBaked { get; protected set; }

		public bool isAssetLoaded { get; protected set; }

		public Mesh mesh { get; protected set; }

		public Material material { get; protected set; }

		public bool castShadows { get; protected set; }

		public bool tileDither { get; protected set; }

		public int sqrDrawDistance { get; protected set; }

		public virtual void poolClaim()
		{
		}

		public virtual void poolRelease()
		{
			this.assetReference = AssetReference<FoliageInstancedMeshInfoAsset>.invalid;
			object typeFromHandle = typeof(ListPool<Matrix4x4>);
			lock (typeFromHandle)
			{
				foreach (List<Matrix4x4> list in this.matrices)
				{
					ListPool<Matrix4x4>.release(list);
				}
				this.matrices.Clear();
			}
			object typeFromHandle2 = typeof(ListPool<bool>);
			lock (typeFromHandle2)
			{
				foreach (List<bool> list2 in this.clearWhenBaked)
				{
					ListPool<bool>.release(list2);
				}
				this.clearWhenBaked.Clear();
			}
			this.isAssetLoaded = false;
			this.mesh = null;
			this.material = null;
		}

		public virtual void clearGeneratedInstances()
		{
			for (int i = 0; i < this.matrices.Count; i++)
			{
				List<Matrix4x4> list = this.matrices[i];
				List<bool> list2 = this.clearWhenBaked[i];
				for (int j = list.Count - 1; j >= 0; j--)
				{
					if (list2[j])
					{
						list.RemoveAt(j);
						list2.RemoveAt(j);
					}
				}
			}
		}

		public virtual void applyScale()
		{
			FoliageInstancedMeshInfoAsset foliageInstancedMeshInfoAsset = Assets.find<FoliageInstancedMeshInfoAsset>(this.assetReference);
			if (foliageInstancedMeshInfoAsset == null)
			{
				return;
			}
			for (int i = 0; i < this.matrices.Count; i++)
			{
				List<Matrix4x4> list = this.matrices[i];
				List<bool> list2 = this.clearWhenBaked[i];
				for (int j = list.Count - 1; j >= 0; j--)
				{
					Matrix4x4 matrix4x = list[j];
					Vector3 position = matrix4x.GetPosition();
					Quaternion rotation = matrix4x.GetRotation();
					Vector3 randomScale = foliageInstancedMeshInfoAsset.randomScale;
					matrix4x = Matrix4x4.TRS(position, rotation, randomScale);
					list[j] = matrix4x;
				}
			}
		}

		protected virtual void getOrAddLists(out List<Matrix4x4> matrixList, out List<bool> clearWhenBakedList)
		{
			matrixList = null;
			foreach (List<Matrix4x4> list in this.matrices)
			{
				if (list.Count < 1023)
				{
					matrixList = list;
					break;
				}
			}
			if (matrixList == null)
			{
				object typeFromHandle = typeof(ListPool<Matrix4x4>);
				lock (typeFromHandle)
				{
					matrixList = ListPool<Matrix4x4>.claim();
				}
				this.matrices.Add(matrixList);
			}
			clearWhenBakedList = null;
			foreach (List<bool> list2 in this.clearWhenBaked)
			{
				if (list2.Count < 1023)
				{
					clearWhenBakedList = list2;
					break;
				}
			}
			if (clearWhenBakedList == null)
			{
				object typeFromHandle2 = typeof(ListPool<bool>);
				lock (typeFromHandle2)
				{
					clearWhenBakedList = ListPool<bool>.claim();
				}
				this.clearWhenBaked.Add(clearWhenBakedList);
			}
		}

		public virtual void addInstanceRandom(FoliageInstanceGroup group)
		{
			List<Matrix4x4> list;
			List<bool> list2;
			this.getOrAddLists(out list, out list2);
			int index = Random.Range(0, list.Count);
			list.Insert(index, group.matrix);
			list2.Insert(index, group.clearWhenBaked);
		}

		public virtual void addInstanceAppend(FoliageInstanceGroup group)
		{
			List<Matrix4x4> list;
			List<bool> list2;
			this.getOrAddLists(out list, out list2);
			list.Add(group.matrix);
			list2.Add(group.clearWhenBaked);
		}

		public virtual void removeInstance(int matricesIndex, int matrixIndex)
		{
			List<Matrix4x4> list = this.matrices[matricesIndex];
			List<bool> list2 = this.clearWhenBaked[matricesIndex];
			list.RemoveAt(matrixIndex);
			list2.RemoveAt(matrixIndex);
		}

		public virtual void loadAsset()
		{
			if (this.isAssetLoaded)
			{
				return;
			}
			this.isAssetLoaded = true;
			FoliageInstancedMeshInfoAsset foliageInstancedMeshInfoAsset = Assets.find<FoliageInstancedMeshInfoAsset>(this.assetReference);
			if (foliageInstancedMeshInfoAsset == null)
			{
				return;
			}
			this.mesh = Assets.load<Mesh>(foliageInstancedMeshInfoAsset.mesh);
			this.material = Assets.load<Material>(foliageInstancedMeshInfoAsset.material);
			this.castShadows = foliageInstancedMeshInfoAsset.castShadows;
			this.tileDither = foliageInstancedMeshInfoAsset.tileDither;
			if (foliageInstancedMeshInfoAsset.drawDistance == -1)
			{
				this.sqrDrawDistance = -1;
			}
			else
			{
				this.sqrDrawDistance = foliageInstancedMeshInfoAsset.drawDistance * foliageInstancedMeshInfoAsset.drawDistance;
			}
		}

		public AssetReference<FoliageInstancedMeshInfoAsset> assetReference;
	}
}

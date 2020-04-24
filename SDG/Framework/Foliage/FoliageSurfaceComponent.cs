using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public class FoliageSurfaceComponent : MonoBehaviour, IFoliageSurface
	{
		public FoliageBounds getFoliageSurfaceBounds()
		{
			bool activeSelf = base.gameObject.activeSelf;
			if (!activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			FoliageBounds result = new FoliageBounds(this.surfaceCollider.bounds);
			if (!activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return result;
		}

		public bool getFoliageSurfaceInfo(Vector3 position, out Vector3 surfacePosition, out Vector3 surfaceNormal)
		{
			RaycastHit raycastHit;
			if (this.surfaceCollider.Raycast(new Ray(position, Vector3.down), ref raycastHit, 1024f))
			{
				surfacePosition = raycastHit.point;
				surfaceNormal = raycastHit.normal;
				return true;
			}
			surfacePosition = Vector3.zero;
			surfaceNormal = Vector3.up;
			return false;
		}

		public void bakeFoliageSurface(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			FoliageInfoCollectionAsset foliageInfoCollectionAsset = Assets.find<FoliageInfoCollectionAsset>(this.foliage);
			if (foliageInfoCollectionAsset == null)
			{
				return;
			}
			bool activeSelf = base.gameObject.activeSelf;
			if (!activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			Bounds worldBounds = foliageTile.worldBounds;
			Vector3 min = worldBounds.min;
			Vector3 max = worldBounds.max;
			Bounds bounds = this.surfaceCollider.bounds;
			Vector3 min2 = bounds.min;
			Vector3 max2 = bounds.max;
			Bounds bounds2 = default(Bounds);
			bounds2.min = new Vector3(Mathf.Max(min.x, min2.x), min2.y, Mathf.Max(min.z, min2.z));
			bounds2.max = new Vector3(Mathf.Min(max.x, max2.x), max2.y, Mathf.Min(max.z, max2.z));
			foliageInfoCollectionAsset.bakeFoliage(bakeSettings, this, bounds2, 1f);
			if (!activeSelf)
			{
				base.gameObject.SetActive(false);
			}
		}

		protected void OnEnable()
		{
			if (this.isRegistered)
			{
				return;
			}
			this.isRegistered = true;
			FoliageSystem.addSurface(this);
		}

		protected void OnDestroy()
		{
			if (!this.isRegistered)
			{
				return;
			}
			this.isRegistered = false;
			FoliageSystem.removeSurface(this);
		}

		public AssetReference<FoliageInfoCollectionAsset> foliage;

		public Collider surfaceCollider;

		protected bool isRegistered;
	}
}

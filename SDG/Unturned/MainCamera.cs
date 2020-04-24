using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	public class MainCamera : MonoBehaviour
	{
		public static Camera instance
		{
			get
			{
				return MainCamera._instance;
			}
			protected set
			{
				if (MainCamera.instance != value)
				{
					MainCamera._instance = value;
					MainCamera.triggerInstanceChanged();
				}
			}
		}

		public static bool isAvailable
		{
			get
			{
				return MainCamera._isAvailable;
			}
			protected set
			{
				if (MainCamera.isAvailable != value)
				{
					MainCamera._isAvailable = value;
					MainCamera.triggerAvailabilityChanged();
				}
			}
		}

		public static Plane[] frustumPlanes
		{
			get
			{
				if (MainCamera.instance != null && MainCamera.instance.transform.hasChanged)
				{
					MainCamera._frustumPlanes = GeometryUtility.CalculateFrustumPlanes(MainCamera.instance);
					MainCamera.instance.transform.hasChanged = false;
				}
				return MainCamera._frustumPlanes;
			}
		}

		public static event MainCameraInstanceChangedHandler instanceChanged;

		public static event MainCameraAvailabilityChangedHandler availabilityChanged;

		public IEnumerator activate()
		{
			yield return new WaitForEndOfFrame();
			MainCamera.isAvailable = true;
			yield break;
		}

		protected static void triggerInstanceChanged()
		{
			if (MainCamera.instanceChanged != null)
			{
				MainCamera.instanceChanged();
			}
		}

		protected static void triggerAvailabilityChanged()
		{
			if (MainCamera.availabilityChanged != null)
			{
				MainCamera.availabilityChanged();
			}
		}

		public void Awake()
		{
			MainCamera.isAvailable = false;
			MainCamera.instance = base.transform.GetComponent<Camera>();
			base.StartCoroutine(this.activate());
		}

		protected static Camera _instance;

		protected static bool _isAvailable;

		protected static Plane[] _frustumPlanes = new Plane[6];
	}
}

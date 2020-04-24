using System;
using UnityEngine;

[ExecuteInEditMode]
public class WaterTile : MonoBehaviour
{
	public void Start()
	{
		this.AcquireComponents();
	}

	private void AcquireComponents()
	{
		if (!this.reflection)
		{
			if (base.transform.parent)
			{
				this.reflection = base.transform.parent.GetComponent<PlanarReflection>();
			}
			else
			{
				this.reflection = base.transform.GetComponent<PlanarReflection>();
			}
		}
	}

	public void OnWillRenderObject()
	{
		Camera current = Camera.current;
		if (this.reflection)
		{
			this.reflection.WaterTileBeingRendered(base.transform, current);
		}
		if (this.waterBase)
		{
			this.waterBase.WaterTileBeingRendered(base.transform, current);
		}
	}

	public PlanarReflection reflection;

	public WaterBase waterBase;
}

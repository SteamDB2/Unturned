using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class DynamicGridObstacle : MonoBehaviour
{
	private void Start()
	{
		this.col = base.GetComponent<Collider>();
		if (base.GetComponent<Collider>() == null)
		{
			Debug.LogError("A collider must be attached to the GameObject for DynamicGridObstacle to work");
		}
		base.StartCoroutine(this.UpdateGraphs());
	}

	private IEnumerator UpdateGraphs()
	{
		if (this.col == null || AstarPath.active == null)
		{
			Debug.LogWarning("No collider is attached to the GameObject. Canceling check");
			yield break;
		}
		while (this.col)
		{
			while (this.isWaitingForUpdate)
			{
				yield return new WaitForSeconds(this.checkTime);
			}
			Bounds newBounds = this.col.bounds;
			Bounds merged = newBounds;
			merged.Encapsulate(this.prevBounds);
			Vector3 minDiff = merged.min - newBounds.min;
			Vector3 maxDiff = merged.max - newBounds.max;
			if (Mathf.Abs(minDiff.x) > this.updateError || Mathf.Abs(minDiff.y) > this.updateError || Mathf.Abs(minDiff.z) > this.updateError || Mathf.Abs(maxDiff.x) > this.updateError || Mathf.Abs(maxDiff.y) > this.updateError || Mathf.Abs(maxDiff.z) > this.updateError)
			{
				this.isWaitingForUpdate = true;
				this.DoUpdateGraphs();
			}
			yield return new WaitForSeconds(this.checkTime);
		}
		this.OnDestroy();
		yield break;
	}

	public void OnDestroy()
	{
		if (AstarPath.active != null)
		{
			GraphUpdateObject ob = new GraphUpdateObject(this.prevBounds);
			AstarPath.active.UpdateGraphs(ob);
		}
	}

	public void DoUpdateGraphs()
	{
		if (this.col == null)
		{
			return;
		}
		this.isWaitingForUpdate = false;
		Bounds bounds = this.col.bounds;
		Bounds bounds2 = bounds;
		bounds2.Encapsulate(this.prevBounds);
		if (this.BoundsVolume(bounds2) < this.BoundsVolume(bounds) + this.BoundsVolume(this.prevBounds))
		{
			AstarPath.active.UpdateGraphs(bounds2);
		}
		else
		{
			AstarPath.active.UpdateGraphs(this.prevBounds);
			AstarPath.active.UpdateGraphs(bounds);
		}
		this.prevBounds = bounds;
	}

	public float BoundsVolume(Bounds b)
	{
		return Math.Abs(b.size.x * b.size.y * b.size.z);
	}

	private Collider col;

	public float updateError = 1f;

	public float checkTime = 0.2f;

	private Bounds prevBounds;

	private bool isWaitingForUpdate;
}

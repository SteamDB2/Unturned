using System;
using Pathfinding;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown("p"))
		{
			this.PlaceObject();
		}
		if (Input.GetKeyDown("r"))
		{
			this.RemoveObject();
		}
	}

	public void PlaceObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity))
		{
			Vector3 point = raycastHit.point;
			GameObject gameObject = Object.Instantiate<GameObject>(this.go, point, Quaternion.identity);
			if (this.issueGUOs)
			{
				Bounds bounds = gameObject.GetComponent<Collider>().bounds;
				GraphUpdateObject ob = new GraphUpdateObject(bounds);
				AstarPath.active.UpdateGraphs(ob);
				if (this.direct)
				{
					AstarPath.active.FlushGraphUpdates();
				}
			}
		}
	}

	public void RemoveObject()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity))
		{
			if (raycastHit.collider.isTrigger || raycastHit.transform.gameObject.name == "Ground")
			{
				return;
			}
			Bounds bounds = raycastHit.collider.bounds;
			Object.Destroy(raycastHit.collider);
			Object.Destroy(raycastHit.collider.gameObject);
			if (this.issueGUOs)
			{
				GraphUpdateObject ob = new GraphUpdateObject(bounds);
				AstarPath.active.UpdateGraphs(ob, 0f);
				if (this.direct)
				{
					AstarPath.active.FlushGraphUpdates();
				}
			}
		}
	}

	public GameObject go;

	public bool direct;

	public bool issueGUOs = true;
}

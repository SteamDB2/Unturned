using System;
using UnityEngine;

public class AstarSmoothFollow2 : MonoBehaviour
{
	private void LateUpdate()
	{
		Vector3 vector;
		if (this.staticOffset)
		{
			vector = this.target.position + new Vector3(0f, this.height, this.distance);
		}
		else if (this.followBehind)
		{
			vector = this.target.TransformPoint(0f, this.height, -this.distance);
		}
		else
		{
			vector = this.target.TransformPoint(0f, this.height, this.distance);
		}
		base.transform.position = Vector3.Lerp(base.transform.position, vector, Time.deltaTime * this.damping);
		if (this.smoothRotation)
		{
			Quaternion quaternion = Quaternion.LookRotation(this.target.position - base.transform.position, this.target.up);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * this.rotationDamping);
		}
		else
		{
			base.transform.LookAt(this.target, this.target.up);
		}
	}

	public Transform target;

	public float distance = 3f;

	public float height = 3f;

	public float damping = 5f;

	public bool smoothRotation = true;

	public bool followBehind = true;

	public float rotationDamping = 10f;

	public bool staticOffset;
}

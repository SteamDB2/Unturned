using System;
using UnityEngine;

public class LaserHelper : MonoBehaviour
{
	private void OnEnable()
	{
		if (this.begin == null)
		{
			return;
		}
		if (this.end == null)
		{
			return;
		}
		if (this.laser == null)
		{
			return;
		}
		Vector3 vector = this.end.position - this.begin.position;
		this.laser.position = this.begin.position + vector / 2f;
		this.laser.rotation = Quaternion.LookRotation(vector) * Quaternion.Euler(-90f, 0f, 0f);
		this.laser.localScale = new Vector3(1f, vector.magnitude / 2f, 1f);
	}

	public Transform begin;

	public Transform end;

	public Transform laser;
}

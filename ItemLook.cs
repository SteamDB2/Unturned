using System;
using UnityEngine;

public class ItemLook : MonoBehaviour
{
	private void Update()
	{
		this._yaw = Mathf.Lerp(this._yaw, this.yaw, 4f * Time.deltaTime);
		this.inspectCamera.transform.rotation = Quaternion.Euler(20f, this._yaw, 0f);
		this.inspectCamera.transform.position = this.pos - this.inspectCamera.transform.forward * 2.25f;
	}

	public Camera inspectCamera;

	public float _yaw;

	public float yaw;

	public Vector3 pos;
}

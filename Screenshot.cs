using System;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(102))
		{
			Debug.Log("A");
			Application.CaptureScreenshot("Screenshot.png", 8);
			Debug.Log("B");
		}
	}
}

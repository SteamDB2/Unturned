using System;
using UnityEngine;

[ExecuteInEditMode]
public class NavMeshRenderer : MonoBehaviour
{
	public string SomeFunction()
	{
		return this.lastLevel;
	}

	private void Update()
	{
	}

	private string lastLevel = string.Empty;
}

using System;
using UnityEngine;

public class MeshContainer
{
	public MeshContainer(Mesh m)
	{
		this.mesh = m;
		this.vertices = m.vertices;
		this.normals = m.normals;
	}

	public void Update()
	{
		this.mesh.vertices = this.vertices;
		this.mesh.normals = this.normals;
	}

	public Mesh mesh;

	public Vector3[] vertices;

	public Vector3[] normals;
}

using System;
using UnityEngine;

namespace Pathfinding
{
	public class DebugUtility : MonoBehaviour
	{
		public void Awake()
		{
			DebugUtility.active = this;
		}

		public static void DrawCubes(Vector3[] topVerts, Vector3[] bottomVerts, Color[] vertexColors, float width)
		{
			if (DebugUtility.active == null)
			{
				DebugUtility.active = (Object.FindObjectOfType(typeof(DebugUtility)) as DebugUtility);
			}
			if (DebugUtility.active == null)
			{
				throw new NullReferenceException();
			}
			if (topVerts.Length != bottomVerts.Length || topVerts.Length != vertexColors.Length)
			{
				Debug.LogError("Array Lengths are not the same");
				return;
			}
			if (topVerts.Length > 2708)
			{
				Vector3[] array = new Vector3[topVerts.Length - 2708];
				Vector3[] array2 = new Vector3[topVerts.Length - 2708];
				Color[] array3 = new Color[topVerts.Length - 2708];
				for (int i = 2708; i < topVerts.Length; i++)
				{
					array[i - 2708] = topVerts[i];
					array2[i - 2708] = bottomVerts[i];
					array3[i - 2708] = vertexColors[i];
				}
				Vector3[] array4 = new Vector3[2708];
				Vector3[] array5 = new Vector3[2708];
				Color[] array6 = new Color[2708];
				for (int j = 0; j < 2708; j++)
				{
					array4[j] = topVerts[j];
					array5[j] = bottomVerts[j];
					array6[j] = vertexColors[j];
				}
				DebugUtility.DrawCubes(array, array2, array3, width);
				topVerts = array4;
				bottomVerts = array5;
				vertexColors = array6;
			}
			width /= 2f;
			Vector3[] array7 = new Vector3[topVerts.Length * 4 * 6];
			int[] array8 = new int[topVerts.Length * 6 * 6];
			Color[] array9 = new Color[topVerts.Length * 4 * 6];
			for (int k = 0; k < topVerts.Length; k++)
			{
				Vector3 vector = topVerts[k] + new Vector3(0f, DebugUtility.active.offset, 0f);
				Vector3 vector2 = bottomVerts[k] - new Vector3(0f, DebugUtility.active.offset, 0f);
				Vector3 vector3 = vector + new Vector3(-width, 0f, -width);
				Vector3 vector4 = vector + new Vector3(width, 0f, -width);
				Vector3 vector5 = vector + new Vector3(width, 0f, width);
				Vector3 vector6 = vector + new Vector3(-width, 0f, width);
				Vector3 vector7 = vector2 + new Vector3(-width, 0f, -width);
				Vector3 vector8 = vector2 + new Vector3(width, 0f, -width);
				Vector3 vector9 = vector2 + new Vector3(width, 0f, width);
				Vector3 vector10 = vector2 + new Vector3(-width, 0f, width);
				int num = k * 4 * 6;
				Color color = vertexColors[k];
				for (int l = num; l < num + 24; l++)
				{
					array9[l] = color;
				}
				array7[num] = vector3;
				array7[num + 1] = vector6;
				array7[num + 2] = vector5;
				array7[num + 3] = vector4;
				int num2 = k * 6 * 6;
				array8[num2] = num;
				array8[num2 + 1] = num + 1;
				array8[num2 + 2] = num + 2;
				array8[num2 + 3] = num;
				array8[num2 + 4] = num + 2;
				array8[num2 + 5] = num + 3;
				num += 4;
				array7[num + 3] = vector7;
				array7[num + 2] = vector10;
				array7[num + 1] = vector9;
				array7[num] = vector8;
				num2 += 6;
				array8[num2] = num;
				array8[num2 + 1] = num + 1;
				array8[num2 + 2] = num + 2;
				array8[num2 + 3] = num;
				array8[num2 + 4] = num + 2;
				array8[num2 + 5] = num + 3;
				num += 4;
				array7[num] = vector8;
				array7[num + 1] = vector4;
				array7[num + 2] = vector5;
				array7[num + 3] = vector9;
				num2 += 6;
				array8[num2] = num;
				array8[num2 + 1] = num + 1;
				array8[num2 + 2] = num + 2;
				array8[num2 + 3] = num;
				array8[num2 + 4] = num + 2;
				array8[num2 + 5] = num + 3;
				num += 4;
				array7[num + 3] = vector7;
				array7[num + 2] = vector3;
				array7[num + 1] = vector6;
				array7[num] = vector10;
				num2 += 6;
				array8[num2] = num;
				array8[num2 + 1] = num + 1;
				array8[num2 + 2] = num + 2;
				array8[num2 + 3] = num;
				array8[num2 + 4] = num + 2;
				array8[num2 + 5] = num + 3;
				num += 4;
				array7[num + 3] = vector9;
				array7[num + 2] = vector10;
				array7[num + 1] = vector6;
				array7[num] = vector5;
				num2 += 6;
				array8[num2] = num;
				array8[num2 + 1] = num + 1;
				array8[num2 + 2] = num + 2;
				array8[num2 + 3] = num;
				array8[num2 + 4] = num + 2;
				array8[num2 + 5] = num + 3;
				num += 4;
				array7[num] = vector8;
				array7[num + 1] = vector7;
				array7[num + 2] = vector3;
				array7[num + 3] = vector4;
				num2 += 6;
				array8[num2] = num;
				array8[num2 + 1] = num + 1;
				array8[num2 + 2] = num + 2;
				array8[num2 + 3] = num;
				array8[num2 + 4] = num + 2;
				array8[num2 + 5] = num + 3;
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array7;
			mesh.triangles = array8;
			mesh.colors = array9;
			mesh.name = "VoxelMesh";
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			if (DebugUtility.active.optimizeMeshes)
			{
			}
			GameObject gameObject = new GameObject("DebugMesh");
			MeshRenderer meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
			meshRenderer.material = DebugUtility.active.defaultMaterial;
			(gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter).mesh = mesh;
		}

		public static void DrawQuads(Vector3[] verts, float width)
		{
			if (verts.Length >= 16250)
			{
				Vector3[] array = new Vector3[verts.Length - 16250];
				for (int i = 16250; i < verts.Length; i++)
				{
					array[i - 16250] = verts[i];
				}
				Vector3[] array2 = new Vector3[16250];
				for (int j = 0; j < 16250; j++)
				{
					array2[j] = verts[j];
				}
				DebugUtility.DrawQuads(array, width);
				verts = array2;
			}
			width /= 2f;
			Vector3[] array3 = new Vector3[verts.Length * 4];
			int[] array4 = new int[verts.Length * 6];
			for (int k = 0; k < verts.Length; k++)
			{
				Vector3 vector = verts[k];
				int num = k * 4;
				array3[num] = vector + new Vector3(-width, 0f, -width);
				array3[num + 1] = vector + new Vector3(-width, 0f, width);
				array3[num + 2] = vector + new Vector3(width, 0f, width);
				array3[num + 3] = vector + new Vector3(width, 0f, -width);
				int num2 = k * 6;
				array4[num2] = num;
				array4[num2 + 1] = num + 1;
				array4[num2 + 2] = num + 2;
				array4[num2 + 3] = num;
				array4[num2 + 4] = num + 2;
				array4[num2 + 5] = num + 3;
			}
			Mesh mesh = new Mesh();
			mesh.name = "DebugUtility_Mesh";
			mesh.vertices = array3;
			mesh.triangles = array4;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			GameObject gameObject = new GameObject("DebugMesh");
			MeshRenderer meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
			meshRenderer.material = DebugUtility.active.defaultMaterial;
			(gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter).mesh = mesh;
		}

		public static void TestMeshLimit()
		{
			Vector3[] array = new Vector3[64000];
			int[] array2 = new int[96000];
			for (int i = 0; i < 16000; i++)
			{
				Vector3 vector = Random.onUnitSphere * 10f;
				int num = i * 4;
				array[num] = vector + new Vector3(-0.1f, 0f, -0.1f);
				array[num + 1] = vector + new Vector3(-0.1f, 0f, 0.1f);
				array[num + 2] = vector + new Vector3(0.1f, 0f, 0.1f);
				array[num + 3] = vector + new Vector3(0.1f, 0f, -0.1f);
				int num2 = i * 6;
				array2[num2] = num;
				array2[num2 + 1] = num + 1;
				array2[num2 + 2] = num + 2;
				array2[num2 + 3] = num;
				array2[num2 + 4] = num + 2;
				array2[num2 + 5] = num + 3;
			}
			Mesh mesh = new Mesh();
			mesh.name = "DebugUtility_Mesh";
			mesh.vertices = array;
			mesh.triangles = array2;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			GameObject gameObject = new GameObject("DebugMesh");
			gameObject.AddComponent(typeof(MeshRenderer));
			(gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter).mesh = mesh;
		}

		public Material defaultMaterial;

		public static DebugUtility active;

		public float offset = 0.2f;

		public bool optimizeMeshes;
	}
}

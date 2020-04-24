using System;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

namespace SDG.Unturned
{
	public class HighlighterTool
	{
		public static void color(Transform target, Color color)
		{
			if (target == null)
			{
				return;
			}
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().material.color = color;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = target.FindChild("Model_" + i);
					if (!(transform == null))
					{
						if (transform.GetComponent<Renderer>() != null)
						{
							transform.GetComponent<Renderer>().material.color = color;
						}
					}
				}
			}
		}

		public static void destroyMaterials(Transform target)
		{
			if (target == null)
			{
				return;
			}
			if (target.GetComponent<Renderer>() != null)
			{
				Object.DestroyImmediate(target.GetComponent<Renderer>().material);
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = target.FindChild("Model_" + i);
					if (!(transform == null))
					{
						if (transform.GetComponent<Renderer>() != null)
						{
							Object.DestroyImmediate(transform.GetComponent<Renderer>().material);
						}
					}
				}
			}
		}

		public static void help(Transform target, bool isValid)
		{
			HighlighterTool.help(target, isValid, false);
		}

		public static void help(Transform target, bool isValid, bool isRecursive)
		{
			Material sharedMaterial = (!isValid) ? ((Material)Resources.Load("Materials/Bad")) : ((Material)Resources.Load("Materials/Good"));
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().sharedMaterial = sharedMaterial;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform;
					if (isRecursive)
					{
						transform = target.FindChildRecursive("Model_" + i);
					}
					else
					{
						transform = target.FindChild("Model_" + i);
					}
					if (!(transform == null))
					{
						if (transform.GetComponent<Renderer>() != null)
						{
							transform.GetComponent<Renderer>().sharedMaterial = sharedMaterial;
						}
					}
				}
			}
		}

		public static void guide(Transform target)
		{
			Material sharedMaterial = (Material)Resources.Load("Materials/Guide");
			HighlighterTool.renderers.Clear();
			target.GetComponentsInChildren<Renderer>(true, HighlighterTool.renderers);
			for (int i = 0; i < HighlighterTool.renderers.Count; i++)
			{
				if (!(HighlighterTool.renderers[i].transform != target) || HighlighterTool.renderers[i].name.IndexOf("Model") != -1)
				{
					HighlighterTool.renderers[i].sharedMaterial = sharedMaterial;
				}
			}
			List<Collider> list = new List<Collider>();
			target.GetComponentsInChildren<Collider>(list);
			for (int j = 0; j < list.Count; j++)
			{
				Object.Destroy(list[j]);
			}
		}

		public static void highlight(Transform target, Color color)
		{
			if (target.CompareTag("Player") || target.CompareTag("Enemy") || target.CompareTag("Zombie") || target.CompareTag("Animal") || target.CompareTag("Agent"))
			{
				return;
			}
			Highlighter highlighter = target.GetComponent<Highlighter>();
			if (highlighter == null)
			{
				highlighter = target.gameObject.AddComponent<Highlighter>();
			}
			highlighter.ConstantOn(color);
			highlighter.SeeThroughOff();
		}

		public static void unhighlight(Transform target)
		{
			Highlighter component = target.GetComponent<Highlighter>();
			if (component == null)
			{
				return;
			}
			Object.DestroyImmediate(component);
		}

		public static void skin(Transform target, Material skin)
		{
			if (target.GetComponent<Renderer>() != null)
			{
				target.GetComponent<Renderer>().sharedMaterial = skin;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = target.FindChild("Model_" + i);
					if (!(transform == null))
					{
						if (transform.GetComponent<Renderer>() != null)
						{
							transform.GetComponent<Renderer>().sharedMaterial = skin;
						}
					}
				}
			}
		}

		public static Material getMaterial(Transform target)
		{
			if (target == null)
			{
				return null;
			}
			Renderer component = target.GetComponent<Renderer>();
			if (component != null)
			{
				return component.sharedMaterial;
			}
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.FindChild("Model_" + i);
				if (transform == null)
				{
					return null;
				}
				component = transform.GetComponent<Renderer>();
				if (component != null)
				{
					return component.sharedMaterial;
				}
			}
			return null;
		}

		public static Material getMaterialInstance(Transform target)
		{
			if (target == null)
			{
				return null;
			}
			Renderer component = target.GetComponent<Renderer>();
			if (component != null)
			{
				return component.material;
			}
			Material material = null;
			Material material2 = null;
			for (int i = 0; i < 4; i++)
			{
				Transform transform = target.FindChild("Model_" + i);
				if (transform == null)
				{
					break;
				}
				component = transform.GetComponent<Renderer>();
				if (component != null)
				{
					if (material == null)
					{
						material2 = component.sharedMaterial;
						material = component.material;
					}
					else if (component.sharedMaterial == material2)
					{
						component.sharedMaterial = material;
					}
				}
			}
			return material;
		}

		public static void rematerialize(Transform target, Material newMaterial, out Material oldMaterial)
		{
			oldMaterial = null;
			Renderer component = target.GetComponent<Renderer>();
			if (component != null)
			{
				oldMaterial = component.sharedMaterial;
				component.sharedMaterial = newMaterial;
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					Transform transform = target.FindChild("Model_" + i);
					if (!(transform == null))
					{
						component = transform.GetComponent<Renderer>();
						if (component != null)
						{
							oldMaterial = component.sharedMaterial;
							component.sharedMaterial = newMaterial;
						}
					}
				}
			}
		}

		private static HighlighterBatch getBatchable()
		{
			if (HighlighterTool.batchablePoolIndex < HighlighterTool.batchablePool.Count)
			{
				HighlighterBatch highlighterBatch = HighlighterTool.batchablePool[HighlighterTool.batchablePoolIndex];
				highlighterBatch.texture = null;
				highlighterBatch.meshes.Clear();
				highlighterBatch.renderers.Clear();
				HighlighterTool.batchablePoolIndex++;
				return highlighterBatch;
			}
			HighlighterBatch highlighterBatch2 = new HighlighterBatch();
			HighlighterTool.batchablePool.Add(highlighterBatch2);
			HighlighterTool.batchablePoolIndex++;
			return highlighterBatch2;
		}

		private static void checkBatchable(List<GameObject> list, MeshFilter mesh, MeshRenderer renderer)
		{
			if (mesh != null && mesh.sharedMesh != null && renderer != null && renderer.sharedMaterials != null && renderer.sharedMaterials.Length == 1)
			{
				Texture2D texture2D = (Texture2D)renderer.sharedMaterial.mainTexture;
				HighlighterShaderGroup highlighterShaderGroup = null;
				if (texture2D != null && texture2D.wrapMode == 1 && texture2D.width <= 128 && texture2D.height <= 128)
				{
					if (renderer.sharedMaterial.shader.name == "Standard")
					{
						if (renderer.sharedMaterial.GetFloat("_Mode") == 0f && texture2D.filterMode == null)
						{
							highlighterShaderGroup = HighlighterTool.batchableOpaque;
						}
					}
					else if (renderer.sharedMaterial.shader.name == "Custom/Card")
					{
						highlighterShaderGroup = HighlighterTool.batchableCard;
					}
					else if (renderer.sharedMaterial.shader.name == "Custom/Foliage" && texture2D.filterMode == 2)
					{
						highlighterShaderGroup = HighlighterTool.batchableFoliage;
					}
				}
				if (highlighterShaderGroup != null)
				{
					HighlighterBatch highlighterBatch = null;
					if (!highlighterShaderGroup.batchableTextures.TryGetValue(texture2D, out highlighterBatch))
					{
						highlighterBatch = HighlighterTool.getBatchable();
						highlighterBatch.texture = texture2D;
						highlighterShaderGroup.batchableTextures.Add(texture2D, highlighterBatch);
					}
					if (highlighterBatch != null)
					{
						List<MeshFilter> list2;
						if (!highlighterBatch.meshes.TryGetValue(mesh.sharedMesh, out list2))
						{
							list2 = new List<MeshFilter>();
							highlighterBatch.meshes.Add(mesh.sharedMesh, list2);
						}
						list2.Add(mesh);
						highlighterBatch.renderers.Add(renderer);
						list.Add(mesh.gameObject);
					}
				}
				else
				{
					List<GameObject> list3 = null;
					if (!HighlighterTool.batchableMaterials.TryGetValue(renderer.sharedMaterial, out list3))
					{
						list3 = new List<GameObject>();
						HighlighterTool.batchableMaterials.Add(renderer.sharedMaterial, list3);
					}
					list3.Add(mesh.gameObject);
				}
			}
		}

		private static void batch(HighlighterShaderGroup group)
		{
			Material materialTemplate = group.materialTemplate;
			Dictionary<Texture2D, HighlighterBatch> batchableTextures = group.batchableTextures;
			if (batchableTextures.Count > 0)
			{
				Texture2D texture2D = new Texture2D(16, 16);
				texture2D.name = "Atlas";
				texture2D.wrapMode = 1;
				texture2D.filterMode = group.filterMode;
				HighlighterBatch[] array = new HighlighterBatch[batchableTextures.Count];
				batchableTextures.Values.CopyTo(array, 0);
				Texture2D[] array2 = new Texture2D[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					HighlighterBatch highlighterBatch = array[i];
					Texture2D texture = highlighterBatch.texture;
					RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0, 0, 0);
					Graphics.Blit(texture, temporary);
					RenderTexture active = RenderTexture.active;
					RenderTexture.active = temporary;
					Texture2D texture2D2 = new Texture2D(texture.width, texture.height, 5, false, true);
					texture2D2.name = "Copy";
					texture2D2.ReadPixels(new Rect(0f, 0f, (float)texture.width, (float)texture.height), 0, 0);
					texture2D2.Apply();
					array2[i] = texture2D2;
					RenderTexture.active = active;
					RenderTexture.ReleaseTemporary(temporary);
				}
				Rect[] array3 = texture2D.PackTextures(array2, 0, 1024, true);
				if (array3 != null)
				{
					Material material = Object.Instantiate<Material>(materialTemplate);
					material.name = "Material";
					material.mainTexture = texture2D;
					for (int j = 0; j < array.Length; j++)
					{
						HighlighterBatch highlighterBatch2 = array[j];
						List<MeshFilter>[] array4 = new List<MeshFilter>[highlighterBatch2.meshes.Count];
						highlighterBatch2.meshes.Values.CopyTo(array4, 0);
						for (int k = 0; k < array4.Length; k++)
						{
							Mesh mesh = array4[k][0].mesh;
							Vector2[] uv = mesh.uv;
							for (int l = 0; l < uv.Length; l++)
							{
								uv[l].x = array3[j].x + uv[l].x * array3[j].width;
								uv[l].y = array3[j].y + uv[l].y * array3[j].height;
							}
							mesh.uv = uv;
							if (array4[k].Count > 1)
							{
								for (int m = 1; m < array4[k].Count; m++)
								{
									array4[k][m].sharedMesh = mesh;
								}
							}
						}
						for (int n = 0; n < highlighterBatch2.renderers.Count; n++)
						{
							highlighterBatch2.renderers[n].sharedMaterial = material;
						}
					}
				}
				else
				{
					Object.DestroyImmediate(texture2D);
				}
				for (int num = 0; num < array2.Length; num++)
				{
					Object.DestroyImmediate(array2[num]);
				}
			}
		}

		public static void beginBatch()
		{
			if (HighlighterTool.batchableOpaque == null)
			{
				HighlighterTool.batchableOpaque = new HighlighterShaderGroup();
				HighlighterTool.batchableOpaque.materialTemplate = (Material)Resources.Load("Level/Opaque");
			}
			if (HighlighterTool.batchableCard == null)
			{
				HighlighterTool.batchableCard = new HighlighterShaderGroup();
				HighlighterTool.batchableCard.materialTemplate = (Material)Resources.Load("Level/Card");
			}
			if (HighlighterTool.batchableFoliage == null)
			{
				HighlighterTool.batchableFoliage = new HighlighterShaderGroup();
				HighlighterTool.batchableFoliage.materialTemplate = (Material)Resources.Load("Level/Foliage");
				HighlighterTool.batchableFoliage.filterMode = 2;
			}
			HighlighterTool.batchableOpaque.batchableTextures.Clear();
			HighlighterTool.batchableCard.batchableTextures.Clear();
			HighlighterTool.batchableFoliage.batchableTextures.Clear();
			HighlighterTool.batchableGameObjects.Clear();
		}

		public static void collectBatch(List<GameObject> targets)
		{
			if (targets.Count == 0)
			{
				return;
			}
			HighlighterTool.batchableMaterials.Clear();
			List<GameObject> list = new List<GameObject>();
			HighlighterTool.batchableGameObjects.Add(list);
			for (int i = 0; i < targets.Count; i++)
			{
				GameObject gameObject = targets[i];
				HighlighterTool.lods.Clear();
				gameObject.GetComponentsInChildren<MeshFilter>(HighlighterTool.lods);
				for (int j = 0; j < HighlighterTool.lods.Count; j++)
				{
					MeshFilter meshFilter = HighlighterTool.lods[j];
					MeshRenderer component = meshFilter.gameObject.GetComponent<MeshRenderer>();
					HighlighterTool.checkBatchable(list, meshFilter, component);
				}
			}
			if (HighlighterTool.batchableMaterials.Count > 0)
			{
				List<GameObject>[] array = new List<GameObject>[HighlighterTool.batchableMaterials.Count];
				HighlighterTool.batchableMaterials.Values.CopyTo(array, 0);
				for (int k = 0; k < array.Length; k++)
				{
					if (array[k].Count >= 2)
					{
						StaticBatchingUtility.Combine(array[k].ToArray(), Level.roots.gameObject);
					}
				}
			}
		}

		public static void endBatch()
		{
			HighlighterTool.batch(HighlighterTool.batchableOpaque);
			HighlighterTool.batch(HighlighterTool.batchableCard);
			HighlighterTool.batch(HighlighterTool.batchableFoliage);
			for (int i = 0; i < HighlighterTool.batchableGameObjects.Count; i++)
			{
				if (HighlighterTool.batchableGameObjects[i].Count != 0)
				{
					StaticBatchingUtility.Combine(HighlighterTool.batchableGameObjects[i].ToArray(), Level.roots.gameObject);
				}
			}
		}

		private static List<Renderer> renderers = new List<Renderer>();

		private static List<MeshFilter> lods = new List<MeshFilter>();

		private static HighlighterShaderGroup batchableOpaque;

		private static HighlighterShaderGroup batchableCard;

		private static HighlighterShaderGroup batchableFoliage;

		private static List<List<GameObject>> batchableGameObjects = new List<List<GameObject>>();

		private static Dictionary<Material, List<GameObject>> batchableMaterials = new Dictionary<Material, List<GameObject>>();

		private static List<HighlighterBatch> batchablePool = new List<HighlighterBatch>();

		private static int batchablePoolIndex = 0;
	}
}

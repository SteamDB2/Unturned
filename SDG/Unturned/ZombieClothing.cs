using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ZombieClothing
	{
		public static Material ghostMaterial { get; private set; }

		public static Material paint(ushort shirt, ushort pants, bool isMega)
		{
			Texture2D texture2D = Object.Instantiate<Texture2D>((!isMega) ? ZombieClothing.zombieTexture : ZombieClothing.megaTexture);
			texture2D.name = string.Concat(new object[]
			{
				"Zombie_",
				(!isMega) ? "Normal" : "Mega",
				"_",
				shirt,
				"_",
				pants
			});
			texture2D.hideFlags = 61;
			if (shirt != 0)
			{
				ItemShirtAsset itemShirtAsset = (ItemShirtAsset)Assets.find(EAssetType.ITEM, shirt);
				if (itemShirtAsset != null)
				{
					for (int i = 0; i < itemShirtAsset.shirt.width; i++)
					{
						for (int j = 0; j < itemShirtAsset.shirt.height; j++)
						{
							if (itemShirtAsset.shirt.GetPixel(i, j).a > 0f)
							{
								texture2D.SetPixel(i, j, itemShirtAsset.shirt.GetPixel(i, j));
							}
						}
					}
				}
			}
			if (pants != 0)
			{
				ItemPantsAsset itemPantsAsset = (ItemPantsAsset)Assets.find(EAssetType.ITEM, pants);
				if (itemPantsAsset != null)
				{
					for (int k = 0; k < itemPantsAsset.pants.width; k++)
					{
						for (int l = 0; l < itemPantsAsset.pants.height; l++)
						{
							if (itemPantsAsset.pants.GetPixel(k, l).a > 0f)
							{
								texture2D.SetPixel(k, l, itemPantsAsset.pants.GetPixel(k, l));
							}
						}
					}
				}
			}
			texture2D.Apply();
			Material material = new Material(ZombieClothing.shader);
			material.name = string.Concat(new object[]
			{
				"Zombie_",
				(!isMega) ? "Normal" : "Mega",
				"_",
				shirt,
				"_",
				pants
			});
			material.hideFlags = 61;
			material.mainTexture = texture2D;
			material.SetFloat("_Glossiness", 0f);
			return material;
		}

		public static void apply(Transform zombie, bool isMega, SkinnedMeshRenderer renderer_0, SkinnedMeshRenderer renderer_1, byte type, byte shirt, byte pants, byte hat, byte gear, out Transform attachmentModel_0, out Transform attachmentModel_1)
		{
			attachmentModel_0 = null;
			attachmentModel_1 = null;
			Transform transform = zombie.FindChild("Skeleton");
			Transform transform2 = transform.FindChild("Spine");
			Transform transform3 = transform2.FindChild("Skull");
			ZombieTable zombieTable = LevelZombies.tables[(int)type];
			if (shirt == 255)
			{
				shirt = (byte)zombieTable.slots[0].table.Count;
			}
			if (pants == 255)
			{
				pants = (byte)zombieTable.slots[1].table.Count;
			}
			Material sharedMaterial = ZombieClothing.clothes[(int)type][(int)shirt, (int)pants];
			if (renderer_0 != null)
			{
				renderer_0.sharedMesh = ((!isMega) ? ZombieClothing.zombieMesh_0 : ZombieClothing.megaMesh_0);
				renderer_0.sharedMaterial = sharedMaterial;
			}
			if (renderer_1 != null)
			{
				renderer_1.sharedMesh = ((!isMega) ? ZombieClothing.zombieMesh_1 : ZombieClothing.megaMesh_1);
				renderer_1.sharedMaterial = sharedMaterial;
			}
			Transform transform4 = transform3.FindChild("Hat");
			if (transform4 != null)
			{
				Object.Destroy(transform4.gameObject);
			}
			Transform transform5 = transform2.FindChild("Backpack");
			if (transform5 != null)
			{
				Object.Destroy(transform5.gameObject);
			}
			Transform transform6 = transform2.FindChild("Vest");
			if (transform6 != null)
			{
				Object.Destroy(transform6.gameObject);
			}
			Transform transform7 = transform3.FindChild("Mask");
			if (transform7 != null)
			{
				Object.Destroy(transform7.gameObject);
			}
			Transform transform8 = transform3.FindChild("Glasses");
			if (transform8 != null)
			{
				Object.Destroy(transform8.gameObject);
			}
			if (hat != 255)
			{
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, zombieTable.slots[2].table[(int)hat].item);
				if (itemAsset != null)
				{
					if (itemAsset.type == EItemType.HAT)
					{
						transform4 = Object.Instantiate<GameObject>(((ItemHatAsset)itemAsset).hat).transform;
						transform4.name = "Hat";
						transform4.transform.parent = transform3;
						transform4.transform.localPosition = Vector3.zero;
						transform4.transform.localRotation = Quaternion.identity;
						transform4.transform.localScale = Vector3.one;
						Object.Destroy(transform4.GetComponent<Collider>());
						attachmentModel_0 = transform4.transform;
					}
					else if (itemAsset.type == EItemType.BACKPACK)
					{
						transform5 = Object.Instantiate<GameObject>(((ItemBackpackAsset)itemAsset).backpack).transform;
						transform5.name = "Backpack";
						transform5.transform.parent = transform2;
						transform5.transform.localPosition = Vector3.zero;
						transform5.transform.localRotation = Quaternion.identity;
						transform5.transform.localScale = ((!isMega) ? Vector3.one : new Vector3(1.05f, 1f, 1.1f));
						Object.Destroy(transform5.GetComponent<Collider>());
						attachmentModel_0 = transform5.transform;
					}
					else if (itemAsset.type == EItemType.VEST)
					{
						transform6 = Object.Instantiate<GameObject>(((ItemVestAsset)itemAsset).vest).transform;
						transform6.name = "Vest";
						transform6.transform.parent = transform2;
						transform6.transform.localPosition = Vector3.zero;
						transform6.transform.localRotation = Quaternion.identity;
						transform6.transform.localScale = ((!isMega) ? Vector3.one : new Vector3(1.05f, 1f, 1.1f));
						Object.Destroy(transform6.GetComponent<Collider>());
						attachmentModel_0 = transform6.transform;
					}
					else if (itemAsset.type == EItemType.MASK)
					{
						transform7 = Object.Instantiate<GameObject>(((ItemMaskAsset)itemAsset).mask).transform;
						transform7.name = "Mask";
						transform7.transform.parent = transform3;
						transform7.transform.localPosition = Vector3.zero;
						transform7.transform.localRotation = Quaternion.identity;
						transform7.transform.localScale = Vector3.one;
						Object.Destroy(transform7.GetComponent<Collider>());
						attachmentModel_0 = transform7.transform;
					}
					else if (itemAsset.type == EItemType.GLASSES)
					{
						transform8 = Object.Instantiate<GameObject>(((ItemGlassesAsset)itemAsset).glasses).transform;
						transform8.name = "Glasses";
						transform8.transform.parent = transform3;
						transform8.transform.localPosition = Vector3.zero;
						transform8.transform.localRotation = Quaternion.identity;
						transform8.transform.localScale = Vector3.one;
						Object.Destroy(transform8.GetComponent<Collider>());
						attachmentModel_0 = transform8.transform;
					}
				}
			}
			if (gear != 255)
			{
				ItemAsset itemAsset2 = (ItemAsset)Assets.find(EAssetType.ITEM, zombieTable.slots[3].table[(int)gear].item);
				if (itemAsset2 != null)
				{
					if (itemAsset2.type == EItemType.HAT)
					{
						transform4 = Object.Instantiate<GameObject>(((ItemHatAsset)itemAsset2).hat).transform;
						transform4.name = "Hat";
						transform4.transform.parent = transform3;
						transform4.transform.localPosition = Vector3.zero;
						transform4.transform.localRotation = Quaternion.identity;
						transform4.transform.localScale = Vector3.one;
						Object.Destroy(transform4.GetComponent<Collider>());
						attachmentModel_1 = transform4.transform;
					}
					else if (itemAsset2.type == EItemType.BACKPACK)
					{
						transform5 = Object.Instantiate<GameObject>(((ItemBackpackAsset)itemAsset2).backpack).transform;
						transform5.name = "Backpack";
						transform5.transform.parent = transform2;
						transform5.transform.localPosition = Vector3.zero;
						transform5.transform.localRotation = Quaternion.identity;
						transform5.transform.localScale = ((!isMega) ? Vector3.one : new Vector3(1.05f, 1f, 1.1f));
						Object.Destroy(transform5.GetComponent<Collider>());
						attachmentModel_1 = transform5.transform;
					}
					else if (itemAsset2.type == EItemType.VEST)
					{
						transform6 = Object.Instantiate<GameObject>(((ItemVestAsset)itemAsset2).vest).transform;
						transform6.name = "Vest";
						transform6.transform.parent = transform2;
						transform6.transform.localPosition = Vector3.zero;
						transform6.transform.localRotation = Quaternion.identity;
						transform6.transform.localScale = ((!isMega) ? Vector3.one : new Vector3(1.05f, 1f, 1.1f));
						Object.Destroy(transform6.GetComponent<Collider>());
						attachmentModel_1 = transform6.transform;
					}
					else if (itemAsset2.type == EItemType.MASK)
					{
						transform7 = Object.Instantiate<GameObject>(((ItemMaskAsset)itemAsset2).mask).transform;
						transform7.name = "Mask";
						transform7.transform.parent = transform3;
						transform7.transform.localPosition = Vector3.zero;
						transform7.transform.localRotation = Quaternion.identity;
						transform7.transform.localScale = Vector3.one;
						Object.Destroy(transform7.GetComponent<Collider>());
						attachmentModel_1 = transform7.transform;
					}
					else if (itemAsset2.type == EItemType.GLASSES)
					{
						transform8 = Object.Instantiate<GameObject>(((ItemGlassesAsset)itemAsset2).glasses).transform;
						transform8.name = "Glasses";
						transform8.transform.parent = transform3;
						transform8.transform.localPosition = Vector3.zero;
						transform8.transform.localRotation = Quaternion.identity;
						transform8.transform.localScale = Vector3.one;
						Object.Destroy(transform8.GetComponent<Collider>());
						attachmentModel_1 = transform8.transform;
					}
				}
			}
		}

		public static void build()
		{
			if (ZombieClothing.ghostMaterial == null)
			{
				ZombieClothing.ghostMaterial = (Material)Resources.Load("Characters/Ghost");
			}
			if (ZombieClothing.megaMesh_0 == null)
			{
				ZombieClothing.megaMesh_0 = ((GameObject)Resources.Load("Characters/Mega_0")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.megaMesh_1 == null)
			{
				ZombieClothing.megaMesh_1 = ((GameObject)Resources.Load("Characters/Mega_1")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.zombieMesh_0 == null)
			{
				ZombieClothing.zombieMesh_0 = ((GameObject)Resources.Load("Characters/Zombie_0")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.zombieMesh_1 == null)
			{
				ZombieClothing.zombieMesh_1 = ((GameObject)Resources.Load("Characters/Zombie_1")).GetComponent<MeshFilter>().sharedMesh;
			}
			if (ZombieClothing.megaTexture == null)
			{
				ZombieClothing.megaTexture = (Texture2D)Resources.Load("Characters/Mega");
			}
			if (ZombieClothing.zombieTexture == null)
			{
				ZombieClothing.zombieTexture = (Texture2D)Resources.Load("Characters/Zombie");
			}
			if (ZombieClothing.shader == null)
			{
				ZombieClothing.shader = Shader.Find("Standard");
			}
			if (ZombieClothing.clothes != null)
			{
				for (int i = 0; i < ZombieClothing.clothes.GetLength(0); i++)
				{
					for (int j = 0; j < ZombieClothing.clothes[i].GetLength(0); j++)
					{
						for (int k = 0; k < ZombieClothing.clothes[i].GetLength(1); k++)
						{
							if (ZombieClothing.clothes[i][j, k] != null)
							{
								Object.DestroyImmediate(ZombieClothing.clothes[i][j, k].mainTexture);
								Object.DestroyImmediate(ZombieClothing.clothes[i][j, k]);
								ZombieClothing.clothes[i][j, k] = null;
							}
						}
					}
				}
			}
			if (LevelZombies.tables == null)
			{
				ZombieClothing.clothes = null;
				return;
			}
			ZombieClothing.clothes = new Material[LevelZombies.tables.Count][,];
			byte b = 0;
			while ((int)b < LevelZombies.tables.Count)
			{
				ZombieTable zombieTable = LevelZombies.tables[(int)b];
				ZombieClothing.clothes[(int)b] = new Material[zombieTable.slots[0].table.Count + 1, zombieTable.slots[1].table.Count + 1];
				byte b2 = 0;
				while ((int)b2 < zombieTable.slots[0].table.Count + 1)
				{
					ushort shirt = 0;
					if ((int)b2 < zombieTable.slots[0].table.Count)
					{
						shirt = zombieTable.slots[0].table[(int)b2].item;
					}
					byte b3 = 0;
					while ((int)b3 < zombieTable.slots[1].table.Count + 1)
					{
						ushort pants = 0;
						if ((int)b3 < zombieTable.slots[1].table.Count)
						{
							pants = zombieTable.slots[1].table[(int)b3].item;
						}
						ZombieClothing.clothes[(int)b][(int)b2, (int)b3] = ZombieClothing.paint(shirt, pants, zombieTable.isMega);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
		}

		private static Mesh megaMesh_0;

		private static Mesh megaMesh_1;

		private static Mesh zombieMesh_0;

		private static Mesh zombieMesh_1;

		private static Texture2D megaTexture;

		private static Texture2D zombieTexture;

		private static Shader shader;

		private static Material[][,] clothes;
	}
}

using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class RagdollTool
	{
		private static void applySkeleton(Transform skeleton_0, Transform skeleton_1)
		{
			if (skeleton_0 == null || skeleton_1 == null)
			{
				return;
			}
			for (int i = 0; i < skeleton_1.childCount; i++)
			{
				Transform transform = null;
				Transform child = skeleton_1.GetChild(i);
				for (int j = i; j < skeleton_0.childCount; j++)
				{
					transform = skeleton_0.GetChild(j);
					if (transform.name == child.name)
					{
						break;
					}
				}
				if (transform != null)
				{
					child.localPosition = transform.localPosition;
					child.localRotation = transform.localRotation;
					if (transform.childCount > 0 && child.childCount > 0)
					{
						RagdollTool.applySkeleton(transform, child);
					}
				}
			}
		}

		public static void ragdollPlayer(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, PlayerClothing clothes)
		{
			if (!GraphicsSettings.ragdolls)
			{
				return;
			}
			ragdoll.y += 8f;
			ragdoll.x += Random.Range(-16f, 16f);
			ragdoll.z += Random.Range(-16f, 16f);
			ragdoll *= (float)((!(Player.player != null) || Player.player.skills.boost != EPlayerBoost.FLIGHT) ? 32 : 256);
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Ragdoll_Player"), point + Vector3.up * 0.1f, rotation * Quaternion.Euler(90f, 0f, 0f))).transform;
			transform.name = "Ragdoll";
			transform.parent = Level.effects;
			if (skeleton != null)
			{
				RagdollTool.applySkeleton(skeleton, transform.FindChild("Skeleton"));
			}
			transform.FindChild("Skeleton").FindChild("Spine").GetComponent<Rigidbody>().AddForce(ragdoll);
			Object.Destroy(transform.gameObject, GraphicsSettings.effect);
			if (clothes != null && clothes.thirdClothes != null)
			{
				HumanClothes component = transform.GetComponent<HumanClothes>();
				component.skin = clothes.skin;
				component.color = clothes.color;
				component.face = clothes.face;
				component.hair = clothes.hair;
				component.beard = clothes.beard;
				component.shirt = clothes.shirt;
				component.pants = clothes.pants;
				component.hat = clothes.hat;
				component.backpack = clothes.backpack;
				component.vest = clothes.vest;
				component.mask = clothes.mask;
				component.glasses = clothes.glasses;
				component.visualShirt = clothes.visualShirt;
				component.visualPants = clothes.visualPants;
				component.visualHat = clothes.visualHat;
				component.visualBackpack = clothes.visualBackpack;
				component.visualVest = clothes.visualVest;
				component.visualMask = clothes.visualMask;
				component.visualGlasses = clothes.visualGlasses;
				component.isVisual = clothes.isVisual;
				component.apply();
			}
		}

		public static void ragdollZombie(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, byte type, byte shirt, byte pants, byte hat, byte gear, bool isMega)
		{
			if (!GraphicsSettings.ragdolls)
			{
				return;
			}
			ragdoll.y += 8f;
			ragdoll.x += Random.Range(-16f, 16f);
			ragdoll.z += Random.Range(-16f, 16f);
			ragdoll *= (float)((!(Player.player != null) || Player.player.skills.boost != EPlayerBoost.FLIGHT) ? 32 : 256);
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Ragdoll_Zombie"), point + Vector3.up * 0.1f, rotation * Quaternion.Euler(90f, 0f, 0f))).transform;
			transform.name = "Ragdoll";
			transform.parent = Level.effects;
			if (isMega)
			{
				transform.localScale = Vector3.one * 1.5f;
			}
			else
			{
				transform.localScale = Vector3.one;
			}
			if (skeleton != null)
			{
				RagdollTool.applySkeleton(skeleton, transform.FindChild("Skeleton"));
			}
			transform.FindChild("Skeleton").FindChild("Spine").GetComponent<Rigidbody>().AddForce(ragdoll);
			Object.Destroy(transform.gameObject, GraphicsSettings.effect);
			Transform transform2;
			Transform transform3;
			ZombieClothing.apply(transform, isMega, null, transform.FindChild("Model_1").GetComponent<SkinnedMeshRenderer>(), type, shirt, pants, hat, gear, out transform2, out transform3);
		}

		public static void ragdollAnimal(Vector3 point, Quaternion rotation, Transform skeleton, Vector3 ragdoll, ushort id)
		{
			if (!GraphicsSettings.ragdolls)
			{
				return;
			}
			ragdoll.y += 8f;
			ragdoll.x += Random.Range(-16f, 16f);
			ragdoll.z += Random.Range(-16f, 16f);
			ragdoll *= (float)((!(Player.player != null) || Player.player.skills.boost != EPlayerBoost.FLIGHT) ? 32 : 256);
			AnimalAsset animalAsset = (AnimalAsset)Assets.find(EAssetType.ANIMAL, id);
			if (animalAsset == null)
			{
				return;
			}
			Transform transform = Object.Instantiate<GameObject>(animalAsset.ragdoll, point + Vector3.up * 0.1f, rotation * Quaternion.Euler(0f, 90f, 0f)).transform;
			transform.name = "Ragdoll";
			transform.parent = Level.effects;
			if (skeleton != null)
			{
				RagdollTool.applySkeleton(skeleton, transform.FindChild("Skeleton"));
			}
			transform.FindChild("Skeleton").FindChild("Spine").GetComponent<Rigidbody>().AddForce(ragdoll);
			Object.Destroy(transform.gameObject, GraphicsSettings.effect);
		}
	}
}

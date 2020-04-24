using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class StructureTool : MonoBehaviour
	{
		public static Transform getStructure(ushort id, byte hp)
		{
			ItemStructureAsset asset = (ItemStructureAsset)Assets.find(EAssetType.ITEM, id);
			return StructureTool.getStructure(id, hp, 0UL, 0UL, asset);
		}

		public static Transform getStructure(ushort id, byte hp, ulong owner, ulong group, ItemStructureAsset asset)
		{
			if (asset != null)
			{
				Transform transform;
				if (Dedicator.isDedicated)
				{
					transform = Object.Instantiate<GameObject>(asset.clip).transform;
				}
				else
				{
					transform = Object.Instantiate<GameObject>(asset.structure).transform;
				}
				transform.name = id.ToString();
				if (Provider.isServer)
				{
					Transform transform2 = Object.Instantiate<GameObject>(asset.nav).transform;
					transform2.name = "Nav";
					transform2.parent = transform;
					transform2.localPosition = Vector3.zero;
					transform2.localRotation = Quaternion.identity;
				}
				if (!asset.isUnpickupable)
				{
					Interactable2HP interactable2HP = transform.gameObject.AddComponent<Interactable2HP>();
					interactable2HP.hp = hp;
					Interactable2SalvageStructure interactable2SalvageStructure = transform.gameObject.AddComponent<Interactable2SalvageStructure>();
					interactable2SalvageStructure.hp = interactable2HP;
					interactable2SalvageStructure.owner = owner;
					interactable2SalvageStructure.group = group;
				}
				return transform;
			}
			Transform transform3 = new GameObject().transform;
			transform3.name = id.ToString();
			transform3.tag = "Structure";
			transform3.gameObject.layer = LayerMasks.STRUCTURE;
			return transform3;
		}
	}
}

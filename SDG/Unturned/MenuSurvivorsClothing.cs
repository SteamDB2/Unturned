using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuSurvivorsClothing : MonoBehaviour
	{
		private void onClickedMouse()
		{
			if (!MenuSurvivorsClothingUI.active && !MenuSurvivorsClothingItemUI.active)
			{
				return;
			}
			Ray ray = MainCamera.instance.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			Physics.Raycast(ray, ref raycastHit, 64f, RayMasks.CLOTHING_INTERACT);
			if (raycastHit.transform != null)
			{
				if (raycastHit.transform.CompareTag("Player"))
				{
					ELimb limb = DamageTool.getLimb(raycastHit.transform);
					if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
					{
						if (Characters.active.packagePants != 0UL)
						{
							Characters.package(Characters.active.packagePants);
						}
					}
					else if ((limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM || limb == ELimb.SPINE) && Characters.active.packageShirt != 0UL)
					{
						Characters.package(Characters.active.packageShirt);
					}
				}
				else if (raycastHit.transform.CompareTag("Enemy"))
				{
					if (raycastHit.transform.name == "Hat")
					{
						if (Characters.active.packageHat != 0UL)
						{
							Characters.package(Characters.active.packageHat);
						}
					}
					else if (raycastHit.transform.name == "Glasses")
					{
						if (Characters.active.packageGlasses != 0UL)
						{
							Characters.package(Characters.active.packageGlasses);
						}
					}
					else if (raycastHit.transform.name == "Mask")
					{
						if (Characters.active.packageMask != 0UL)
						{
							Characters.package(Characters.active.packageMask);
						}
					}
					else if (raycastHit.transform.name == "Vest")
					{
						if (Characters.active.packageVest != 0UL)
						{
							Characters.package(Characters.active.packageVest);
						}
					}
					else if (raycastHit.transform.name == "Backpack" && Characters.active.packageBackpack != 0UL)
					{
						Characters.package(Characters.active.packageBackpack);
					}
				}
				if (MenuSurvivorsClothingItemUI.active)
				{
					MenuSurvivorsClothingItemUI.viewItem();
				}
			}
		}

		private void Start()
		{
			if (MenuUI.window == null)
			{
				return;
			}
			SleekWindow window = MenuUI.window;
			window.onClickedMouse = (ClickedMouse)Delegate.Combine(window.onClickedMouse, new ClickedMouse(this.onClickedMouse));
		}
	}
}

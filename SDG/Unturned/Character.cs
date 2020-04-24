using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Character
	{
		public Character()
		{
			this.face = (byte)Random.Range(0, (int)Customization.FACES_FREE);
			this.hair = (byte)Random.Range(0, (int)Customization.HAIRS_FREE);
			this.beard = 0;
			this.skin = Customization.SKINS[Random.Range(0, Customization.SKINS.Length)];
			this.color = Customization.COLORS[Random.Range(0, Customization.COLORS.Length)];
			this.hand = false;
			this.name = Provider.clientName;
			this.nick = Provider.clientName;
			this.group = CSteamID.Nil;
			this.skillset = (EPlayerSkillset)Random.Range(1, (int)Customization.SKILLSETS);
			this.applyHero();
		}

		public Character(ushort newShirt, ushort newPants, ushort newHat, ushort newBackpack, ushort newVest, ushort newMask, ushort newGlasses, ulong newPackageShirt, ulong newPackagePants, ulong newPackageHat, ulong newPackageBackpack, ulong newPackageVest, ulong newPackageMask, ulong newPackageGlasses, ushort newPrimaryItem, byte[] newPrimaryState, ushort newSecondaryItem, byte[] newSecondaryState, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, bool newHand, string newName, string newNick, CSteamID newGroup, EPlayerSkillset newSkillset)
		{
			this.shirt = newShirt;
			this.pants = newPants;
			this.hat = newHat;
			this.backpack = newBackpack;
			this.vest = newVest;
			this.mask = newMask;
			this.glasses = newGlasses;
			this.packageShirt = newPackageShirt;
			this.packagePants = newPackagePants;
			this.packageHat = newPackageHat;
			this.packageBackpack = newPackageBackpack;
			this.packageVest = newPackageVest;
			this.packageMask = newPackageMask;
			this.packageGlasses = newPackageGlasses;
			this.primaryItem = newPrimaryItem;
			this.secondaryItem = newSecondaryItem;
			this.primaryState = newPrimaryState;
			this.secondaryState = newSecondaryState;
			this.face = newFace;
			this.hair = newHair;
			this.beard = newBeard;
			this.skin = newSkin;
			this.color = newColor;
			this.hand = newHand;
			this.name = newName;
			this.nick = newNick;
			this.group = newGroup;
			this.skillset = newSkillset;
		}

		public void applyHero()
		{
			this.shirt = 0;
			this.pants = 0;
			this.hat = 0;
			this.backpack = 0;
			this.vest = 0;
			this.mask = 0;
			this.glasses = 0;
			this.primaryItem = 0;
			this.primaryState = new byte[0];
			this.secondaryItem = 0;
			this.secondaryState = new byte[0];
			for (int i = 0; i < PlayerInventory.SKILLSETS_HERO[(int)((byte)this.skillset)].Length; i++)
			{
				ushort id = PlayerInventory.SKILLSETS_HERO[(int)((byte)this.skillset)][i];
				ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
				if (itemAsset != null)
				{
					switch (itemAsset.type)
					{
					case EItemType.HAT:
						this.hat = id;
						break;
					case EItemType.PANTS:
						this.pants = id;
						break;
					case EItemType.SHIRT:
						this.shirt = id;
						break;
					case EItemType.MASK:
						this.mask = id;
						break;
					case EItemType.BACKPACK:
						this.backpack = id;
						break;
					case EItemType.VEST:
						this.vest = id;
						break;
					case EItemType.GLASSES:
						this.glasses = id;
						break;
					case EItemType.GUN:
					case EItemType.MELEE:
						if (itemAsset.slot == ESlotType.PRIMARY)
						{
							this.primaryItem = id;
							this.primaryState = itemAsset.getState(EItemOrigin.ADMIN);
						}
						else
						{
							this.secondaryItem = id;
							this.secondaryState = itemAsset.getState(EItemOrigin.ADMIN);
						}
						break;
					}
				}
			}
		}

		public ushort shirt;

		public ushort pants;

		public ushort hat;

		public ushort backpack;

		public ushort vest;

		public ushort mask;

		public ushort glasses;

		public ulong packageShirt;

		public ulong packagePants;

		public ulong packageHat;

		public ulong packageBackpack;

		public ulong packageVest;

		public ulong packageMask;

		public ulong packageGlasses;

		public ushort primaryItem;

		public byte[] primaryState;

		public ushort secondaryItem;

		public byte[] secondaryState;

		public byte face;

		public byte hair;

		public byte beard;

		public Color skin;

		public Color color;

		public bool hand;

		public string name;

		public string nick;

		public CSteamID group;

		public EPlayerSkillset skillset;
	}
}

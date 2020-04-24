using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableMannequin : Interactable, IManualOnDestroy
	{
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		public bool isUpdatable
		{
			get
			{
				return Time.realtimeSinceStartup - this.updated > 0.5f;
			}
		}

		public HumanClothes clothes { get; private set; }

		public int visualShirt
		{
			get
			{
				return this.clothes.visualShirt;
			}
		}

		public int visualPants
		{
			get
			{
				return this.clothes.visualPants;
			}
		}

		public int visualHat
		{
			get
			{
				return this.clothes.visualHat;
			}
		}

		public int visualBackpack
		{
			get
			{
				return this.clothes.visualBackpack;
			}
		}

		public int visualVest
		{
			get
			{
				return this.clothes.visualVest;
			}
		}

		public int visualMask
		{
			get
			{
				return this.clothes.visualMask;
			}
		}

		public int visualGlasses
		{
			get
			{
				return this.clothes.visualGlasses;
			}
		}

		public ushort shirt
		{
			get
			{
				return this.clothes.shirt;
			}
		}

		public ushort pants
		{
			get
			{
				return this.clothes.pants;
			}
		}

		public ushort hat
		{
			get
			{
				return this.clothes.hat;
			}
		}

		public ushort backpack
		{
			get
			{
				return this.clothes.backpack;
			}
		}

		public ushort vest
		{
			get
			{
				return this.clothes.vest;
			}
		}

		public ushort mask
		{
			get
			{
				return this.clothes.mask;
			}
		}

		public ushort glasses
		{
			get
			{
				return this.clothes.glasses;
			}
		}

		public bool checkUpdate(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			return (Provider.isServer && !Dedicator.isDedicated) || !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		public byte getComp(bool mirror, byte pose)
		{
			byte b = (!mirror) ? 0 : 1;
			return (byte)((int)b << 7 | (int)pose);
		}

		public void applyPose(byte poseComp)
		{
			this.pose_comp = poseComp;
			this.mirror = ((poseComp >> 7 & 1) == 1);
			this.pose = (poseComp & 127);
		}

		public void setPose(byte poseComp)
		{
			this.applyPose(poseComp);
			this.updatePose();
		}

		public void rebuildState()
		{
			Block block = new Block();
			block.write(this.owner, this.group);
			block.writeInt32(this.visualShirt);
			block.writeInt32(this.visualPants);
			block.writeInt32(this.visualHat);
			block.writeInt32(this.visualBackpack);
			block.writeInt32(this.visualVest);
			block.writeInt32(this.visualMask);
			block.writeInt32(this.visualGlasses);
			block.writeUInt16(this.clothes.shirt);
			block.writeByte(this.shirtQuality);
			block.writeUInt16(this.clothes.pants);
			block.writeByte(this.pantsQuality);
			block.writeUInt16(this.clothes.hat);
			block.writeByte(this.hatQuality);
			block.writeUInt16(this.clothes.backpack);
			block.writeByte(this.backpackQuality);
			block.writeUInt16(this.clothes.vest);
			block.writeByte(this.vestQuality);
			block.writeUInt16(this.clothes.mask);
			block.writeByte(this.maskQuality);
			block.writeUInt16(this.clothes.glasses);
			block.writeByte(this.glassesQuality);
			block.writeByteArray(this.shirtState);
			block.writeByteArray(this.pantsState);
			block.writeByteArray(this.hatState);
			block.writeByteArray(this.backpackState);
			block.writeByteArray(this.vestState);
			block.writeByteArray(this.maskState);
			block.writeByteArray(this.glassesState);
			block.writeByte(this.pose_comp);
			int size;
			byte[] bytes = block.getBytes(out size);
			BarricadeManager.updateState(base.transform, bytes, size);
			this.updated = Time.realtimeSinceStartup;
		}

		public void updateVisuals(int newVisualShirt, int newVisualPants, int newVisualHat, int newVisualBackpack, int newVisualVest, int newVisualMask, int newVisualGlasses)
		{
			this.clothes.visualShirt = newVisualShirt;
			this.clothes.visualPants = newVisualPants;
			this.clothes.visualHat = newVisualHat;
			this.clothes.visualBackpack = newVisualBackpack;
			this.clothes.visualVest = newVisualVest;
			this.clothes.visualMask = newVisualMask;
			this.clothes.visualGlasses = newVisualGlasses;
		}

		public void clearVisuals()
		{
			this.updateVisuals(0, 0, 0, 0, 0, 0, 0);
		}

		public void updateClothes(ushort newShirt, byte newShirtQuality, byte[] newShirtState, ushort newPants, byte newPantsQuality, byte[] newPantsState, ushort newHat, byte newHatQuality, byte[] newHatState, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState, ushort newVest, byte newVestQuality, byte[] newVestState, ushort newMask, byte newMaskQuality, byte[] newMaskState, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
		{
			this.clothes.shirt = newShirt;
			this.shirtQuality = newShirtQuality;
			this.shirtState = newShirtState;
			this.clothes.pants = newPants;
			this.pantsQuality = newPantsQuality;
			this.pantsState = newPantsState;
			this.clothes.hat = newHat;
			this.hatQuality = newHatQuality;
			this.hatState = newHatState;
			this.clothes.backpack = newBackpack;
			this.backpackQuality = newBackpackQuality;
			this.backpackState = newBackpackState;
			this.clothes.vest = newVest;
			this.vestQuality = newVestQuality;
			this.vestState = newVestState;
			this.clothes.mask = newMask;
			this.maskQuality = newMaskQuality;
			this.maskState = newMaskState;
			this.clothes.glasses = newGlasses;
			this.glassesQuality = newGlassesQuality;
			this.glassesState = newGlassesState;
		}

		public void dropClothes()
		{
			if (this.shirt != 0)
			{
				ItemManager.dropItem(new Item(this.shirt, 1, this.shirtQuality, this.shirtState), base.transform.position, false, true, true);
			}
			if (this.pants != 0)
			{
				ItemManager.dropItem(new Item(this.pants, 1, this.pantsQuality, this.pantsState), base.transform.position, false, true, true);
			}
			if (this.hat != 0)
			{
				ItemManager.dropItem(new Item(this.hat, 1, this.hatQuality, this.hatState), base.transform.position, false, true, true);
			}
			if (this.backpack != 0)
			{
				ItemManager.dropItem(new Item(this.backpack, 1, this.backpackQuality, this.backpackState), base.transform.position, false, true, true);
			}
			if (this.vest != 0)
			{
				ItemManager.dropItem(new Item(this.vest, 1, this.vestQuality, this.vestState), base.transform.position, false, true, true);
			}
			if (this.mask != 0)
			{
				ItemManager.dropItem(new Item(this.mask, 1, this.maskQuality, this.maskState), base.transform.position, false, true, true);
			}
			if (this.glasses != 0)
			{
				ItemManager.dropItem(new Item(this.glasses, 1, this.glassesQuality, this.glassesState), base.transform.position, false, true, true);
			}
			this.clearClothes();
		}

		public void clearClothes()
		{
			this.updateClothes(0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0], 0, 0, new byte[0]);
		}

		public void updatePose()
		{
			string text;
			switch (this.pose)
			{
			case 0:
				text = "T";
				break;
			case 1:
				text = "Classic";
				break;
			case 2:
				text = "Lie";
				break;
			default:
				return;
			}
			if (this.anim != null)
			{
				this.anim.transform.localScale = new Vector3((float)((!this.mirror) ? 1 : -1), 1f, 1f);
				this.anim.Play(text);
			}
		}

		public void updateState(byte[] state)
		{
			Block block = new Block(state);
			this._owner = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
			this._group = new CSteamID((ulong)block.read(Types.UINT64_TYPE));
			this.clothes.skin = new Color32(210, 210, 210, byte.MaxValue);
			this.clothes.visualShirt = block.readInt32();
			this.clothes.visualPants = block.readInt32();
			this.clothes.visualHat = block.readInt32();
			this.clothes.visualBackpack = block.readInt32();
			this.clothes.visualVest = block.readInt32();
			this.clothes.visualMask = block.readInt32();
			this.clothes.visualGlasses = block.readInt32();
			this.clothes.shirt = block.readUInt16();
			this.shirtQuality = block.readByte();
			this.clothes.pants = block.readUInt16();
			this.pantsQuality = block.readByte();
			this.clothes.hat = block.readUInt16();
			this.hatQuality = block.readByte();
			this.clothes.backpack = block.readUInt16();
			this.backpackQuality = block.readByte();
			this.clothes.vest = block.readUInt16();
			this.vestQuality = block.readByte();
			this.clothes.mask = block.readUInt16();
			this.maskQuality = block.readByte();
			this.clothes.glasses = block.readUInt16();
			this.glassesQuality = block.readByte();
			this.shirtState = block.readByteArray();
			this.pantsState = block.readByteArray();
			this.hatState = block.readByteArray();
			this.backpackState = block.readByteArray();
			this.vestState = block.readByteArray();
			this.maskState = block.readByteArray();
			this.glassesState = block.readByteArray();
			this.clothes.apply();
			this.setPose(block.readByte());
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			Transform transform = base.transform.FindChild("Root");
			this.anim = transform.GetComponent<Animation>();
			this.clothes = transform.GetComponent<HumanClothes>();
			this.updateState(state);
		}

		public override bool checkUseable()
		{
			return this.checkUpdate(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		public override void use()
		{
			if (Input.GetKey(ControlsSettings.other))
			{
				if (Player.player.equipment.useable is UseableClothing)
				{
					BarricadeManager.updateMannequin(base.transform, EMannequinUpdateMode.ADD);
				}
				else
				{
					BarricadeManager.updateMannequin(base.transform, EMannequinUpdateMode.REMOVE);
				}
			}
			else
			{
				PlayerBarricadeMannequinUI.open(this);
				PlayerLifeUI.close();
			}
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.USE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = string.Empty;
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		public void ManualOnDestroy()
		{
			if (!Provider.isServer)
			{
				return;
			}
			this.dropClothes();
		}

		private CSteamID _owner;

		private CSteamID _group;

		private bool isLocked;

		public byte pose_comp;

		public bool mirror;

		public byte pose;

		private float updated;

		private Animation anim;

		public byte shirtQuality;

		public byte pantsQuality;

		public byte hatQuality;

		public byte backpackQuality;

		public byte vestQuality;

		public byte maskQuality;

		public byte glassesQuality;

		public byte[] shirtState;

		public byte[] pantsState;

		public byte[] hatState;

		public byte[] backpackState;

		public byte[] vestState;

		public byte[] maskState;

		public byte[] glassesState;
	}
}

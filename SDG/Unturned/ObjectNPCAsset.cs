using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ObjectNPCAsset : ObjectAsset
	{
		public ObjectNPCAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			this.npcName = localization.format("Character");
			this.npcName = ItemTool.filterRarityRichText(this.npcName);
			this.shirt = data.readUInt16("Shirt");
			this.pants = data.readUInt16("Pants");
			this.hat = data.readUInt16("Hat");
			this.backpack = data.readUInt16("Backpack");
			this.vest = data.readUInt16("Vest");
			this.mask = data.readUInt16("Mask");
			this.glasses = data.readUInt16("Glasses");
			this.face = data.readByte("Face");
			this.hair = data.readByte("Hair");
			this.beard = data.readByte("Beard");
			this.skin = Palette.hex(data.readString("Color_Skin"));
			this.color = Palette.hex(data.readString("Color_Hair"));
			this.isBackward = data.has("Backward");
			this.primary = data.readUInt16("Primary");
			this.secondary = data.readUInt16("Secondary");
			this.tertiary = data.readUInt16("Tertiary");
			if (data.has("Equipped"))
			{
				this.equipped = (ESlotType)Enum.Parse(typeof(ESlotType), data.readString("Equipped"), true);
			}
			else
			{
				this.equipped = ESlotType.NONE;
			}
			this.dialogue = data.readUInt16("Dialogue");
			if (data.has("Pose"))
			{
				this.pose = (ENPCPose)Enum.Parse(typeof(ENPCPose), data.readString("Pose"), true);
			}
			else
			{
				this.pose = ENPCPose.STAND;
			}
			if (data.has("Pose_Lean"))
			{
				this.poseLean = data.readSingle("Pose_Lean");
			}
			if (data.has("Pose_Pitch"))
			{
				this.posePitch = data.readSingle("Pose_Pitch");
			}
			else
			{
				this.posePitch = 90f;
			}
			if (data.has("Pose_Head_Offset"))
			{
				this.poseHeadOffset = data.readSingle("Pose_Head_Offset");
			}
			else if (this.pose == ENPCPose.CROUCH)
			{
				this.poseHeadOffset = 0.1f;
			}
		}

		public string npcName { get; protected set; }

		public ushort shirt { get; protected set; }

		public ushort pants { get; protected set; }

		public ushort hat { get; protected set; }

		public ushort backpack { get; protected set; }

		public ushort vest { get; protected set; }

		public ushort mask { get; protected set; }

		public ushort glasses { get; protected set; }

		public byte face { get; protected set; }

		public byte hair { get; protected set; }

		public byte beard { get; protected set; }

		public Color skin { get; protected set; }

		public Color color { get; protected set; }

		public bool isBackward { get; protected set; }

		public ushort primary { get; protected set; }

		public ushort secondary { get; protected set; }

		public ushort tertiary { get; protected set; }

		public ESlotType equipped { get; protected set; }

		public ushort dialogue { get; protected set; }

		public ENPCPose pose { get; protected set; }

		public float poseLean { get; protected set; }

		public float posePitch { get; protected set; }

		public float poseHeadOffset { get; protected set; }
	}
}

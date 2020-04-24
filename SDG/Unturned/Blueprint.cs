using System;

namespace SDG.Unturned
{
	public class Blueprint
	{
		public Blueprint(ushort newSource, byte newID, EBlueprintType newType, BlueprintSupply[] newSupplies, BlueprintOutput[] newOutputs, ushort newTool, bool newToolCritical, ushort newBuild, byte newLevel, EBlueprintSkill newSkill, bool newTransferState, string newMap)
		{
			this._source = newSource;
			this._id = newID;
			this._type = newType;
			this._supplies = newSupplies;
			this._outputs = newOutputs;
			this._tool = newTool;
			this._toolCritical = newToolCritical;
			this._build = newBuild;
			this._level = newLevel;
			this._skill = newSkill;
			this._transferState = newTransferState;
			this.map = newMap;
			this.hasSupplies = false;
			this.hasTool = false;
			this.tools = 0;
		}

		public ushort source
		{
			get
			{
				return this._source;
			}
		}

		public byte id
		{
			get
			{
				return this._id;
			}
		}

		public EBlueprintType type
		{
			get
			{
				return this._type;
			}
		}

		public BlueprintSupply[] supplies
		{
			get
			{
				return this._supplies;
			}
		}

		public BlueprintOutput[] outputs
		{
			get
			{
				return this._outputs;
			}
		}

		public ushort tool
		{
			get
			{
				return this._tool;
			}
		}

		public bool toolCritical
		{
			get
			{
				return this._toolCritical;
			}
		}

		public ushort build
		{
			get
			{
				return this._build;
			}
		}

		public byte level
		{
			get
			{
				return this._level;
			}
		}

		public EBlueprintSkill skill
		{
			get
			{
				return this._skill;
			}
		}

		public bool transferState
		{
			get
			{
				return this._transferState;
			}
		}

		public string map { get; private set; }

		public override string ToString()
		{
			string text = string.Empty;
			text += this.type;
			text += ": ";
			byte b = 0;
			while ((int)b < this.supplies.Length)
			{
				if (b > 0)
				{
					text += " + ";
				}
				text += this.supplies[(int)b].id;
				text += "x";
				text += this.supplies[(int)b].amount;
				b += 1;
			}
			text += " = ";
			byte b2 = 0;
			while ((int)b2 < this.outputs.Length)
			{
				if (b2 > 0)
				{
					text += " + ";
				}
				text += this.outputs[(int)b2].id;
				text += "x";
				text += this.outputs[(int)b2].amount;
				b2 += 1;
			}
			return text;
		}

		private ushort _source;

		private byte _id;

		private EBlueprintType _type;

		private BlueprintSupply[] _supplies;

		private BlueprintOutput[] _outputs;

		private ushort _tool;

		private bool _toolCritical;

		private ushort _build;

		private byte _level;

		private EBlueprintSkill _skill;

		private bool _transferState;

		public bool hasSupplies;

		public bool hasTool;

		public bool hasItem;

		public bool hasSkills;

		public ushort tools;

		public ushort products;

		public ushort items;
	}
}

using System;

namespace SDG.Unturned
{
	public class LevelInfo
	{
		public LevelInfo(string newPath, string newName, ELevelSize newSize, ELevelType newType, bool newEditable, LevelInfoConfigData newConfigData)
		{
			this._path = newPath;
			this._name = newName;
			this._size = newSize;
			this._type = newType;
			this._isEditable = newEditable;
			this.configData = newConfigData;
		}

		public string path
		{
			get
			{
				return this._path;
			}
		}

		public string name
		{
			get
			{
				return this._name;
			}
		}

		public bool canAnalyticsTrack
		{
			get
			{
				return this.isSpecial;
			}
		}

		public bool hasTriggers
		{
			get
			{
				return this.isSpecial;
			}
		}

		public bool isSpecial
		{
			get
			{
				return this.name == "Alpha Valley" || this.name == "Monolith" || this.name == "Paintball_Arena_0" || this.name == "PEI" || this.name == "PEI Arena" || this.name == "Tutorial" || this.name == "Washington" || this.name == "Washington Arena" || this.name == "Yukon" || this.name == "Russia" || this.name == "Hawaii" || this.name == "Germany";
			}
		}

		public ELevelSize size
		{
			get
			{
				return this._size;
			}
		}

		public ELevelType type
		{
			get
			{
				return this._type;
			}
		}

		public bool isEditable
		{
			get
			{
				return this._isEditable;
			}
		}

		public LevelInfoConfigData configData { get; private set; }

		private string _path;

		private string _name;

		public bool isFromWorkshop;

		private ELevelSize _size;

		private ELevelType _type;

		private bool _isEditable;
	}
}

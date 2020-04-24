using System;

namespace SDG.Unturned
{
	public class Action
	{
		public Action(ushort newSource, EActionType newType, ActionBlueprint[] newBlueprints, string newText, string newTooltip, string newKey)
		{
			this._source = newSource;
			this._type = newType;
			this._blueprints = newBlueprints;
			this._text = newText;
			this._tooltip = newTooltip;
			this._key = newKey;
		}

		public ushort source
		{
			get
			{
				return this._source;
			}
		}

		public EActionType type
		{
			get
			{
				return this._type;
			}
		}

		public ActionBlueprint[] blueprints
		{
			get
			{
				return this._blueprints;
			}
		}

		public string text
		{
			get
			{
				return this._text;
			}
		}

		public string tooltip
		{
			get
			{
				return this._tooltip;
			}
		}

		public string key
		{
			get
			{
				return this._key;
			}
		}

		private ushort _source;

		private EActionType _type;

		private ActionBlueprint[] _blueprints;

		private string _text;

		private string _tooltip;

		private string _key;
	}
}

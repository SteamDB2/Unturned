using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	public class LimitedKeyValueTableReader : KeyValueTableReader
	{
		public LimitedKeyValueTableReader()
		{
			this.limit = null;
		}

		public LimitedKeyValueTableReader(StreamReader input) : this(null, input)
		{
		}

		public LimitedKeyValueTableReader(string newLimit, StreamReader input) : base(input)
		{
			this.limit = newLimit;
		}

		public string limit { get; protected set; }

		protected override bool canContinueReadDictionary(StreamReader input, Dictionary<string, object> scope)
		{
			return !(this.dictionaryKey == this.limit) && base.canContinueReadDictionary(input, scope);
		}
	}
}

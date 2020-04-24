using System;
using System.IO;
using Newtonsoft.Json;

namespace SDG.Framework.IO.Serialization
{
	public class JsonTextWriterFormatted : JsonTextWriter
	{
		public JsonTextWriterFormatted(TextWriter textWriter) : base(textWriter)
		{
			base.IndentChar = '\t';
			base.Indentation = 1;
		}

		public override void WriteStartArray()
		{
			base.Formatting = 0;
			base.WriteIndent();
			base.WriteStartArray();
			base.Formatting = 1;
		}
	}
}

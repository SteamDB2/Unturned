using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Utilities;

namespace SDG.Framework.Translations
{
	public struct TranslationReference : IFormattedFileReadable, IFormattedFileWritable, IEquatable<TranslationReference>
	{
		public TranslationReference(string path)
		{
			TranslationUtility.tryParse(path, out this.ns, out this.token);
		}

		public TranslationReference(string newNamespace, string newToken)
		{
			this.ns = newNamespace;
			this.token = newToken;
		}

		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.ns) && !string.IsNullOrEmpty(this.token);
			}
		}

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.ns = reader.readValue<string>("Namespace");
			this.token = reader.readValue<string>("Token");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue("Namespace", this.ns);
			writer.writeValue("Token", this.token);
			writer.endObject();
		}

		public override string ToString()
		{
			return "#" + this.ns + "::" + this.token;
		}

		public static bool operator ==(TranslationReference x, TranslationReference y)
		{
			return x.ns == y.ns && x.token == y.token;
		}

		public static bool operator !=(TranslationReference x, TranslationReference y)
		{
			return !(x == y);
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (this.ns != null)
			{
				num ^= this.ns.GetHashCode();
			}
			if (this.token != null)
			{
				num ^= this.token.GetHashCode();
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			TranslationReference translationReference = (TranslationReference)obj;
			return this.ns == translationReference.ns && this.token == translationReference.token;
		}

		public bool Equals(TranslationReference other)
		{
			return this.ns == other.ns && this.token == other.token;
		}

		public static TranslationReference invalid = new TranslationReference(null, null);

		public string ns;

		public string token;
	}
}

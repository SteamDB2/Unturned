using System;
using System.Collections.Generic;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Framework.Translations
{
	public class TranslationBranch : IFormattedFileReadable, IFormattedFileWritable
	{
		public TranslationBranch(Translation newTranslation, TranslationBranch newParentBranch, string newKey)
		{
			this.translation = newTranslation;
			this.parentBranch = newParentBranch;
			this._key = newKey;
		}

		public Translation translation { get; protected set; }

		public TranslationBranch parentBranch { get; protected set; }

		public TranslationReference getReferenceTo()
		{
			string referenceToPath = this.getReferenceToPath(string.Empty);
			return new TranslationReference(referenceToPath);
		}

		public event TranslationBranchKeyChangedHandler keyChanged;

		protected string getReferenceToPath(string path)
		{
			path = this.key + path;
			if (this.parentBranch != null && this.parentBranch.parentBranch != null)
			{
				path = '.' + path;
				path = this.parentBranch.getReferenceToPath(path);
			}
			else
			{
				path = string.Concat(new object[]
				{
					'#',
					this.translation.ns,
					"::",
					path
				});
			}
			return path;
		}

		public string key
		{
			get
			{
				return this._key;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				if (this.key == value)
				{
					return;
				}
				if (this.parentBranch.branches.ContainsKey(value))
				{
					return;
				}
				if (this.parentBranch.branches.Remove(this.key))
				{
					string key = this.key;
					this._key = value;
					this.parentBranch.branches.Add(this.key, this);
					this.recursiveTriggerKeyChanged(key, this.key);
				}
			}
		}

		public Dictionary<string, TranslationBranch> branches { get; protected set; }

		public TranslationLeaf leaf { get; protected set; }

		public virtual TranslationBranch addBranch(string key)
		{
			TranslationBranch translationBranch = new TranslationBranch(this.translation, this, key);
			this.branches.Add(key, translationBranch);
			return translationBranch;
		}

		public virtual void addLeaf()
		{
			this.leaf = new TranslationLeaf(this.translation, this);
		}

		public virtual void addBranches()
		{
			this.branches = new Dictionary<string, TranslationBranch>();
		}

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject(this.key);
			if (reader == null)
			{
				return;
			}
			if (reader.containsKey("Text") && reader.containsKey("Version"))
			{
				this.addLeaf();
				reader.readKey("Text");
				this.leaf.text = reader.readValue<string>();
				reader.readKey("Version");
				this.leaf.version = reader.readValue<int>();
			}
			else
			{
				this.addBranches();
				foreach (string key in reader.getKeys())
				{
					TranslationBranch translationBranch = this.addBranch(key);
					reader.readKey(key);
					translationBranch.read(reader);
				}
			}
		}

		public void write(IFormattedFileWriter writer)
		{
			if (string.IsNullOrEmpty(this.key))
			{
				return;
			}
			if (this.leaf != null)
			{
				string text = this.leaf.text;
				if (text == null)
				{
					text = string.Empty;
				}
				writer.beginObject(this.key);
				text = text.Replace("\"", "\\\"");
				text = text.Replace("'", "\\'");
				writer.writeValue("Text", text);
				writer.writeValue<int>("Version", this.leaf.version);
				writer.endObject();
			}
			else if (this.branches != null)
			{
				writer.beginObject(this.key);
				foreach (KeyValuePair<string, TranslationBranch> keyValuePair in this.branches)
				{
					TranslationBranch value = keyValuePair.Value;
					value.write(writer);
				}
				writer.endObject();
			}
		}

		protected virtual void recursiveTriggerKeyChanged(string oldKey, string newKey)
		{
			this.triggerKeyChanged(oldKey, newKey);
			if (this.branches != null)
			{
				foreach (KeyValuePair<string, TranslationBranch> keyValuePair in this.branches)
				{
					TranslationBranch value = keyValuePair.Value;
					value.triggerKeyChanged(oldKey, newKey);
				}
			}
		}

		protected virtual void triggerKeyChanged(string oldKey, string newKey)
		{
			TranslationBranchKeyChangedHandler translationBranchKeyChangedHandler = this.keyChanged;
			if (translationBranchKeyChangedHandler != null)
			{
				translationBranchKeyChangedHandler(this, oldKey, newKey);
			}
		}

		protected string _key;
	}
}

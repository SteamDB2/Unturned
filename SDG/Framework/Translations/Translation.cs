using System;
using System.IO;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;

namespace SDG.Framework.Translations
{
	public class Translation : IDirtyable, IFormattedFileReadable, IFormattedFileWritable
	{
		public Translation(string newPath, string newLanguage, string newNamespace)
		{
			this.path = newPath;
			this.language = newLanguage;
			this.ns = newNamespace;
		}

		public bool isDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				if (this.isDirty == value)
				{
					return;
				}
				this._isDirty = value;
				if (this.isDirty)
				{
					DirtyManager.markDirty(this);
				}
				else
				{
					DirtyManager.markClean(this);
				}
			}
		}

		public string path { get; protected set; }

		public string language { get; protected set; }

		public string ns { get; protected set; }

		public TranslationBranch tree { get; protected set; }

		public virtual TranslationLeaf addLeaf(string token)
		{
			TranslationBranch translationBranch = this.tree;
			string[] array = token.Split(Translation.DELIMITERS);
			for (int i = 0; i < array.Length; i++)
			{
				TranslationBranch translationBranch2;
				if (!translationBranch.branches.TryGetValue(array[i], out translationBranch2))
				{
					translationBranch2 = translationBranch.addBranch(array[i]);
				}
				translationBranch = translationBranch2;
				if (i == array.Length - 1)
				{
					if (translationBranch.leaf == null)
					{
						translationBranch.addLeaf();
					}
				}
				else
				{
					if (translationBranch.leaf != null)
					{
						return null;
					}
					if (translationBranch.branches == null)
					{
						translationBranch.addBranches();
					}
				}
			}
			if (translationBranch != null && translationBranch.leaf != null)
			{
				return translationBranch.leaf;
			}
			return null;
		}

		public virtual TranslationLeaf getLeaf(string token)
		{
			TranslationBranch tree = this.tree;
			string[] array = token.Split(Translation.DELIMITERS);
			for (int i = 0; i < array.Length; i++)
			{
				if (tree == null || tree.branches == null)
				{
					break;
				}
				tree.branches.TryGetValue(array[i], out tree);
			}
			if (tree != null && tree.leaf != null)
			{
				return tree.leaf;
			}
			return null;
		}

		public virtual string translate(string token)
		{
			TranslationLeaf leaf = this.getLeaf(token);
			if (leaf != null)
			{
				return leaf.text;
			}
			return null;
		}

		public virtual void load()
		{
			this.tree = new TranslationBranch(this, null, "Translation");
			using (StreamReader streamReader = new StreamReader(this.path))
			{
				IFormattedFileReader reader = new KeyValueTableReader(streamReader);
				this.read(reader);
			}
		}

		public virtual void unload()
		{
			this.tree = null;
		}

		public virtual void save()
		{
			string path = this.path;
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter writer = new KeyValueTableWriter(streamWriter);
				this.write(writer);
			}
		}

		public virtual void read(IFormattedFileReader reader)
		{
			this.tree.read(reader);
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject("Metadata");
			writer.writeValue("Language", this.language);
			writer.writeValue("Namespace", this.ns);
			writer.endObject();
			this.tree.write(writer);
		}

		protected static readonly char[] DELIMITERS = new char[]
		{
			'/',
			'.'
		};

		protected bool _isDirty;
	}
}

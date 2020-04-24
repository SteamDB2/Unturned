using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	public class KeyValueTableReader : IFormattedFileReader
	{
		public KeyValueTableReader()
		{
			this.table = new Dictionary<string, object>();
		}

		public KeyValueTableReader(StreamReader input)
		{
			this.table = new Dictionary<string, object>();
			this.readDictionary(input, this.table);
		}

		public Dictionary<string, object> table { get; protected set; }

		public virtual IEnumerable<string> getKeys()
		{
			return this.table.Keys;
		}

		public virtual bool containsKey(string key)
		{
			return this.table.ContainsKey(key);
		}

		public virtual void readKey(string key)
		{
			this.key = key;
			this.index = -1;
		}

		public virtual int readArrayLength(string key)
		{
			this.readKey(key);
			return this.readArrayLength();
		}

		public virtual int readArrayLength()
		{
			object obj;
			if (this.table.TryGetValue(this.key, out obj))
			{
				return (obj as List<object>).Count;
			}
			return 0;
		}

		public virtual void readArrayIndex(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
		}

		public virtual void readArrayIndex(int index)
		{
			this.index = index;
		}

		public virtual string readValue(string key)
		{
			this.readKey(key);
			return this.readValue();
		}

		public virtual string readValue(int index)
		{
			this.readArrayIndex(index);
			return this.readValue();
		}

		public virtual string readValue(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readValue();
		}

		public virtual string readValue()
		{
			if (this.index == -1)
			{
				object obj;
				if (!this.table.TryGetValue(this.key, out obj))
				{
					return null;
				}
				return (string)obj;
			}
			else
			{
				object obj2;
				if (this.table.TryGetValue(this.key, out obj2))
				{
					object obj3 = (obj2 as List<object>)[this.index];
					return (string)obj3;
				}
				return null;
			}
		}

		public virtual object readValue(Type type, string key)
		{
			this.readKey(key);
			return this.readValue(type);
		}

		public virtual object readValue(Type type, int index)
		{
			this.readArrayIndex(index);
			return this.readValue(type);
		}

		public virtual object readValue(Type type, string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readValue(type);
		}

		public virtual object readValue(Type type)
		{
			if (typeof(IFormattedFileReadable).IsAssignableFrom(type))
			{
				IFormattedFileReadable formattedFileReadable = Activator.CreateInstance(type) as IFormattedFileReadable;
				formattedFileReadable.read(this);
				return formattedFileReadable;
			}
			return KeyValueTableTypeReaderRegistry.read(this, type);
		}

		public virtual T readValue<T>(string key)
		{
			this.readKey(key);
			return this.readValue<T>();
		}

		public virtual T readValue<T>(int index)
		{
			this.readArrayIndex(index);
			return this.readValue<T>();
		}

		public virtual T readValue<T>(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readValue<T>();
		}

		public virtual T readValue<T>()
		{
			if (typeof(IFormattedFileReadable).IsAssignableFrom(typeof(T)))
			{
				IFormattedFileReadable formattedFileReadable = Activator.CreateInstance<T>() as IFormattedFileReadable;
				formattedFileReadable.read(this);
				return (T)((object)formattedFileReadable);
			}
			return KeyValueTableTypeReaderRegistry.read<T>(this);
		}

		public virtual IFormattedFileReader readObject(string key)
		{
			this.readKey(key);
			return this.readObject();
		}

		public virtual IFormattedFileReader readObject(int index)
		{
			this.readArrayIndex(index);
			return this.readObject();
		}

		public virtual IFormattedFileReader readObject(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readObject();
		}

		public virtual IFormattedFileReader readObject()
		{
			if (this.index == -1)
			{
				object obj;
				if (this.table.TryGetValue(this.key, out obj))
				{
					return obj as IFormattedFileReader;
				}
				return null;
			}
			else
			{
				object obj2;
				if (this.table.TryGetValue(this.key, out obj2))
				{
					return (obj2 as List<object>)[this.index] as IFormattedFileReader;
				}
				return null;
			}
		}

		protected virtual bool canContinueReadDictionary(StreamReader input, Dictionary<string, object> scope)
		{
			return true;
		}

		public virtual void readDictionary(StreamReader input, Dictionary<string, object> scope)
		{
			this.dictionaryKey = null;
			this.dictionaryInQuotes = false;
			this.dictionaryIgnoreNextChar = false;
			while (!input.EndOfStream)
			{
				char c = (char)input.Read();
				if (this.dictionaryIgnoreNextChar)
				{
					KeyValueTableReader.builder.Append(c);
					this.dictionaryIgnoreNextChar = false;
				}
				else if (c == '\\')
				{
					this.dictionaryIgnoreNextChar = true;
				}
				else if (c == '"')
				{
					if (this.dictionaryInQuotes)
					{
						this.dictionaryInQuotes = false;
						if (string.IsNullOrEmpty(this.dictionaryKey))
						{
							this.dictionaryKey = KeyValueTableReader.builder.ToString();
						}
						else
						{
							string value = KeyValueTableReader.builder.ToString();
							if (!scope.ContainsKey(this.dictionaryKey))
							{
								scope.Add(this.dictionaryKey, value);
							}
							if (!this.canContinueReadDictionary(input, scope))
							{
								return;
							}
							this.dictionaryKey = null;
						}
					}
					else
					{
						this.dictionaryInQuotes = true;
						KeyValueTableReader.builder.Length = 0;
					}
				}
				else if (this.dictionaryInQuotes)
				{
					KeyValueTableReader.builder.Append(c);
				}
				else if (c == '{')
				{
					object obj;
					if (scope.TryGetValue(this.dictionaryKey, out obj))
					{
						KeyValueTableReader keyValueTableReader = (KeyValueTableReader)obj;
						keyValueTableReader.readDictionary(input, keyValueTableReader.table);
					}
					else
					{
						KeyValueTableReader keyValueTableReader2 = new KeyValueTableReader(input);
						obj = keyValueTableReader2;
						scope.Add(this.dictionaryKey, keyValueTableReader2);
					}
					if (!this.canContinueReadDictionary(input, scope))
					{
						return;
					}
					this.dictionaryKey = null;
				}
				else
				{
					if (c == '}')
					{
						return;
					}
					if (c == '[')
					{
						object obj2;
						if (!scope.TryGetValue(this.dictionaryKey, out obj2))
						{
							obj2 = new List<object>();
							scope.Add(this.dictionaryKey, obj2);
						}
						this.readList(input, (List<object>)obj2);
						if (!this.canContinueReadDictionary(input, scope))
						{
							return;
						}
						this.dictionaryKey = null;
					}
				}
			}
		}

		public virtual void readList(StreamReader input, List<object> scope)
		{
			this.listInQuotes = false;
			this.listIgnoreNextChar = false;
			while (!input.EndOfStream)
			{
				char c = (char)input.Read();
				if (this.listIgnoreNextChar)
				{
					KeyValueTableReader.builder.Append(c);
					this.listIgnoreNextChar = false;
				}
				else if (c == '\\')
				{
					this.listIgnoreNextChar = true;
				}
				else if (c == '"')
				{
					if (this.listInQuotes)
					{
						this.listInQuotes = false;
						string item = KeyValueTableReader.builder.ToString();
						scope.Add(item);
					}
					else
					{
						this.listInQuotes = true;
						KeyValueTableReader.builder.Length = 0;
					}
				}
				else if (this.listInQuotes)
				{
					KeyValueTableReader.builder.Append(c);
				}
				else if (c == '{')
				{
					KeyValueTableReader item2 = new KeyValueTableReader(input);
					scope.Add(item2);
				}
				else if (c == ']')
				{
					return;
				}
			}
		}

		protected static StringBuilder builder = new StringBuilder();

		protected string key;

		protected int index;

		protected string dictionaryKey;

		protected bool dictionaryInQuotes;

		protected bool dictionaryIgnoreNextChar;

		protected bool listInQuotes;

		protected bool listIgnoreNextChar;
	}
}

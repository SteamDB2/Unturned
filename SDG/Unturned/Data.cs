using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Data
	{
		public Data(string content)
		{
			this.data = new Dictionary<string, string>();
			StringReader stringReader = null;
			try
			{
				stringReader = new StringReader(content);
				string text = string.Empty;
				while ((text = stringReader.ReadLine()) != null)
				{
					if (!(text == string.Empty) && (text.Length <= 1 || !(text.Substring(0, 2) == "//")))
					{
						int num = text.IndexOf(' ');
						string text2;
						string value;
						if (num != -1)
						{
							text2 = text.Substring(0, num);
							value = text.Substring(num + 1, text.Length - num - 1);
						}
						else
						{
							text2 = text;
							value = string.Empty;
						}
						if (this.data.ContainsKey(text2))
						{
							Assets.errors.Add("Data already contains " + text2 + "!");
							this.hasError = true;
						}
						else
						{
							this.data.Add(text2, value);
						}
					}
				}
				this._hash = Hash.SHA1(content);
			}
			catch (Exception ex)
			{
				Assets.errors.Add("Failed to setup data: " + ex.Message);
				Debug.LogException(ex);
				this.data.Clear();
				this._hash = null;
			}
			finally
			{
				if (stringReader != null)
				{
					stringReader.Close();
				}
			}
		}

		public Data()
		{
			this.data = new Dictionary<string, string>();
			this._hash = null;
		}

		public byte[] hash
		{
			get
			{
				return this._hash;
			}
		}

		public bool isEmpty
		{
			get
			{
				return this.data.Count == 0;
			}
		}

		public bool hasError { get; protected set; }

		public string readString(string key)
		{
			string result;
			this.data.TryGetValue(key, out result);
			return result;
		}

		public bool readBoolean(string key)
		{
			string a = this.readString(key);
			return a == "y" || a == "1" || a == "True";
		}

		public byte readByte(string key)
		{
			byte result;
			byte.TryParse(this.readString(key), out result);
			return result;
		}

		public byte[] readByteArray(string key)
		{
			string s = this.readString(key);
			return Encoding.UTF8.GetBytes(s);
		}

		public short readInt16(string key)
		{
			short result;
			short.TryParse(this.readString(key), out result);
			return result;
		}

		public ushort readUInt16(string key)
		{
			ushort result;
			ushort.TryParse(this.readString(key), out result);
			return result;
		}

		public int readInt32(string key)
		{
			int result;
			int.TryParse(this.readString(key), out result);
			return result;
		}

		public uint readUInt32(string key)
		{
			uint result;
			uint.TryParse(this.readString(key), out result);
			return result;
		}

		public long readInt64(string key)
		{
			long result;
			long.TryParse(this.readString(key), out result);
			return result;
		}

		public ulong readUInt64(string key)
		{
			ulong result;
			ulong.TryParse(this.readString(key), out result);
			return result;
		}

		public float readSingle(string key)
		{
			float result;
			float.TryParse(this.readString(key), out result);
			return result;
		}

		public Vector3 readVector3(string key)
		{
			return new Vector3(this.readSingle(key + "_X"), this.readSingle(key + "_Y"), this.readSingle(key + "_Z"));
		}

		public Quaternion readQuaternion(string key)
		{
			return Quaternion.Euler((float)(this.readByte(key + "_X") * 2), (float)this.readByte(key + "_Y"), (float)this.readByte(key + "_Z"));
		}

		public Color readColor(string key)
		{
			return new Color(this.readSingle(key + "_R"), this.readSingle(key + "_G"), this.readSingle(key + "_B"));
		}

		public CSteamID readSteamID(string key)
		{
			return new CSteamID(this.readUInt64(key));
		}

		public Guid readGUID(string key)
		{
			return new Guid(this.readString(key));
		}

		public void writeString(string key, string value)
		{
			this.data.Add(key, value);
		}

		public void writeBoolean(string key, bool value)
		{
			this.data.Add(key, (!value) ? "n" : "y");
		}

		public void writeByte(string key, byte value)
		{
			this.data.Add(key, value.ToString());
		}

		public void writeByteArray(string key, byte[] value)
		{
			this.data.Add(key, Encoding.UTF8.GetString(value));
		}

		public void writeInt16(string key, short value)
		{
			this.data.Add(key, value.ToString());
		}

		public void writeUInt16(string key, ushort value)
		{
			this.data.Add(key, value.ToString());
		}

		public void writeInt32(string key, int value)
		{
			this.data.Add(key, value.ToString());
		}

		public void writeUInt32(string key, uint value)
		{
			this.data.Add(key, value.ToString());
		}

		public void writeInt64(string key, long value)
		{
			this.data.Add(key, value.ToString());
		}

		public void writeUInt64(string key, ulong value)
		{
			this.data.Add(key, value.ToString());
		}

		public void writeSingle(string key, float value)
		{
			this.data.Add(key, (Mathf.Floor(value * 100f) / 100f).ToString());
		}

		public void writeVector3(string key, Vector3 value)
		{
			this.writeSingle(key + "_X", value.x);
			this.writeSingle(key + "_Y", value.y);
			this.writeSingle(key + "_Z", value.z);
		}

		public void writeQuaternion(string key, Quaternion value)
		{
			Vector3 eulerAngles = value.eulerAngles;
			this.writeByte(key + "_X", MeasurementTool.angleToByte(eulerAngles.x));
			this.writeByte(key + "_Y", MeasurementTool.angleToByte(eulerAngles.y));
			this.writeByte(key + "_Z", MeasurementTool.angleToByte(eulerAngles.z));
		}

		public void writeColor(string key, Color value)
		{
			this.writeSingle(key + "_R", value.r);
			this.writeSingle(key + "_G", value.g);
			this.writeSingle(key + "_B", value.b);
		}

		public void writeSteamID(string key, CSteamID value)
		{
			this.writeUInt64(key, value.m_SteamID);
		}

		public void writeGUID(string key, Guid value)
		{
			this.writeString(key, value.ToString("N"));
		}

		public string getFile()
		{
			string text = string.Empty;
			char c = (!this.isCSV) ? ' ' : ',';
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					keyValuePair.Key,
					c,
					keyValuePair.Value,
					"\n"
				});
			}
			return text;
		}

		public string[] getLines()
		{
			string[] array = new string[this.data.Count];
			char c = (!this.isCSV) ? ' ' : ',';
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				array[num] = keyValuePair.Key + c + keyValuePair.Value;
				num++;
			}
			return array;
		}

		public KeyValuePair<string, string>[] getContents()
		{
			KeyValuePair<string, string>[] array = new KeyValuePair<string, string>[this.data.Count];
			int num = 0;
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				array[num] = keyValuePair;
				num++;
			}
			return array;
		}

		public string[] getValuesWithKey(string key)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				if (keyValuePair.Key == key)
				{
					list.Add(keyValuePair.Value);
				}
			}
			return list.ToArray();
		}

		public string[] getKeysWithValue(string value)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in this.data)
			{
				if (keyValuePair.Value == value)
				{
					list.Add(keyValuePair.Key);
				}
			}
			return list.ToArray();
		}

		public bool has(string key)
		{
			return this.data.ContainsKey(key);
		}

		public void reset()
		{
			this.data.Clear();
		}

		private Dictionary<string, string> data;

		private byte[] _hash;

		public bool isCSV;
	}
}

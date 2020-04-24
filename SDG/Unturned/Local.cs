using System;

namespace SDG.Unturned
{
	public class Local
	{
		public Local(Data newData)
		{
			this.data = newData;
		}

		public Local()
		{
			this.data = null;
		}

		public string read(string key)
		{
			if (this.data != null)
			{
				return this.data.readString(key);
			}
			return "#" + key.ToUpper();
		}

		public string format(string key)
		{
			if (this.data == null)
			{
				return "#" + key.ToUpper();
			}
			string text = this.data.readString(key);
			if (text != null)
			{
				return text;
			}
			return "#" + key.ToUpper();
		}

		public string format(string key, params object[] values)
		{
			if (this.data == null)
			{
				return "#" + key.ToUpper();
			}
			if (values == null)
			{
				return "#" + key.ToUpper();
			}
			string text = this.data.readString(key);
			if (text != null)
			{
				return string.Format(text, values);
			}
			return "#" + key.ToUpper();
		}

		public bool has(string key)
		{
			return this.data != null && this.data.has(key);
		}

		private Data data;
	}
}

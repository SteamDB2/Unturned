using System;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

namespace Pathfinding.Serialization
{
	public class IntKeyDictionaryConverter : JsonConverter
	{
		public override bool CanConvert(Type type)
		{
			return object.Equals(type, typeof(Dictionary<int, int>)) || object.Equals(type, typeof(SortedDictionary<int, int>));
		}

		public override object ReadJson(Type type, Dictionary<string, object> values)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (KeyValuePair<string, object> keyValuePair in values)
			{
				dictionary.Add(Convert.ToInt32(keyValuePair.Key), Convert.ToInt32(keyValuePair.Value));
			}
			return dictionary;
		}

		public override Dictionary<string, object> WriteJson(Type type, object value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Dictionary<int, int> dictionary2 = (Dictionary<int, int>)value;
			foreach (KeyValuePair<int, int> keyValuePair in dictionary2)
			{
				dictionary.Add(keyValuePair.Key.ToString(), keyValuePair.Value);
			}
			return dictionary;
		}
	}
}

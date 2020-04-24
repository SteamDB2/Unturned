using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class TypeRegistryDictionary
	{
		public TypeRegistryDictionary(Type newBaseType)
		{
			this.baseType = newBaseType;
		}

		public Type getType(string key)
		{
			Type result = null;
			this.typesDictionary.TryGetValue(key, out result);
			return result;
		}

		public void addType(string key, Type value)
		{
			if (this.baseType.IsAssignableFrom(value))
			{
				this.typesDictionary.Add(key, value);
				return;
			}
			throw new ArgumentException(this.baseType + " is not assignable from " + value, "value");
		}

		public void removeType(string key)
		{
			this.typesDictionary.Remove(key);
		}

		private Type baseType;

		private Dictionary<string, Type> typesDictionary = new Dictionary<string, Type>();
	}
}

using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class TypeRegistryList
	{
		public TypeRegistryList(Type newBaseType)
		{
			this.baseType = newBaseType;
		}

		public List<Type> getTypes()
		{
			return this.typesList;
		}

		public void addType(Type type)
		{
			if (this.baseType.IsAssignableFrom(type))
			{
				this.typesList.Add(type);
				return;
			}
			throw new ArgumentException(this.baseType + " is not assignable from " + type, "type");
		}

		public void removeType(Type type)
		{
			this.typesList.RemoveFast(type);
		}

		private Type baseType;

		private List<Type> typesList = new List<Type>();
	}
}

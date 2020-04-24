using System;
using SDG.Framework.Debug;
using SDG.Framework.Translations;
using SDG.Unturned;

namespace System
{
	public static class TypeExtension
	{
		public static TranslatedText getTranslatedNameText(this Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(InspectableAttribute), false);
			if (customAttributes.Length > 0)
			{
				InspectableAttribute inspectableAttribute = customAttributes[0] as InspectableAttribute;
				return new TranslatedText(inspectableAttribute.name);
			}
			return new TranslatedTextFallback(type.Name);
		}

		public static TypeReference<T> getReferenceTo<T>(this Type type)
		{
			return new TypeReference<T>(type.AssemblyQualifiedName);
		}

		public static object getDefaultValue(this Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}
	}
}

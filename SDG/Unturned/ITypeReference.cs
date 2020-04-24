using System;
using SDG.Framework.Debug;

namespace SDG.Unturned
{
	public interface ITypeReference
	{
		[Inspectable("#SDG::Asset.TypeReference.Name.Name", null)]
		string assemblyQualifiedName { get; set; }

		Type type { get; }

		bool isValid { get; }
	}
}

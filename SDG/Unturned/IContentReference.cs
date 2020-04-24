using System;
using SDG.Framework.Debug;

namespace SDG.Unturned
{
	public interface IContentReference
	{
		[Inspectable("#SDG::Asset.ContentReference.Name.Name", null)]
		string name { get; set; }

		[Inspectable("#SDG::Asset.ContentReference.Path.Name", null)]
		string path { get; set; }

		bool isValid { get; }
	}
}

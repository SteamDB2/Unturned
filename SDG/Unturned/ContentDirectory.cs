using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class ContentDirectory
	{
		public ContentDirectory(string newName, ContentDirectory newParent)
		{
			this.name = newName;
			this.parent = newParent;
			this.files = new List<ContentFile>();
			this.directories = new Dictionary<string, ContentDirectory>();
		}

		public string name { get; protected set; }

		public ContentDirectory parent { get; protected set; }

		public List<ContentFile> files { get; protected set; }

		public Dictionary<string, ContentDirectory> directories { get; protected set; }
	}
}

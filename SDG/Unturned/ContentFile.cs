using System;
using System.IO;

namespace SDG.Unturned
{
	public class ContentFile
	{
		public ContentFile(RootContentDirectory newRootDirectory, ContentDirectory newDirectory, string newPath, string newFile)
		{
			this.rootDirectory = newRootDirectory;
			this.directory = newDirectory;
			this.path = newPath;
			this.file = newFile;
			this.name = Path.GetFileNameWithoutExtension(this.file);
			this.guessedType = ContentTypeGuesserRegistry.guess(Path.GetExtension(this.file));
		}

		public RootContentDirectory rootDirectory { get; protected set; }

		public ContentDirectory directory { get; protected set; }

		public string path { get; protected set; }

		public string file { get; protected set; }

		public string name { get; protected set; }

		public Type guessedType { get; protected set; }
	}
}

using System;
using System.Runtime.InteropServices;

namespace SDG.Framework.Debug
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InspectableFilePath : IInspectablePath
	{
		public InspectableFilePath(string newExtension)
		{
			this.absolutePath = string.Empty;
			this.extension = newExtension;
		}

		public string absolutePath { get; set; }

		public string extension { get; private set; }

		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.absolutePath);
			}
		}

		public override string ToString()
		{
			return this.absolutePath;
		}
	}
}

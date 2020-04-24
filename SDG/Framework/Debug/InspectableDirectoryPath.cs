using System;
using System.Runtime.InteropServices;

namespace SDG.Framework.Debug
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InspectableDirectoryPath : IInspectablePath
	{
		public string absolutePath { get; set; }

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

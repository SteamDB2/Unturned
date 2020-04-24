using System;
using System.Collections.Generic;
using SDG.Framework.UI.Sleek2;

namespace SDG.Framework.UI.Devkit
{
	public class DevkitToolbarBranch
	{
		public DevkitToolbarBranch()
		{
			this.tree = new Dictionary<string, DevkitToolbarBranch>();
		}

		public Dictionary<string, DevkitToolbarBranch> tree { get; protected set; }

		public Sleek2HoverDropdown dropdown;
	}
}

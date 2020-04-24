using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class StructureRegion
	{
		public StructureRegion()
		{
			this._drops = new List<StructureDrop>();
			this._structures = new List<StructureData>();
			this.isNetworked = false;
		}

		public List<StructureDrop> drops
		{
			get
			{
				return this._drops;
			}
		}

		public List<StructureData> structures
		{
			get
			{
				return this._structures;
			}
		}

		public void destroy()
		{
			ushort num = 0;
			while ((int)num < this.drops.Count)
			{
				IManualOnDestroy component = this.drops[(int)num].model.GetComponent<IManualOnDestroy>();
				if (component != null)
				{
					component.ManualOnDestroy();
				}
				Object.Destroy(this.drops[(int)num].model.gameObject);
				this.drops[(int)num].model.position = Vector3.zero;
				num += 1;
			}
			this.drops.Clear();
		}

		private List<StructureDrop> _drops;

		private List<StructureData> _structures;

		public bool isNetworked;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class BarricadeRegion
	{
		public BarricadeRegion(Transform newParent)
		{
			this._drops = new List<BarricadeDrop>();
			this._barricades = new List<BarricadeData>();
			this._parent = newParent;
			this.isNetworked = false;
		}

		public List<BarricadeDrop> drops
		{
			get
			{
				return this._drops;
			}
		}

		public List<BarricadeData> barricades
		{
			get
			{
				return this._barricades;
			}
		}

		public Transform parent
		{
			get
			{
				return this._parent;
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

		private List<BarricadeDrop> _drops;

		private List<BarricadeData> _barricades;

		private Transform _parent;

		public bool isNetworked;
	}
}

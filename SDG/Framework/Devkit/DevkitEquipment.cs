using System;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DevkitEquipment : MonoBehaviour
	{
		public static DevkitEquipment instance { get; protected set; }

		public IDevkitTool tool { get; protected set; }

		public virtual void equip(IDevkitTool newTool)
		{
			this.dequip();
			this.tool = newTool;
			if (this.tool != null)
			{
				this.tool.equip();
			}
		}

		public virtual void dequip()
		{
			if (this.tool != null)
			{
				this.tool.dequip();
			}
			this.tool = null;
		}

		protected void Update()
		{
			if (this.tool != null)
			{
				this.tool.update();
			}
		}

		protected void Awake()
		{
			DevkitEquipment.instance = this;
		}

		protected void OnDestroy()
		{
			this.dequip();
		}
	}
}

using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	public class DevkitGameObjectDestructionTransaction : IDevkitTransaction
	{
		public DevkitGameObjectDestructionTransaction(GameObject newGO)
		{
			this.go = newGO;
			this.isActive = false;
		}

		public bool delta
		{
			get
			{
				return true;
			}
		}

		public void undo()
		{
			if (this.go != null)
			{
				this.go.SetActive(true);
			}
			this.isActive = true;
		}

		public void redo()
		{
			if (this.go != null)
			{
				this.go.SetActive(false);
			}
			this.isActive = false;
		}

		public void begin()
		{
			if (this.go != null)
			{
				this.go.SetActive(false);
			}
		}

		public void end()
		{
		}

		public void forget()
		{
			if (this.go != null && !this.isActive)
			{
				Object.Destroy(this.go);
			}
		}

		protected GameObject go;

		protected bool isActive;
	}
}

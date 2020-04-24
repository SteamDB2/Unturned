using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class InteractablePower : Interactable
	{
		public bool isWired
		{
			get
			{
				return this._isWired;
			}
		}

		protected virtual void updateWired()
		{
		}

		public void updateWired(bool newWired)
		{
			if (newWired == this.isWired)
			{
				return;
			}
			this._isWired = newWired;
			this.updateWired();
		}

		private void Start()
		{
			if (Level.isEditor)
			{
				this.updateWired(true);
			}
			else
			{
				ushort maxValue = ushort.MaxValue;
				if (base.isPlant)
				{
					byte b;
					byte b2;
					BarricadeRegion barricadeRegion;
					BarricadeManager.tryGetPlant(base.transform.parent, out b, out b2, out maxValue, out barricadeRegion);
				}
				List<InteractableGenerator> list = PowerTool.checkGenerators(base.transform.position, 64f, maxValue);
				for (int i = 0; i < list.Count; i++)
				{
					InteractableGenerator interactableGenerator = list[i];
					if (interactableGenerator.isPowered && interactableGenerator.fuel > 0 && (interactableGenerator.transform.position - base.transform.position).sqrMagnitude < interactableGenerator.sqrWirerange)
					{
						this.updateWired(true);
						return;
					}
				}
			}
		}

		protected bool _isWired;
	}
}

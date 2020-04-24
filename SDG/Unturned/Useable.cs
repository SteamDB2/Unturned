using System;

namespace SDG.Unturned
{
	public class Useable : PlayerCaller
	{
		public virtual void startPrimary()
		{
		}

		public virtual void stopPrimary()
		{
		}

		public virtual void startSecondary()
		{
		}

		public virtual void stopSecondary()
		{
		}

		public virtual bool canInspect
		{
			get
			{
				return true;
			}
		}

		public virtual void equip()
		{
		}

		public virtual void dequip()
		{
		}

		public virtual void tick()
		{
		}

		public virtual void simulate(uint simulation, bool inputSteady)
		{
		}

		public virtual void tock(uint clock)
		{
		}

		public virtual void updateState(byte[] newState)
		{
		}
	}
}

using System;
using Steamworks;

namespace SDG.Unturned
{
	public class Command : IComparable<Command>
	{
		public string command
		{
			get
			{
				return this._command;
			}
		}

		public string info
		{
			get
			{
				return this._info;
			}
		}

		public string help
		{
			get
			{
				return this._help;
			}
		}

		protected virtual void execute(CSteamID executorID, string parameter)
		{
		}

		public virtual bool check(CSteamID executorID, string method, string parameter)
		{
			if (method.ToLower() == this.command.ToLower())
			{
				this.execute(executorID, parameter);
				return true;
			}
			return false;
		}

		public int CompareTo(Command other)
		{
			return this.command.CompareTo(other.command);
		}

		protected Local localization;

		protected string _command;

		protected string _info;

		protected string _help;
	}
}

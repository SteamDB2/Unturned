using System;
using System.Reflection;
using UnityEngine;

namespace SDG.Unturned
{
	public class SteamChannelMethod
	{
		public SteamChannelMethod(Component newComponent, MethodInfo newMethod, Type[] newTypes)
		{
			this._component = newComponent;
			this._method = newMethod;
			this._types = newTypes;
		}

		public Component component
		{
			get
			{
				return this._component;
			}
		}

		public MethodInfo method
		{
			get
			{
				return this._method;
			}
		}

		public Type[] types
		{
			get
			{
				return this._types;
			}
		}

		private Component _component;

		private MethodInfo _method;

		private Type[] _types;
	}
}

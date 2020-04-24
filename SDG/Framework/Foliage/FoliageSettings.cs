using System;
using SDG.Framework.Debug;

namespace SDG.Framework.Foliage
{
	public class FoliageSettings
	{
		[TerminalCommandProperty("foliage.enabled", "whether to draw", false)]
		public static bool enabled
		{
			get
			{
				return FoliageSettings._enabled;
			}
			set
			{
				FoliageSettings._enabled = value;
			}
		}

		[TerminalCommandProperty("foliage.enabled_focus", "whether to draw foliage at scope/binocular focus point", false)]
		public static bool drawFocus
		{
			get
			{
				return FoliageSettings._drawFocus;
			}
			set
			{
				FoliageSettings._drawFocus = value;
			}
		}

		[TerminalCommandProperty("foliage.draw_distance", "how many tiles to render", 0)]
		public static int drawDistance
		{
			get
			{
				return FoliageSettings._drawDistance;
			}
			set
			{
				FoliageSettings._drawDistance = value;
			}
		}

		[TerminalCommandProperty("foliage.draw_focus_distance", "how many tiles to render from focus point", 0)]
		public static int drawFocusDistance
		{
			get
			{
				return FoliageSettings._drawFocusDistance;
			}
			set
			{
				FoliageSettings._drawFocusDistance = value;
			}
		}

		[TerminalCommandProperty("foliage.instance_density", "multiplier for number of instanced meshes", 0)]
		public static float instanceDensity
		{
			get
			{
				return FoliageSettings._instanceDensity;
			}
			set
			{
				FoliageSettings._instanceDensity = value;
			}
		}

		[TerminalCommandProperty("foliage.force_instancing_off", "disable instancing to test as if using an old GPU", false)]
		public static bool forceInstancingOff
		{
			get
			{
				return FoliageSettings._forceInstancingOff;
			}
			set
			{
				FoliageSettings._forceInstancingOff = value;
			}
		}

		[TerminalCommandProperty("foliage.focus_distance", "how far to find focus point on ground", 0)]
		public static float focusDistance
		{
			get
			{
				return FoliageSettings._focusDistance;
			}
			set
			{
				FoliageSettings._focusDistance = value;
			}
		}

		private static bool _enabled;

		private static bool _drawFocus;

		private static int _drawDistance;

		private static int _drawFocusDistance;

		private static float _instanceDensity;

		private static bool _forceInstancingOff;

		private static float _focusDistance;
	}
}

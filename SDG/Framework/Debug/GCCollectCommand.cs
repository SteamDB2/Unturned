using System;

namespace SDG.Framework.Debug
{
	public class GCCollectCommand
	{
		[TerminalCommandMethod("gc.collect", "force garbage collection, only useful for debugging")]
		public static void gc_collect()
		{
			GC.Collect();
			TerminalUtility.printCommandPass("Garbage collected!");
		}
	}
}

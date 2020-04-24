using System;

namespace SDG.Framework.Debug
{
	[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
	public class InspectableEnumAttribute : Attribute
	{
		public InspectableEnumAttribute(EEnumInspectionMode inspectionMode)
		{
			this.inspectionMode = inspectionMode;
		}

		public EEnumInspectionMode inspectionMode;
	}
}

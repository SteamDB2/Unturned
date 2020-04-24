using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Components
{
	public delegate void ContentReferenceDockedHandler<T>(ContentReference<T> contentReference) where T : Object;
}

using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Unturned
{
	public static class DecalSystem
	{
		static DecalSystem()
		{
			DecalSystem.decalVisibilityGroup.color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			DecalSystem.decalVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(DecalSystem.decalVisibilityGroup);
		}

		public static VolumeVisibilityGroup decalVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("decal_volumes", new TranslationReference("#SDG::Devkit.Visibility.Decal_Volumes"), true);

		public static HashSet<Decal> decalsDiffuse
		{
			get
			{
				return DecalSystem._decalsDiffuse;
			}
		}

		public static void add(Decal decal)
		{
			if (decal == null)
			{
				return;
			}
			DecalSystem.remove(decal);
			EDecalType type = decal.type;
			if (type == EDecalType.DIFFUSE)
			{
				DecalSystem.decalsDiffuse.Add(decal);
			}
		}

		public static void remove(Decal decal)
		{
			if (decal == null)
			{
				return;
			}
			EDecalType type = decal.type;
			if (type == EDecalType.DIFFUSE)
			{
				DecalSystem.decalsDiffuse.Remove(decal);
			}
		}

		private static HashSet<Decal> _decalsDiffuse = new HashSet<Decal>();
	}
}

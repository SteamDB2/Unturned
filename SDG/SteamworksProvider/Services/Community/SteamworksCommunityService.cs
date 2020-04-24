using System;
using System.Collections.Generic;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.SteamworksProvider.Services.Community
{
	public class SteamworksCommunityService : Service, ICommunityService, IService
	{
		public SteamworksCommunityService()
		{
			this.cachedGroups = new Dictionary<CSteamID, SteamGroup>();
		}

		public void setStatus(string status)
		{
			SteamFriends.SetRichPresence("status", status);
		}

		public Texture2D getIcon(int id)
		{
			uint num;
			uint num2;
			if (id == -1 || !SteamUtils.GetImageSize(id, ref num, ref num2))
			{
				return null;
			}
			Texture2D texture2D = new Texture2D((int)num, (int)num2, 4, false);
			texture2D.name = "Steam_Community_Icon_Buffer";
			texture2D.hideFlags = 61;
			byte[] array = new byte[num * num2 * 4u];
			if (SteamUtils.GetImageRGBA(id, array, array.Length))
			{
				texture2D.LoadRawTextureData(array);
				texture2D.Apply();
				Texture2D texture2D2 = new Texture2D((int)num, (int)num2, 4, false, true);
				texture2D2.name = "Steam_Community_Icon";
				texture2D2.hideFlags = 61;
				int num3 = 0;
				while ((long)num3 < (long)((ulong)num2))
				{
					texture2D2.SetPixels(0, num3, (int)num, 1, texture2D.GetPixels(0, (int)(num2 - 1u - (uint)num3), (int)num, 1));
					num3++;
				}
				texture2D2.Apply();
				Object.DestroyImmediate(texture2D);
				return texture2D2;
			}
			Object.DestroyImmediate(texture2D);
			return null;
		}

		public Texture2D getIcon(CSteamID steamID)
		{
			return this.getIcon(SteamFriends.GetSmallFriendAvatar(steamID));
		}

		public SteamGroup getCachedGroup(CSteamID steamID)
		{
			SteamGroup result;
			this.cachedGroups.TryGetValue(steamID, out result);
			return result;
		}

		public SteamGroup[] getGroups()
		{
			SteamGroup[] array = new SteamGroup[SteamFriends.GetClanCount()];
			for (int i = 0; i < array.Length; i++)
			{
				CSteamID clanByIndex = SteamFriends.GetClanByIndex(i);
				SteamGroup steamGroup = this.getCachedGroup(clanByIndex);
				if (steamGroup == null)
				{
					string clanName = SteamFriends.GetClanName(clanByIndex);
					Texture2D icon = this.getIcon(clanByIndex);
					steamGroup = new SteamGroup(clanByIndex, clanName, icon);
					this.cachedGroups.Add(clanByIndex, steamGroup);
				}
				array[i] = steamGroup;
			}
			return array;
		}

		public bool checkGroup(CSteamID steamID)
		{
			for (int i = 0; i < SteamFriends.GetClanCount(); i++)
			{
				if (SteamFriends.GetClanByIndex(i) == steamID)
				{
					return true;
				}
			}
			return false;
		}

		private Dictionary<CSteamID, SteamGroup> cachedGroups;
	}
}

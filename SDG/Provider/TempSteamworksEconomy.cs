using System;
using System.Collections.Generic;
using System.Globalization;
using SDG.Framework.Debug;
using SDG.Framework.IO.Deserialization;
using SDG.SteamworksProvider;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Provider
{
	public class TempSteamworksEconomy
	{
		public TempSteamworksEconomy(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			TempSteamworksEconomy.econInfo = new List<UnturnedEconInfo>();
			IDeserializer deserializer = new JSONDeserializer();
			TempSteamworksEconomy.econInfo = deserializer.deserialize<List<UnturnedEconInfo>>(ReadWrite.PATH + "/EconInfo.json");
			if (this.appInfo.isDedicated)
			{
				this.inventoryResultReady = Callback<SteamInventoryResultReady_t>.CreateGameServer(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onInventoryResultReady));
			}
			else
			{
				this.inventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onInventoryResultReady));
				this.gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.onGameOverlayActivated));
			}
		}

		public bool canOpenInventory
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		public void open(ulong id)
		{
			SteamFriends.ActivateGameOverlayToWebPage(string.Concat(new object[]
			{
				"http://steamcommunity.com/profiles/",
				SteamUser.GetSteamID(),
				"/inventory/?sellOnLoad=1#",
				SteamUtils.GetAppID(),
				"_2_",
				id
			}));
		}

		public static List<UnturnedEconInfo> econInfo { get; private set; }

		public SteamItemDetails_t[] inventory
		{
			get
			{
				return this.inventoryDetails;
			}
		}

		public ulong getInventoryPackage(int item)
		{
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Length; i++)
				{
					if (this.inventoryDetails[i].m_iDefinition.m_SteamItemDef == item)
					{
						return this.inventoryDetails[i].m_itemId.m_SteamItemInstanceID;
					}
				}
			}
			return 0UL;
		}

		public int getInventoryItem(ulong package)
		{
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Length; i++)
				{
					if (this.inventoryDetails[i].m_itemId.m_SteamItemInstanceID == package)
					{
						return this.inventoryDetails[i].m_iDefinition.m_SteamItemDef;
					}
				}
			}
			return 0;
		}

		public string getInventoryName(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return string.Empty;
			}
			return unturnedEconInfo.name;
		}

		public string getInventoryType(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return string.Empty;
			}
			return unturnedEconInfo.type;
		}

		public string getInventoryDescription(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return string.Empty;
			}
			return unturnedEconInfo.description;
		}

		public bool getInventoryMarketable(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			return unturnedEconInfo != null && unturnedEconInfo.marketable;
		}

		public Color getInventoryColor(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return Color.white;
			}
			uint num;
			if (unturnedEconInfo.name_color != null && unturnedEconInfo.name_color.Length > 0 && uint.TryParse(unturnedEconInfo.name_color, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out num))
			{
				uint num2 = num >> 16 & 255u;
				uint num3 = num >> 8 & 255u;
				uint num4 = num & 255u;
				return new Color(num2 / 255f, num3 / 255f, num4 / 255f);
			}
			return Color.white;
		}

		public ushort getInventoryMythicID(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return 0;
			}
			return (ushort)unturnedEconInfo.item_effect;
		}

		public ushort getInventoryItemID(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return 0;
			}
			return (ushort)unturnedEconInfo.item_id;
		}

		public ushort getInventorySkinID(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return 0;
			}
			return (ushort)unturnedEconInfo.item_skin;
		}

		public void consumeItem(ulong instance)
		{
			Terminal.print("Consume item: " + instance, null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			SteamInventoryResult_t steamInventoryResult_t;
			SteamInventory.ConsumeItem(ref steamInventoryResult_t, (SteamItemInstanceID_t)instance, 1u);
		}

		public void exchangeInventory(int generate, params ulong[] destroy)
		{
			Terminal.print(string.Concat(new object[]
			{
				"Exchange ",
				destroy.Length,
				" items for ",
				generate
			}), null, Provider.STEAM_IC, Provider.STEAM_DC, true);
			SteamItemDef_t[] array = new SteamItemDef_t[1];
			uint[] array2 = new uint[1];
			array[0] = (SteamItemDef_t)generate;
			array2[0] = 1u;
			SteamItemInstanceID_t[] array3 = new SteamItemInstanceID_t[destroy.Length];
			uint[] array4 = new uint[destroy.Length];
			for (int i = 0; i < destroy.Length; i++)
			{
				array3[i] = (SteamItemInstanceID_t)destroy[i];
				array4[i] = 1u;
			}
			SteamInventory.ExchangeItems(ref this.exchangeResult, array, array2, (uint)array.Length, array3, array4, (uint)array3.Length);
		}

		public void updateInventory()
		{
			if (Time.realtimeSinceStartup - this.lastHeartbeat < 30f)
			{
				return;
			}
			this.lastHeartbeat = Time.realtimeSinceStartup;
			SteamInventory.SendItemDropHeartbeat();
		}

		public void dropInventory()
		{
			SteamInventory.TriggerItemDrop(ref this.dropResult, (SteamItemDef_t)10000);
		}

		public void refreshInventory()
		{
			if (!SteamInventory.GetAllItems(ref this.inventoryResult))
			{
				Provider.isLoadingInventory = false;
			}
		}

		public void addLocalItem(SteamItemDetails_t item)
		{
			SteamItemDetails_t[] array = new SteamItemDetails_t[this.inventory.Length + 1];
			for (int i = 0; i < this.inventory.Length; i++)
			{
				array[i] = this.inventory[i];
			}
			array[this.inventory.Length] = item;
			this.inventoryDetails = array;
		}

		private void onInventoryResultReady(SteamInventoryResultReady_t callback)
		{
			if (this.appInfo.isDedicated)
			{
				SteamPending steamPending = null;
				for (int i = 0; i < Provider.pending.Count; i++)
				{
					if (Provider.pending[i].inventoryResult == callback.m_handle)
					{
						steamPending = Provider.pending[i];
						break;
					}
				}
				if (steamPending == null)
				{
					return;
				}
				if (callback.m_result != 1 || !SteamGameServerInventory.CheckResultSteamID(callback.m_handle, steamPending.playerID.steamID))
				{
					Debug.Log(string.Concat(new object[]
					{
						"inventory auth: ",
						callback.m_result,
						" ",
						SteamGameServerInventory.CheckResultSteamID(callback.m_handle, steamPending.playerID.steamID)
					}));
					Provider.reject(steamPending.playerID.steamID, ESteamRejection.AUTH_ECON_VERIFY);
					return;
				}
				uint num = 0u;
				if (SteamGameServerInventory.GetResultItems(callback.m_handle, null, ref num) && num > 0u)
				{
					steamPending.inventoryDetails = new SteamItemDetails_t[num];
					SteamGameServerInventory.GetResultItems(callback.m_handle, steamPending.inventoryDetails, ref num);
				}
				steamPending.inventoryDetailsReady();
			}
			else if (this.promoResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.promoResult)
			{
				SteamInventory.DestroyResult(this.promoResult);
				this.promoResult = SteamInventoryResult_t.Invalid;
			}
			else if (this.exchangeResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.exchangeResult)
			{
				SteamItemDetails_t[] array = null;
				uint num2 = 0u;
				if (SteamInventory.GetResultItems(this.exchangeResult, null, ref num2) && num2 > 0u)
				{
					array = new SteamItemDetails_t[num2];
					SteamInventory.GetResultItems(this.exchangeResult, array, ref num2);
				}
				Terminal.print("onInventoryResultReady: Exchange " + num2, null, Provider.STEAM_IC, Provider.STEAM_DC, true);
				if (array != null && num2 > 0u)
				{
					SteamItemDetails_t item = array[(int)((UIntPtr)(num2 - 1u))];
					this.addLocalItem(item);
					if (this.onInventoryExchanged != null)
					{
						this.onInventoryExchanged(item.m_iDefinition.m_SteamItemDef, item.m_unQuantity, item.m_itemId.m_SteamItemInstanceID);
					}
					this.refreshInventory();
				}
				SteamInventory.DestroyResult(this.exchangeResult);
				this.exchangeResult = SteamInventoryResult_t.Invalid;
			}
			else if (this.dropResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.dropResult)
			{
				SteamItemDetails_t[] array2 = null;
				uint num3 = 0u;
				if (SteamInventory.GetResultItems(this.dropResult, null, ref num3) && num3 > 0u)
				{
					array2 = new SteamItemDetails_t[num3];
					SteamInventory.GetResultItems(this.dropResult, array2, ref num3);
				}
				Terminal.print("onInventoryResultReady: Drop " + num3, null, Provider.STEAM_IC, Provider.STEAM_DC, true);
				if (array2 != null && num3 > 0u)
				{
					SteamItemDetails_t item2 = array2[0];
					this.addLocalItem(item2);
					if (this.onInventoryDropped != null)
					{
						this.onInventoryDropped(item2.m_iDefinition.m_SteamItemDef, item2.m_unQuantity, item2.m_itemId.m_SteamItemInstanceID);
					}
					this.refreshInventory();
				}
				SteamInventory.DestroyResult(this.dropResult);
				this.dropResult = SteamInventoryResult_t.Invalid;
			}
			else if (this.inventoryResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.inventoryResult)
			{
				uint num4 = 0u;
				if (SteamInventory.GetResultItems(this.inventoryResult, null, ref num4) && num4 > 0u)
				{
					this.inventoryDetails = new SteamItemDetails_t[num4];
					SteamInventory.GetResultItems(this.inventoryResult, this.inventoryDetails, ref num4);
				}
				if (this.onInventoryRefreshed != null)
				{
					this.onInventoryRefreshed();
				}
				this.isInventoryAvailable = true;
				Provider.isLoadingInventory = false;
				SteamInventory.DestroyResult(this.inventoryResult);
				this.inventoryResult = SteamInventoryResult_t.Invalid;
			}
		}

		private void onGameOverlayActivated(GameOverlayActivated_t callback)
		{
			if (callback.m_bActive == 0)
			{
				this.refreshInventory();
			}
		}

		public void loadTranslationEconInfo()
		{
			string path = ReadWrite.PATH + "/EconInfo_" + Provider.language;
			if (!ReadWrite.fileExists(path, false, false))
			{
				path = Provider.path + Provider.language + "/EconInfo.json";
				if (!ReadWrite.fileExists(path, false, false))
				{
					return;
				}
			}
			IDeserializer deserializer = new JSONDeserializer();
			List<UnturnedEconInfo> list = new List<UnturnedEconInfo>();
			list = deserializer.deserialize<List<UnturnedEconInfo>>(path);
			using (List<UnturnedEconInfo>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UnturnedEconInfo translatedItem = enumerator.Current;
					UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == translatedItem.itemdefid);
					if (unturnedEconInfo != null)
					{
						unturnedEconInfo.name = translatedItem.name;
						unturnedEconInfo.type = translatedItem.type;
						unturnedEconInfo.description = translatedItem.description;
					}
				}
			}
		}

		public TempSteamworksEconomy.InventoryRefreshed onInventoryRefreshed;

		public TempSteamworksEconomy.InventoryDropped onInventoryDropped;

		public TempSteamworksEconomy.InventoryExchanged onInventoryExchanged;

		public SteamInventoryResult_t promoResult = SteamInventoryResult_t.Invalid;

		public SteamInventoryResult_t exchangeResult = SteamInventoryResult_t.Invalid;

		public SteamInventoryResult_t dropResult = SteamInventoryResult_t.Invalid;

		public SteamInventoryResult_t wearingResult = SteamInventoryResult_t.Invalid;

		public SteamInventoryResult_t inventoryResult = SteamInventoryResult_t.Invalid;

		public SteamItemDetails_t[] inventoryDetails = new SteamItemDetails_t[0];

		public bool isInventoryAvailable;

		private SteamworksAppInfo appInfo;

		private float lastHeartbeat;

		private Callback<SteamInventoryResultReady_t> inventoryResultReady;

		private Callback<GameOverlayActivated_t> gameOverlayActivated;

		public delegate void InventoryRefreshed();

		public delegate void InventoryDropped(int item, ushort quantity, ulong instance);

		public delegate void InventoryExchanged(int item, ushort quantity, ulong instance);
	}
}

using System;
using System.Collections.Generic;
using SDG.Provider.Services;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	public class SteamworksEconomyService : Service, IEconomyService, IService
	{
		public SteamworksEconomyService()
		{
			this.steamworksEconomyRequestHandles = new List<SteamworksEconomyRequestHandle>();
			this.steamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onSteamInventoryResultReady));
		}

		public bool canOpenInventory
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		public IEconomyRequestHandle requestInventory(EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			SteamInventoryResult_t steamInventoryResult;
			SteamInventory.GetAllItems(ref steamInventoryResult);
			return this.addInventoryRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
		}

		public IEconomyRequestHandle requestPromo(EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			SteamInventoryResult_t steamInventoryResult;
			SteamInventory.GrantPromoItems(ref steamInventoryResult);
			return this.addInventoryRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
		}

		public IEconomyRequestHandle exchangeItems(IEconomyItemInstance[] inputItemInstanceIDs, uint[] inputItemQuantities, IEconomyItemDefinition[] outputItemDefinitionIDs, uint[] outputItemQuantities, EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			if (inputItemInstanceIDs.Length != inputItemQuantities.Length)
			{
				throw new ArgumentException("Input item arrays need to be the same length.", "inputItemQuantities");
			}
			if (outputItemDefinitionIDs.Length != outputItemQuantities.Length)
			{
				throw new ArgumentException("Output item arrays need to be the same length.", "outputItemQuantities");
			}
			SteamItemInstanceID_t[] array = new SteamItemInstanceID_t[inputItemInstanceIDs.Length];
			for (int i = 0; i < inputItemInstanceIDs.Length; i++)
			{
				SteamworksEconomyItemInstance steamworksEconomyItemInstance = (SteamworksEconomyItemInstance)inputItemInstanceIDs[i];
				array[i] = steamworksEconomyItemInstance.steamItemInstanceID;
			}
			SteamItemDef_t[] array2 = new SteamItemDef_t[outputItemDefinitionIDs.Length];
			for (int j = 0; j < outputItemDefinitionIDs.Length; j++)
			{
				SteamworksEconomyItemDefinition steamworksEconomyItemDefinition = (SteamworksEconomyItemDefinition)outputItemDefinitionIDs[j];
				array2[j] = steamworksEconomyItemDefinition.steamItemDef;
			}
			SteamInventoryResult_t steamInventoryResult;
			SteamInventory.ExchangeItems(ref steamInventoryResult, array2, outputItemQuantities, (uint)array2.Length, array, inputItemQuantities, (uint)array.Length);
			return this.addInventoryRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
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

		private SteamworksEconomyRequestHandle findSteamworksEconomyRequestHandles(SteamInventoryResult_t steamInventoryResult)
		{
			return this.steamworksEconomyRequestHandles.Find((SteamworksEconomyRequestHandle handle) => handle.steamInventoryResult == steamInventoryResult);
		}

		private IEconomyRequestHandle addInventoryRequestHandle(SteamInventoryResult_t steamInventoryResult, EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			SteamworksEconomyRequestHandle steamworksEconomyRequestHandle = new SteamworksEconomyRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
			this.steamworksEconomyRequestHandles.Add(steamworksEconomyRequestHandle);
			return steamworksEconomyRequestHandle;
		}

		private IEconomyRequestResult createInventoryRequestResult(SteamInventoryResult_t steamInventoryResult)
		{
			uint num = 0u;
			SteamworksEconomyItem[] array2;
			if (SteamGameServerInventory.GetResultItems(steamInventoryResult, null, ref num) && num > 0u)
			{
				SteamItemDetails_t[] array = new SteamItemDetails_t[num];
				SteamGameServerInventory.GetResultItems(steamInventoryResult, array, ref num);
				array2 = new SteamworksEconomyItem[num];
				for (uint num2 = 0u; num2 < num; num2 += 1u)
				{
					SteamItemDetails_t newSteamItemDetail = array[(int)((UIntPtr)num2)];
					SteamworksEconomyItem steamworksEconomyItem = new SteamworksEconomyItem(newSteamItemDetail);
					array2[(int)((UIntPtr)num2)] = steamworksEconomyItem;
				}
			}
			else
			{
				array2 = new SteamworksEconomyItem[0];
			}
			return new EconomyRequestResult(EEconomyRequestState.SUCCESS, array2);
		}

		private void onSteamInventoryResultReady(SteamInventoryResultReady_t callback)
		{
			SteamworksEconomyRequestHandle steamworksEconomyRequestHandle = this.findSteamworksEconomyRequestHandles(callback.m_handle);
			if (steamworksEconomyRequestHandle == null)
			{
				return;
			}
			IEconomyRequestResult inventoryRequestResult = this.createInventoryRequestResult(steamworksEconomyRequestHandle.steamInventoryResult);
			steamworksEconomyRequestHandle.triggerInventoryRequestReadyCallback(inventoryRequestResult);
			SteamInventory.DestroyResult(steamworksEconomyRequestHandle.steamInventoryResult);
		}

		private List<SteamworksEconomyRequestHandle> steamworksEconomyRequestHandles;

		private Callback<SteamInventoryResultReady_t> steamInventoryResultReady;
	}
}

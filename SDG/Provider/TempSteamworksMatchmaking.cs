using System;
using System.Collections.Generic;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Provider
{
	public class TempSteamworksMatchmaking
	{
		public TempSteamworksMatchmaking()
		{
			this.serverPingResponse = new ISteamMatchmakingPingResponse(new ISteamMatchmakingPingResponse.ServerResponded(this.onPingResponded), new ISteamMatchmakingPingResponse.ServerFailedToRespond(this.onPingFailedToRespond));
			this.serverListResponse = new ISteamMatchmakingServerListResponse(new ISteamMatchmakingServerListResponse.ServerResponded(this.onServerListResponded), new ISteamMatchmakingServerListResponse.ServerFailedToRespond(this.onServerListFailedToRespond), new ISteamMatchmakingServerListResponse.RefreshComplete(this.onRefreshComplete));
			this.playersResponse = new ISteamMatchmakingPlayersResponse(new ISteamMatchmakingPlayersResponse.AddPlayerToList(this.onAddPlayerToList), new ISteamMatchmakingPlayersResponse.PlayersFailedToRespond(this.onPlayersFailedToRespond), new ISteamMatchmakingPlayersResponse.PlayersRefreshComplete(this.onPlayersRefreshComplete));
			this.rulesResponse = new ISteamMatchmakingRulesResponse(new ISteamMatchmakingRulesResponse.RulesResponded(this.onRulesResponded), new ISteamMatchmakingRulesResponse.RulesFailedToRespond(this.onRulesFailedToRespond), new ISteamMatchmakingRulesResponse.RulesRefreshComplete(this.onRulesRefreshComplete));
		}

		public void initializeMatchmaking()
		{
			if (this.matchmakingBestServer != null)
			{
				this.matchmakingIgnored.Add(this.matchmakingBestServer.steamID.m_SteamID);
			}
			this.matchmakingBestServer = null;
		}

		public ESteamServerList currentList
		{
			get
			{
				return this._currentList;
			}
		}

		public List<SteamServerInfo> serverList
		{
			get
			{
				return this._serverList;
			}
		}

		public bool isAttemptingServerQuery { get; private set; }

		public IComparer<SteamServerInfo> serverInfoComparer
		{
			get
			{
				return this._serverInfoComparer;
			}
		}

		public void sortMasterServer(IComparer<SteamServerInfo> newServerInfoComparer)
		{
			this._serverInfoComparer = newServerInfoComparer;
			this.serverList.Sort(this.serverInfoComparer);
			if (this.onMasterServerResorted != null)
			{
				this.onMasterServerResorted();
			}
		}

		private void cleanupServerQuery()
		{
			if (this.serverQuery == HServerQuery.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.CancelServerQuery(this.serverQuery);
			this.serverQuery = HServerQuery.Invalid;
		}

		private void cleanupPlayersQuery()
		{
			if (this.playersQuery == HServerQuery.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.CancelServerQuery(this.playersQuery);
			this.playersQuery = HServerQuery.Invalid;
		}

		private void cleanupRulesQuery()
		{
			if (this.rulesQuery == HServerQuery.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.CancelServerQuery(this.rulesQuery);
			this.rulesQuery = HServerQuery.Invalid;
		}

		private void cleanupServerListRequest()
		{
			if (this.serverListRequest == HServerListRequest.Invalid)
			{
				return;
			}
			SteamMatchmakingServers.ReleaseRequest(this.serverListRequest);
			this.serverListRequest = HServerListRequest.Invalid;
			this.serverListRefreshIndex = -1;
		}

		public void connect(SteamConnectionInfo info)
		{
			if (Provider.isConnected)
			{
				return;
			}
			this.connectionInfo = info;
			this.serverQueryAttempts = 0;
			this.isAttemptingServerQuery = true;
			this.autoJoinServerQuery = false;
			this.attemptServerQuery();
		}

		public void cancel()
		{
			if (!this.isAttemptingServerQuery)
			{
				return;
			}
			this.serverQueryAttempts = 10;
		}

		private void attemptServerQuery()
		{
			this.cleanupServerQuery();
			this.serverQuery = SteamMatchmakingServers.PingServer(this.connectionInfo.ip, this.connectionInfo.port + 1, this.serverPingResponse);
			this.serverQueryAttempts++;
			if (this.onAttemptUpdated != null)
			{
				this.onAttemptUpdated(this.serverQueryAttempts);
			}
		}

		public void refreshMasterServer(ESteamServerList list, string filterMap, EPassword filterPassword, EWorkshop filterWorkshop, EPlugins filterPlugins, EAttendance filterAttendance, EVACProtectionFilter filterVACProtection, EBattlEyeProtectionFilter filterBattlEyeProtection, bool filterPro, ECombat filterCombat, ECheats filterCheats, EGameMode filterMode, ECameraMode filterCamera)
		{
			this._currentList = list;
			if (this.onMasterServerRemoved != null)
			{
				this.onMasterServerRemoved();
			}
			this.cleanupServerListRequest();
			this._serverList = new List<SteamServerInfo>();
			if (list == ESteamServerList.HISTORY)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestHistoryServerList(Provider.APP_ID, new MatchMakingKeyValuePair_t[0], 0u, this.serverListResponse);
				return;
			}
			if (list == ESteamServerList.FAVORITES)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestFavoritesServerList(Provider.APP_ID, new MatchMakingKeyValuePair_t[0], 0u, this.serverListResponse);
				return;
			}
			if (list == ESteamServerList.LAN)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestLANServerList(Provider.APP_ID, this.serverListResponse);
				return;
			}
			this.filters = new List<MatchMakingKeyValuePair_t>();
			MatchMakingKeyValuePair_t item = default(MatchMakingKeyValuePair_t);
			item.m_szKey = "gamedir";
			item.m_szValue = "unturned";
			this.filters.Add(item);
			if (filterMap.Length > 0)
			{
				MatchMakingKeyValuePair_t item2 = default(MatchMakingKeyValuePair_t);
				item2.m_szKey = "map";
				item2.m_szValue = filterMap.ToLower();
				this.filters.Add(item2);
			}
			if (filterAttendance == EAttendance.EMPTY)
			{
				MatchMakingKeyValuePair_t item3 = default(MatchMakingKeyValuePair_t);
				item3.m_szKey = "noplayers";
				item3.m_szValue = "1";
				this.filters.Add(item3);
			}
			else if (filterAttendance == EAttendance.SPACE)
			{
				MatchMakingKeyValuePair_t item4 = default(MatchMakingKeyValuePair_t);
				item4.m_szKey = "notfull";
				item4.m_szValue = "1";
				this.filters.Add(item4);
				MatchMakingKeyValuePair_t item5 = default(MatchMakingKeyValuePair_t);
				item5.m_szKey = "hasplayers";
				item5.m_szValue = "1";
				this.filters.Add(item5);
			}
			MatchMakingKeyValuePair_t item6 = default(MatchMakingKeyValuePair_t);
			item6.m_szKey = "gamedataand";
			if (filterPassword == EPassword.YES)
			{
				item6.m_szValue = "PASS";
			}
			else if (filterPassword == EPassword.NO)
			{
				item6.m_szValue = "SSAP";
			}
			if (filterVACProtection == EVACProtectionFilter.Secure)
			{
				item6.m_szValue += ",";
				item6.m_szValue += "VAC_ON";
				MatchMakingKeyValuePair_t item7 = default(MatchMakingKeyValuePair_t);
				item7.m_szKey = "secure";
				item7.m_szValue = "1";
				this.filters.Add(item7);
			}
			else if (filterVACProtection == EVACProtectionFilter.Insecure)
			{
				item6.m_szValue += ",";
				item6.m_szValue += "VAC_OFF";
			}
			item6.m_szValue += ",";
			item6.m_szValue += Provider.APP_VERSION;
			this.filters.Add(item6);
			MatchMakingKeyValuePair_t item8 = default(MatchMakingKeyValuePair_t);
			item8.m_szKey = "gametagsand";
			if (filterWorkshop == EWorkshop.YES)
			{
				item8.m_szValue = "WORK";
			}
			else if (filterWorkshop == EWorkshop.NO)
			{
				item8.m_szValue = "KROW";
			}
			if (filterCombat == ECombat.PVP)
			{
				item8.m_szValue += ",PVP";
			}
			else if (filterCombat == ECombat.PVE)
			{
				item8.m_szValue += ",PVE";
			}
			if (filterCheats == ECheats.YES)
			{
				item8.m_szValue += ",CHEATS";
			}
			else if (filterCheats == ECheats.NO)
			{
				item8.m_szValue += ",STAEHC";
			}
			if (filterMode != EGameMode.ANY)
			{
				item8.m_szValue = item8.m_szValue + "," + filterMode.ToString();
			}
			if (filterCamera != ECameraMode.ANY)
			{
				item8.m_szValue = item8.m_szValue + "," + filterCamera.ToString();
			}
			if (filterPro)
			{
				item8.m_szValue += ",GOLDONLY";
			}
			else
			{
				item8.m_szValue += ",YLNODLOG";
			}
			if (filterBattlEyeProtection == EBattlEyeProtectionFilter.Secure)
			{
				item8.m_szValue += ",BATTLEYE_ON";
			}
			else if (filterBattlEyeProtection == EBattlEyeProtectionFilter.Insecure)
			{
				item8.m_szValue += ",BATTLEYE_OFF";
			}
			this.filters.Add(item8);
			if (list == ESteamServerList.INTERNET)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestInternetServerList(Provider.APP_ID, this.filters.ToArray(), (uint)this.filters.Count, this.serverListResponse);
				return;
			}
			if (list == ESteamServerList.FRIENDS)
			{
				this.serverListRequest = SteamMatchmakingServers.RequestFriendsServerList(Provider.APP_ID, this.filters.ToArray(), (uint)this.filters.Count, this.serverListResponse);
				return;
			}
		}

		public void refreshPlayers(uint ip, ushort port)
		{
			this.cleanupPlayersQuery();
			this.playersQuery = SteamMatchmakingServers.PlayerDetails(ip, port + 1, this.playersResponse);
		}

		public void refreshRules(uint ip, ushort port)
		{
			this.cleanupRulesQuery();
			this.rulesMap = new Dictionary<string, string>();
			this.rulesQuery = SteamMatchmakingServers.ServerRules(ip, port + 1, this.rulesResponse);
		}

		private void onPingResponded(gameserveritem_t data)
		{
			this.isAttemptingServerQuery = false;
			this.cleanupServerQuery();
			if (data.m_nAppID == Provider.APP_ID.m_AppId)
			{
				SteamServerInfo steamServerInfo = new SteamServerInfo(data);
				if (!steamServerInfo.isPro || Provider.isPro)
				{
					if (!steamServerInfo.isPassworded || this.connectionInfo.password != string.Empty)
					{
						if (this.autoJoinServerQuery)
						{
							Provider.connect(steamServerInfo, this.connectionInfo.password);
						}
						else
						{
							MenuUI.closeAll();
							MenuUI.closeAlert();
							MenuPlayServerInfoUI.open(steamServerInfo, this.connectionInfo.password, MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT);
						}
					}
					else
					{
						Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PASSWORD;
					}
				}
				else
				{
					Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_SERVER;
				}
			}
			else
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
			}
			if (this.onTimedOut != null)
			{
				this.onTimedOut();
			}
		}

		private void onPingFailedToRespond()
		{
			if (this.serverQueryAttempts < 10)
			{
				this.attemptServerQuery();
			}
			else
			{
				this.isAttemptingServerQuery = false;
				this.cleanupServerQuery();
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
				if (this.onTimedOut != null)
				{
					this.onTimedOut();
				}
			}
		}

		private void onServerListResponded(HServerListRequest request, int index)
		{
			if (request != this.serverListRequest)
			{
				return;
			}
			gameserveritem_t serverDetails = SteamMatchmakingServers.GetServerDetails(request, index);
			if (this.matchmakingIgnored.Contains(serverDetails.m_steamID.m_SteamID))
			{
				return;
			}
			SteamServerInfo steamServerInfo = new SteamServerInfo(serverDetails);
			if (index == this.serverListRefreshIndex)
			{
				if (this.onMasterServerQueryRefreshed != null)
				{
					this.onMasterServerQueryRefreshed(steamServerInfo);
				}
				return;
			}
			if (FilterSettings.filterPlugins == EPlugins.NO)
			{
				if (serverDetails.m_nBotPlayers != 0)
				{
					return;
				}
			}
			else if (FilterSettings.filterPlugins == EPlugins.YES && serverDetails.m_nBotPlayers != 1)
			{
				return;
			}
			if (steamServerInfo.maxPlayers < (int)CommandMaxPlayers.MIN_NUMBER)
			{
				return;
			}
			if (this.currentList == ESteamServerList.INTERNET)
			{
				if (steamServerInfo.maxPlayers > (int)(CommandMaxPlayers.MAX_NUMBER / 2))
				{
					return;
				}
			}
			else if (steamServerInfo.maxPlayers > (int)CommandMaxPlayers.MAX_NUMBER)
			{
				return;
			}
			if (PlaySettings.serversName != null && PlaySettings.serversName.Length > 1 && steamServerInfo.name.IndexOf(PlaySettings.serversName, StringComparison.OrdinalIgnoreCase) == -1)
			{
				return;
			}
			int num = this.serverList.BinarySearch(steamServerInfo, this.serverInfoComparer);
			if (num < 0)
			{
				num = ~num;
			}
			this.serverList.Insert(num, steamServerInfo);
			if (this.onMasterServerAdded != null)
			{
				this.onMasterServerAdded(num, steamServerInfo);
			}
			this.matchmakingBestServer = null;
			int num2 = 25;
			while (this.matchmakingBestServer == null && num2 <= OptionsSettings.maxMatchmakingPing)
			{
				int num3 = -1;
				foreach (SteamServerInfo steamServerInfo2 in this.serverList)
				{
					if (steamServerInfo2.players < OptionsSettings.minMatchmakingPlayers)
					{
						break;
					}
					if (steamServerInfo2.players != num3)
					{
						num3 = steamServerInfo2.players;
						if (steamServerInfo2.ping <= num2)
						{
							this.matchmakingBestServer = steamServerInfo2;
							break;
						}
					}
				}
				num2 += 25;
			}
			if (this.matchmakingProgressed != null)
			{
				this.matchmakingProgressed();
			}
		}

		private void onServerListFailedToRespond(HServerListRequest request, int index)
		{
		}

		private void onRefreshComplete(HServerListRequest request, EMatchMakingServerResponse response)
		{
			if (request == this.serverListRequest)
			{
				if (this.onMasterServerRefreshed != null)
				{
					this.onMasterServerRefreshed(response);
				}
				if (this.matchmakingFinished != null)
				{
					this.matchmakingFinished();
				}
				if (response == 2 || this.serverList.Count == 0)
				{
					Debug.Log("No servers found on the master server.");
					return;
				}
				if (response == 1)
				{
					Debug.LogError("Failed to connect to the master server.");
					return;
				}
				if (response == null)
				{
					Debug.Log("Successfully refreshed the master server.");
					return;
				}
			}
		}

		private void onAddPlayerToList(string name, int score, float time)
		{
			if (this.onPlayersQueryRefreshed != null)
			{
				this.onPlayersQueryRefreshed(name, score, time);
			}
		}

		private void onPlayersFailedToRespond()
		{
		}

		private void onPlayersRefreshComplete()
		{
		}

		private void onRulesResponded(string key, string value)
		{
			if (this.rulesMap == null)
			{
				return;
			}
			this.rulesMap.Add(key, value);
		}

		private void onRulesFailedToRespond()
		{
		}

		private void onRulesRefreshComplete()
		{
			if (this.onRulesQueryRefreshed != null)
			{
				this.onRulesQueryRefreshed(this.rulesMap);
			}
		}

		public TempSteamworksMatchmaking.MasterServerAdded onMasterServerAdded;

		public TempSteamworksMatchmaking.MasterServerRemoved onMasterServerRemoved;

		public TempSteamworksMatchmaking.MasterServerResorted onMasterServerResorted;

		public TempSteamworksMatchmaking.MasterServerRefreshed onMasterServerRefreshed;

		public TempSteamworksMatchmaking.MasterServerQueryRefreshed onMasterServerQueryRefreshed;

		public TempSteamworksMatchmaking.AttemptUpdated onAttemptUpdated;

		public TempSteamworksMatchmaking.TimedOut onTimedOut;

		public TempSteamworksMatchmaking.MatchmakingProgressedHandler matchmakingProgressed;

		public TempSteamworksMatchmaking.MatchmakingFinishedHandler matchmakingFinished;

		private HashSet<ulong> matchmakingIgnored = new HashSet<ulong>();

		public SteamServerInfo matchmakingBestServer;

		public TempSteamworksMatchmaking.PlayersQueryRefreshed onPlayersQueryRefreshed;

		public TempSteamworksMatchmaking.RulesQueryRefreshed onRulesQueryRefreshed;

		private SteamConnectionInfo connectionInfo;

		private ESteamServerList _currentList;

		private List<SteamServerInfo> _serverList;

		private List<MatchMakingKeyValuePair_t> filters;

		private ISteamMatchmakingPingResponse serverPingResponse;

		private ISteamMatchmakingServerListResponse serverListResponse;

		private ISteamMatchmakingPlayersResponse playersResponse;

		private ISteamMatchmakingRulesResponse rulesResponse;

		private HServerQuery playersQuery = HServerQuery.Invalid;

		private HServerQuery rulesQuery = HServerQuery.Invalid;

		private Dictionary<string, string> rulesMap;

		private HServerQuery serverQuery = HServerQuery.Invalid;

		private int serverQueryAttempts;

		public bool autoJoinServerQuery;

		private HServerListRequest serverListRequest = HServerListRequest.Invalid;

		private int serverListRefreshIndex = -1;

		private IComparer<SteamServerInfo> _serverInfoComparer = new SteamServerInfoPingAscendingComparator();

		public delegate void MasterServerAdded(int insert, SteamServerInfo server);

		public delegate void MasterServerRemoved();

		public delegate void MasterServerResorted();

		public delegate void MasterServerRefreshed(EMatchMakingServerResponse response);

		public delegate void MasterServerQueryRefreshed(SteamServerInfo server);

		public delegate void AttemptUpdated(int attempt);

		public delegate void TimedOut();

		public delegate void MatchmakingProgressedHandler();

		public delegate void MatchmakingFinishedHandler();

		public delegate void PlayersQueryRefreshed(string name, int score, float time);

		public delegate void RulesQueryRefreshed(Dictionary<string, string> rulesMap);
	}
}

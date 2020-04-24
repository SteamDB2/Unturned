using System;
using SDG.Framework.Landscapes;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class EffectManager : SteamCaller
	{
		public static EffectManager instance
		{
			get
			{
				return EffectManager.manager;
			}
		}

		public static GameObject Instantiate(GameObject element)
		{
			GameObject gameObject = EffectManager.pool.Instantiate(element);
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.Stop(true);
				component.Clear(true);
			}
			gameObject.transform.parent = Level.effects;
			gameObject.tag = "Debris";
			gameObject.layer = LayerMasks.DEBRIS;
			return gameObject;
		}

		public static void Destroy(GameObject element)
		{
			if (element == null)
			{
				return;
			}
			EffectManager.pool.Destroy(element);
		}

		public static void Destroy(GameObject element, float t)
		{
			if (element == null)
			{
				return;
			}
			EffectManager.pool.Destroy(element, t);
		}

		[SteamCall]
		public void tellEffectClearByID(CSteamID steamID, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				string b = id.ToString();
				for (int i = 0; i < Level.effects.childCount; i++)
				{
					Transform child = Level.effects.GetChild(i);
					if (child.gameObject.activeSelf)
					{
						if (!(child.name != b))
						{
							EffectManager.Destroy(child.gameObject);
						}
					}
				}
			}
		}

		public static void askEffectClearByID(ushort id, CSteamID steamID)
		{
			if (Provider.isServer)
			{
				EffectManager.manager.channel.send("tellEffectClearByID", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					id
				});
			}
		}

		[SteamCall]
		public void tellEffectClearAll(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < Level.effects.childCount; i++)
				{
					Transform child = Level.effects.GetChild(i);
					if (child.gameObject.activeSelf)
					{
						if (!(child.name == "System"))
						{
							EffectManager.Destroy(child.gameObject);
						}
					}
				}
			}
		}

		public static void askEffectClearAll()
		{
			if (Provider.isServer)
			{
				EffectManager.manager.channel.send("tellEffectClearAll", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
			}
		}

		public static void sendEffect(ushort id, byte x, byte y, byte area, Vector3 point, Vector3 normal)
		{
			EffectManager.manager.channel.send("tellEffectPointNormal", ESteamCall.CLIENTS, x, y, area, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				point,
				normal
			});
		}

		public static void sendEffect(ushort id, byte x, byte y, byte area, Vector3 point)
		{
			EffectManager.manager.channel.send("tellEffectPoint", ESteamCall.CLIENTS, x, y, area, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				point
			});
		}

		public static void sendEffectReliable(ushort id, byte x, byte y, byte area, Vector3 point, Vector3 normal)
		{
			EffectManager.manager.channel.send("tellEffectPointNormal", ESteamCall.CLIENTS, x, y, area, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				point,
				normal
			});
		}

		public static void sendEffectReliable(ushort id, byte x, byte y, byte area, Vector3 point)
		{
			EffectManager.manager.channel.send("tellEffectPoint", ESteamCall.CLIENTS, x, y, area, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				point
			});
		}

		public static void sendEffect(ushort id, float radius, Vector3 point, Vector3 normal)
		{
			EffectManager.manager.channel.send("tellEffectPointNormal", ESteamCall.CLIENTS, point, radius, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				point,
				normal
			});
		}

		public static void sendEffect(ushort id, float radius, Vector3 point)
		{
			EffectManager.manager.channel.send("tellEffectPoint", ESteamCall.CLIENTS, point, radius, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				point
			});
		}

		public static void sendEffectReliable(ushort id, float radius, Vector3 point, Vector3 normal)
		{
			EffectManager.manager.channel.send("tellEffectPointNormal", ESteamCall.CLIENTS, point, radius, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				point,
				normal
			});
		}

		public static void sendEffectReliable(ushort id, float radius, Vector3 point)
		{
			EffectManager.manager.channel.send("tellEffectPoint", ESteamCall.CLIENTS, point, radius, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				point
			});
		}

		public static void sendEffect(ushort id, CSteamID steamID, Vector3 point, Vector3 normal)
		{
			EffectManager.manager.channel.send("tellEffectPointNormal", steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				point,
				normal
			});
		}

		public static void sendEffect(ushort id, CSteamID steamID, Vector3 point)
		{
			EffectManager.manager.channel.send("tellEffectPoint", steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				point
			});
		}

		public static void sendEffectReliable(ushort id, CSteamID steamID, Vector3 point, Vector3 normal)
		{
			EffectManager.manager.channel.send("tellEffectPointNormal", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				point,
				normal
			});
		}

		public static void sendEffectReliable(ushort id, CSteamID steamID, Vector3 point)
		{
			EffectManager.manager.channel.send("tellEffectPoint", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				point
			});
		}

		[SteamCall]
		public void tellEffectPointNormal(CSteamID steamID, ushort id, Vector3 point, Vector3 normal)
		{
			if (base.channel.checkServer(steamID))
			{
				EffectManager.effect(id, point, normal);
			}
		}

		[SteamCall]
		public void tellEffectPoint(CSteamID steamID, ushort id, Vector3 point)
		{
			if (base.channel.checkServer(steamID))
			{
				EffectManager.effect(id, point, Vector3.up);
			}
		}

		public static Transform effect(ushort id, Vector3 point, Vector3 normal)
		{
			EffectAsset effectAsset = (EffectAsset)Assets.find(EAssetType.EFFECT, id);
			if (effectAsset == null)
			{
				return null;
			}
			if (effectAsset.splatterTemperature != EPlayerTemperature.NONE)
			{
				Transform transform = new GameObject().transform;
				transform.name = "Temperature";
				transform.parent = Level.effects;
				transform.position = point + Vector3.down * -2f;
				transform.localScale = Vector3.one * 6f;
				transform.gameObject.SetActive(false);
				transform.gameObject.AddComponent<TemperatureTrigger>().temperature = effectAsset.splatterTemperature;
				transform.gameObject.SetActive(true);
				Object.Destroy(transform.gameObject, effectAsset.splatterLifetime - effectAsset.splatterLifetimeSpread);
			}
			if (Dedicator.isDedicated)
			{
				return null;
			}
			if (GraphicsSettings.effectQuality == EGraphicQuality.OFF && !effectAsset.splatterLiquid)
			{
				return null;
			}
			Transform transform2 = EffectManager.pool.Instantiate(effectAsset.effect, point, Quaternion.LookRotation(normal) * Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360))).transform;
			transform2.name = id.ToString();
			transform2.parent = Level.effects;
			if (effectAsset.splatter > 0 && (!effectAsset.gore || OptionsSettings.gore))
			{
				for (int i = 0; i < (int)(effectAsset.splatter * ((effectAsset.splatterLiquid || !(Player.player != null) || Player.player.skills.boost != EPlayerBoost.SPLATTERIFIC) ? 1 : 8)); i++)
				{
					RaycastHit raycastHit;
					if (effectAsset.splatterLiquid)
					{
						float num = Random.Range(0f, 6.28318548f);
						float num2 = Random.Range(1f, 6f);
						Ray ray;
						ray..ctor(point + new Vector3(Mathf.Cos(num) * num2, 0f, Mathf.Sin(num) * num2), Vector3.down);
						int splatter = RayMasks.SPLATTER;
						LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray, 8f, ref splatter);
						Physics.Raycast(ray, ref raycastHit, 8f, splatter);
					}
					else
					{
						Ray ray2;
						ray2..ctor(point, -2f * normal + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
						int splatter2 = RayMasks.SPLATTER;
						LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray2, 8f, ref splatter2);
						Physics.Raycast(ray2, ref raycastHit, 8f, splatter2);
					}
					if (raycastHit.transform != null)
					{
						EPhysicsMaterial material = DamageTool.getMaterial(raycastHit.point, raycastHit.transform, raycastHit.collider);
						if (!PhysicsTool.isMaterialDynamic(material))
						{
							float num3 = Random.Range(1f, 2f);
							Transform transform3 = EffectManager.pool.Instantiate(effectAsset.splatters[Random.Range(0, effectAsset.splatters.Length)], raycastHit.point + raycastHit.normal * Random.Range(0.04f, 0.06f), Quaternion.LookRotation(raycastHit.normal) * Quaternion.Euler(0f, 0f, (float)Random.Range(0, 360))).transform;
							transform3.name = "Splatter";
							transform3.parent = Level.effects;
							transform3.localScale = new Vector3(num3, num3, num3);
							transform3.gameObject.SetActive(true);
							if (effectAsset.splatterLifetime > 1.401298E-45f)
							{
								EffectManager.pool.Destroy(transform3.gameObject, effectAsset.splatterLifetime + Random.Range(-effectAsset.splatterLifetimeSpread, effectAsset.splatterLifetimeSpread));
							}
							else
							{
								EffectManager.pool.Destroy(transform3.gameObject, GraphicsSettings.effect);
							}
						}
					}
				}
			}
			if (effectAsset.gore)
			{
				transform2.GetComponent<ParticleSystem>().emission.enabled = OptionsSettings.gore;
			}
			if (!effectAsset.isStatic && transform2.GetComponent<AudioSource>() != null)
			{
				transform2.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
			}
			if (effectAsset.lifetime > 1.401298E-45f)
			{
				EffectManager.pool.Destroy(transform2.gameObject, effectAsset.lifetime + Random.Range(-effectAsset.lifetimeSpread, effectAsset.lifetimeSpread));
			}
			else
			{
				float num4 = 0f;
				MeshRenderer component = transform2.GetComponent<MeshRenderer>();
				if (component == null)
				{
					ParticleSystem component2 = transform2.GetComponent<ParticleSystem>();
					if (component2 != null)
					{
						if (component2.main.loop)
						{
							num4 = component2.main.startLifetime.constantMax;
						}
						else
						{
							num4 = component2.main.duration + component2.main.startLifetime.constantMax;
						}
					}
					AudioSource component3 = transform2.GetComponent<AudioSource>();
					if (component3 != null && component3.clip != null && component3.clip.length > num4)
					{
						num4 = component3.clip.length;
					}
				}
				if (num4 < 1.401298E-45f)
				{
					num4 = GraphicsSettings.effect;
				}
				EffectManager.pool.Destroy(transform2.gameObject, num4);
			}
			if (effectAsset.blast > 0 && GraphicsSettings.blast && GraphicsSettings.renderMode == ERenderMode.DEFERRED)
			{
				EffectManager.effect(effectAsset.blast, point, new Vector3(Random.Range(-0.1f, 0.1f), 1f, Random.Range(-0.1f, 0.1f)));
			}
			return transform2;
		}

		private void onLevelLoaded(int level)
		{
			EffectManager.pool = new GameObjectPoolDictionary();
			if (Dedicator.isDedicated)
			{
				return;
			}
			Asset[] array = Assets.find(EAssetType.EFFECT);
			for (int i = 0; i < array.Length; i++)
			{
				EffectAsset effectAsset = array[i] as EffectAsset;
				if (effectAsset != null && !(effectAsset.effect == null) && effectAsset.preload != 0)
				{
					EffectManager.pool.Instantiate(effectAsset.effect, Level.effects, effectAsset.id.ToString(), (int)effectAsset.preload);
					if (effectAsset.splatter > 0 && effectAsset.splatterPreload > 0)
					{
						for (int j = 0; j < effectAsset.splatters.Length; j++)
						{
							EffectManager.pool.Instantiate(effectAsset.splatters[j], Level.effects, "Splatter", (int)effectAsset.splatterPreload);
						}
					}
				}
			}
		}

		private void Start()
		{
			EffectManager.manager = this;
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		public static readonly float SMALL = 64f;

		public static readonly float MEDIUM = 128f;

		public static readonly float LARGE = 256f;

		public static readonly float INSANE = 512f;

		private static EffectManager manager;

		private static GameObjectPoolDictionary pool;
	}
}

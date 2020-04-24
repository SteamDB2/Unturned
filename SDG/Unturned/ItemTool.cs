using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	public class ItemTool : MonoBehaviour
	{
		public static string filterRarityRichText(string desc)
		{
			if (!string.IsNullOrEmpty(desc))
			{
				desc = desc.Replace("color=common", "color=#ffffff");
				desc = desc.Replace("color=uncommon", "color=#1f871f");
				desc = desc.Replace("color=rare", "color=#4b64fa");
				desc = desc.Replace("color=epic", "color=#964bfa");
				desc = desc.Replace("color=legendary", "color=#c832fa");
				desc = desc.Replace("color=mythical", "color=#fa3219");
				desc = desc.Replace("color=red", "color=#bf1f1f");
				desc = desc.Replace("color=green", "color=#1f871f");
				desc = desc.Replace("color=blue", "color=#3298c8");
				desc = desc.Replace("color=orange", "color=#ab8019");
				desc = desc.Replace("color=yellow", "color=#dcb413");
				desc = desc.Replace("color=purple", "color=#6a466d");
			}
			return desc;
		}

		public static Color getRarityColorHighlight(EItemRarity rarity)
		{
			switch (rarity)
			{
			case EItemRarity.COMMON:
				return ItemTool.RARITY_COMMON_HIGHLIGHT;
			case EItemRarity.UNCOMMON:
				return ItemTool.RARITY_UNCOMMON_HIGHLIGHT;
			case EItemRarity.RARE:
				return ItemTool.RARITY_RARE_HIGHLIGHT;
			case EItemRarity.EPIC:
				return ItemTool.RARITY_EPIC_HIGHLIGHT;
			case EItemRarity.LEGENDARY:
				return ItemTool.RARITY_LEGENDARY_HIGHLIGHT;
			case EItemRarity.MYTHICAL:
				return ItemTool.RARITY_MYTHICAL_HIGHLIGHT;
			default:
				return Color.white;
			}
		}

		public static Color getRarityColorUI(EItemRarity rarity)
		{
			switch (rarity)
			{
			case EItemRarity.COMMON:
				return ItemTool.RARITY_COMMON_UI;
			case EItemRarity.UNCOMMON:
				return ItemTool.RARITY_UNCOMMON_UI;
			case EItemRarity.RARE:
				return ItemTool.RARITY_RARE_UI;
			case EItemRarity.EPIC:
				return ItemTool.RARITY_EPIC_UI;
			case EItemRarity.LEGENDARY:
				return ItemTool.RARITY_LEGENDARY_UI;
			case EItemRarity.MYTHICAL:
				return ItemTool.RARITY_MYTHICAL_UI;
			default:
				return Color.white;
			}
		}

		public static Color getQualityColor(float quality)
		{
			if (quality < 0.5f)
			{
				return Color.Lerp(Palette.COLOR_R, Palette.COLOR_Y, quality * 2f);
			}
			return Color.Lerp(Palette.COLOR_Y, Palette.COLOR_G, (quality - 0.5f) * 2f);
		}

		private static Transform getEffectSystem(ushort mythicID, EEffectType type)
		{
			MythicAsset mythicAsset = (MythicAsset)Assets.find(EAssetType.MYTHIC, mythicID);
			if (mythicAsset == null)
			{
				return null;
			}
			Object @object;
			switch (type)
			{
			case EEffectType.AREA:
				@object = mythicAsset.systemArea;
				break;
			case EEffectType.HOOK:
				@object = mythicAsset.systemHook;
				break;
			case EEffectType.THIRD:
				@object = mythicAsset.systemThird;
				break;
			case EEffectType.FIRST:
				@object = mythicAsset.systemFirst;
				break;
			default:
				return null;
			}
			if (@object == null)
			{
				return null;
			}
			Transform transform = ((GameObject)Object.Instantiate(@object)).transform;
			transform.name = "System";
			return transform;
		}

		public static void applyEffect(Transform[] bones, Transform[] systems, ushort mythicID, EEffectType type)
		{
			if (mythicID == 0)
			{
				return;
			}
			if (bones == null || systems == null)
			{
				return;
			}
			for (int i = 0; i < bones.Length; i++)
			{
				systems[i] = ItemTool.applyEffect(bones[i], mythicID, type);
			}
		}

		public static Transform applyEffect(Transform model, ushort mythicID, EEffectType type)
		{
			if (mythicID == 0)
			{
				return null;
			}
			if (model == null)
			{
				return null;
			}
			Transform transform = model.FindChild("Effect");
			Transform effectSystem = ItemTool.getEffectSystem(mythicID, type);
			if (effectSystem != null)
			{
				if (transform != null)
				{
					effectSystem.parent = transform;
					MythicLockee mythicLockee = effectSystem.gameObject.AddComponent<MythicLockee>();
					MythicLocker mythicLocker = transform.gameObject.AddComponent<MythicLocker>();
					mythicLocker.system = mythicLockee;
					mythicLockee.locker = mythicLocker;
				}
				else
				{
					effectSystem.parent = model;
					MythicLockee mythicLockee2 = effectSystem.gameObject.AddComponent<MythicLockee>();
					MythicLocker mythicLocker2 = model.gameObject.AddComponent<MythicLocker>();
					mythicLocker2.system = mythicLockee2;
					mythicLockee2.locker = mythicLocker2;
				}
				effectSystem.localPosition = Vector3.zero;
				effectSystem.localRotation = Quaternion.identity;
			}
			return effectSystem;
		}

		public static bool tryForceGiveItem(Player player, ushort id, byte amount)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
			if (itemAsset == null || itemAsset.isPro)
			{
				return false;
			}
			for (int i = 0; i < (int)amount; i++)
			{
				Item item = new Item(id, EItemOrigin.ADMIN);
				player.inventory.forceAddItem(item, true);
			}
			return true;
		}

		public static bool checkUseable(byte page, ushort id)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
			if (itemAsset == null)
			{
				return false;
			}
			if (itemAsset.slot == ESlotType.NONE)
			{
				return itemAsset.isUseable;
			}
			if (itemAsset.slot == ESlotType.PRIMARY)
			{
				return page == 0 && itemAsset.isUseable;
			}
			return itemAsset.slot == ESlotType.SECONDARY && (page == 0 || page == 1) && itemAsset.isUseable;
		}

		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel)
		{
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
			return ItemTool.getItem(id, skin, quality, state, viewmodel, itemAsset);
		}

		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, out Material tempMaterial)
		{
			SkinAsset skinAsset = null;
			if (skin != 0)
			{
				skinAsset = (SkinAsset)Assets.find(EAssetType.SKIN, skin);
			}
			return ItemTool.getItem(id, skin, quality, state, viewmodel, itemAsset, skinAsset, out tempMaterial);
		}

		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset)
		{
			SkinAsset skinAsset = null;
			if (skin != 0)
			{
				skinAsset = (SkinAsset)Assets.find(EAssetType.SKIN, skin);
			}
			return ItemTool.getItem(id, skin, quality, state, viewmodel, itemAsset, skinAsset);
		}

		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset)
		{
			Material material;
			return ItemTool.getItem(id, skin, quality, state, viewmodel, itemAsset, skinAsset, out material);
		}

		public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset, out Material tempMaterial)
		{
			tempMaterial = null;
			if (itemAsset != null && itemAsset.item != null)
			{
				if (id != itemAsset.id)
				{
					Debug.LogError("ID and asset ID are not in sync!");
				}
				Transform transform = Object.Instantiate<GameObject>(itemAsset.item).transform;
				transform.name = id.ToString();
				if (viewmodel)
				{
					Layerer.viewmodel(transform);
				}
				if (skinAsset != null && skinAsset.primarySkin != null)
				{
					if (skinAsset.isPattern)
					{
						Material material = Object.Instantiate<Material>(skinAsset.primarySkin);
						material.SetTexture("_AlbedoBase", itemAsset.albedoBase);
						material.SetTexture("_MetallicBase", itemAsset.metallicBase);
						material.SetTexture("_EmissionBase", itemAsset.emissionBase);
						HighlighterTool.rematerialize(transform, material, out tempMaterial);
					}
					else
					{
						HighlighterTool.rematerialize(transform, skinAsset.primarySkin, out tempMaterial);
					}
				}
				if (itemAsset.type == EItemType.GUN)
				{
					Attachments attachments = transform.gameObject.AddComponent<Attachments>();
					attachments.isSkinned = true;
					attachments.updateGun((ItemGunAsset)itemAsset, skinAsset);
					attachments.updateAttachments(state, viewmodel);
				}
				return transform;
			}
			Transform transform2 = new GameObject().transform;
			transform2.name = id.ToString();
			if (viewmodel)
			{
				transform2.tag = "Viewmodel";
				transform2.gameObject.layer = LayerMasks.VIEWMODEL;
			}
			else
			{
				transform2.tag = "Item";
				transform2.gameObject.layer = LayerMasks.ITEM;
			}
			return transform2;
		}

		public static Texture2D getCard(Transform item, Transform hook_0, Transform hook_1, int width, int height, float size, float range)
		{
			if (item == null)
			{
				return null;
			}
			item.position = new Vector3(-256f, -256f, 0f);
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 16, 0, 2);
			temporary.name = "Card_Render";
			RenderTexture.active = temporary;
			ItemTool.tool.GetComponent<Camera>().targetTexture = temporary;
			ItemTool.tool.GetComponent<Camera>().orthographicSize = size;
			Texture2D texture2D = new Texture2D(width * 2, height, 5, false, false);
			texture2D.name = "Card_Atlas";
			texture2D.filterMode = 0;
			texture2D.wrapMode = 1;
			bool fog = RenderSettings.fog;
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientSkyColor = RenderSettings.ambientSkyColor;
			Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
			Color ambientGroundColor = RenderSettings.ambientGroundColor;
			RenderSettings.fog = false;
			RenderSettings.ambientMode = 1;
			RenderSettings.ambientSkyColor = Color.white;
			RenderSettings.ambientEquatorColor = Color.white;
			RenderSettings.ambientGroundColor = Color.white;
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(false);
			}
			ItemTool.tool.GetComponent<Camera>().cullingMask = RayMasks.RESOURCE;
			ItemTool.tool.GetComponent<Camera>().farClipPlane = range;
			ItemTool.tool.transform.position = hook_0.position;
			ItemTool.tool.transform.rotation = hook_0.rotation;
			ItemTool.tool.GetComponent<Camera>().Render();
			texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
			ItemTool.tool.transform.position = hook_1.position;
			ItemTool.tool.transform.rotation = hook_1.rotation;
			ItemTool.tool.GetComponent<Camera>().Render();
			texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), width, 0);
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(true);
			}
			RenderSettings.fog = fog;
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			item.position = new Vector3(0f, -256f, -256f);
			Object.Destroy(item.gameObject);
			for (int i = 0; i < texture2D.width; i++)
			{
				for (int j = 0; j < texture2D.height; j++)
				{
					Color32 color = texture2D.GetPixel(i, j);
					if (color.r == 255 && color.g == 255 && color.b == 255)
					{
						color.a = 0;
					}
					else
					{
						color.a = byte.MaxValue;
					}
					texture2D.SetPixel(i, j, color);
				}
			}
			texture2D.Apply();
			RenderTexture.ReleaseTemporary(temporary);
			return texture2D;
		}

		public static void getIcon(ushort id, byte quality, byte[] state, ItemIconReady callback)
		{
			ushort num = 0;
			SkinAsset skinAsset = null;
			int num2;
			if (Player.player != null && Player.player.channel.owner.skins.TryGetValue(id, out num2) && num2 != 0)
			{
				num = Provider.provider.economyService.getInventorySkinID(num2);
				skinAsset = (SkinAsset)Assets.find(EAssetType.SKIN, num);
			}
			ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
			ItemTool.getIcon(id, num, quality, state, itemAsset, skinAsset, (int)(itemAsset.size_x * 50), (int)(itemAsset.size_y * 50), false, callback);
		}

		public static void getIcon(ushort id, byte quality, byte[] state, ItemAsset itemAsset, ItemIconReady callback)
		{
			ushort num = 0;
			SkinAsset skinAsset = null;
			int num2;
			if (Player.player != null && Player.player.channel.owner.skins.TryGetValue(id, out num2) && num2 != 0)
			{
				num = Provider.provider.economyService.getInventorySkinID(num2);
				skinAsset = (SkinAsset)Assets.find(EAssetType.SKIN, num);
			}
			ItemTool.getIcon(id, num, quality, state, itemAsset, skinAsset, (int)(itemAsset.size_x * 50), (int)(itemAsset.size_y * 50), false, callback);
		}

		public static void getIcon(ushort id, byte quality, byte[] state, ItemAsset itemAsset, int x, int y, ItemIconReady callback)
		{
			ushort num = 0;
			SkinAsset skinAsset = null;
			int num2;
			if (Player.player != null && Player.player.channel.owner.skins.TryGetValue(id, out num2) && num2 != 0)
			{
				num = Provider.provider.economyService.getInventorySkinID(num2);
				skinAsset = (SkinAsset)Assets.find(EAssetType.SKIN, num);
			}
			ItemTool.getIcon(id, num, quality, state, itemAsset, skinAsset, x, y, false, callback);
		}

		public static void getIcon(ushort id, ushort skin, byte quality, byte[] state, ItemAsset itemAsset, SkinAsset skinAsset, int x, int y, bool scale, ItemIconReady callback)
		{
			if (itemAsset != null && id != itemAsset.id)
			{
				Debug.LogError("ID and item asset ID are not in sync!");
			}
			if (skinAsset != null && skin != skinAsset.id)
			{
				Debug.LogError("ID and skin asset ID are not in sync!");
			}
			Texture2D texture2D;
			if (state.Length == 0 && skin == 0 && x == (int)(itemAsset.size_x * 50) && y == (int)(itemAsset.size_y * 50) && ItemTool.cache.TryGetValue(id, out texture2D))
			{
				if (texture2D != null)
				{
					callback(texture2D);
					return;
				}
				ItemTool.cache.Remove(id);
			}
			ItemIconInfo itemIconInfo = new ItemIconInfo();
			itemIconInfo.id = id;
			itemIconInfo.skin = skin;
			itemIconInfo.quality = quality;
			itemIconInfo.state = state;
			itemIconInfo.itemAsset = itemAsset;
			itemIconInfo.skinAsset = skinAsset;
			itemIconInfo.x = x;
			itemIconInfo.y = y;
			itemIconInfo.scale = scale;
			itemIconInfo.callback = callback;
			ItemTool.icons.Enqueue(itemIconInfo);
		}

		private void Update()
		{
			if (ItemTool.icons == null || ItemTool.icons.Count == 0)
			{
				return;
			}
			ItemIconInfo itemIconInfo = ItemTool.icons.Dequeue();
			if (itemIconInfo == null)
			{
				return;
			}
			if (itemIconInfo.itemAsset == null)
			{
				return;
			}
			Transform item = ItemTool.getItem(itemIconInfo.id, itemIconInfo.skin, itemIconInfo.quality, itemIconInfo.state, false, itemIconInfo.itemAsset, itemIconInfo.skinAsset);
			item.position = new Vector3(-256f, -256f, 0f);
			Transform transform;
			if (itemIconInfo.scale && itemIconInfo.skin != 0)
			{
				if (itemIconInfo.itemAsset.size2_z == 0f)
				{
					item.position = new Vector3(0f, -256f, -256f);
					Object.Destroy(item.gameObject);
					Assets.errors.Add("Failed to create a skin icon of size 0 for " + itemIconInfo.id + ".");
					return;
				}
				transform = item.FindChild("Icon2");
				if (transform == null)
				{
					item.position = new Vector3(0f, -256f, -256f);
					Object.Destroy(item.gameObject);
					Assets.errors.Add("Failed to find a skin icon hook on " + itemIconInfo.id + ".");
					return;
				}
			}
			else
			{
				if (itemIconInfo.itemAsset.size_z == 0f)
				{
					item.position = new Vector3(0f, -256f, -256f);
					Object.Destroy(item.gameObject);
					Assets.errors.Add("Failed to create an item icon of size 0 for " + itemIconInfo.id + ".");
					return;
				}
				transform = item.FindChild("Icon");
				if (transform == null)
				{
					item.position = new Vector3(0f, -256f, -256f);
					Object.Destroy(item.gameObject);
					Assets.errors.Add("Failed to find an item icon hook on " + itemIconInfo.id + ".");
					return;
				}
			}
			ItemTool.tool.transform.position = transform.position;
			ItemTool.tool.transform.rotation = transform.rotation;
			RenderTexture temporary = RenderTexture.GetTemporary(itemIconInfo.x, itemIconInfo.y, 16, 0, 2);
			temporary.name = string.Concat(new object[]
			{
				"Item_Render_",
				itemIconInfo.id,
				"_",
				itemIconInfo.skin
			});
			RenderTexture.active = temporary;
			ItemTool.tool.GetComponent<Camera>().targetTexture = temporary;
			ItemTool.tool.GetComponent<Camera>().orthographicSize = itemIconInfo.itemAsset.size_z;
			if (itemIconInfo.scale)
			{
				if (itemIconInfo.skin != 0)
				{
					ItemTool.tool.GetComponent<Camera>().orthographicSize = itemIconInfo.itemAsset.size2_z;
				}
				else
				{
					float num = (float)itemIconInfo.itemAsset.size_x / (float)itemIconInfo.itemAsset.size_y;
					ItemTool.tool.GetComponent<Camera>().orthographicSize *= num;
				}
			}
			bool fog = RenderSettings.fog;
			AmbientMode ambientMode = RenderSettings.ambientMode;
			Color ambientSkyColor = RenderSettings.ambientSkyColor;
			Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
			Color ambientGroundColor = RenderSettings.ambientGroundColor;
			RenderSettings.fog = false;
			RenderSettings.ambientMode = 1;
			RenderSettings.ambientSkyColor = Color.white;
			RenderSettings.ambientEquatorColor = Color.white;
			RenderSettings.ambientGroundColor = Color.white;
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(false);
			}
			ItemTool.tool.GetComponent<Light>().enabled = true;
			ItemTool.tool.GetComponent<Camera>().cullingMask = RayMasks.ITEM;
			ItemTool.tool.GetComponent<Camera>().farClipPlane = 16f;
			ItemTool.tool.GetComponent<Camera>().Render();
			ItemTool.tool.GetComponent<Light>().enabled = false;
			if (Provider.isConnected)
			{
				LevelLighting.setEnabled(true);
			}
			RenderSettings.fog = fog;
			RenderSettings.ambientMode = ambientMode;
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			item.position = new Vector3(0f, -256f, -256f);
			Object.Destroy(item.gameObject);
			Texture2D texture2D = new Texture2D(itemIconInfo.x, itemIconInfo.y, 5, false, true);
			texture2D.name = string.Concat(new object[]
			{
				"Item_Icon_",
				itemIconInfo.id,
				"_",
				itemIconInfo.skin
			});
			texture2D.filterMode = 0;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)itemIconInfo.x, (float)itemIconInfo.y), 0, 0);
			texture2D.Apply();
			RenderTexture.ReleaseTemporary(temporary);
			if (itemIconInfo.callback != null)
			{
				itemIconInfo.callback(texture2D);
			}
			if (itemIconInfo.state.Length == 0 && itemIconInfo.skin == 0 && itemIconInfo.x == (int)(itemIconInfo.itemAsset.size_x * 50) && itemIconInfo.y == (int)(itemIconInfo.itemAsset.size_y * 50) && !ItemTool.cache.ContainsKey(itemIconInfo.id))
			{
				ItemTool.cache.Add(itemIconInfo.id, texture2D);
			}
		}

		private void Start()
		{
			ItemTool.tool = this;
			ItemTool.icons = new Queue<ItemIconInfo>();
		}

		private static readonly Color RARITY_COMMON_HIGHLIGHT = Color.white;

		private static readonly Color RARITY_COMMON_UI = Color.white;

		private static readonly Color RARITY_UNCOMMON_HIGHLIGHT = Color.green;

		private static readonly Color RARITY_UNCOMMON_UI = new Color(0.121568628f, 0.5294118f, 0.121568628f);

		private static readonly Color RARITY_RARE_HIGHLIGHT = Color.blue;

		private static readonly Color RARITY_RARE_UI = new Color(0.294117659f, 0.392156869f, 0.980392158f);

		private static readonly Color RARITY_EPIC_HIGHLIGHT = new Color(0.33f, 0f, 1f);

		private static readonly Color RARITY_EPIC_UI = new Color(0.5882353f, 0.294117659f, 0.980392158f);

		private static readonly Color RARITY_LEGENDARY_HIGHLIGHT = Color.magenta;

		private static readonly Color RARITY_LEGENDARY_UI = new Color(0.784313738f, 0.196078435f, 0.980392158f);

		private static readonly Color RARITY_MYTHICAL_HIGHLIGHT = Color.red;

		private static readonly Color RARITY_MYTHICAL_UI = new Color(0.980392158f, 0.196078435f, 0.09803922f);

		private static ItemTool tool;

		private static Dictionary<ushort, Texture2D> cache = new Dictionary<ushort, Texture2D>();

		private static Queue<ItemIconInfo> icons;
	}
}

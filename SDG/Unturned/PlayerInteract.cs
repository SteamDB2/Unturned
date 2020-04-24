using System;
using SDG.Framework.Utilities;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerInteract : PlayerCaller
	{
		public static Interactable interactable
		{
			get
			{
				return PlayerInteract._interactable;
			}
		}

		public static Interactable2 interactable2
		{
			get
			{
				return PlayerInteract._interactable2;
			}
		}

		private float salvageTime
		{
			get
			{
				if (Provider.isServer || base.channel.owner.isAdmin)
				{
					return 1f;
				}
				return 8f;
			}
		}

		private void hotkey(byte button)
		{
			VehicleManager.swapVehicle(button);
		}

		[SteamCall]
		public void askInspect(CSteamID steamID)
		{
			if (base.channel.checkOwner(steamID) && base.player.equipment.canInspect)
			{
				base.channel.send("tellInspect", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
		}

		[SteamCall]
		public void tellInspect(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				base.player.equipment.inspect();
			}
		}

		private void onPurchaseUpdated(PurchaseNode node)
		{
			if (node == null)
			{
				PlayerInteract.purchaseAsset = null;
			}
			else
			{
				PlayerInteract.purchaseAsset = (ItemAsset)Assets.find(EAssetType.ITEM, node.id);
			}
		}

		private void Update()
		{
			if (base.channel.isOwner)
			{
				if (base.player.stance.stance != EPlayerStance.DRIVING && base.player.stance.stance != EPlayerStance.SITTING && !base.player.life.isDead && !base.player.workzone.isBuilding)
				{
					if (Time.realtimeSinceStartup - PlayerInteract.lastInteract > 0.1f)
					{
						PlayerInteract.lastInteract = Time.realtimeSinceStartup;
						if (base.player.look.isCam)
						{
							PhysicsUtility.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), out PlayerInteract.hit, 4f, RayMasks.PLAYER_INTERACT, 0);
						}
						else
						{
							PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out PlayerInteract.hit, (float)((base.player.look.perspective != EPlayerPerspective.THIRD) ? 4 : 6), RayMasks.PLAYER_INTERACT, 0);
						}
					}
					if (PlayerInteract.hit.transform != PlayerInteract.focus)
					{
						if (PlayerInteract.focus != null && PlayerInteract.interactable != null)
						{
							InteractableDoorHinge component = PlayerInteract.focus.GetComponent<InteractableDoorHinge>();
							if (component != null)
							{
								HighlighterTool.unhighlight(PlayerInteract.focus.parent.parent);
							}
							else
							{
								HighlighterTool.unhighlight(PlayerInteract.focus);
							}
						}
						PlayerInteract.focus = null;
						PlayerInteract.target = null;
						PlayerInteract._interactable = null;
						PlayerInteract._interactable2 = null;
						if (PlayerInteract.hit.transform != null)
						{
							PlayerInteract.focus = PlayerInteract.hit.transform;
							PlayerInteract._interactable = PlayerInteract.focus.GetComponent<Interactable>();
							PlayerInteract._interactable2 = PlayerInteract.focus.GetComponent<Interactable2>();
							if (PlayerInteract.interactable != null)
							{
								PlayerInteract.target = PlayerInteract.focus.FindChildRecursive("Target");
								if (PlayerInteract.interactable.checkInteractable())
								{
									if (PlayerUI.window.isEnabled)
									{
										if (PlayerInteract.interactable.checkUseable())
										{
											Color color;
											if (!PlayerInteract.interactable.checkHighlight(out color))
											{
												color = Color.green;
											}
											InteractableDoorHinge component2 = PlayerInteract.focus.GetComponent<InteractableDoorHinge>();
											if (component2 != null)
											{
												HighlighterTool.highlight(PlayerInteract.focus.parent.parent, color);
											}
											else
											{
												HighlighterTool.highlight(PlayerInteract.focus, color);
											}
										}
										else
										{
											Color color = Color.red;
											InteractableDoorHinge component3 = PlayerInteract.focus.GetComponent<InteractableDoorHinge>();
											if (component3 != null)
											{
												HighlighterTool.highlight(PlayerInteract.focus.parent.parent, color);
											}
											else
											{
												HighlighterTool.highlight(PlayerInteract.focus, color);
											}
										}
									}
								}
								else
								{
									PlayerInteract.target = null;
									PlayerInteract._interactable = null;
								}
							}
						}
					}
				}
				else
				{
					if (PlayerInteract.focus != null && PlayerInteract.interactable != null)
					{
						InteractableDoorHinge component4 = PlayerInteract.focus.GetComponent<InteractableDoorHinge>();
						if (component4 != null)
						{
							HighlighterTool.unhighlight(PlayerInteract.focus.parent.parent);
						}
						else
						{
							HighlighterTool.unhighlight(PlayerInteract.focus);
						}
					}
					PlayerInteract.focus = null;
					PlayerInteract.target = null;
					PlayerInteract._interactable = null;
					PlayerInteract._interactable2 = null;
				}
			}
			if (base.channel.isOwner && !base.player.life.isDead)
			{
				if (PlayerInteract.interactable != null)
				{
					EPlayerMessage message;
					string text;
					Color color2;
					if (PlayerInteract.interactable.checkHint(out message, out text, out color2) && !PlayerUI.window.showCursor)
					{
						if (PlayerInteract.interactable.CompareTag("Item"))
						{
							PlayerUI.hint((!(PlayerInteract.target != null)) ? PlayerInteract.focus : PlayerInteract.target, message, text, color2, new object[]
							{
								((InteractableItem)PlayerInteract.interactable).item,
								((InteractableItem)PlayerInteract.interactable).asset
							});
						}
						else
						{
							PlayerUI.hint((!(PlayerInteract.target != null)) ? PlayerInteract.focus : PlayerInteract.target, message, text, color2, new object[0]);
						}
					}
				}
				else if (PlayerInteract.purchaseAsset != null && base.player.movement.purchaseNode != null && !PlayerUI.window.showCursor)
				{
					PlayerUI.hint(null, EPlayerMessage.PURCHASE, string.Empty, Color.white, new object[]
					{
						PlayerInteract.purchaseAsset.itemName,
						base.player.movement.purchaseNode.cost
					});
				}
				else if (PlayerInteract.focus != null && PlayerInteract.focus.CompareTag("Enemy"))
				{
					Player player = DamageTool.getPlayer(PlayerInteract.focus);
					if (player != null && player != Player.player && !PlayerUI.window.showCursor)
					{
						PlayerUI.hint(null, EPlayerMessage.ENEMY, string.Empty, Color.white, new object[]
						{
							player.channel.owner
						});
					}
				}
				EPlayerMessage message2;
				float data;
				if (PlayerInteract.interactable2 != null && PlayerInteract.interactable2.checkHint(out message2, out data) && !PlayerUI.window.showCursor)
				{
					PlayerUI.hint2(message2, (!PlayerInteract.isHoldingKey) ? 0f : ((Time.realtimeSinceStartup - PlayerInteract.lastKeyDown) / this.salvageTime), data);
				}
				if ((base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING) && !Input.GetKey(304))
				{
					if (Input.GetKeyDown(282))
					{
						this.hotkey(0);
					}
					if (Input.GetKeyDown(283))
					{
						this.hotkey(1);
					}
					if (Input.GetKeyDown(284))
					{
						this.hotkey(2);
					}
					if (Input.GetKeyDown(285))
					{
						this.hotkey(3);
					}
					if (Input.GetKeyDown(286))
					{
						this.hotkey(4);
					}
					if (Input.GetKeyDown(287))
					{
						this.hotkey(5);
					}
					if (Input.GetKeyDown(288))
					{
						this.hotkey(6);
					}
					if (Input.GetKeyDown(289))
					{
						this.hotkey(7);
					}
					if (Input.GetKeyDown(290))
					{
						this.hotkey(8);
					}
					if (Input.GetKeyDown(291))
					{
						this.hotkey(9);
					}
				}
				if (Input.GetKeyDown(ControlsSettings.interact))
				{
					PlayerInteract.lastKeyDown = Time.realtimeSinceStartup;
					PlayerInteract.isHoldingKey = true;
				}
				if (Input.GetKeyDown(ControlsSettings.inspect) && ControlsSettings.inspect != ControlsSettings.interact && base.player.equipment.canInspect)
				{
					base.channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				}
				if (PlayerInteract.isHoldingKey)
				{
					if (Input.GetKeyUp(ControlsSettings.interact))
					{
						PlayerInteract.isHoldingKey = false;
						if (PlayerUI.window.showCursor)
						{
							if (base.player.inventory.isStoring)
							{
								PlayerDashboardUI.close();
								PlayerLifeUI.open();
							}
							else if (PlayerBarricadeSignUI.active)
							{
								PlayerBarricadeSignUI.close();
								PlayerLifeUI.open();
							}
							else if (PlayerBarricadeStereoUI.active)
							{
								PlayerBarricadeStereoUI.close();
								PlayerLifeUI.open();
							}
							else if (PlayerBarricadeLibraryUI.active)
							{
								PlayerBarricadeLibraryUI.close();
								PlayerLifeUI.open();
							}
							else if (PlayerBarricadeMannequinUI.active)
							{
								PlayerBarricadeMannequinUI.close();
								PlayerLifeUI.open();
							}
							else if (PlayerNPCDialogueUI.active)
							{
								if (PlayerNPCDialogueUI.dialogueAnimating)
								{
									PlayerNPCDialogueUI.skipText();
								}
								else if (PlayerNPCDialogueUI.dialogueHasNextPage)
								{
									PlayerNPCDialogueUI.nextPage();
								}
								else
								{
									PlayerNPCDialogueUI.close();
									PlayerLifeUI.open();
								}
							}
							else if (PlayerNPCQuestUI.active)
							{
								PlayerNPCQuestUI.closeNicely();
							}
							else if (PlayerNPCVendorUI.active)
							{
								PlayerNPCVendorUI.closeNicely();
							}
						}
						else if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
						{
							VehicleManager.exitVehicle();
						}
						else if (PlayerInteract.focus != null && PlayerInteract.interactable != null)
						{
							if (PlayerInteract.interactable.checkUseable())
							{
								PlayerInteract.interactable.use();
							}
						}
						else if (PlayerInteract.purchaseAsset != null)
						{
							if (base.player.skills.experience >= base.player.movement.purchaseNode.cost)
							{
								base.player.skills.sendPurchase(base.player.movement.purchaseNode);
							}
						}
						else if (ControlsSettings.inspect == ControlsSettings.interact && base.player.equipment.canInspect)
						{
							base.channel.send("askInspect", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
						}
					}
					else if (Time.realtimeSinceStartup - PlayerInteract.lastKeyDown > this.salvageTime)
					{
						PlayerInteract.isHoldingKey = false;
						if (!PlayerUI.window.showCursor && PlayerInteract.interactable2 != null)
						{
							PlayerInteract.interactable2.use();
						}
					}
				}
			}
		}

		private void Start()
		{
			if (base.channel.isOwner)
			{
				PlayerMovement movement = base.player.movement;
				movement.onPurchaseUpdated = (PurchaseUpdated)Delegate.Combine(movement.onPurchaseUpdated, new PurchaseUpdated(this.onPurchaseUpdated));
			}
		}

		private static Transform focus;

		private static Transform target;

		private static ItemAsset purchaseAsset;

		private static Interactable _interactable;

		private static Interactable2 _interactable2;

		private static RaycastHit hit;

		private Color highlight;

		private static float lastInteract;

		private static float lastKeyDown;

		private static bool isHoldingKey;
	}
}

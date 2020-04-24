using System;
using Pathfinding;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableObjectBinaryState : InteractableObject
	{
		public bool isUsed
		{
			get
			{
				return this._isUsed;
			}
		}

		public bool isUsable
		{
			get
			{
				return Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityDelay && (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired);
			}
		}

		public bool checkCanReset(float multiplier)
		{
			return this.isUsed && base.objectAsset.interactabilityReset > 1f && Time.realtimeSinceStartup - this.lastUsed > base.objectAsset.interactabilityReset * multiplier;
		}

		private void initAnimationComponent()
		{
			Transform transform = base.transform.FindChild("Root");
			if (transform != null)
			{
				this.animationComponent = transform.GetComponent<Animation>();
			}
		}

		private void updateAnimationComponent(bool applyInstantly)
		{
			if (this.animationComponent != null)
			{
				if (this.isUsed)
				{
					this.animationComponent.Play("Open");
				}
				else
				{
					this.animationComponent.Play("Close");
				}
				if (applyInstantly)
				{
					if (this.isUsed)
					{
						this.animationComponent["Open"].normalizedTime = 1f;
					}
					else
					{
						this.animationComponent["Close"].normalizedTime = 1f;
					}
				}
			}
		}

		private void initAudioSourceComponent()
		{
			this.audioSourceComponent = base.transform.GetComponent<AudioSource>();
		}

		private void updateAudioSourceComponent()
		{
			if (this.audioSourceComponent != null && !Dedicator.isDedicated)
			{
				this.audioSourceComponent.Play();
			}
		}

		private void initCutComponent()
		{
			if (base.objectAsset.interactabilityNav != EObjectInteractabilityNav.NONE)
			{
				Transform transform = base.transform.FindChild("Nav");
				if (transform != null)
				{
					Transform transform2 = transform.FindChild("Blocker");
					if (transform2 != null)
					{
						this.cutComponent = transform2.GetComponent<NavmeshCut>();
						this.cutHeight = this.cutComponent.height;
					}
				}
			}
		}

		private void updateCutComponent()
		{
			if (this.cutComponent != null)
			{
				if ((base.objectAsset.interactabilityNav == EObjectInteractabilityNav.ON && !this.isUsed) || (base.objectAsset.interactabilityNav == EObjectInteractabilityNav.OFF && this.isUsed))
				{
					this.cutHeight = this.cutComponent.height;
					this.cutComponent.height = 0f;
				}
				else
				{
					this.cutComponent.height = this.cutHeight;
				}
				this.cutComponent.ForceUpdate();
			}
		}

		private void initToggleGameObject()
		{
			Transform transform = base.transform.FindChildRecursive("Toggle");
			LightLODTool.applyLightLOD(transform);
			if (transform != null)
			{
				this.material = HighlighterTool.getMaterialInstance(transform.parent);
				this.toggleGameObject = transform.gameObject;
			}
		}

		private void updateToggleGameObject()
		{
			if (this.toggleGameObject != null)
			{
				if (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.STAY)
				{
					if (this.material != null)
					{
						this.material.SetColor("_EmissionColor", (!this.isUsed || !base.isWired) ? Color.black : Color.white);
					}
					this.toggleGameObject.SetActive(this.isUsed && base.isWired);
				}
				else
				{
					if (this.material != null)
					{
						this.material.SetColor("_EmissionColor", (!this.isUsed) ? Color.black : Color.white);
					}
					this.toggleGameObject.SetActive(this.isUsed);
				}
			}
		}

		public void updateToggle(bool newUsed)
		{
			this.lastUsed = Time.realtimeSinceStartup;
			this._isUsed = newUsed;
			this.updateAnimationComponent(false);
			this.updateCutComponent();
			this.updateAudioSourceComponent();
			this.updateToggleGameObject();
		}

		protected override void updateWired()
		{
			this.updateToggleGameObject();
		}

		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._isUsed = (state[0] == 1);
			if (!this.isInit)
			{
				this.isInit = true;
				this.initAnimationComponent();
				this.initCutComponent();
				this.initAudioSourceComponent();
				this.initToggleGameObject();
			}
			this.updateAnimationComponent(true);
			this.updateCutComponent();
			this.updateToggleGameObject();
		}

		public override void use()
		{
			ObjectManager.toggleObjectBinaryState(base.transform);
		}

		public override bool checkInteractable()
		{
			return !base.objectAsset.interactabilityRemote;
		}

		public override bool checkUseable()
		{
			return (base.objectAsset.interactabilityPower == EObjectInteractabilityPower.NONE || base.isWired) && base.objectAsset.areInteractabilityConditionsMet(Player.player);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			for (int i = 0; i < base.objectAsset.interactabilityConditions.Length; i++)
			{
				INPCCondition inpccondition = base.objectAsset.interactabilityConditions[i];
				if (!inpccondition.isConditionMet(Player.player))
				{
					message = EPlayerMessage.CONDITION;
					text = inpccondition.formatCondition(Player.player);
					color = Color.white;
					return true;
				}
			}
			if (base.objectAsset.interactabilityPower != EObjectInteractabilityPower.NONE && !base.isWired)
			{
				message = EPlayerMessage.POWER;
			}
			else if (this.isUsed)
			{
				switch (base.objectAsset.interactabilityHint)
				{
				case EObjectInteractabilityHint.DOOR:
					message = EPlayerMessage.DOOR_CLOSE;
					break;
				case EObjectInteractabilityHint.SWITCH:
					message = EPlayerMessage.SPOT_OFF;
					break;
				case EObjectInteractabilityHint.FIRE:
					message = EPlayerMessage.FIRE_OFF;
					break;
				case EObjectInteractabilityHint.GENERATOR:
					message = EPlayerMessage.GENERATOR_OFF;
					break;
				case EObjectInteractabilityHint.USE:
					message = EPlayerMessage.USE;
					break;
				default:
					message = EPlayerMessage.NONE;
					break;
				}
			}
			else
			{
				switch (base.objectAsset.interactabilityHint)
				{
				case EObjectInteractabilityHint.DOOR:
					message = EPlayerMessage.DOOR_OPEN;
					break;
				case EObjectInteractabilityHint.SWITCH:
					message = EPlayerMessage.SPOT_ON;
					break;
				case EObjectInteractabilityHint.FIRE:
					message = EPlayerMessage.FIRE_ON;
					break;
				case EObjectInteractabilityHint.GENERATOR:
					message = EPlayerMessage.GENERATOR_ON;
					break;
				case EObjectInteractabilityHint.USE:
					message = EPlayerMessage.USE;
					break;
				default:
					message = EPlayerMessage.NONE;
					break;
				}
			}
			text = string.Empty;
			color = Color.white;
			return true;
		}

		private void OnEnable()
		{
			this.updateAnimationComponent(true);
		}

		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
			}
		}

		private bool _isUsed;

		private bool isInit;

		private float lastUsed = -9999f;

		private Animation animationComponent;

		private AudioSource audioSourceComponent;

		private NavmeshCut cutComponent;

		private float cutHeight;

		private Material material;

		private GameObject toggleGameObject;
	}
}

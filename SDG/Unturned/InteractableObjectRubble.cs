using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableObjectRubble : MonoBehaviour
	{
		public ObjectAsset asset { get; protected set; }

		public byte getSectionCount()
		{
			return (byte)this.rubbleInfos.Length;
		}

		public Transform getSection(byte section)
		{
			return this.rubbleInfos[(int)section].section;
		}

		public bool isAllAlive()
		{
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				RubbleInfo rubbleInfo = this.rubbleInfos[(int)b];
				if (rubbleInfo.isDead)
				{
					return false;
				}
				b += 1;
			}
			return true;
		}

		public bool isAllDead()
		{
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				RubbleInfo rubbleInfo = this.rubbleInfos[(int)b];
				if (!rubbleInfo.isDead)
				{
					return false;
				}
				b += 1;
			}
			return true;
		}

		public bool isSectionDead(byte section)
		{
			return this.rubbleInfos[(int)section].isDead;
		}

		public void askDamage(byte section, ushort amount)
		{
			if (section == 255)
			{
				section = 0;
				while ((int)section < this.rubbleInfos.Length)
				{
					this.rubbleInfos[(int)section].askDamage(amount);
					section += 1;
				}
			}
			else
			{
				this.rubbleInfos[(int)section].askDamage(amount);
			}
		}

		public byte checkCanReset(float multiplier)
		{
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				if (this.rubbleInfos[(int)b].isDead && this.asset.rubbleReset > 1f && Time.realtimeSinceStartup - this.rubbleInfos[(int)b].lastDead > this.asset.rubbleReset * multiplier)
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		public byte getSection(Transform section)
		{
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				RubbleInfo rubbleInfo = this.rubbleInfos[(int)b];
				if (section == rubbleInfo.section || section.parent == rubbleInfo.section || section.parent.parent == rubbleInfo.section)
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		public void updateRubble(byte section, bool isAlive, bool playEffect, Vector3 ragdoll)
		{
			RubbleInfo rubbleInfo = this.rubbleInfos[(int)section];
			if (isAlive)
			{
				rubbleInfo.health = this.asset.rubbleHealth;
			}
			else
			{
				rubbleInfo.lastDead = Time.realtimeSinceStartup;
				rubbleInfo.health = 0;
			}
			bool flag = this.isAllDead();
			if (rubbleInfo.aliveGameObject != null)
			{
				rubbleInfo.aliveGameObject.SetActive(!rubbleInfo.isDead);
			}
			if (rubbleInfo.deadGameObject != null)
			{
				rubbleInfo.deadGameObject.SetActive(rubbleInfo.isDead && (!flag || this.asset.rubbleFinale == 0));
			}
			if (this.aliveGameObject != null)
			{
				this.aliveGameObject.SetActive(!flag);
			}
			if (this.deadGameObject != null)
			{
				this.deadGameObject.SetActive(flag);
			}
			if (!Dedicator.isDedicated && playEffect)
			{
				if (rubbleInfo.ragdolls != null && GraphicsSettings.debris && rubbleInfo.isDead)
				{
					for (int i = 0; i < rubbleInfo.ragdolls.Length; i++)
					{
						RubbleRagdollInfo rubbleRagdollInfo = rubbleInfo.ragdolls[i];
						if (rubbleRagdollInfo != null)
						{
							Vector3 vector = ragdoll;
							if (rubbleRagdollInfo.forceTransform != null)
							{
								vector = rubbleRagdollInfo.forceTransform.forward * vector.magnitude * rubbleRagdollInfo.forceTransform.localScale.z;
								vector += rubbleRagdollInfo.forceTransform.right * Random.Range(-16f, 16f) * rubbleRagdollInfo.forceTransform.localScale.x;
								vector += rubbleRagdollInfo.forceTransform.up * Random.Range(-16f, 16f) * rubbleRagdollInfo.forceTransform.localScale.y;
							}
							else
							{
								vector.y += 8f;
								vector.x += Random.Range(-16f, 16f);
								vector.z += Random.Range(-16f, 16f);
							}
							vector *= (float)((!(Player.player != null) || Player.player.skills.boost != EPlayerBoost.FLIGHT) ? 2 : 4);
							GameObject gameObject = Object.Instantiate<GameObject>(rubbleRagdollInfo.ragdollGameObject, rubbleRagdollInfo.ragdollGameObject.transform.position, rubbleRagdollInfo.ragdollGameObject.transform.rotation);
							gameObject.name = "Ragdoll";
							gameObject.transform.parent = Level.effects;
							gameObject.transform.localScale = base.transform.localScale;
							gameObject.SetActive(true);
							gameObject.gameObject.AddComponent<Rigidbody>();
							gameObject.GetComponent<Rigidbody>().interpolation = 1;
							gameObject.GetComponent<Rigidbody>().collisionDetectionMode = 0;
							gameObject.GetComponent<Rigidbody>().AddForce(vector);
							gameObject.GetComponent<Rigidbody>().drag = 0.5f;
							gameObject.GetComponent<Rigidbody>().angularDrag = 0.1f;
							Object.Destroy(gameObject, 8f);
						}
					}
				}
				if (this.asset.rubbleEffect != 0 && rubbleInfo.isDead)
				{
					if (rubbleInfo.effectTransform != null)
					{
						EffectManager.effect(this.asset.rubbleEffect, rubbleInfo.effectTransform.position, rubbleInfo.effectTransform.forward);
					}
					else
					{
						EffectManager.effect(this.asset.rubbleEffect, rubbleInfo.section.position, Vector3.up);
					}
				}
				if (this.asset.rubbleFinale != 0 && flag)
				{
					if (this.finaleTransform != null)
					{
						EffectManager.effect(this.asset.rubbleFinale, this.finaleTransform.position, this.finaleTransform.forward);
					}
					else
					{
						EffectManager.effect(this.asset.rubbleFinale, base.transform.position, Vector3.up);
					}
				}
			}
		}

		public void updateState(Asset asset, byte[] state)
		{
			this.asset = (asset as ObjectAsset);
			Transform transform = base.transform.FindChild("Sections");
			if (transform != null)
			{
				this.rubbleInfos = new RubbleInfo[transform.childCount];
				for (int i = 0; i < this.rubbleInfos.Length; i++)
				{
					Transform section = transform.FindChild("Section_" + i);
					RubbleInfo rubbleInfo = new RubbleInfo();
					rubbleInfo.section = section;
					this.rubbleInfos[i] = rubbleInfo;
				}
				Transform transform2 = base.transform.FindChild("Alive");
				if (transform2 != null)
				{
					this.aliveGameObject = transform2.gameObject;
				}
				Transform transform3 = base.transform.FindChild("Dead");
				if (transform3 != null)
				{
					this.deadGameObject = transform3.gameObject;
				}
				this.finaleTransform = base.transform.FindChild("Finale");
			}
			else
			{
				this.rubbleInfos = new RubbleInfo[1];
				RubbleInfo rubbleInfo2 = new RubbleInfo();
				rubbleInfo2.section = base.transform;
				this.rubbleInfos[0] = rubbleInfo2;
			}
			byte b = 0;
			while ((int)b < this.rubbleInfos.Length)
			{
				RubbleInfo rubbleInfo3 = this.rubbleInfos[(int)b];
				Transform section2 = rubbleInfo3.section;
				Transform transform4 = section2.FindChild("Alive");
				if (transform4 != null)
				{
					rubbleInfo3.aliveGameObject = transform4.gameObject;
				}
				Transform transform5 = section2.FindChild("Dead");
				if (transform5 != null)
				{
					rubbleInfo3.deadGameObject = transform5.gameObject;
				}
				Transform transform6 = section2.FindChild("Ragdolls");
				if (transform6 != null)
				{
					rubbleInfo3.ragdolls = new RubbleRagdollInfo[transform6.childCount];
					for (int j = 0; j < rubbleInfo3.ragdolls.Length; j++)
					{
						Transform transform7 = transform6.FindChild("Ragdoll_" + j);
						Transform transform8 = transform7.FindChild("Ragdoll");
						if (transform8 != null)
						{
							rubbleInfo3.ragdolls[j] = new RubbleRagdollInfo();
							rubbleInfo3.ragdolls[j].ragdollGameObject = transform8.gameObject;
							rubbleInfo3.ragdolls[j].forceTransform = transform7.FindChild("Force");
						}
					}
				}
				else
				{
					Transform transform9 = section2.FindChild("Ragdoll");
					if (transform9 != null)
					{
						rubbleInfo3.ragdolls = new RubbleRagdollInfo[1];
						rubbleInfo3.ragdolls[0] = new RubbleRagdollInfo();
						rubbleInfo3.ragdolls[0].ragdollGameObject = transform9.gameObject;
						rubbleInfo3.ragdolls[0].forceTransform = section2.FindChild("Force");
					}
				}
				rubbleInfo3.effectTransform = section2.FindChild("Effect");
				b += 1;
			}
			byte b2 = 0;
			while ((int)b2 < this.rubbleInfos.Length)
			{
				bool isAlive = (state[state.Length - 1] & Types.SHIFTS[(int)b2]) == Types.SHIFTS[(int)b2];
				this.updateRubble(b2, isAlive, false, Vector3.zero);
				b2 += 1;
			}
		}

		private RubbleInfo[] rubbleInfos;

		private GameObject aliveGameObject;

		private GameObject deadGameObject;

		private Transform finaleTransform;
	}
}

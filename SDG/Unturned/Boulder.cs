using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Boulder : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (this.isExploded)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (other.transform.CompareTag("Agent"))
			{
				return;
			}
			this.isExploded = true;
			Vector3 normalized = (base.transform.position - this.lastPos).normalized;
			if (Provider.isServer)
			{
				float num = Mathf.Clamp(base.transform.parent.GetComponent<Rigidbody>().velocity.magnitude, 0f, 20f);
				if (num < 3f)
				{
					return;
				}
				if (other.transform.CompareTag("Player"))
				{
					Player player = DamageTool.getPlayer(other.transform);
					if (player != null)
					{
						EPlayerKill eplayerKill;
						DamageTool.damage(player, EDeathCause.BOULDER, ELimb.SPINE, CSteamID.Nil, normalized, Boulder.DAMAGE_PLAYER, num, out eplayerKill);
					}
				}
				else if (other.transform.CompareTag("Vehicle"))
				{
					VehicleManager.damage(other.transform.GetComponent<InteractableVehicle>(), Boulder.DAMAGE_VEHICLE, num, true);
				}
				else if (other.transform.CompareTag("Barricade"))
				{
					Transform transform = other.transform;
					InteractableDoorHinge component = transform.GetComponent<InteractableDoorHinge>();
					if (component != null)
					{
						transform = component.transform.parent.parent;
					}
					BarricadeManager.damage(transform, Boulder.DAMAGE_BARRICADE, num, true);
				}
				else if (other.transform.CompareTag("Structure"))
				{
					StructureManager.damage(other.transform, normalized, Boulder.DAMAGE_STRUCTURE, num, true);
				}
				else if (other.transform.CompareTag("Resource"))
				{
					EPlayerKill eplayerKill2;
					uint num2;
					ResourceManager.damage(other.transform, normalized, Boulder.DAMAGE_RESOURCE, num, 1f, out eplayerKill2, out num2);
				}
				else
				{
					InteractableObjectRubble componentInParent = other.transform.GetComponentInParent<InteractableObjectRubble>();
					if (componentInParent != null)
					{
						EPlayerKill eplayerKill3;
						uint num3;
						DamageTool.damage(componentInParent.transform, normalized, componentInParent.getSection(other.transform), Boulder.DAMAGE_OBJECT, num, out eplayerKill3, out num3);
					}
				}
			}
			if (!Dedicator.isDedicated)
			{
				EffectManager.effect(52, base.transform.position, -normalized);
			}
		}

		private void FixedUpdate()
		{
			this.lastPos = base.transform.position;
		}

		private void Awake()
		{
			this.lastPos = base.transform.position;
		}

		private static readonly float DAMAGE_PLAYER = 3f;

		private static readonly float DAMAGE_BARRICADE = 15f;

		private static readonly float DAMAGE_STRUCTURE = 15f;

		private static readonly float DAMAGE_OBJECT = 25f;

		private static readonly float DAMAGE_VEHICLE = 10f;

		private static readonly float DAMAGE_RESOURCE = 25f;

		private bool isExploded;

		private Vector3 lastPos;
	}
}

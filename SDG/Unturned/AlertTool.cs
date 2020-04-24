using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	internal class AlertTool
	{
		private static bool check(Vector3 forward, Vector3 offset, float sqrRadius, bool sneak, Vector3 spotDir, bool isSpotOn)
		{
			if (isSpotOn && offset.sqrMagnitude < 576f)
			{
				float num = Vector3.Dot(spotDir, offset.normalized);
				if (num > 0.75f)
				{
					return true;
				}
			}
			if (offset.sqrMagnitude > sqrRadius)
			{
				return false;
			}
			float num2 = Vector3.Dot(forward, offset.normalized);
			return (double)num2 <= 0.5 || !sneak;
		}

		public static void alert(Player player, Vector3 position, float radius, bool sneak, Vector3 spotDir, bool isSpotOn)
		{
			radius *= Provider.modeConfigData.Players.Detect_Radius_Multiplier;
			radius = Mathf.Clamp(radius, 0f, 64f);
			if (player == null)
			{
				return;
			}
			float sqrRadius = radius * radius;
			RaycastHit raycastHit;
			if (player.movement.nav != 255)
			{
				if (ZombieManager.regions[(int)player.movement.nav].hasBeacon)
				{
					for (int i = 0; i < ZombieManager.regions[(int)player.movement.nav].zombies.Count; i++)
					{
						Zombie zombie = ZombieManager.regions[(int)player.movement.nav].zombies[i];
						if (!zombie.isDead)
						{
							if (zombie.checkAlert(player))
							{
								zombie.alert(player);
							}
						}
					}
				}
				AlertTool.zombiesInRadius.Clear();
				ZombieManager.getZombiesInRadius(position, sqrRadius, AlertTool.zombiesInRadius);
				for (int j = 0; j < AlertTool.zombiesInRadius.Count; j++)
				{
					Zombie zombie2 = AlertTool.zombiesInRadius[j];
					if (!zombie2.isDead)
					{
						if (zombie2.checkAlert(player))
						{
							Vector3 vector = zombie2.transform.position - position;
							if (AlertTool.check(zombie2.transform.forward, vector, sqrRadius, sneak, spotDir, isSpotOn))
							{
								Physics.Raycast(zombie2.transform.position + Vector3.up, -vector, ref raycastHit, vector.magnitude * 0.95f, RayMasks.BLOCK_VISION);
								if (!(raycastHit.transform != null))
								{
									zombie2.alert(player);
								}
							}
						}
					}
				}
			}
			AlertTool.animalsInRadius.Clear();
			AnimalManager.getAnimalsInRadius(position, sqrRadius, AlertTool.animalsInRadius);
			for (int k = 0; k < AlertTool.animalsInRadius.Count; k++)
			{
				Animal animal = AlertTool.animalsInRadius[k];
				if (!animal.isDead)
				{
					if (animal.asset != null)
					{
						if (animal.asset.behaviour == EAnimalBehaviour.DEFENSE)
						{
							if (!animal.isFleeing)
							{
								Vector3 vector2 = animal.transform.position - position;
								if (!AlertTool.check(animal.transform.forward, vector2, sqrRadius, sneak, spotDir, isSpotOn))
								{
									goto IL_385;
								}
								Physics.Raycast(animal.transform.position + Vector3.up, -vector2, ref raycastHit, vector2.magnitude * 0.95f, RayMasks.BLOCK_VISION);
								if (raycastHit.transform != null)
								{
									goto IL_385;
								}
							}
							animal.alertPoint(player.transform.position, true);
						}
						else if (animal.asset.behaviour == EAnimalBehaviour.OFFENSE)
						{
							if (animal.checkAlert(player))
							{
								Vector3 vector3 = animal.transform.position - position;
								if (AlertTool.check(animal.transform.forward, vector3, sqrRadius, sneak, spotDir, isSpotOn))
								{
									Physics.Raycast(animal.transform.position + Vector3.up, -vector3, ref raycastHit, vector3.magnitude * 0.95f, RayMasks.BLOCK_VISION);
									if (!(raycastHit.transform != null))
									{
										animal.alertPlayer(player, true);
									}
								}
							}
						}
					}
				}
				IL_385:;
			}
		}

		public static void alert(Vector3 position, float radius)
		{
			float sqrRadius = radius * radius;
			if (LevelNavigation.checkNavigation(position))
			{
				AlertTool.zombiesInRadius.Clear();
				ZombieManager.getZombiesInRadius(position, sqrRadius, AlertTool.zombiesInRadius);
				for (int i = 0; i < AlertTool.zombiesInRadius.Count; i++)
				{
					Zombie zombie = AlertTool.zombiesInRadius[i];
					if (!zombie.isDead)
					{
						zombie.alert(position, true);
					}
				}
			}
			AlertTool.animalsInRadius.Clear();
			AnimalManager.getAnimalsInRadius(position, sqrRadius, AlertTool.animalsInRadius);
			for (int j = 0; j < AlertTool.animalsInRadius.Count; j++)
			{
				Animal animal = AlertTool.animalsInRadius[j];
				if (!animal.isDead)
				{
					if (animal.asset != null)
					{
						if (animal.asset.behaviour != EAnimalBehaviour.IGNORE)
						{
							animal.alertPoint(position, true);
						}
					}
				}
			}
		}

		private static List<Zombie> zombiesInRadius = new List<Zombie>();

		private static List<Animal> animalsInRadius = new List<Animal>();
	}
}

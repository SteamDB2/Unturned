using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class PowerTool
	{
		public static void checkInteractables<T>(Vector3 point, float radius, ushort plant, List<T> interactablesInRadius) where T : Interactable
		{
			float sqrRadius = radius * radius;
			if (plant == 65535)
			{
				PowerTool.regionsInRadius.Clear();
				Regions.getRegionsInRadius(point, radius, PowerTool.regionsInRadius);
				PowerTool.barricadesInRadius.Clear();
				BarricadeManager.getBarricadesInRadius(point, sqrRadius, PowerTool.regionsInRadius, PowerTool.barricadesInRadius);
				ObjectManager.getObjectsInRadius(point, sqrRadius, PowerTool.regionsInRadius, PowerTool.barricadesInRadius);
			}
			else
			{
				PowerTool.barricadesInRadius.Clear();
				BarricadeManager.getBarricadesInRadius(point, sqrRadius, plant, PowerTool.barricadesInRadius);
			}
			for (int i = 0; i < PowerTool.barricadesInRadius.Count; i++)
			{
				T component = PowerTool.barricadesInRadius[i].GetComponent<T>();
				if (!(component == null))
				{
					interactablesInRadius.Add(component);
				}
			}
		}

		public static void checkInteractables<T>(Vector3 point, float radius, List<T> interactablesInRadius) where T : Interactable
		{
			float sqrRadius = radius * radius;
			PowerTool.regionsInRadius.Clear();
			Regions.getRegionsInRadius(point, radius, PowerTool.regionsInRadius);
			PowerTool.barricadesInRadius.Clear();
			BarricadeManager.getBarricadesInRadius(point, sqrRadius, PowerTool.regionsInRadius, PowerTool.barricadesInRadius);
			BarricadeManager.getBarricadesInRadius(point, sqrRadius, PowerTool.barricadesInRadius);
			for (int i = 0; i < PowerTool.barricadesInRadius.Count; i++)
			{
				T component = PowerTool.barricadesInRadius[i].GetComponent<T>();
				if (!(component == null))
				{
					interactablesInRadius.Add(component);
				}
			}
		}

		public static bool checkFires(Vector3 point, float radius)
		{
			PowerTool.firesInRadius.Clear();
			PowerTool.checkInteractables<InteractableFire>(point, radius, ushort.MaxValue, PowerTool.firesInRadius);
			for (int i = 0; i < PowerTool.firesInRadius.Count; i++)
			{
				if (PowerTool.firesInRadius[i].isLit)
				{
					return true;
				}
			}
			PowerTool.ovensInRadius.Clear();
			PowerTool.checkInteractables<InteractableOven>(point, radius, ushort.MaxValue, PowerTool.ovensInRadius);
			for (int j = 0; j < PowerTool.ovensInRadius.Count; j++)
			{
				if (PowerTool.ovensInRadius[j].isWired && PowerTool.ovensInRadius[j].isLit)
				{
					return true;
				}
			}
			return false;
		}

		public static List<InteractableGenerator> checkGenerators(Vector3 point, float radius, ushort plant)
		{
			PowerTool.generatorsInRadius.Clear();
			PowerTool.checkInteractables<InteractableGenerator>(point, radius, plant, PowerTool.generatorsInRadius);
			return PowerTool.generatorsInRadius;
		}

		public static List<InteractablePower> checkPower(Vector3 point, float radius, ushort plant)
		{
			PowerTool.powerInRadius.Clear();
			PowerTool.checkInteractables<InteractablePower>(point, radius, plant, PowerTool.powerInRadius);
			return PowerTool.powerInRadius;
		}

		private static List<RegionCoordinate> regionsInRadius = new List<RegionCoordinate>(4);

		private static List<Transform> barricadesInRadius = new List<Transform>();

		private static List<InteractableFire> firesInRadius = new List<InteractableFire>();

		private static List<InteractableOven> ovensInRadius = new List<InteractableOven>();

		private static List<InteractablePower> powerInRadius = new List<InteractablePower>();

		private static List<InteractableGenerator> generatorsInRadius = new List<InteractableGenerator>();
	}
}

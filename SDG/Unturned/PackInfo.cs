using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class PackInfo
	{
		public PackInfo()
		{
			this.spawns = new List<AnimalSpawnpoint>();
			this.animals = new List<Animal>();
			this.wanderAngle = Random.Range(0f, 360f);
		}

		public List<AnimalSpawnpoint> spawns { get; private set; }

		public List<Animal> animals { get; private set; }

		public float wanderAngle
		{
			get
			{
				return this._wanderAngle;
			}
			set
			{
				this._wanderAngle = value;
				this.wanderNormal = new Vector3(Mathf.Cos(0.0174532924f * this.wanderAngle), 0f, Mathf.Sin(0.0174532924f * this.wanderAngle));
			}
		}

		public Vector3 getWanderDirection()
		{
			return this.wanderNormal;
		}

		public Vector3 getAverageSpawnPoint()
		{
			if (this.avgSpawnPoint == null)
			{
				this.avgSpawnPoint = new Vector3?(Vector3.zero);
				for (int i = 0; i < this.spawns.Count; i++)
				{
					AnimalSpawnpoint animalSpawnpoint = this.spawns[i];
					if (animalSpawnpoint != null)
					{
						Vector3? vector = this.avgSpawnPoint;
						this.avgSpawnPoint = ((vector == null) ? null : new Vector3?(vector.GetValueOrDefault() + animalSpawnpoint.point));
					}
				}
				Vector3? vector2 = this.avgSpawnPoint;
				this.avgSpawnPoint = ((vector2 == null) ? null : new Vector3?(vector2.GetValueOrDefault() / (float)this.spawns.Count));
			}
			return this.avgSpawnPoint.Value;
		}

		public Vector3 getAverageAnimalPoint()
		{
			if (Time.frameCount > this.avgAnimalPointRecalculation)
			{
				this.avgAnimalPoint = Vector3.zero;
				for (int i = 0; i < this.animals.Count; i++)
				{
					Animal animal = this.animals[i];
					if (!(animal == null))
					{
						this.avgAnimalPoint += animal.transform.position;
					}
				}
				this.avgAnimalPoint /= (float)this.animals.Count;
				this.avgAnimalPointRecalculation = Time.frameCount;
			}
			return this.avgAnimalPoint;
		}

		private Vector3 wanderNormal;

		private float _wanderAngle;

		private Vector3? avgSpawnPoint;

		private int avgAnimalPointRecalculation;

		private Vector3 avgAnimalPoint;
	}
}

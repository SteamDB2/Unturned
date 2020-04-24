using System;
using UnityEngine;

namespace Pathfinding
{
	[RequireComponent(typeof(LocalAvoidance))]
	[Obsolete("Use the RVO system instead")]
	public class LocalAvoidanceMover : MonoBehaviour
	{
		private void Start()
		{
			this.targetPoint = base.transform.forward * this.targetPointDist + base.transform.position;
			this.controller = base.GetComponent<LocalAvoidance>();
		}

		private void Update()
		{
			if (this.controller != null)
			{
				this.controller.SimpleMove((this.targetPoint - base.transform.position).normalized * this.speed);
			}
		}

		public float targetPointDist = 10f;

		public float speed = 2f;

		private Vector3 targetPoint;

		private LocalAvoidance controller;
	}
}

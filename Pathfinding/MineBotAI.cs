using System;
using UnityEngine;

namespace Pathfinding
{
	[RequireComponent(typeof(Seeker))]
	public class MineBotAI : AIPath
	{
		public new void Start()
		{
			this.anim["forward"].layer = 10;
			this.anim.Play("awake");
			this.anim.Play("forward");
			this.anim["awake"].wrapMode = 1;
			this.anim["awake"].speed = 0f;
			this.anim["awake"].normalizedTime = 1f;
			base.Start();
		}

		public override void OnTargetReached()
		{
			if (this.endOfPathEffect != null && Vector3.Distance(this.tr.position, this.lastTarget) > 1f)
			{
				Object.Instantiate<GameObject>(this.endOfPathEffect, this.tr.position, this.tr.rotation);
				this.lastTarget = this.tr.position;
			}
		}

		public override Vector3 GetFeetPosition()
		{
			return this.tr.position;
		}

		protected void Update()
		{
			Vector3 vector2;
			if (this.canMove)
			{
				Vector3 vector = base.CalculateVelocity(this.GetFeetPosition());
				this.RotateTowards(this.targetDirection);
				vector.y = 0f;
				if (vector.sqrMagnitude <= this.sleepVelocity * this.sleepVelocity)
				{
					vector = Vector3.zero;
				}
				if (this.rvoController != null)
				{
					this.rvoController.Move(vector);
					vector2 = this.rvoController.velocity;
				}
				else if (this.navController != null)
				{
					vector2 = Vector3.zero;
				}
				else if (this.controller != null)
				{
					this.controller.SimpleMove(vector);
					vector2 = this.controller.velocity;
				}
				else
				{
					Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
					vector2 = Vector3.zero;
				}
			}
			else
			{
				vector2 = Vector3.zero;
			}
			Vector3 vector3 = this.tr.InverseTransformDirection(vector2);
			vector3.y = 0f;
			if (vector2.sqrMagnitude <= this.sleepVelocity * this.sleepVelocity)
			{
				this.anim.Blend("forward", 0f, 0.2f);
			}
			else
			{
				this.anim.Blend("forward", 1f, 0.2f);
				AnimationState animationState = this.anim["forward"];
				float z = vector3.z;
				animationState.speed = z * this.animationSpeed;
			}
		}

		public Animation anim;

		public float sleepVelocity = 0.4f;

		public float animationSpeed = 0.2f;

		public GameObject endOfPathEffect;

		protected Vector3 lastTarget;
	}
}

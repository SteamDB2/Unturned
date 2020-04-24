using System;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[AddComponentMenu("Pathfinding/AI/AIPath (generic)")]
public class AIPath : MonoBehaviour
{
	public bool TargetReached
	{
		get
		{
			return this.targetReached;
		}
	}

	protected virtual void Awake()
	{
		this.seeker = base.GetComponent<Seeker>();
		this.tr = base.transform;
		this.controller = base.GetComponent<CharacterController>();
		this.navController = base.GetComponent<NavmeshController>();
		this.rvoController = base.GetComponent<RVOController>();
		if (this.rvoController != null)
		{
			this.rvoController.enableRotation = false;
		}
		this.rigid = base.GetComponent<Rigidbody>();
	}

	protected virtual void Start()
	{
		this.startHasRun = true;
		this.OnEnable();
	}

	protected virtual void OnEnable()
	{
		this.lastRepath = -9999f;
		this.canSearchAgain = true;
		this.lastFoundWaypointPosition = this.GetFeetPosition();
		if (this.startHasRun)
		{
			Seeker seeker = this.seeker;
			seeker.pathCallback = (OnPathDelegate)Delegate.Combine(seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
		}
	}

	public void OnDisable()
	{
		if (this.seeker != null && !this.seeker.IsDone())
		{
			this.seeker.GetCurrentPath().Error();
		}
		if (this.path != null)
		{
			this.path.Release(this);
		}
		this.path = null;
		Seeker seeker = this.seeker;
		seeker.pathCallback = (OnPathDelegate)Delegate.Remove(seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
	}

	public virtual void SearchPath()
	{
		if (this.target == null)
		{
			throw new InvalidOperationException("Target is null");
		}
		this.lastRepath = Time.time;
		Vector3 position = this.target.position;
		this.canSearchAgain = false;
		this.seeker.StartPath(this.GetFeetPosition(), position);
	}

	public virtual void OnTargetReached()
	{
	}

	public virtual void OnPathComplete(Path _p)
	{
		ABPath abpath = _p as ABPath;
		if (abpath == null)
		{
			throw new Exception("This function only handles ABPaths, do not use special path types");
		}
		this.canSearchAgain = true;
		abpath.Claim(this);
		if (abpath.error)
		{
			abpath.Release(this);
			return;
		}
		if (this.path != null)
		{
			this.path.Release(this);
		}
		this.path = abpath;
		this.currentWaypointIndex = 0;
		this.targetReached = false;
		if (this.closestOnPathCheck)
		{
			Vector3 vector = (Time.time - this.lastFoundWaypointTime >= 0.3f) ? abpath.originalStartPoint : this.lastFoundWaypointPosition;
			Vector3 feetPosition = this.GetFeetPosition();
			Vector3 vector2 = feetPosition - vector;
			float magnitude = vector2.magnitude;
			vector2 /= magnitude;
			int num = (int)(magnitude / this.pickNextWaypointDist);
			for (int i = 0; i <= num; i++)
			{
				this.CalculateVelocity(vector);
				vector += vector2;
			}
		}
	}

	public void stop()
	{
		this.path = null;
	}

	public virtual Vector3 GetFeetPosition()
	{
		if (this.rvoController != null)
		{
			return this.tr.position - Vector3.up * this.rvoController.height * 0.5f;
		}
		if (this.controller != null)
		{
			return this.tr.position;
		}
		return this.tr.position;
	}

	public void move(float delta)
	{
		if (!this.canMove)
		{
			return;
		}
		if (Time.time - this.lastRepath >= this.repathRate && this.canSearchAgain && this.canSearch && this.target != null)
		{
			this.SearchPath();
		}
		if (this.path == null)
		{
			return;
		}
		Vector3 vector = this.CalculateVelocity(base.transform.position);
		vector.y = Physics.gravity.y * 2f;
		this.RotateTowards(this.targetDirection);
		if (this.rvoController != null)
		{
			this.rvoController.Move(vector);
		}
		else if (!(this.navController != null))
		{
			if (this.controller != null && this.controller.enabled)
			{
				this.controller.Move(vector * delta);
			}
			else if (this.rigid != null)
			{
				this.rigid.AddForce(vector);
			}
			else
			{
				base.transform.Translate(vector * delta, 0);
			}
		}
	}

	protected float XZSqrMagnitude(Vector3 a, Vector3 b)
	{
		float num = b.x - a.x;
		float num2 = b.z - a.z;
		return num * num + num2 * num2;
	}

	protected Vector3 CalculateVelocity(Vector3 currentPosition)
	{
		if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
		{
			return Vector3.zero;
		}
		List<Vector3> vectorPath = this.path.vectorPath;
		if (vectorPath.Count == 1)
		{
			vectorPath.Insert(0, currentPosition);
		}
		if (this.currentWaypointIndex >= vectorPath.Count)
		{
			this.currentWaypointIndex = vectorPath.Count - 1;
		}
		if (this.currentWaypointIndex <= 1)
		{
			this.currentWaypointIndex = 1;
		}
		while (this.currentWaypointIndex < vectorPath.Count - 1)
		{
			float num = this.XZSqrMagnitude(vectorPath[this.currentWaypointIndex], currentPosition);
			if (num < this.pickNextWaypointDist * this.pickNextWaypointDist)
			{
				this.lastFoundWaypointPosition = currentPosition;
				this.lastFoundWaypointTime = Time.time;
				this.currentWaypointIndex++;
			}
			else
			{
				IL_FB:
				Vector3 vector = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
				Vector3 vector2 = this.CalculateTargetPoint(currentPosition, vectorPath[this.currentWaypointIndex - 1], vectorPath[this.currentWaypointIndex], this.currentWaypointIndex == vectorPath.Count - 1);
				vector = vector2 - currentPosition;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				float num2 = Mathf.Clamp01(magnitude / this.slowdownDistance);
				if (this.canTurn)
				{
					this.targetDirection = vector;
				}
				this.targetPoint = vector2;
				if (this.currentWaypointIndex == vectorPath.Count - 1 && magnitude <= this.endReachedDistance)
				{
					if (!this.targetReached)
					{
						this.targetReached = true;
						this.OnTargetReached();
					}
					return Vector3.zero;
				}
				Vector3 forward = this.tr.forward;
				float num3 = Vector3.Dot(vector.normalized, forward);
				float num4 = this.speed * Mathf.Max(num3, this.minMoveScale) * num2;
				if (Time.deltaTime > 0f)
				{
					num4 = Mathf.Clamp(num4, 0f, magnitude / (Time.deltaTime * 2f));
				}
				return forward * num4;
			}
		}
		goto IL_FB;
	}

	protected virtual void RotateTowards(Vector3 dir)
	{
		if (dir == Vector3.zero)
		{
			return;
		}
		Quaternion quaternion = this.tr.rotation;
		Quaternion quaternion2 = Quaternion.LookRotation(dir);
		Vector3 eulerAngles = Quaternion.Slerp(quaternion, quaternion2, this.turningSpeed * Time.deltaTime).eulerAngles;
		eulerAngles.z = 0f;
		eulerAngles.x = 0f;
		quaternion = Quaternion.Euler(eulerAngles);
		this.tr.rotation = quaternion;
	}

	protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b, bool canGoDirectly)
	{
		if (canGoDirectly && (b - this.target.position).sqrMagnitude < 16f)
		{
			return this.target.position;
		}
		a.y = p.y;
		b.y = p.y;
		float magnitude = (a - b).magnitude;
		if (magnitude == 0f)
		{
			return a;
		}
		float num = AstarMath.Clamp01(AstarMath.NearestPointFactor(a, b, p));
		Vector3 vector = (b - a) * num + a;
		float magnitude2 = (vector - p).magnitude;
		float num2 = Mathf.Clamp(this.forwardLook - magnitude2, 0f, this.forwardLook);
		float num3 = num2 / magnitude;
		num3 = Mathf.Clamp(num3 + num, 0f, 1f);
		return (b - a) * num3 + a;
	}

	public float repathRate = 0.5f;

	public Transform target;

	public bool canSearch = true;

	public bool canMove = true;

	public bool canTurn = true;

	public bool canSmooth = true;

	public float speed = 3f;

	public float turningSpeed = 5f;

	public float slowdownDistance = 0.6f;

	public float pickNextWaypointDist = 2f;

	public float forwardLook = 1f;

	public float endReachedDistance = 0.2f;

	public bool closestOnPathCheck = true;

	protected float minMoveScale = 0.05f;

	protected Seeker seeker;

	protected Transform tr;

	protected float lastRepath = -9999f;

	protected Path path;

	protected CharacterController controller;

	protected NavmeshController navController;

	protected RVOController rvoController;

	protected Rigidbody rigid;

	protected int currentWaypointIndex;

	protected bool targetReached;

	protected bool canSearchAgain = true;

	protected Vector3 lastFoundWaypointPosition;

	protected float lastFoundWaypointTime = -9999f;

	private bool startHasRun;

	protected Vector3 targetPoint;

	public Vector3 targetDirection;
}

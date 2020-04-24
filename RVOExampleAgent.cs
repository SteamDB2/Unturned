using System;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

public class RVOExampleAgent : MonoBehaviour
{
	public void Awake()
	{
		this.seeker = base.GetComponent<Seeker>();
	}

	public void Start()
	{
		this.SetTarget(-base.transform.position);
		this.controller = base.GetComponent<RVOController>();
	}

	public void SetTarget(Vector3 target)
	{
		this.target = target;
		this.RecalculatePath();
	}

	public void SetColor(Color col)
	{
		if (this.rends == null)
		{
			this.rends = base.GetComponentsInChildren<MeshRenderer>();
		}
		foreach (MeshRenderer meshRenderer in this.rends)
		{
			Color color = meshRenderer.material.GetColor("_TintColor");
			AnimationCurve animationCurve = AnimationCurve.Linear(0f, color.r, 1f, col.r);
			AnimationCurve animationCurve2 = AnimationCurve.Linear(0f, color.g, 1f, col.g);
			AnimationCurve animationCurve3 = AnimationCurve.Linear(0f, color.b, 1f, col.b);
			AnimationClip animationClip = new AnimationClip();
			animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.r", animationCurve);
			animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.g", animationCurve2);
			animationClip.SetCurve(string.Empty, typeof(Material), "_TintColor.b", animationCurve3);
			Animation animation = meshRenderer.gameObject.GetComponent<Animation>();
			if (animation == null)
			{
				animation = meshRenderer.gameObject.AddComponent<Animation>();
			}
			animationClip.wrapMode = 1;
			animation.AddClip(animationClip, "ColorAnim");
			animation.Play("ColorAnim");
		}
	}

	public void RecalculatePath()
	{
		this.canSearchAgain = false;
		this.nextRepath = Time.time + this.repathRate * (Random.value + 0.5f);
		this.seeker.StartPath(base.transform.position, this.target, new OnPathDelegate(this.OnPathComplete));
	}

	public void OnPathComplete(Path _p)
	{
		ABPath abpath = _p as ABPath;
		this.canSearchAgain = true;
		if (this.path != null)
		{
			this.path.Release(this);
		}
		this.path = abpath;
		abpath.Claim(this);
		if (abpath.error)
		{
			this.wp = 0;
			this.vectorPath = null;
			return;
		}
		Vector3 originalStartPoint = abpath.originalStartPoint;
		Vector3 position = base.transform.position;
		originalStartPoint.y = position.y;
		float magnitude = (position - originalStartPoint).magnitude;
		this.wp = 0;
		this.vectorPath = abpath.vectorPath;
		for (float num = 0f; num <= magnitude; num += this.moveNextDist * 0.6f)
		{
			this.wp--;
			Vector3 vector = originalStartPoint + (position - originalStartPoint) * num;
			Vector3 vector2;
			do
			{
				this.wp++;
				vector2 = this.vectorPath[this.wp];
				vector2.y = vector.y;
			}
			while ((vector - vector2).sqrMagnitude < this.moveNextDist * this.moveNextDist && this.wp != this.vectorPath.Count - 1);
		}
	}

	public void Update()
	{
		if (Time.time >= this.nextRepath && this.canSearchAgain)
		{
			this.RecalculatePath();
		}
		Vector3 vector = Vector3.zero;
		Vector3 position = base.transform.position;
		if (this.vectorPath != null && this.vectorPath.Count != 0)
		{
			Vector3 vector2 = this.vectorPath[this.wp];
			vector2.y = position.y;
			while ((position - vector2).sqrMagnitude < this.moveNextDist * this.moveNextDist && this.wp != this.vectorPath.Count - 1)
			{
				this.wp++;
				vector2 = this.vectorPath[this.wp];
				vector2.y = position.y;
			}
			vector = vector2 - position;
			float magnitude = vector.magnitude;
			if (magnitude > 0f)
			{
				float num = Mathf.Min(magnitude, this.controller.maxSpeed);
				vector *= num / magnitude;
			}
		}
		this.controller.Move(vector);
	}

	public float repathRate = 1f;

	private float nextRepath;

	private Vector3 target;

	private bool canSearchAgain = true;

	private RVOController controller;

	private Path path;

	private List<Vector3> vectorPath;

	private int wp;

	public float moveNextDist = 1f;

	private Seeker seeker;

	private MeshRenderer[] rends;
}

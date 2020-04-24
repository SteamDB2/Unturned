using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[AddComponentMenu("Pathfinding/Seeker")]
public class Seeker : MonoBehaviour
{
	public Path GetCurrentPath()
	{
		return this.path;
	}

	public void Awake()
	{
		this.onPathDelegate = new OnPathDelegate(this.OnPathComplete);
		this.onPartialPathDelegate = new OnPathDelegate(this.OnPartialPathComplete);
		this.startEndModifier.Awake(this);
	}

	public void OnDestroy()
	{
		this.ReleaseClaimedPath();
		this.startEndModifier.OnDestroy(this);
	}

	public void ReleaseClaimedPath()
	{
		if (this.prevPath != null)
		{
			this.prevPath.ReleaseSilent(this);
			this.prevPath = null;
		}
	}

	public void RegisterModifier(IPathModifier mod)
	{
		if (this.modifiers == null)
		{
			this.modifiers = new List<IPathModifier>(1);
		}
		this.modifiers.Add(mod);
	}

	public void DeregisterModifier(IPathModifier mod)
	{
		if (this.modifiers == null)
		{
			return;
		}
		this.modifiers.Remove(mod);
	}

	public void PostProcess(Path p)
	{
		this.RunModifiers(Seeker.ModifierPass.PostProcess, p);
	}

	public void RunModifiers(Seeker.ModifierPass pass, Path p)
	{
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = 0; i < this.modifiers.Count - 1; i++)
			{
				if (this.modifiers[i].Priority < this.modifiers[i + 1].Priority)
				{
					IPathModifier value = this.modifiers[i];
					this.modifiers[i] = this.modifiers[i + 1];
					this.modifiers[i + 1] = value;
					flag = true;
				}
			}
		}
		if (pass != Seeker.ModifierPass.PreProcess)
		{
			if (pass != Seeker.ModifierPass.PostProcessOriginal)
			{
				if (pass == Seeker.ModifierPass.PostProcess)
				{
					if (this.postProcessPath != null)
					{
						this.postProcessPath(p);
					}
				}
			}
			else if (this.postProcessOriginalPath != null)
			{
				this.postProcessOriginalPath(p);
			}
		}
		else if (this.preProcessPath != null)
		{
			this.preProcessPath(p);
		}
		if (this.modifiers.Count == 0)
		{
			return;
		}
		ModifierData modifierData = ModifierData.All;
		IPathModifier pathModifier = this.modifiers[0];
		for (int j = 0; j < this.modifiers.Count; j++)
		{
			MonoModifier monoModifier = this.modifiers[j] as MonoModifier;
			if (!(monoModifier != null) || monoModifier.enabled)
			{
				if (pass != Seeker.ModifierPass.PreProcess)
				{
					if (pass != Seeker.ModifierPass.PostProcessOriginal)
					{
						if (pass == Seeker.ModifierPass.PostProcess)
						{
							ModifierData modifierData2 = ModifierConverter.Convert(p, modifierData, this.modifiers[j].input);
							if (modifierData2 != ModifierData.None)
							{
								this.modifiers[j].Apply(p, modifierData2);
								modifierData = this.modifiers[j].output;
							}
							else
							{
								Debug.Log(string.Concat(new string[]
								{
									"Error converting ",
									(j <= 0) ? "original" : pathModifier.GetType().Name,
									"'s output to ",
									this.modifiers[j].GetType().Name,
									"'s input.\nTry rearranging the modifier priorities on the Seeker."
								}));
								modifierData = ModifierData.None;
							}
							pathModifier = this.modifiers[j];
						}
					}
					else
					{
						this.modifiers[j].ApplyOriginal(p);
					}
				}
				else
				{
					this.modifiers[j].PreProcess(p);
				}
				if (modifierData == ModifierData.None)
				{
					break;
				}
			}
		}
	}

	public bool IsDone()
	{
		return this.path == null || this.path.GetState() >= PathState.Returned;
	}

	public void OnPathComplete(Path p)
	{
		this.OnPathComplete(p, true, true);
	}

	public void OnPathComplete(Path p, bool runModifiers, bool sendCallbacks)
	{
		if (p != null && p != this.path && sendCallbacks)
		{
			return;
		}
		if (this == null || p == null || p != this.path)
		{
			return;
		}
		if (!this.path.error && runModifiers)
		{
			this.RunModifiers(Seeker.ModifierPass.PostProcessOriginal, this.path);
			this.RunModifiers(Seeker.ModifierPass.PostProcess, this.path);
		}
		if (sendCallbacks)
		{
			p.Claim(this);
			this.lastCompletedNodePath = p.path;
			this.lastCompletedVectorPath = p.vectorPath;
			if (this.tmpPathCallback != null)
			{
				this.tmpPathCallback(p);
			}
			if (this.pathCallback != null)
			{
				this.pathCallback(p);
			}
			if (this.prevPath != null)
			{
				this.prevPath.ReleaseSilent(this);
			}
			this.prevPath = p;
			if (!this.drawGizmos)
			{
				this.ReleaseClaimedPath();
			}
		}
	}

	public void OnPartialPathComplete(Path p)
	{
		this.OnPathComplete(p, true, false);
	}

	public void OnMultiPathComplete(Path p)
	{
		this.OnPathComplete(p, false, true);
	}

	public ABPath GetNewPath(Vector3 start, Vector3 end)
	{
		return ABPath.Construct(start, end, null);
	}

	public Path StartPath(Vector3 start, Vector3 end)
	{
		return this.StartPath(start, end, null, -1);
	}

	public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback)
	{
		return this.StartPath(start, end, callback, -1);
	}

	public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback, int graphMask)
	{
		Path newPath = this.GetNewPath(start, end);
		return this.StartPath(newPath, callback, graphMask);
	}

	public Path StartPath(Path p, OnPathDelegate callback = null, int graphMask = -1)
	{
		p.enabledTags = this.traversableTags.tagsChange;
		p.tagPenalties = this.tagPenalties;
		if (this.path != null && this.path.GetState() <= PathState.Processing && this.lastPathID == (uint)this.path.pathID)
		{
			this.path.Error();
			this.path.LogError("Canceled path because a new one was requested.\nThis happens when a new path is requested from the seeker when one was already being calculated.\nFor example if a unit got a new order, you might request a new path directly instead of waiting for the now invalid path to be calculated. Which is probably what you want.\nIf you are getting this a lot, you might want to consider how you are scheduling path requests.");
		}
		this.path = p;
		Path path = this.path;
		path.callback = (OnPathDelegate)Delegate.Combine(path.callback, this.onPathDelegate);
		this.path.nnConstraint.graphMask = graphMask;
		this.tmpPathCallback = callback;
		this.lastPathID = (uint)this.path.pathID;
		this.RunModifiers(Seeker.ModifierPass.PreProcess, this.path);
		AstarPath.StartPath(this.path, false);
		return this.path;
	}

	public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, OnPathDelegate callback = null, int graphMask = -1)
	{
		MultiTargetPath multiTargetPath = MultiTargetPath.Construct(start, endPoints, null, null);
		multiTargetPath.pathsForAll = pathsForAll;
		return this.StartMultiTargetPath(multiTargetPath, callback, graphMask);
	}

	public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, OnPathDelegate callback = null, int graphMask = -1)
	{
		MultiTargetPath multiTargetPath = MultiTargetPath.Construct(startPoints, end, null, null);
		multiTargetPath.pathsForAll = pathsForAll;
		return this.StartMultiTargetPath(multiTargetPath, callback, graphMask);
	}

	public MultiTargetPath StartMultiTargetPath(MultiTargetPath p, OnPathDelegate callback = null, int graphMask = -1)
	{
		if (this.path != null && this.path.GetState() <= PathState.Processing && this.lastPathID == (uint)this.path.pathID)
		{
			this.path.ForceLogError("Canceled path because a new one was requested");
		}
		OnPathDelegate[] array = new OnPathDelegate[p.targetPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.onPartialPathDelegate;
		}
		p.callbacks = array;
		p.callback = (OnPathDelegate)Delegate.Combine(p.callback, new OnPathDelegate(this.OnMultiPathComplete));
		p.nnConstraint.graphMask = graphMask;
		this.path = p;
		this.tmpPathCallback = callback;
		this.lastPathID = (uint)this.path.pathID;
		this.RunModifiers(Seeker.ModifierPass.PreProcess, this.path);
		AstarPath.StartPath(this.path, false);
		return p;
	}

	public IEnumerator DelayPathStart(Path p)
	{
		yield return null;
		this.RunModifiers(Seeker.ModifierPass.PreProcess, p);
		AstarPath.StartPath(p, false);
		yield break;
	}

	public void OnDrawGizmos()
	{
		if (this.lastCompletedNodePath == null || !this.drawGizmos)
		{
			return;
		}
		if (this.detailedGizmos)
		{
			Gizmos.color = new Color(0.7f, 0.5f, 0.1f, 0.5f);
			if (this.lastCompletedNodePath != null)
			{
				for (int i = 0; i < this.lastCompletedNodePath.Count - 1; i++)
				{
					Gizmos.DrawLine((Vector3)this.lastCompletedNodePath[i].position, (Vector3)this.lastCompletedNodePath[i + 1].position);
				}
			}
		}
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		if (this.lastCompletedVectorPath != null)
		{
			for (int j = 0; j < this.lastCompletedVectorPath.Count - 1; j++)
			{
				Gizmos.DrawLine(this.lastCompletedVectorPath[j], this.lastCompletedVectorPath[j + 1]);
			}
		}
	}

	public bool drawGizmos = true;

	public bool detailedGizmos;

	[HideInInspector]
	public bool saveGetNearestHints = true;

	public StartEndModifier startEndModifier = new StartEndModifier();

	[HideInInspector]
	public TagMask traversableTags = new TagMask(-1, -1);

	[HideInInspector]
	public int[] tagPenalties = new int[32];

	public OnPathDelegate pathCallback;

	public OnPathDelegate preProcessPath;

	public OnPathDelegate postProcessOriginalPath;

	public OnPathDelegate postProcessPath;

	[NonSerialized]
	public List<Vector3> lastCompletedVectorPath;

	[NonSerialized]
	public List<GraphNode> lastCompletedNodePath;

	[NonSerialized]
	protected Path path;

	private Path prevPath;

	private GraphNode startHint;

	private GraphNode endHint;

	private OnPathDelegate onPathDelegate;

	private OnPathDelegate onPartialPathDelegate;

	private OnPathDelegate tmpPathCallback;

	protected uint lastPathID;

	private List<IPathModifier> modifiers = new List<IPathModifier>();

	public enum ModifierPass
	{
		PreProcess,
		PostProcessOriginal,
		PostProcess
	}
}

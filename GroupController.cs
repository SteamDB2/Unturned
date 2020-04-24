using System;
using System.Collections.Generic;
using Pathfinding.RVO;
using Pathfinding.RVO.Sampled;
using UnityEngine;

public class GroupController : MonoBehaviour
{
	public void Start()
	{
		this.cam = Camera.main;
		RVOSimulator rvosimulator = Object.FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator;
		if (rvosimulator == null)
		{
			base.enabled = false;
			throw new Exception("No RVOSimulator in the scene. Please add one");
		}
		this.sim = rvosimulator.GetSimulator();
	}

	public void Update()
	{
		if (Screen.fullScreen && Screen.width != Screen.resolutions[Screen.resolutions.Length - 1].width)
		{
			Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, true);
		}
		if (this.adjustCamera)
		{
			List<Agent> agents = this.sim.GetAgents();
			float num = 0f;
			for (int i = 0; i < agents.Count; i++)
			{
				float num2 = Mathf.Max(Mathf.Abs(agents[i].InterpolatedPosition.x), Mathf.Abs(agents[i].InterpolatedPosition.z));
				if (num2 > num)
				{
					num = num2;
				}
			}
			float num3 = num / Mathf.Tan(this.cam.fieldOfView * 0.0174532924f / 2f);
			float num4 = num / Mathf.Tan(Mathf.Atan(Mathf.Tan(this.cam.fieldOfView * 0.0174532924f / 2f) * this.cam.aspect));
			this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, new Vector3(0f, Mathf.Max(num3, num4) * 1.1f, 0f), Time.smoothDeltaTime * 2f);
		}
		if (Input.GetKey(97) && Input.GetKeyDown(323))
		{
			this.Order();
		}
	}

	private void OnGUI()
	{
		if (Event.current.type == 1 && Event.current.button == 0 && !Input.GetKey(97))
		{
			this.Select(this.start, this.end);
			this.wasDown = false;
		}
		if (Event.current.type == 3 && Event.current.button == 0)
		{
			this.end = Event.current.mousePosition;
			if (!this.wasDown)
			{
				this.start = this.end;
				this.wasDown = true;
			}
		}
		if (Input.GetKey(97))
		{
			this.wasDown = false;
		}
		if (this.wasDown)
		{
			Rect rect = Rect.MinMaxRect(Mathf.Min(this.start.x, this.end.x), Mathf.Min(this.start.y, this.end.y), Mathf.Max(this.start.x, this.end.x), Mathf.Max(this.start.y, this.end.y));
			if (rect.width > 4f && rect.height > 4f)
			{
				GUI.Box(rect, string.Empty, this.selectionBox);
			}
		}
	}

	public void Order()
	{
		Ray ray = this.cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, ref raycastHit))
		{
			float num = 0f;
			for (int i = 0; i < this.selection.Count; i++)
			{
				num += this.selection[i].GetComponent<RVOController>().radius;
			}
			float num2 = num / 3.14159274f;
			num2 *= 2f;
			for (int j = 0; j < this.selection.Count; j++)
			{
				float num3 = 6.28318548f * (float)j / (float)this.selection.Count;
				Vector3 target = raycastHit.point + new Vector3(Mathf.Cos(num3), 0f, Mathf.Sin(num3)) * num2;
				this.selection[j].SetTarget(target);
				this.selection[j].SetColor(this.GetColor(num3));
				this.selection[j].RecalculatePath();
			}
		}
	}

	public void Select(Vector2 _start, Vector2 _end)
	{
		_start.y = (float)Screen.height - _start.y;
		_end.y = (float)Screen.height - _end.y;
		Vector2 vector = Vector2.Min(_start, _end);
		Vector2 vector2 = Vector2.Max(_start, _end);
		if ((vector2 - vector).sqrMagnitude < 16f)
		{
			return;
		}
		this.selection.Clear();
		RVOExampleAgent[] array = Object.FindObjectsOfType(typeof(RVOExampleAgent)) as RVOExampleAgent[];
		for (int i = 0; i < array.Length; i++)
		{
			Vector2 vector3 = this.cam.WorldToScreenPoint(array[i].transform.position);
			if (vector3.x > vector.x && vector3.y > vector.y && vector3.x < vector2.x && vector3.y < vector2.y)
			{
				this.selection.Add(array[i]);
			}
		}
	}

	public Color GetColor(float angle)
	{
		return GroupController.HSVToRGB(angle * 57.2957764f, 0.8f, 0.6f);
	}

	private static Color HSVToRGB(float h, float s, float v)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = s * v;
		float num5 = h / 60f;
		float num6 = num4 * (1f - Math.Abs(num5 % 2f - 1f));
		if (num5 < 1f)
		{
			num = num4;
			num2 = num6;
		}
		else if (num5 < 2f)
		{
			num = num6;
			num2 = num4;
		}
		else if (num5 < 3f)
		{
			num2 = num4;
			num3 = num6;
		}
		else if (num5 < 4f)
		{
			num2 = num6;
			num3 = num4;
		}
		else if (num5 < 5f)
		{
			num = num6;
			num3 = num4;
		}
		else if (num5 < 6f)
		{
			num = num4;
			num3 = num6;
		}
		float num7 = v - num4;
		num += num7;
		num2 += num7;
		num3 += num7;
		return new Color(num, num2, num3);
	}

	public GUIStyle selectionBox;

	public bool adjustCamera = true;

	private Vector2 start;

	private Vector2 end;

	private bool wasDown;

	private List<RVOExampleAgent> selection = new List<RVOExampleAgent>();

	private Simulator sim;

	private Camera cam;

	private const float rad2Deg = 57.2957764f;
}

using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class RoadPath
	{
		public RoadPath(Transform vertex)
		{
			this.vertex = vertex;
			this.meshRenderer = vertex.GetComponent<MeshRenderer>();
			this.tangents = new Transform[2];
			this.tangents[0] = vertex.FindChild("Tangent_0");
			this.tangents[1] = vertex.FindChild("Tangent_1");
			this.meshRenderers = new MeshRenderer[2];
			this.meshRenderers[0] = this.tangents[0].GetComponent<MeshRenderer>();
			this.meshRenderers[1] = this.tangents[1].GetComponent<MeshRenderer>();
			this.lineRenderers = new LineRenderer[2];
			this.lineRenderers[0] = this.tangents[0].GetComponent<LineRenderer>();
			this.lineRenderers[1] = this.tangents[1].GetComponent<LineRenderer>();
			this.unhighlightVertex();
			this.unhighlightTangent(0);
			this.unhighlightTangent(1);
		}

		public void highlightVertex()
		{
			this.meshRenderer.material.color = Color.red;
		}

		public void unhighlightVertex()
		{
			this.meshRenderer.material.color = Color.white;
		}

		public void highlightTangent(int index)
		{
			this.meshRenderers[index].material.color = Color.red;
			this.lineRenderers[index].material.color = Color.red;
		}

		public void unhighlightTangent(int index)
		{
			Color color;
			if (index == 0)
			{
				color = Color.yellow;
			}
			else
			{
				color = Color.blue;
			}
			this.meshRenderers[index].material.color = color;
			this.lineRenderers[index].material.color = color;
		}

		public void setTangent(int index, Vector3 tangent)
		{
			this.tangents[index].localPosition = tangent;
			this.lineRenderers[index].SetPosition(1, -tangent);
		}

		public void remove()
		{
			Object.Destroy(this.vertex.gameObject);
		}

		public Transform vertex;

		private MeshRenderer meshRenderer;

		public Transform[] tangents;

		private MeshRenderer[] meshRenderers;

		private LineRenderer[] lineRenderers;
	}
}

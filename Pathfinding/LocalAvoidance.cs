using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[RequireComponent(typeof(CharacterController))]
	public class LocalAvoidance : MonoBehaviour
	{
		private void Start()
		{
			this.controller = base.GetComponent<CharacterController>();
			this.agents = (Object.FindObjectsOfType(typeof(LocalAvoidance)) as LocalAvoidance[]);
		}

		public void Update()
		{
			this.SimpleMove(base.transform.forward * this.speed);
		}

		public Vector3 GetVelocity()
		{
			return this.preVelocity;
		}

		public void LateUpdate()
		{
			this.preVelocity = this.velocity;
		}

		public void SimpleMove(Vector3 desiredMovement)
		{
			Vector3 vector = Random.insideUnitSphere * 0.1f;
			vector.y = 0f;
			Vector3 vector2 = this.ClampMovement(desiredMovement + vector);
			if (vector2 != Vector3.zero)
			{
				vector2 /= this.delta;
			}
			if (this.drawGizmos)
			{
				Debug.DrawRay(base.transform.position, desiredMovement, Color.magenta);
				Debug.DrawRay(base.transform.position, vector2, Color.yellow);
				Debug.DrawRay(base.transform.position + vector2, Vector3.up, Color.yellow);
			}
			this.controller.SimpleMove(vector2);
			this.velocity = this.controller.velocity;
			Debug.DrawRay(base.transform.position, this.velocity, Color.blue);
		}

		public Vector3 ClampMovement(Vector3 direction)
		{
			Vector3 vector = direction * this.delta;
			Vector3 vector2 = base.transform.position + direction;
			Vector3 vector3 = vector2;
			float num = 0f;
			int num2 = 0;
			this.vos.Clear();
			float magnitude = this.velocity.magnitude;
			foreach (LocalAvoidance localAvoidance in this.agents)
			{
				if (!(localAvoidance == this) && !(localAvoidance == null))
				{
					Vector3 vector4 = localAvoidance.transform.position - base.transform.position;
					float magnitude2 = vector4.magnitude;
					float num3 = this.radius + localAvoidance.radius;
					if (magnitude2 <= vector.magnitude * this.delta + num3 + magnitude + localAvoidance.GetVelocity().magnitude)
					{
						if (num2 <= 50)
						{
							num2++;
							LocalAvoidance.VO vo = new LocalAvoidance.VO();
							vo.origin = base.transform.position + Vector3.Lerp(this.velocity * this.delta, localAvoidance.GetVelocity() * this.delta, this.responability);
							vo.direction = vector4.normalized;
							if (num3 > vector4.magnitude)
							{
								vo.angle = 1.57079637f;
							}
							else
							{
								vo.angle = (float)Math.Asin((double)(num3 / magnitude2));
							}
							vo.limit = magnitude2 - num3;
							if (vo.limit < 0f)
							{
								vo.origin += vo.direction * vo.limit;
								vo.limit = 0f;
							}
							float num4 = Mathf.Atan2(vo.direction.z, vo.direction.x);
							vo.pRight = new Vector3(Mathf.Cos(num4 + vo.angle), 0f, Mathf.Sin(num4 + vo.angle));
							vo.pLeft = new Vector3(Mathf.Cos(num4 - vo.angle), 0f, Mathf.Sin(num4 - vo.angle));
							vo.nLeft = new Vector3(Mathf.Cos(num4 + vo.angle - 1.57079637f), 0f, Mathf.Sin(num4 + vo.angle - 1.57079637f));
							vo.nRight = new Vector3(Mathf.Cos(num4 - vo.angle + 1.57079637f), 0f, Mathf.Sin(num4 - vo.angle + 1.57079637f));
							this.vos.Add(vo);
						}
					}
				}
			}
			if (this.resType == LocalAvoidance.ResolutionType.Geometric)
			{
				for (int j = 0; j < this.vos.Count; j++)
				{
					if (this.vos[j].Contains(vector3))
					{
						num = float.PositiveInfinity;
						if (this.drawGizmos)
						{
							Debug.DrawRay(vector3, Vector3.down, Color.red);
						}
						vector3 = base.transform.position;
						break;
					}
				}
				if (this.drawGizmos)
				{
					for (int k = 0; k < this.vos.Count; k++)
					{
						this.vos[k].Draw(Color.black);
					}
				}
				if (num == 0f)
				{
					return vector;
				}
				List<LocalAvoidance.VOLine> list = new List<LocalAvoidance.VOLine>();
				for (int l = 0; l < this.vos.Count; l++)
				{
					LocalAvoidance.VO vo2 = this.vos[l];
					float num5 = (float)Math.Atan2((double)vo2.direction.z, (double)vo2.direction.x);
					Vector3 vector5 = vo2.origin + new Vector3((float)Math.Cos((double)(num5 + vo2.angle)), 0f, (float)Math.Sin((double)(num5 + vo2.angle))) * vo2.limit;
					Vector3 vector6 = vo2.origin + new Vector3((float)Math.Cos((double)(num5 - vo2.angle)), 0f, (float)Math.Sin((double)(num5 - vo2.angle))) * vo2.limit;
					Vector3 end = vector5 + new Vector3((float)Math.Cos((double)(num5 + vo2.angle)), 0f, (float)Math.Sin((double)(num5 + vo2.angle))) * 100f;
					Vector3 end2 = vector6 + new Vector3((float)Math.Cos((double)(num5 - vo2.angle)), 0f, (float)Math.Sin((double)(num5 - vo2.angle))) * 100f;
					int num6 = (!Polygon.Left(vo2.origin, vo2.origin + vo2.direction, base.transform.position + this.velocity)) ? 2 : 1;
					list.Add(new LocalAvoidance.VOLine(vo2, vector5, end, true, 1, num6 == 1));
					list.Add(new LocalAvoidance.VOLine(vo2, vector6, end2, true, 2, num6 == 2));
					list.Add(new LocalAvoidance.VOLine(vo2, vector5, vector6, false, 3, false));
					bool flag = false;
					bool flag2 = false;
					if (!flag)
					{
						for (int m = 0; m < this.vos.Count; m++)
						{
							if (m != l && this.vos[m].Contains(vector5))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						for (int n = 0; n < this.vos.Count; n++)
						{
							if (n != l && this.vos[n].Contains(vector6))
							{
								flag2 = true;
								break;
							}
						}
					}
					vo2.AddInt(0f, flag, 1);
					vo2.AddInt(0f, flag2, 2);
					vo2.AddInt(0f, flag, 3);
					vo2.AddInt(1f, flag2, 3);
				}
				for (int num7 = 0; num7 < list.Count; num7++)
				{
					for (int num8 = num7 + 1; num8 < list.Count; num8++)
					{
						LocalAvoidance.VOLine voline = list[num7];
						LocalAvoidance.VOLine voline2 = list[num8];
						if (voline.vo != voline2.vo)
						{
							float num9;
							float num10;
							if (Polygon.IntersectionFactor(voline.start, voline.end, voline2.start, voline2.end, out num9, out num10))
							{
								if (num9 >= 0f && num10 >= 0f && (voline.inf || num9 <= 1f) && (voline2.inf || num10 <= 1f))
								{
									Vector3 p = voline.start + (voline.end - voline.start) * num9;
									bool flag3 = voline.wrongSide || voline2.wrongSide;
									if (!flag3)
									{
										for (int num11 = 0; num11 < this.vos.Count; num11++)
										{
											if (this.vos[num11] != voline.vo && this.vos[num11] != voline2.vo && this.vos[num11].Contains(p))
											{
												flag3 = true;
												break;
											}
										}
									}
									voline.vo.AddInt(num9, flag3, voline.id);
									voline2.vo.AddInt(num10, flag3, voline2.id);
									if (this.drawGizmos)
									{
										Debug.DrawRay(voline.start + (voline.end - voline.start) * num9, Vector3.up, (!flag3) ? Color.green : Color.magenta);
									}
								}
							}
						}
					}
				}
				for (int num12 = 0; num12 < this.vos.Count; num12++)
				{
					Vector3 vector7;
					if (this.vos[num12].FinalInts(vector2, base.transform.position + this.velocity, this.drawGizmos, out vector7))
					{
						float sqrMagnitude = (vector7 - vector2).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							vector3 = vector7;
							num = sqrMagnitude;
							if (this.drawGizmos)
							{
								Debug.DrawLine(vector2 + Vector3.up, vector3 + Vector3.up, Color.red);
							}
						}
					}
				}
				if (this.drawGizmos)
				{
					Debug.DrawLine(vector2 + Vector3.up, vector3 + Vector3.up, Color.red);
				}
				return Vector3.ClampMagnitude(vector3 - base.transform.position, vector.magnitude * this.maxSpeedScale);
			}
			else
			{
				if (this.resType == LocalAvoidance.ResolutionType.Sampled)
				{
					Vector3 vector8 = vector;
					Vector3 normalized = vector8.normalized;
					Vector3 vector9 = Vector3.Cross(normalized, Vector3.up);
					int num13 = 10;
					int num14 = 0;
					while (num14 < 10)
					{
						float num15 = (float)(3.1415926535897931 * (double)this.circlePoint / (double)num13);
						float num16 = (float)(3.1415926535897931 - (double)this.circlePoint * 3.1415926535897931) * 0.5f;
						for (int num17 = 0; num17 < num13; num17++)
						{
							float num18 = num15 * (float)num17;
							Vector3 vector10 = base.transform.position + vector - (vector8 * (float)Math.Sin((double)(num18 + num16)) * (float)num14 * this.circleScale + vector9 * (float)Math.Cos((double)(num18 + num16)) * (float)num14 * this.circleScale);
							if (this.CheckSample(vector10, this.vos))
							{
								return vector10 - base.transform.position;
							}
						}
						num14++;
						num13 += 2;
					}
					for (int num19 = 0; num19 < this.samples.Length; num19++)
					{
						Vector3 vector11 = base.transform.position + this.samples[num19].x * vector9 + this.samples[num19].z * normalized + this.samples[num19].y * vector8;
						if (this.CheckSample(vector11, this.vos))
						{
							return vector11 - base.transform.position;
						}
					}
					return Vector3.zero;
				}
				return Vector3.zero;
			}
		}

		public bool CheckSample(Vector3 sample, List<LocalAvoidance.VO> vos)
		{
			bool flag = false;
			for (int i = 0; i < vos.Count; i++)
			{
				if (vos[i].Contains(sample))
				{
					if (this.drawGizmos)
					{
						Debug.DrawRay(sample, Vector3.up, Color.red);
					}
					flag = true;
					break;
				}
			}
			if (this.drawGizmos && !flag)
			{
				Debug.DrawRay(sample, Vector3.up, Color.yellow);
			}
			return !flag;
		}

		public float speed = 2f;

		public float delta = 1f;

		public float responability = 0.5f;

		public LocalAvoidance.ResolutionType resType = LocalAvoidance.ResolutionType.Geometric;

		private Vector3 velocity;

		public float radius = 0.5f;

		public float maxSpeedScale = 1.5f;

		public Vector3[] samples;

		public float sampleScale = 1f;

		public float circleScale = 0.5f;

		public float circlePoint = 0.5f;

		public bool drawGizmos;

		protected CharacterController controller;

		protected LocalAvoidance[] agents;

		private Vector3 preVelocity;

		public const float Rad2Deg = 57.29578f;

		private const int maxVOCounter = 50;

		private List<LocalAvoidance.VO> vos = new List<LocalAvoidance.VO>();

		public enum ResolutionType
		{
			Sampled,
			Geometric
		}

		public struct VOLine
		{
			public VOLine(LocalAvoidance.VO vo, Vector3 start, Vector3 end, bool inf, int id, bool wrongSide)
			{
				this.vo = vo;
				this.start = start;
				this.end = end;
				this.inf = inf;
				this.id = id;
				this.wrongSide = wrongSide;
			}

			public LocalAvoidance.VO vo;

			public Vector3 start;

			public Vector3 end;

			public bool inf;

			public int id;

			public bool wrongSide;
		}

		public struct VOIntersection
		{
			public VOIntersection(LocalAvoidance.VO vo1, LocalAvoidance.VO vo2, float factor1, float factor2, bool inside = false)
			{
				this.vo1 = vo1;
				this.vo2 = vo2;
				this.factor1 = factor1;
				this.factor2 = factor2;
				this.inside = inside;
			}

			public LocalAvoidance.VO vo1;

			public LocalAvoidance.VO vo2;

			public float factor1;

			public float factor2;

			public bool inside;
		}

		public class HalfPlane
		{
			public bool Contains(Vector3 p)
			{
				p -= this.point;
				return Vector3.Dot(this.normal, p) >= 0f;
			}

			public Vector3 ClosestPoint(Vector3 p)
			{
				p -= this.point;
				Vector3 vector = Vector3.Cross(this.normal, Vector3.up);
				float num = Vector3.Dot(vector, p);
				return this.point + vector * num;
			}

			public Vector3 ClosestPoint(Vector3 p, float minX, float maxX)
			{
				p -= this.point;
				Vector3 vector = Vector3.Cross(this.normal, Vector3.up);
				if (vector.x < 0f)
				{
					vector = -vector;
				}
				float num = Vector3.Dot(vector, p);
				float num2 = (minX - this.point.x) / vector.x;
				float num3 = (maxX - this.point.x) / vector.x;
				num = Mathf.Clamp(num, num2, num3);
				return this.point + vector * num;
			}

			public Vector3 Intersection(LocalAvoidance.HalfPlane hp)
			{
				Vector3 dir = Vector3.Cross(this.normal, Vector3.up);
				Vector3 dir2 = Vector3.Cross(hp.normal, Vector3.up);
				return Polygon.IntersectionPointOptimized(this.point, dir, hp.point, dir2);
			}

			public void DrawBounds(float left, float right)
			{
				Vector3 vector = Vector3.Cross(this.normal, Vector3.up);
				if (vector.x < 0f)
				{
					vector = -vector;
				}
				float num = (left - this.point.x) / vector.x;
				float num2 = (right - this.point.x) / vector.x;
				Debug.DrawLine(this.point + vector * num + Vector3.up * 0.1f, this.point + vector * num2 + Vector3.up * 0.1f, Color.yellow);
			}

			public void Draw()
			{
				Vector3 vector = Vector3.Cross(this.normal, Vector3.up);
				Debug.DrawLine(this.point - vector * 10f, this.point + vector * 10f, Color.blue);
				Debug.DrawRay(this.point, this.normal, new Color(0.8f, 0.1f, 0.2f));
			}

			public Vector3 point;

			public Vector3 normal;
		}

		public enum IntersectionState
		{
			Inside,
			Outside,
			Enter,
			Exit
		}

		public struct IntersectionPair : IComparable<LocalAvoidance.IntersectionPair>
		{
			public IntersectionPair(float factor, bool inside)
			{
				this.factor = factor;
				this.state = ((!inside) ? LocalAvoidance.IntersectionState.Outside : LocalAvoidance.IntersectionState.Inside);
			}

			public void SetState(LocalAvoidance.IntersectionState s)
			{
				this.state = s;
			}

			public int CompareTo(LocalAvoidance.IntersectionPair o)
			{
				if (o.factor < this.factor)
				{
					return 1;
				}
				if (o.factor > this.factor)
				{
					return -1;
				}
				return 0;
			}

			public float factor;

			public LocalAvoidance.IntersectionState state;
		}

		public class VO
		{
			public void AddInt(float factor, bool inside, int id)
			{
				if (id != 1)
				{
					if (id != 2)
					{
						if (id == 3)
						{
							this.ints3.Add(new LocalAvoidance.IntersectionPair(factor, inside));
						}
					}
					else
					{
						this.ints2.Add(new LocalAvoidance.IntersectionPair(factor, inside));
					}
				}
				else
				{
					this.ints1.Add(new LocalAvoidance.IntersectionPair(factor, inside));
				}
			}

			public bool FinalInts(Vector3 target, Vector3 closeEdgeConstraint, bool drawGizmos, out Vector3 closest)
			{
				this.ints1.Sort();
				this.ints2.Sort();
				this.ints3.Sort();
				float num = (float)Math.Atan2((double)this.direction.z, (double)this.direction.x);
				Vector3 vector = Vector3.Cross(this.direction, Vector3.up);
				Vector3 vector2 = vector * (float)Math.Tan((double)this.angle) * this.limit;
				Vector3 vector3 = this.origin + this.direction * this.limit + vector2;
				Vector3 vector4 = this.origin + this.direction * this.limit - vector2;
				Vector3 vector5 = vector3 + new Vector3((float)Math.Cos((double)(num + this.angle)), 0f, (float)Math.Sin((double)(num + this.angle))) * 100f;
				Vector3 vector6 = vector4 + new Vector3((float)Math.Cos((double)(num - this.angle)), 0f, (float)Math.Sin((double)(num - this.angle))) * 100f;
				bool flag = false;
				closest = Vector3.zero;
				int num2 = (Vector3.Dot(closeEdgeConstraint - this.origin, vector) <= 0f) ? 1 : 2;
				for (int i = 1; i <= 3; i++)
				{
					if (i != num2)
					{
						List<LocalAvoidance.IntersectionPair> list = (i != 1) ? ((i != 2) ? this.ints3 : this.ints2) : this.ints1;
						Vector3 vector7 = (i != 1 && i != 3) ? vector4 : vector3;
						Vector3 vector8 = (i != 1) ? ((i != 2) ? vector4 : vector6) : vector5;
						float num3 = AstarMath.NearestPointFactor(vector7, vector8, target);
						float num4 = float.PositiveInfinity;
						float num5 = float.NegativeInfinity;
						bool flag2 = false;
						for (int j = 0; j < list.Count - ((i != 3) ? 0 : 1); j++)
						{
							if (drawGizmos)
							{
								Debug.DrawRay(vector7 + (vector8 - vector7) * list[j].factor, Vector3.down, (list[j].state != LocalAvoidance.IntersectionState.Outside) ? Color.red : Color.green);
							}
							if (list[j].state == LocalAvoidance.IntersectionState.Outside && ((j == list.Count - 1 && (j == 0 || list[j - 1].state != LocalAvoidance.IntersectionState.Outside)) || (j < list.Count - 1 && list[j + 1].state == LocalAvoidance.IntersectionState.Outside)))
							{
								flag2 = true;
								float factor = list[j].factor;
								float num6 = (j != list.Count - 1) ? list[j + 1].factor : ((i != 3) ? float.PositiveInfinity : 1f);
								if (drawGizmos)
								{
									Debug.DrawLine(vector7 + (vector8 - vector7) * factor + Vector3.up, vector7 + (vector8 - vector7) * Mathf.Clamp01(num6) + Vector3.up, Color.green);
								}
								if (factor <= num3 && num6 >= num3)
								{
									num4 = num3;
									num5 = num3;
									break;
								}
								if (num6 < num3 && num6 > num5)
								{
									num5 = num6;
								}
								else if (factor > num3 && factor < num4)
								{
									num4 = factor;
								}
							}
						}
						if (flag2)
						{
							float num7 = (num4 != float.NegativeInfinity) ? ((num5 != float.PositiveInfinity) ? ((Mathf.Abs(num3 - num4) >= Mathf.Abs(num3 - num5)) ? num5 : num4) : num4) : num5;
							Vector3 vector9 = vector7 + (vector8 - vector7) * num7;
							if (!flag || (vector9 - target).sqrMagnitude < (closest - target).sqrMagnitude)
							{
								closest = vector9;
							}
							if (drawGizmos)
							{
								Debug.DrawLine(target, closest, Color.yellow);
							}
							flag = true;
						}
					}
				}
				return flag;
			}

			public bool Contains(Vector3 p)
			{
				return Vector3.Dot(this.nLeft, p - this.origin) > 0f && Vector3.Dot(this.nRight, p - this.origin) > 0f && Vector3.Dot(this.direction, p - this.origin) > this.limit;
			}

			public float ScoreContains(Vector3 p)
			{
				return 0f;
			}

			public void Draw(Color c)
			{
				float num = (float)Math.Atan2((double)this.direction.z, (double)this.direction.x);
				Vector3 vector = Vector3.Cross(this.direction, Vector3.up) * (float)Math.Tan((double)this.angle) * this.limit;
				Debug.DrawLine(this.origin + this.direction * this.limit + vector, this.origin + this.direction * this.limit - vector, c);
				Debug.DrawRay(this.origin + this.direction * this.limit + vector, new Vector3((float)Math.Cos((double)(num + this.angle)), 0f, (float)Math.Sin((double)(num + this.angle))) * 10f, c);
				Debug.DrawRay(this.origin + this.direction * this.limit - vector, new Vector3((float)Math.Cos((double)(num - this.angle)), 0f, (float)Math.Sin((double)(num - this.angle))) * 10f, c);
			}

			public static explicit operator LocalAvoidance.HalfPlane(LocalAvoidance.VO vo)
			{
				return new LocalAvoidance.HalfPlane
				{
					point = vo.origin + vo.direction * vo.limit,
					normal = -vo.direction
				};
			}

			public Vector3 origin;

			public Vector3 direction;

			public float angle;

			public float limit;

			public Vector3 pLeft;

			public Vector3 pRight;

			public Vector3 nLeft;

			public Vector3 nRight;

			public List<LocalAvoidance.IntersectionPair> ints1 = new List<LocalAvoidance.IntersectionPair>();

			public List<LocalAvoidance.IntersectionPair> ints2 = new List<LocalAvoidance.IntersectionPair>();

			public List<LocalAvoidance.IntersectionPair> ints3 = new List<LocalAvoidance.IntersectionPair>();
		}
	}
}

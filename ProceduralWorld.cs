using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class ProceduralWorld : MonoBehaviour
{
	private void Start()
	{
		this.Update();
		AstarPath.active.Scan();
	}

	private void Update()
	{
		Int2 @int = new Int2(Mathf.RoundToInt((this.target.position.x - this.tileSize * 0.5f) / this.tileSize), Mathf.RoundToInt((this.target.position.z - this.tileSize * 0.5f) / this.tileSize));
		this.range = ((this.range >= 1) ? this.range : 1);
		bool flag = true;
		while (flag)
		{
			flag = false;
			foreach (KeyValuePair<Int2, ProceduralWorld.ProceduralTile> keyValuePair in this.tiles)
			{
				if (Mathf.Abs(keyValuePair.Key.x - @int.x) > this.range || Mathf.Abs(keyValuePair.Key.y - @int.y) > this.range)
				{
					keyValuePair.Value.Destroy();
					this.tiles.Remove(keyValuePair.Key);
					flag = true;
					break;
				}
			}
		}
		for (int i = @int.x - this.range; i <= @int.x + this.range; i++)
		{
			for (int j = @int.y - this.range; j <= @int.y + this.range; j++)
			{
				if (!this.tiles.ContainsKey(new Int2(i, j)))
				{
					ProceduralWorld.ProceduralTile proceduralTile = new ProceduralWorld.ProceduralTile(this, i, j);
					base.StartCoroutine(proceduralTile.Generate());
					this.tiles.Add(new Int2(i, j), proceduralTile);
				}
			}
		}
		for (int k = @int.x - 1; k <= @int.x + 1; k++)
		{
			for (int l = @int.y - 1; l <= @int.y + 1; l++)
			{
				this.tiles[new Int2(k, l)].ForceFinish();
			}
		}
	}

	public Transform target;

	public ProceduralWorld.ProceduralPrefab[] prefabs;

	public int range;

	public float tileSize = 100f;

	public int subTiles = 20;

	private Dictionary<Int2, ProceduralWorld.ProceduralTile> tiles = new Dictionary<Int2, ProceduralWorld.ProceduralTile>();

	[Serializable]
	public class ProceduralPrefab
	{
		public GameObject prefab;

		public float density;

		public float perlin;

		public float perlinPower = 1f;

		public Vector2 perlinOffset = Vector2.zero;

		public float perlinScale = 1f;

		public float random = 1f;

		public bool singleFixed;
	}

	private class ProceduralTile
	{
		public ProceduralTile(ProceduralWorld world, int x, int z)
		{
			this.x = x;
			this.z = z;
			this.world = world;
			this.rnd = new Random(x * 10007 ^ z * 36007);
		}

		public IEnumerator Generate()
		{
			this.ie = this.InternalGenerate();
			GameObject rt = new GameObject(string.Concat(new object[]
			{
				"Tile ",
				this.x,
				" ",
				this.z
			}));
			this.root = rt.transform;
			while (this.ie != null && this.root != null && this.ie.MoveNext())
			{
				yield return this.ie.Current;
			}
			this.ie = null;
			yield break;
		}

		public void ForceFinish()
		{
			while (this.ie != null && this.root != null && this.ie.MoveNext())
			{
			}
			this.ie = null;
		}

		private Vector3 RandomInside()
		{
			Vector3 result = default(Vector3);
			result.x = ((float)this.x + (float)this.rnd.NextDouble()) * this.world.tileSize;
			result.z = ((float)this.z + (float)this.rnd.NextDouble()) * this.world.tileSize;
			return result;
		}

		private Vector3 RandomInside(float px, float pz)
		{
			Vector3 result = default(Vector3);
			result.x = (px + (float)this.rnd.NextDouble() / (float)this.world.subTiles) * this.world.tileSize;
			result.z = (pz + (float)this.rnd.NextDouble() / (float)this.world.subTiles) * this.world.tileSize;
			return result;
		}

		private Quaternion RandomYRot()
		{
			return Quaternion.Euler(360f * (float)this.rnd.NextDouble(), 0f, 360f * (float)this.rnd.NextDouble());
		}

		private IEnumerator InternalGenerate()
		{
			Debug.Log(string.Concat(new object[]
			{
				"Generating tile ",
				this.x,
				", ",
				this.z
			}));
			int counter = 0;
			float[,] ditherMap = new float[this.world.subTiles + 2, this.world.subTiles + 2];
			for (int i = 0; i < this.world.prefabs.Length; i++)
			{
				ProceduralWorld.ProceduralPrefab pref = this.world.prefabs[i];
				if (pref.singleFixed)
				{
					Vector3 vector;
					vector..ctor(((float)this.x + 0.5f) * this.world.tileSize, 0f, ((float)this.z + 0.5f) * this.world.tileSize);
					GameObject gameObject = Object.Instantiate<GameObject>(pref.prefab, vector, Quaternion.identity);
					gameObject.transform.parent = this.root;
				}
				else
				{
					float subSize = this.world.tileSize / (float)this.world.subTiles;
					for (int k = 0; k < this.world.subTiles; k++)
					{
						for (int l = 0; l < this.world.subTiles; l++)
						{
							ditherMap[k + 1, l + 1] = 0f;
						}
					}
					for (int sx = 0; sx < this.world.subTiles; sx++)
					{
						for (int sz = 0; sz < this.world.subTiles; sz++)
						{
							float px = (float)this.x + (float)sx / (float)this.world.subTiles;
							float pz = (float)this.z + (float)sz / (float)this.world.subTiles;
							float perl = Mathf.Pow(Mathf.PerlinNoise((px + pref.perlinOffset.x) * pref.perlinScale, (pz + pref.perlinOffset.y) * pref.perlinScale), pref.perlinPower);
							float density = pref.density * Mathf.Lerp(1f, perl, pref.perlin) * Mathf.Lerp(1f, (float)this.rnd.NextDouble(), pref.random);
							float fcount = subSize * subSize * density + ditherMap[sx + 1, sz + 1];
							int count = Mathf.RoundToInt(fcount);
							ditherMap[sx + 1 + 1, sz + 1] += 0.4375f * (fcount - (float)count);
							ditherMap[sx + 1 - 1, sz + 1 + 1] += 0.1875f * (fcount - (float)count);
							ditherMap[sx + 1, sz + 1 + 1] += 0.3125f * (fcount - (float)count);
							ditherMap[sx + 1 + 1, sz + 1 + 1] += 0.0625f * (fcount - (float)count);
							for (int j = 0; j < count; j++)
							{
								Vector3 p = this.RandomInside(px, pz);
								GameObject ob = Object.Instantiate<GameObject>(pref.prefab, p, this.RandomYRot());
								ob.transform.parent = this.root;
								counter++;
								if (counter % 2 == 0)
								{
									yield return null;
								}
							}
						}
					}
				}
			}
			ditherMap = null;
			yield return new WaitForSeconds(0.5f);
			if (Application.HasProLicense())
			{
				StaticBatchingUtility.Combine(this.root.gameObject);
			}
			yield break;
		}

		public void Destroy()
		{
			Debug.Log(string.Concat(new object[]
			{
				"Destroying tile ",
				this.x,
				", ",
				this.z
			}));
			Object.Destroy(this.root.gameObject);
			this.root = null;
		}

		private int x;

		private int z;

		private Random rnd;

		private ProceduralWorld world;

		private Transform root;

		private IEnumerator ie;
	}
}

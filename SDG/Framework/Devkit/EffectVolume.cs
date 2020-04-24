using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class EffectVolume : DevkitHierarchyVolume, IDevkitHierarchySpawnable
	{
		public EffectVolume()
		{
			this._emissionMultiplier = 1f;
			this._audioRangeMultiplier = 1f;
		}

		[Inspectable("#SDG::ID", null)]
		public ushort id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
				if (this.effect != null)
				{
					Object.Destroy(this.effect.gameObject);
					this.effect = null;
				}
				EffectAsset effectAsset = Assets.find(EAssetType.EFFECT, this.id) as EffectAsset;
				if (effectAsset != null)
				{
					this.effect = Object.Instantiate<GameObject>(effectAsset.effect).transform;
					this.effect.name = "Effect";
					this.effect.transform.parent = base.transform;
					this.effect.transform.localPosition = new Vector3(0f, 0f, 0f);
					this.effect.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
					this.effect.transform.localScale = new Vector3(1f, 1f, 1f);
					ParticleSystem component = this.effect.GetComponent<ParticleSystem>();
					if (component != null)
					{
						this.maxParticlesBase = component.main.maxParticles;
						this.rateOverTimeBase = component.emission.rateOverTimeMultiplier;
					}
					AudioSource component2 = this.effect.GetComponent<AudioSource>();
					if (component2 != null && component2.clip != null)
					{
						component2.time = Random.Range(0f, component2.clip.length);
					}
				}
				if (this.effect != null)
				{
					this.applyEmission();
					this.applyAudioRange();
				}
			}
		}

		[Inspectable("#SDG::Emission", null)]
		public float emissionMultiplier
		{
			get
			{
				return this._emissionMultiplier;
			}
			set
			{
				this._emissionMultiplier = value;
				if (this.effect != null)
				{
					this.applyEmission();
				}
			}
		}

		[Inspectable("#SDG::Audio_Range", null)]
		public float audioRangeMultiplier
		{
			get
			{
				return this._audioRangeMultiplier;
			}
			set
			{
				this._audioRangeMultiplier = value;
				if (this.effect != null)
				{
					this.applyAudioRange();
				}
			}
		}

		protected virtual void applyEmission()
		{
			if (this.effect == null)
			{
				return;
			}
			ParticleSystem component = this.effect.GetComponent<ParticleSystem>();
			if (component == null)
			{
				return;
			}
			component.main.maxParticles = (int)((float)this.maxParticlesBase * this.emissionMultiplier);
			component.emission.rateOverTimeMultiplier = this.rateOverTimeBase * this.emissionMultiplier;
		}

		protected virtual void applyAudioRange()
		{
			if (this.effect == null)
			{
				return;
			}
			AudioSource component = this.effect.GetComponent<AudioSource>();
			if (component == null)
			{
				return;
			}
			component.maxDistance = this.audioRangeMultiplier;
		}

		public void devkitHierarchySpawn()
		{
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (Level.isEditor && EffectVolumeSystem.effectVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("Emission"))
			{
				this._emissionMultiplier = reader.readValue<float>("Emission");
			}
			if (reader.containsKey("Audio_Range"))
			{
				this._audioRangeMultiplier = reader.readValue<float>("Audio_Range");
			}
			this.id = reader.readValue<ushort>("ID");
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<ushort>("ID", this.id);
			writer.writeValue<float>("Emission", this.emissionMultiplier);
			writer.writeValue<float>("Audio_Range", this.audioRangeMultiplier);
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			EffectVolumeSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			EffectVolumeSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Effect_Volume";
			base.gameObject.layer = LayerMasks.CLIP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			this.updateBoxEnabled();
			EffectVolumeSystem.effectVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void Start()
		{
			this.effect = base.transform.FindChild("Effect");
		}

		protected void OnDestroy()
		{
			EffectVolumeSystem.effectVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		[SerializeField]
		protected ushort _id;

		[SerializeField]
		protected int maxParticlesBase;

		[SerializeField]
		protected float rateOverTimeBase;

		[SerializeField]
		protected float _emissionMultiplier;

		[SerializeField]
		protected float _audioRangeMultiplier;

		protected Transform effect;
	}
}

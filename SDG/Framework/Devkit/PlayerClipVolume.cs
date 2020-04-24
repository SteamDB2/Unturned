using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class PlayerClipVolume : DevkitHierarchyVolume, IDevkitHierarchySpawnable
	{
		public PlayerClipVolume()
		{
			this.blockPlayer = true;
		}

		[Inspectable("#SDG::Devkit.Volumes.Player_Clip.Block_Player", null)]
		public bool blockPlayer
		{
			get
			{
				return this._blockPlayer;
			}
			set
			{
				this._blockPlayer = value;
				this.updateBoxEnabled();
			}
		}

		public override VolumeVisibilityGroup visibilityGroupOverride
		{
			get
			{
				return PlayerClipVolumeSystem.playerClipVisibilityGroup;
			}
		}

		public void devkitHierarchySpawn()
		{
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("Block_Player"))
			{
				this.blockPlayer = reader.readValue<bool>("Block_Player");
			}
			else
			{
				this.blockPlayer = true;
			}
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("Block_Player", this.blockPlayer);
		}

		protected virtual void updateBoxEnabled()
		{
			if (base.box == null)
			{
				return;
			}
			if (Level.isEditor)
			{
				base.box.enabled = PlayerClipVolumeSystem.playerClipVisibilityGroup.isVisible;
			}
			else
			{
				base.box.enabled = this.blockPlayer;
			}
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			PlayerClipVolumeSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			PlayerClipVolumeSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Player_Clip_Volume";
			base.gameObject.layer = LayerMasks.CLIP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			this.updateBoxEnabled();
			PlayerClipVolumeSystem.playerClipVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			PlayerClipVolumeSystem.playerClipVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		[SerializeField]
		protected bool _blockPlayer;
	}
}

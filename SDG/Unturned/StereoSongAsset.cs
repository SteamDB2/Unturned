using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Unturned
{
	public class StereoSongAsset : Asset, IDevkitAssetSpawnable
	{
		public StereoSongAsset()
		{
			this.construct();
		}

		public StereoSongAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.construct();
		}

		public void devkitAssetSpawn()
		{
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.title = reader.readValue<TranslationReference>("Title");
			this.song = reader.readValue<ContentReference<AudioClip>>("Song");
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<TranslationReference>("Title", this.title);
			writer.writeValue<ContentReference<AudioClip>>("Song", this.song);
		}

		protected virtual void construct()
		{
			this.title = TranslationReference.invalid;
			this.song = ContentReference<AudioClip>.invalid;
		}

		[Inspectable("#SDG::Asset.Stereo_Song.Title.Name", null)]
		public TranslationReference title;

		[Inspectable("#SDG::Asset.Stereo_Song.Song.Name", null)]
		public ContentReference<AudioClip> song;
	}
}

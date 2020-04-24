using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		public GrainModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		[Serializable]
		public struct Settings
		{
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			[Range(1f, 3f)]
			[Tooltip("Grain particle size in \"Filmic\" mode.")]
			public float size;

			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}

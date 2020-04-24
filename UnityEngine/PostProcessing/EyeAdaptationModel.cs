using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class EyeAdaptationModel : PostProcessingModel
	{
		public EyeAdaptationModel.Settings settings
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
			this.m_Settings = EyeAdaptationModel.Settings.defaultSettings;
		}

		[SerializeField]
		private EyeAdaptationModel.Settings m_Settings = EyeAdaptationModel.Settings.defaultSettings;

		public enum EyeAdaptationType
		{
			Progressive,
			Fixed
		}

		[Serializable]
		public struct Settings
		{
			public static EyeAdaptationModel.Settings defaultSettings
			{
				get
				{
					return new EyeAdaptationModel.Settings
					{
						lowPercent = 65f,
						highPercent = 95f,
						minLuminance = 0.03f,
						maxLuminance = 2f,
						exposureCompensation = 0.5f,
						adaptationType = EyeAdaptationModel.EyeAdaptationType.Progressive,
						speedUp = 2f,
						speedDown = 1f,
						logMin = -8,
						logMax = 4
					};
				}
			}

			[Range(1f, 99f)]
			[Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float lowPercent;

			[Range(1f, 99f)]
			[Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float highPercent;

			[Min(0f)]
			[Tooltip("Minimum average luminance to consider for auto exposure.")]
			public float minLuminance;

			[Min(0f)]
			[Tooltip("Maximum average luminance to consider for auto exposure.")]
			public float maxLuminance;

			[Min(0f)]
			[Tooltip("Exposure bias. Use this to control the global exposure of the scene.")]
			public float exposureCompensation;

			[Tooltip("Use \"Progressive\" if you want the auto exposure to be animated. Use \"Fixed\" otherwise.")]
			public EyeAdaptationModel.EyeAdaptationType adaptationType;

			[Min(0f)]
			[Tooltip("Adaptation speed from a dark to a light environment.")]
			public float speedUp;

			[Min(0f)]
			[Tooltip("Adaptation speed from a light to a dark environment.")]
			public float speedDown;

			[Range(-16f, -1f)]
			[Tooltip("Lower bound for the brightness range of the generated histogram (Log2).")]
			public int logMin;

			[Range(1f, 16f)]
			[Tooltip("Upper bound for the brightness range of the generated histogram (Log2).")]
			public int logMax;
		}
	}
}

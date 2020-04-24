using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public abstract class FoliageInfoAsset : Asset
	{
		public FoliageInfoAsset()
		{
			this.resetFoliageInfo();
		}

		public FoliageInfoAsset(Bundle bundle, Local localization, byte[] hash) : base(bundle, localization, hash)
		{
			this.resetFoliageInfo();
		}

		public virtual float randomNormalPositionOffset
		{
			get
			{
				return Random.Range(this.minNormalPositionOffset, this.maxNormalPositionOffset);
			}
		}

		public virtual Quaternion randomRotation
		{
			get
			{
				return Quaternion.Euler(new Vector3(Random.Range(this.minRotation.x, this.maxRotation.x), Random.Range(this.minRotation.y, this.maxRotation.y), Random.Range(this.minRotation.z, this.maxRotation.z)));
			}
		}

		public virtual Vector3 randomScale
		{
			get
			{
				return new Vector3(Random.Range(this.minScale.x, this.maxScale.x), Random.Range(this.minScale.y, this.maxScale.y), Random.Range(this.minScale.z, this.maxScale.z));
			}
		}

		public virtual void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			if (!this.isSurfaceWeightValid(surfaceWeight))
			{
				return;
			}
			this.bakeFoliageSteps(surface, bounds, surfaceWeight, collectionWeight, new FoliageInfoAsset.BakeFoliageStepHandler(this.handleBakeFoliageStep));
		}

		public virtual void addFoliageToSurface(Vector3 surfacePosition, Vector3 surfaceNormal, bool clearWhenBaked, bool followRules = false)
		{
			if (followRules && !this.isAngleValid(surfaceNormal))
			{
				return;
			}
			Vector3 position = surfacePosition + surfaceNormal * this.randomNormalPositionOffset;
			if (followRules && !this.isPositionValid(position))
			{
				return;
			}
			Quaternion quaternion = Quaternion.Lerp(MathUtility.IDENTITY_QUATERNION, Quaternion.FromToRotation(Vector3.up, surfaceNormal), this.normalRotationAlignment);
			quaternion *= Quaternion.Euler(this.normalRotationOffset);
			quaternion *= this.randomRotation;
			Vector3 randomScale = this.randomScale;
			this.addFoliage(position, quaternion, randomScale, clearWhenBaked);
		}

		public abstract int getInstanceCountInVolume(IShapeVolume volume);

		protected abstract void addFoliage(Vector3 position, Quaternion rotation, Vector3 scale, bool clearWhenBaked);

		protected virtual void bakeFoliageSteps(IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight, FoliageInfoAsset.BakeFoliageStepHandler callback)
		{
			float num = surfaceWeight * collectionWeight;
			float num2 = bounds.size.x * bounds.size.z;
			float num3 = num2 / this.density * num;
			int num4 = Mathf.FloorToInt(num3);
			if (Random.value < num3 - (float)num4)
			{
				num4++;
			}
			for (int i = 0; i < num4; i++)
			{
				callback(surface, bounds, surfaceWeight, collectionWeight);
			}
		}

		protected virtual Vector3 getTestPosition(Bounds bounds)
		{
			float num = Random.Range(-1f, 1f) * bounds.extents.x;
			float num2 = Random.Range(-1f, 1f) * bounds.extents.z;
			return bounds.center + new Vector3(num, bounds.extents.y, num2);
		}

		protected virtual void handleBakeFoliageStep(IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight)
		{
			Vector3 testPosition = this.getTestPosition(bounds);
			Vector3 surfacePosition;
			Vector3 surfaceNormal;
			if (!surface.getFoliageSurfaceInfo(testPosition, out surfacePosition, out surfaceNormal))
			{
				return;
			}
			this.addFoliageToSurface(surfacePosition, surfaceNormal, true, false);
		}

		protected virtual bool isAngleValid(Vector3 surfaceNormal)
		{
			float num = Vector3.Angle(Vector3.up, surfaceNormal);
			return num >= this.minSurfaceAngle && num <= this.maxSurfaceAngle;
		}

		protected abstract bool isPositionValid(Vector3 position);

		protected virtual bool isSurfaceWeightValid(float surfaceWeight)
		{
			return surfaceWeight >= this.minSurfaceWeight && surfaceWeight <= this.maxSurfaceWeight;
		}

		protected override void readAsset(IFormattedFileReader reader)
		{
			base.readAsset(reader);
			this.density = reader.readValue<float>("Density");
			this.minNormalPositionOffset = reader.readValue<float>("Min_Normal_Position_Offset");
			this.maxNormalPositionOffset = reader.readValue<float>("Max_Normal_Position_Offset");
			this.normalRotationOffset = reader.readValue<Vector3>("Normal_Rotation_Offset");
			if (reader.containsKey("Normal_Rotation_Alignment"))
			{
				this.normalRotationAlignment = reader.readValue<float>("Normal_Rotation_Alignment");
			}
			else
			{
				this.normalRotationAlignment = 1f;
			}
			this.minSurfaceWeight = reader.readValue<float>("Min_Weight");
			this.maxSurfaceWeight = reader.readValue<float>("Max_Weight");
			this.minSurfaceAngle = reader.readValue<float>("Min_Angle");
			this.maxSurfaceAngle = reader.readValue<float>("Max_Angle");
			this.minRotation = reader.readValue<Vector3>("Min_Rotation");
			this.maxRotation = reader.readValue<Vector3>("Max_Rotation");
			this.minScale = reader.readValue<Vector3>("Min_Scale");
			this.maxScale = reader.readValue<Vector3>("Max_Scale");
		}

		protected override void writeAsset(IFormattedFileWriter writer)
		{
			base.writeAsset(writer);
			writer.writeValue<float>("Density", this.density);
			writer.writeValue<float>("Min_Normal_Position_Offset", this.minNormalPositionOffset);
			writer.writeValue<float>("Max_Normal_Position_Offset", this.maxNormalPositionOffset);
			writer.writeValue<Vector3>("Normal_Rotation_Offset", this.normalRotationOffset);
			writer.writeValue<float>("Normal_Rotation_Alignment", this.normalRotationAlignment);
			writer.writeValue<float>("Min_Weight", this.minSurfaceWeight);
			writer.writeValue<float>("Max_Weight", this.maxSurfaceWeight);
			writer.writeValue<float>("Min_Angle", this.minSurfaceAngle);
			writer.writeValue<float>("Max_Angle", this.maxSurfaceAngle);
			writer.writeValue<Vector3>("Min_Rotation", this.minRotation);
			writer.writeValue<Vector3>("Max_Rotation", this.maxRotation);
			writer.writeValue<Vector3>("Min_Scale", this.minScale);
			writer.writeValue<Vector3>("Max_Scale", this.maxScale);
		}

		protected virtual void resetFoliageInfo()
		{
			this.normalRotationAlignment = 1f;
			this.maxSurfaceWeight = 1f;
			this.minScale = Vector3.one;
			this.maxScale = Vector3.one;
		}

		[Inspectable("#SDG::Asset.Foliage.Info.Density.Name", null)]
		public float density;

		[Inspectable("#SDG::Asset.Foliage.Info.Min_Normal_Position_Offset.Name", null)]
		public float minNormalPositionOffset;

		[Inspectable("#SDG::Asset.Foliage.Info.Max_Normal_Position_Offset.Name", null)]
		public float maxNormalPositionOffset;

		[Inspectable("#SDG::Asset.Foliage.Info.Normal_Rotation_Offset.Name", null)]
		public Vector3 normalRotationOffset;

		[Inspectable("#SDG::Asset.Foliage.Info.Normal_Rotation_Alignment.Name", null)]
		public float normalRotationAlignment;

		[Inspectable("#SDG::Asset.Foliage.Info.Min_Weight.Name", null)]
		public float minSurfaceWeight;

		[Inspectable("#SDG::Asset.Foliage.Info.Max_Weight.Name", null)]
		public float maxSurfaceWeight;

		[Inspectable("#SDG::Asset.Foliage.Info.Min_Angle.Name", null)]
		public float minSurfaceAngle;

		[Inspectable("#SDG::Asset.Foliage.Info.Max_Angle.Name", null)]
		public float maxSurfaceAngle;

		[Inspectable("#SDG::Asset.Foliage.Info.Min_Rotation.Name", null)]
		public Vector3 minRotation;

		[Inspectable("#SDG::Asset.Foliage.Info.Max_Rotation.Name", null)]
		public Vector3 maxRotation;

		[Inspectable("#SDG::Asset.Foliage.Info.Min_Scale.Name", null)]
		public Vector3 minScale;

		[Inspectable("#SDG::Asset.Foliage.Info.Max_Scale.Name", null)]
		public Vector3 maxScale;

		protected delegate void BakeFoliageStepHandler(IFoliageSurface surface, Bounds bounds, float surfaceWeight, float collectionWeight);
	}
}

using System;
using SDG.Unturned;
using UnityEngine;

public class PlanarReflection : MonoBehaviour
{
	public static event PlanarReflectionPreRenderHandler preRender;

	public static event PlanarReflectionPostRenderHandler postRender;

	public void Start()
	{
		if (this.sharedMaterial == null)
		{
			this.sharedMaterial = base.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
		}
	}

	private Camera CreateReflectionCameraFor(Camera cam)
	{
		string text = base.gameObject.name + "Reflection" + cam.name;
		GameObject gameObject = new GameObject(text);
		Camera camera = gameObject.AddComponent<Camera>();
		GraphicsSettings.planarReflectionNeedsUpdate = true;
		camera.nearClipPlane = cam.nearClipPlane;
		camera.farClipPlane = cam.farClipPlane;
		camera.backgroundColor = this.clearColor;
		camera.clearFlags = ((!this.reflectSkybox) ? 2 : 1);
		camera.backgroundColor = Color.black;
		camera.enabled = false;
		if (!camera.targetTexture)
		{
			camera.targetTexture = this.CreateTextureFor(cam);
		}
		return camera;
	}

	private RenderTexture CreateTextureFor(Camera cam)
	{
		return new RenderTexture(Mathf.RoundToInt((float)cam.pixelWidth * 0.5f), Mathf.RoundToInt((float)cam.pixelHeight * 0.5f), 16)
		{
			name = "PlanarReflection_RT"
		};
	}

	public void RenderHelpCameras(Camera currentCam)
	{
		if (!this.reflectionCamera)
		{
			this.reflectionCamera = this.CreateReflectionCameraFor(currentCam);
		}
		this.RenderReflectionFor(currentCam, this.reflectionCamera);
	}

	public void LateUpdate()
	{
		this.helped = false;
	}

	public void WaterTileBeingRendered(Transform tr, Camera currentCam)
	{
		if (base.enabled && currentCam.CompareTag("MainCamera"))
		{
			if (this.helped)
			{
				return;
			}
			this.helped = true;
			this.RenderHelpCameras(currentCam);
			if (this.reflectionCamera != null && this.sharedMaterial != null)
			{
				this.sharedMaterial.EnableKeyword("WATER_REFLECTIVE");
				this.sharedMaterial.DisableKeyword("WATER_SIMPLE");
				this.sharedMaterial.SetTexture(this.reflectionSampler, this.reflectionCamera.targetTexture);
			}
		}
		else if (this.reflectionCamera != null && this.sharedMaterial != null)
		{
			this.sharedMaterial.DisableKeyword("WATER_REFLECTIVE");
			this.sharedMaterial.EnableKeyword("WATER_SIMPLE");
			this.sharedMaterial.SetTexture(this.reflectionSampler, null);
		}
	}

	public void OnEnable()
	{
		Shader.EnableKeyword("WATER_REFLECTIVE");
		Shader.DisableKeyword("WATER_SIMPLE");
	}

	public void OnDisable()
	{
		Shader.EnableKeyword("WATER_SIMPLE");
		Shader.DisableKeyword("WATER_REFLECTIVE");
	}

	private void RenderReflectionFor(Camera cam, Camera reflectCamera)
	{
		if (!reflectCamera)
		{
			return;
		}
		if (this.sharedMaterial && !this.sharedMaterial.HasProperty(this.reflectionSampler))
		{
			return;
		}
		if (GraphicsSettings.planarReflectionNeedsUpdate)
		{
			GraphicsSettings.planarReflectionNeedsUpdate = false;
			switch (GraphicsSettings.planarReflectionQuality)
			{
			case EGraphicQuality.LOW:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_LOW;
				break;
			case EGraphicQuality.MEDIUM:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_MEDIUM;
				break;
			case EGraphicQuality.HIGH:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_HIGH;
				break;
			case EGraphicQuality.ULTRA:
				reflectCamera.cullingMask = PlanarReflection.CULLING_MASK_ULTRA;
				break;
			}
			reflectCamera.layerCullDistances = cam.layerCullDistances;
			reflectCamera.layerCullSpherical = cam.layerCullSpherical;
		}
		this.SaneCameraSettings(reflectCamera);
		GL.invertCulling = true;
		Transform transform = base.transform;
		Vector3 eulerAngles = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
		reflectCamera.transform.position = cam.transform.position;
		Vector3 position = transform.transform.position;
		position.y = transform.position.y;
		Vector3 up = transform.transform.up;
		float num = -Vector3.Dot(up, position) - this.clipPlaneOffset;
		Vector4 plane;
		plane..ctor(up.x, up.y, up.z, num);
		Matrix4x4 matrix4x = Matrix4x4.zero;
		matrix4x = PlanarReflection.CalculateReflectionMatrix(matrix4x, plane);
		this.oldpos = cam.transform.position;
		Vector3 position2 = matrix4x.MultiplyPoint(this.oldpos);
		reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
		Vector4 vector = this.CameraSpacePlane(reflectCamera, position, up, 1f);
		reflectCamera.projectionMatrix = cam.CalculateObliqueMatrix(vector);
		reflectCamera.transform.position = position2;
		Vector3 eulerAngles2 = cam.transform.eulerAngles;
		reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
		float lodBias = QualitySettings.lodBias;
		QualitySettings.lodBias = 1f;
		if (PlanarReflection.preRender != null)
		{
			PlanarReflection.preRender();
		}
		reflectCamera.Render();
		if (PlanarReflection.postRender != null)
		{
			PlanarReflection.postRender();
		}
		QualitySettings.lodBias = lodBias;
		GL.invertCulling = false;
	}

	private void SaneCameraSettings(Camera helperCam)
	{
		helperCam.renderingPath = 1;
		helperCam.hdr = false;
	}

	private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
		return reflectionMat;
	}

	private static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 vector = pos + normal * this.clipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 vector2 = worldToCameraMatrix.MultiplyPoint(vector);
		Vector3 vector3 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector3.x, vector3.y, vector3.z, -Vector3.Dot(vector2, vector3));
	}

	private static readonly int CULLING_MASK_LOW = RayMasks.GROUND | RayMasks.GROUND2;

	private static readonly int CULLING_MASK_MEDIUM = PlanarReflection.CULLING_MASK_LOW | RayMasks.SKY | RayMasks.LARGE | RayMasks.RESOURCE | RayMasks.STRUCTURE;

	private static readonly int CULLING_MASK_HIGH = PlanarReflection.CULLING_MASK_MEDIUM | RayMasks.MEDIUM | RayMasks.BARRICADE | RayMasks.VEHICLE;

	private static readonly int CULLING_MASK_ULTRA = PlanarReflection.CULLING_MASK_HIGH | RayMasks.ENEMY | RayMasks.DEBRIS | RayMasks.ENTITY | RayMasks.AGENT;

	public LayerMask reflectionMask;

	public bool reflectSkybox;

	public Color clearColor = Color.grey;

	public string reflectionSampler = "_ReflectionTex";

	public float clipPlaneOffset = 0.07f;

	private Vector3 oldpos = Vector3.zero;

	private Camera reflectionCamera;

	private bool helped;

	public Material sharedMaterial;
}

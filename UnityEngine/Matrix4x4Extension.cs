using System;

namespace UnityEngine
{
	public static class Matrix4x4Extension
	{
		public static Vector3 GetPosition(this Matrix4x4 matrix)
		{
			Vector3 result;
			result.x = matrix.m03;
			result.y = matrix.m13;
			result.z = matrix.m23;
			return result;
		}

		public static Quaternion GetRotation(this Matrix4x4 matrix)
		{
			Vector3 vector;
			vector.x = matrix.m02;
			vector.y = matrix.m12;
			vector.z = matrix.m22;
			Vector3 vector2;
			vector2.x = matrix.m01;
			vector2.y = matrix.m11;
			vector2.z = matrix.m21;
			return Quaternion.LookRotation(vector, vector2);
		}

		public static Vector3 GetScale(this Matrix4x4 matrix)
		{
			Vector4 vector;
			vector..ctor(matrix.m00, matrix.m10, matrix.m20, matrix.m30);
			Vector3 result;
			result.x = vector.magnitude;
			Vector4 vector2;
			vector2..ctor(matrix.m01, matrix.m11, matrix.m21, matrix.m31);
			result.y = vector2.magnitude;
			Vector4 vector3;
			vector3..ctor(matrix.m02, matrix.m12, matrix.m22, matrix.m32);
			result.z = vector3.magnitude;
			return result;
		}
	}
}

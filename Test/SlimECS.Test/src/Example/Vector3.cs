
namespace SlimECS.Test
{
	public struct Vector3
	{
		public float x, y, z;

		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Vector3 operator +(Vector3 left, Vector3 right)
		{
			return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
		}

		public static Vector3 operator -(Vector3 left, Vector3 right)
		{
			return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
		}

		public static Vector3 operator *(Vector3 left, float right)
		{
			return new Vector3(left.x * right, left.y * right, left.z * right);
		}

		public static Vector3 operator /(Vector3 value1, float value2)
		{
			float invDiv = 1.0f / value2;
			return new Vector3(value1.x * invDiv, value1.y * invDiv, value1.z * invDiv);
		}
	}
}


namespace microECS.Test
{
	public struct Vector3 : IComponent
	{
		public float x, y, z;
	}

	public struct Position : IComponent
	{
		public Vector3 pos;
	}

	public struct Velocity : IComponent
	{
		public Vector3 dir;
		public float speed;
	}

	public struct Frozen : IComponent
	{
	}
}

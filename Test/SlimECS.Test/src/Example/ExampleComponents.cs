
namespace SlimECS.Test
{
	public struct Vector3
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

	public struct ComplexStatus : IComponent
	{
		public Frozen status1;
	}
}

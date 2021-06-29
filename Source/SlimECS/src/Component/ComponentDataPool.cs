
namespace SlimECS
{
	public interface IComponentDataPool
	{
	}

	public class ComponentDataPool<T> : IComponentDataPool where T:struct, IComponent
	{
		public readonly StructDataPool<T> pool;

		public ComponentDataPool(int capacity = 0)
		{
			pool = new StructDataPool<T>(capacity);
		}
	}
}

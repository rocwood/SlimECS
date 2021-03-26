namespace SlimECS
{
	public partial class Context
	{
		public bool HasComponent<T>(Entity e) where T : struct, IComponent => Has<T>(e);
		public bool GetComponent<T>(Entity e, out T value) where T : struct, IComponent => Get(e, out value);
		public T GetComponent<T>(Entity e) where T : struct, IComponent => Get<T>(e);
		public bool SetComponent<T>(Entity e, T value) where T : struct, IComponent => Set(e, value);
		public bool RemoveComponent<T>(Entity e) where T : struct, IComponent => Remove<T>(e);

		public bool Has<T>(Entity e) where T : struct, IComponent
		{
			if (e.id <= 0)
				return false;

			var c = GetComponentDataList<T>();
			if (c == null)
				return false;

			return c.Has(e.id, e.slot);
		}

		public bool Get<T>(Entity e, out T value) where T : struct, IComponent
		{
			if (e.id <= 0)
			{
				value = default;
				return false;
			}

			var c = GetComponentDataList<T>();
			if (c == null)
			{
				value = default;
				return false;
			}

			return c.Get(e.id, e.slot, out value);
		}

		public T Get<T>(Entity e) where T : struct, IComponent
		{
			if (e.id <= 0)
				return default;

			var c = GetComponentDataList<T>();
			if (c == null)
				return default;

			return c.Get(e.id, e.slot);
		}

		public bool Set<T>(Entity e, T value) where T : struct, IComponent
		{
			if (e.id <= 0)
				return false;

			var c = GetComponentDataList<T>();
			if (c == null)
				return false;

			c.Set(e.id, e.slot, value);
			return true;
		}

		public ref T Ref<T>(Entity e) where T : struct, IComponent
		{
			if (e.id <= 0)
				return ref EmptyComponentRef<T>.value;

			var c = GetComponentDataList<T>();
			if (c == null)
				return ref EmptyComponentRef<T>.value;

			return ref c .Ref(e.id, e.slot);
		}

		public bool Remove<T>(Entity e) where T : struct, IComponent
		{
			if (e.id <= 0)
				return false;

			var c = GetComponentDataList<T>();
			if (c == null)
				return false;

			return c.Remove(e.id, e.slot);
		}
	}
}

namespace microECS
{

	public class Context
	{
		private readonly short _contextId;
		private readonly EntitySlotList _slotList;
		private IComponentList[] _componentData;

		internal Context(short contextId)
		{
			if (contextId < 0 || contextId >= Entity.contextIdMax)
				throw new ContextIdOverflowException();

			_contextId = contextId;
			_slotList = new EntitySlotList(contextId << Entity.slotBits);
		}

		internal short ContextId => _contextId;

		public Entity CreateEntity()
		{
			var e = _slotList.Create();

			return e;
		}

		public void DestroyEntity(Entity e)
		{
			if (!_slotList.Destroy(e))
				return;

			foreach (var componentList in _componentData)
				componentList.Remove(e.slot & Entity.slotMask);
		}

		public bool GetComponent<T>(Entity e, out T value) where T:struct, IComponent
		{
			if (!_slotList.IsValid(e))
			{
				value = default;
				return false;
			}

			var componentList = GetComponentList<T>();
			value = (T)componentList.Get(e.slot & Entity.slotMask);
			return true;
		}

		public T GetComponent<T>(Entity e) where T:struct, IComponent
		{
			if (!_slotList.IsValid(e))
				return default;
		
			var componentList = GetComponentList<T>();
			return (T)componentList.Get(e.slot & Entity.slotMask);
		}

		public bool SetComponent<T>(Entity e, T value) where T:struct, IComponent
		{
			if (!_slotList.IsValid(e))
				return false;

			var componentList = GetComponentList<T>();
			componentList.Set(e.slot & Entity.slotMask, value);
			return true;
		}

		public bool RemoveComponent<T>(Entity e) where T:struct, IComponent
		{
			if (!_slotList.IsValid(e))
				return false;

			var componentList = GetComponentList<T>();
			componentList.Remove(e.slot & Entity.slotMask);
			return true;
		}

		private IComponentList GetComponentList<T>() where T:struct, IComponent
		{
			int componentIndex = 0;

			var componentList = _componentData[componentIndex];
			return componentList;
		}
	}
}

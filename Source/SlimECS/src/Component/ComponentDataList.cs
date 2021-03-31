namespace SlimECS
{
	internal class ComponentDataList<T> : IComponentDataList<T> where T : struct, IComponent
	{
		struct ComponentData
		{
			public int entityId;
			public T component;
		}

		private StructArray<ComponentData> _data;

		public ComponentDataList()
		{
			_data = new StructArray<ComponentData>(0);
		}

		public bool Has(int entityId, int slot)
		{
			if (entityId <= 0 || slot < 0)
				return false;

			return _data.Ref(slot).entityId == entityId;
		}

		public bool Get(int entityId, int slot, out T value)
		{
			if (entityId <= 0 || slot < 0)
			{
				value = default;
				return false;
			}

			ref var d = ref _data.Ref(slot);
			if (d.entityId != entityId)
			{
				value = default;
				return false;
			}

			value = d.component;
			return true;
		}

		public T Get(int entityId, int slot)
		{
			if (entityId <= 0 || slot < 0)
				return default;

			ref var d = ref _data.Ref(slot);
			if (d.entityId != entityId)
				return default;

			return d.component;
		}

		public void Set(int entityId, int slot, T value)
		{
			if (entityId <= 0 || slot < 0)
				return;

			_data.EnsureAccess(slot);

			ref var d = ref _data.Ref(slot);
			d.component = value;
			d.entityId = entityId;
		}

		public ref T Ref(int entityId, int slot)
		{
			if (entityId <= 0 || slot < 0)
				return ref EmptyComponentRef<T>.value;

			ref var d = ref _data.Ref(slot);
			if (d.entityId != entityId)
				return ref EmptyComponentRef<T>.value;

			return ref d.component;
		}

		public bool Remove(int entityId, int slot)
		{
			if (!Has(entityId, slot))
				return false;

			// TODO: onRemove

			_data[slot] = default;
			return true;
		}
	}
}

namespace microECS
{
	interface IComponentDataList
	{
		bool Has(int index);
		bool Remove(int index);
	}

	interface IComponentDataList<T> : IComponentDataList
	{
		bool Get(int index, out T value);
		void Set(int index, T value);
	}

	class ComponentDataList<T> : IComponentDataList<T> where T : struct, IComponent
	{
		struct ComponentData
		{
			public bool hasValue;
			public T component;
		}

		private StructArray<ComponentData> _data;

		public ComponentDataList()
		{
			_data = new StructArray<ComponentData>(0);
		}

		public bool Has(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			return _data.Ref(index).hasValue;
		}

		public bool Get(int index, out T value)
		{
			if (index < 0 || index >= _data.Length)
			{
				value = default;
				return false;
			}

			ref var d = ref _data.Ref(index);
			if (!d.hasValue)
			{
				value = default;
				return false;
			}

			value = d.component;
			return true;
		}

		public void Set(int index, T value)
		{
			if (index < 0)
				return;

			_data.EnsureAccess(index);

			ref var d = ref _data.Ref(index);
			d.component = value;
			d.hasValue = true;
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			// TODO: onRemove

			ref var d = ref _data.Ref(index);
			if (!d.hasValue)
				return false;

			_data[index] = default;
			return true;
		}
	}

	class ZeroSizeComponentDataList<T> : IComponentDataList<T> where T : struct, IComponent
	{
		private StructArray<bool> _data;

		public ZeroSizeComponentDataList()
		{
			_data = new StructArray<bool>(0);
		}

		public bool Has(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			return _data[index];
		}

		public bool Get(int index, out T value)
		{
			value = default;

			return Has(index);
		}

		public void Set(int index, T value)
		{
			if (index < 0)
				return;

			_data.EnsureAccess(index);
			_data[index] = true;
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			// TODO: onRemove

			_data[index] = false;
			return true;
		}
	}
}

using System;

namespace microECS
{
	interface IComponentDataList
	{
		bool Has(int index);
		bool Remove(int index);
	}

	class ComponentDataList<T> : IComponentDataList where T:struct, IComponent
	{
		private const int _defaultCapacity = 256;
		private const int _maxCapacity = 0x7FEFFFFF;

		struct ComponentData
		{
			public bool hasValue;
			public T component;
		}

		private ComponentData[] _data;

		public ComponentDataList()
		{
			_data = new ComponentData[0];
		}

		public bool Has(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			return _data[index].hasValue;
		}

		public bool Get(int index, out T value)
		{
			if (index < 0 || index >= _data.Length)
			{
				value = default;
				return false;
			}

			ref var d = ref _data[index];
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

			if (index >= _data.Length)
			{
				int size = _data.Length;
				while (index >= size)
				{
					if (size <= 0)
						size = _defaultCapacity;
					else
						size *= 2;

					if ((uint)size > _maxCapacity)
					{
						size = _maxCapacity;
						break;
					}
				}

				if (size > _data.Length)
					Array.Resize(ref _data, size);
			}

			ref var d = ref _data[index];
			d.component = value;
			d.hasValue = true;
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			// TODO: onRemove

			ref var d = ref _data[index];
			if (!d.hasValue)
				return false;

			_data[index] = default;
			return true;
		}
	}
}

using System;

namespace microECS
{
	class StructArray<T> where T : struct
	{
		private const int _defaultCapacity = 256;
		private const int _maxCapacity = 0x7FEFFFFF;

		private T[] _items;

		public StructArray(int capacity = 0)
		{
			if (capacity < 0)
				capacity = 0;

			_items = new T[capacity];
		}

		public int Length => _items.Length;

		public ref T Ref(int index) => ref _items[index];

		public T this[int index]
		{
			get => _items[index];
			set => _items[index] = value;
		}

		public void EnsureAccess(int index)
		{
			int size = _items.Length;
			if (index < size)
				return;

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

			if (size > _items.Length)
				Array.Resize(ref _items, size);
		}
	}
}

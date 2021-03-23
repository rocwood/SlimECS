using System;

namespace SlimECS
{
	class EntityDataList
	{
		private const int DefaultCapacity = 256;
		private const int MaxCapacity = 0x7FEFFFFF;

		private int[] _entities;
		private string[] _names;

		private int _lastId = 0;
		private int _entityCount = 0;

		private int _freeSlotHead = 0;

		public EntityDataList(int capacity)
		{
			if (capacity <= 0)
				capacity = DefaultCapacity;

			_entities = new int[capacity];
			_names = new string[capacity];

			// init free-slot links
			for (int i = 0; i < capacity; i++)
				_entities[i] = -(i + 1);

			_freeSlotHead = 0;
		}

		public int Count => _entityCount;

		public Entity Create(string name)
		{	
			var id = _lastId++;
			int slot = _freeSlotHead;

			EnsureAccess(slot);

			_freeSlotHead = -_entities[slot];

			_entities[slot] = id;
			_names[slot] = name;

			_entityCount++;

			return new Entity(id, slot);
		}

		public void Destroy(Entity e)
		{
			if (!Contains(e))
				return;

			_entities[e.slot] = -_freeSlotHead;
			_names[e.slot] = null;

			_freeSlotHead = e.slot;
			_entityCount--;
		}

		public void SetName(Entity e, string name)
		{
			if (!Contains(e))
				return;

			_names[e.slot] = name;
		}

		public bool Contains(Entity e)
		{
			if (e.id <= 0 || e.slot < 0 || e.slot >= _entities.Length)
				return false;

			return _entities[e.slot] == e.id;
		}

		private void EnsureAccess(int index)
		{
			int size = _entities.Length;
			if (index < size)
				return;

			while (index >= size)
			{
				if (size <= 0)
					size = DefaultCapacity;
				else
					size *= 2;

				if ((uint)size > MaxCapacity)
				{
					size = MaxCapacity;
					break;
				}
			}

			if (size > _entities.Length)
			{
				Array.Resize(ref _entities, size);
				Array.Resize(ref _names, size);
			}
		}
	}
}

using System;

namespace SlimECS
{
	class EntityDataList
	{
		private const int DefaultCapacity = 256;
		private const int MaxCapacity = 0x7FEFFFFF;

		struct EntityData
		{
			public int id;
			public bool toDestroy;
			public bool changed;
			public string name;
		}

		private EntityData[] _entities;

		private int _lastId = 0;
		private int _entityCount = 0;

		private int _freeSlotHead = 0;

		private bool _hasToDestroy = false;
		private bool _hasChanged = false;

		public EntityDataList(int capacity)
		{
			if (capacity <= 0)
				capacity = DefaultCapacity;

			_entities = new EntityData[capacity];

			_freeSlotHead = 0;

			InitFreeSlotLinks(0, capacity);
		}

		private void InitFreeSlotLinks(int start, int end)
		{
			for (int i = start; i < end; i++)
				_entities[i].id = -(i + 1);
		}

		public int Count => _entityCount;

		/*
		internal int Capacity => _entities.Length;
		internal Entity GetAt(int slot)
		{
			int id = _entities[slot];

			return new Entity(id, slot);
		}
		internal Entity GetAt(int slot, out byte flags)
		{
			int id = _entities[slot];
			flags = _flags[slot];

			return new Entity(id, slot);
		}
		*/

		public Entity Create(string name)
		{
			var id = ++_lastId;
			int slot = _freeSlotHead;

			EnsureAccess(slot);

			ref var d = ref _entities[slot];

			_freeSlotHead = -d.id;

			d.id = id;
			d.name = name;
			d.changed = true;
			d.toDestroy = false;

			_entityCount++;

			_hasChanged = true;

			return new Entity(id, slot);
		}

		public void SetName(Entity e, string name)
		{
			if (!IsActive(e))
				return;

			_entities[e.slot].name = name;
		}

		public bool IsActive(Entity e)
		{
			if (e.id <= 0 || e.slot < 0 || e.slot >= _entities.Length)
				return false;

			ref var d = ref _entities[e.slot];
			return d.id == e.id && !d.toDestroy;
		}

		internal void SetChanged(Entity e)
		{
			if (e.id <= 0 || e.slot < 0 || e.slot >= _entities.Length)
				return;

			ref var d = ref _entities[e.slot];
			if (d.id == e.id)
				d.changed = true;

			_hasChanged = true;
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
				int oldSize = _entities.Length;
				Array.Resize(ref _entities, size);

				InitFreeSlotLinks(oldSize, size);
			}
		}

		public void Destroy(Entity e)
		{
			if (e.id <= 0 || e.slot < 0 || e.slot >= _entities.Length)
				return;

			ref var d = ref _entities[e.slot];
			d.toDestroy = true;
			d.changed = true;

			_hasToDestroy = true;
			_hasChanged = true;
		}

		internal void CollectDestroyEntities()
		{
			if (!_hasToDestroy)
				return;

			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				ref var e = ref _entities[slot];
				if (e.toDestroy && e.id > 0)
				{
					e.id = -_freeSlotHead;
					e.toDestroy = false;
					e.changed = false;
					e.name = null;

					_freeSlotHead = slot;
					_entityCount--;
				}
			}

			_hasToDestroy = false;
		}

		internal void ForEach(Action<Entity, bool /*Active*/, bool/*Changed*/> process)
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				ref var e = ref _entities[slot];
				if (e.id > 0)
					process(new Entity(e.id, slot), !e.toDestroy, e.changed);
			}
		}

		internal void ForEachActive(Action<Entity> process)
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				ref var e = ref _entities[slot];
				if (e.id > 0 && !e.toDestroy)
					process(new Entity(e.id, slot));
			}
		}

		internal void ForEachChanged(Action<Entity> process)
		{
			if (!_hasChanged)
				return;

			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				ref var e = ref _entities[slot];
				if (e.changed && e.id > 0)
					process(new Entity(e.id, slot));
			}
		}

		internal void ResetChanged()
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				_entities[slot].changed = false;
			}

			_hasChanged = false;
		}
	}
}

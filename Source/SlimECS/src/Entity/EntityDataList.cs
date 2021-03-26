using System;

namespace SlimECS
{
	class EntityDataList
	{
		private const int DefaultCapacity = 256;
		private const int MaxCapacity = 0x7FEFFFFF;

		private const byte FlagToDestroy = 1;
		private const byte FlagChanged = 2;

		private int[] _entities;
		private byte[] _flags;
		private string[] _names;

		private int _lastId = 0;
		private int _entityCount = 0;

		private int _freeSlotHead = 0;

		public EntityDataList(int capacity)
		{
			if (capacity <= 0)
				capacity = DefaultCapacity;

			_entities = new int[capacity];
			_flags = new byte[capacity];
			_names = new string[capacity];

			_freeSlotHead = 0;

			InitFreeSlotLinks(0, capacity);
		}

		private void InitFreeSlotLinks(int start, int end)
		{
			for (int i = start; i < end; i++)
				_entities[i] = -(i + 1);
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

			_freeSlotHead = -_entities[slot];

			_entities[slot] = id;
			_names[slot] = name;
			_flags[slot] = FlagChanged;

			_entityCount++;

			return new Entity(id, slot);
		}

		public void SetName(Entity e, string name)
		{
			if (!IsActive(e))
				return;

			_names[e.slot] = name;
		}

		public bool IsActive(Entity e)
		{
			if (e.id <= 0 || e.slot < 0 || e.slot >= _entities.Length)
				return false;

			return _entities[e.slot] == e.id 
				&& (_flags[e.slot]&FlagToDestroy) == 0;
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
				Array.Resize(ref _flags, size);
				Array.Resize(ref _names, size);

				InitFreeSlotLinks(oldSize, size);
			}
		}

		public void Destroy(Entity e)
		{
			if (e.id <= 0 || e.slot < 0 || e.slot >= _entities.Length)
				return;

			_flags[e.slot] = FlagChanged | FlagToDestroy;
		}

		internal void CollectDestroyEntities()
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				if ((_flags[slot] & FlagToDestroy) != 0 && _entities[slot] > 0)
				{
					_entities[slot] = -_freeSlotHead;
					_names[slot] = null;
					_flags[slot] = 0;

					_freeSlotHead = slot;
					_entityCount--;
				}
			}
		}
		internal void ForEach(Action<Entity, bool /*Active*/, bool/*Changed*/> process)
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				var e = new Entity(_entities[slot], slot);
				var flags = _flags[slot];

				process(e, (flags & FlagToDestroy) == 0, (flags & FlagChanged) != 0);
			}
		}
		internal void ForEachActive(Action<Entity> process)
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				if (_entities[slot] > 0 && (_flags[slot] & FlagToDestroy) == 0)
					process(new Entity(_entities[slot], slot));
			}
		}
		internal void ForEachChanged(Action<Entity> process)
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
			{
				if ((_flags[slot] & FlagChanged) != 0 && _entities[slot] > 0)
					process(new Entity(_entities[slot], slot));
			}
		}
		internal void ResetChanged()
		{
			int length = _entities.Length;
			for (int slot = 0; slot < length; slot++)
				unchecked { _flags[slot] &= (byte)~FlagChanged; }
		}
	}
}

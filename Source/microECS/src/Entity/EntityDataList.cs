using System;

namespace microECS
{
	struct EntityData
	{
		public readonly int id;
		public string name;

		public EntityData(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public static readonly EntityData Empty = new EntityData();
	}

	class EntityDataList
	{
		private const int _defaultCapacity = 1024;
		private const int _slotModDenominator = 997;
		private const int _findEmptySlotStep = 31;
		private const float _enlargeThresold = 0.8f;

		private readonly int _contextIdShift;

		private int _entityCount = 0;
		private int _lastEntityId = 0;

		private EntityData[] _entities;

		public EntityDataList(int contextIdShift)
		{
			_contextIdShift = contextIdShift;

			_entities = new EntityData[_defaultCapacity];
		}

		public Entity Create(string name)
		{	
			_lastEntityId++;

			var id = _lastEntityId;
			int slot = FindEmptySlot(id);

			_entities[slot] = new EntityData(id, name);
			_entityCount++;

			return new Entity(id, slot, _contextIdShift);
		}

		public bool Destroy(Entity e)
		{
			if (!Contains(e))
				return false;

			_entities[e.slot] = EntityData.Empty;
			_entityCount--;

			return true;
		}

		public void SetName(Entity e, string name)
		{
			if (!Contains(e))
				return;

			_entities[e.slot].name = name;
		}

		public bool Contains(Entity e)
		{
			if (e.id <= 0 || e.location <= 0 || e.contextIdShift != _contextIdShift)
				return false;

			int slot = e.slot;
			if (slot >= _entities.Length)
				return false;

			return _entities[slot].id == e.id;
		}

		private int FindEmptySlot(int id)
		{
			int capacity = _entities.Length;

			// enlarge container(2x) when it's almost full
			if (_entityCount >= capacity * _enlargeThresold)
			{
				capacity *= 2;
				Array.Resize(ref _entities, capacity);
			}

			int start = id % _slotModDenominator % capacity;
			int slot = start;

			// find empty slot, step by _findEmptySlotStep
			for (;;)
			{
				if (_entities[slot].id == 0)
					break;

				slot = (slot + _findEmptySlotStep) % capacity;
				if (slot == start)
					throw new EntityContainerException();
			}

			return slot;
		}
	}
}

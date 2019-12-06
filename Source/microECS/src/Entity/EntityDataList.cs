using System;

namespace microECS
{
	public class EntityDataList
	{
		private const int DefaultCapacity = 1024;
		private const int SlotModDenominator = 997;
		private const int FindEmptySlotStep = 31;
		private const float EnlargeThresold = 0.8f;

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

		private EntityData[] _entities;

		private int _entityCount = 0;
		private int _lastEntityId = 0;

		private readonly int _contextIdShift;

		public int Count => _entityCount;

		public EntityDataList(int contextIdShift)
		{
			_contextIdShift = contextIdShift;

			_entities = new EntityData[DefaultCapacity];
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
			if (_entityCount >= capacity * EnlargeThresold)
			{
				capacity *= 2;
				Array.Resize(ref _entities, capacity);
			}

			int start = id % SlotModDenominator % capacity;
			int slot = start;

			// find empty slot, step by _findEmptySlotStep
			for (;;)
			{
				if (_entities[slot].id == 0)
					break;

				slot = (slot + FindEmptySlotStep) % capacity;
				if (slot == start)
					throw new EntityContainerException();
			}

			return slot;
		}
	}
}

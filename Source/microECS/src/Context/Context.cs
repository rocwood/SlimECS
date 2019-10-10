using System;

namespace microECS
{
	public class Context
	{
		private const int _defaultCapacity = 1024;
		private const int _slotModDenominator = 997;
		private const int _findEmptySlotStep = 31;

		private readonly int _contextId;
		private readonly int _contextIdMask;
		private readonly string _name;

		private object _syncObject = new object();

		private IComponentDataArray[] _components;
		private EntityData[] _entities;

		private int _entityCount = 0;
		private int _lastEntityId = 0;

		struct EntityData
		{
			public Entity entity;
			public string name;

			public EntityData(Entity entity, string name)
			{
				this.entity = entity;
				this.name = name;
			}

			public bool IsEmpty() => entity.id == 0;

			public static readonly EntityData Empty = new EntityData();
		}

		internal Context(int contextId, string name = null)
		{
			if (contextId < 0 || contextId >= Entity.contextIdMax)
				throw new ContextIdOverflowException();

			_contextId = contextId;
			_contextIdMask = contextId << Entity.slotBits;
			_name = name;

			_entities = new EntityData[_defaultCapacity];
		}

		public Entity Create(string name = null)
		{
			lock (_syncObject)
			{
				_lastEntityId++;

				var id = _lastEntityId;
				int slot = FindEmptySlot(id);

				var entity = new Entity(id, slot, _contextIdMask);
				_entities[slot] = new EntityData(entity, name);
				_entityCount++;

				return entity;
			}
		}

		private int FindEmptySlot(int id)
		{
			int slot;
			int capacity = _entities.Length;

			if (_entityCount < capacity)
			{
				int start = id % _slotModDenominator % capacity;
				slot = start;

				// find empty slot, at _findEmptySlotStep
				for (;;)
				{
					if (_entities[slot].IsEmpty())
						break;

					slot = (slot + _findEmptySlotStep) % capacity;
					if (slot == start)
						throw new EntityContainerException();
				}
			}
			else
			{
				// enlarge the entity list when full
				slot = capacity;
				Array.Resize(ref _entities, capacity * 2);
			}

			return slot;
		}

		public bool Destroy(Entity e)
		{
			lock (_syncObject)
			{
				if (!Contains(e))
					return false;

				// destroy all components 
				foreach (var array in _components)
					array.Remove(e.slot);

				// remove entity
				int slot = e.location & Entity.slotMask;
				_entities[slot] = EntityData.Empty;
				_entityCount--;

				return true;
			}
		}

		public bool Contains(Entity e)
		{
			if (e.id <= 0 || e.location <= 0 || (e.location & Entity.contextIdMask) != _contextIdMask)
				return false;

			int slot = e.location & Entity.slotMask;
			if (slot >= _entities.Length)
				return false;

			return _entities[slot].entity == e;
		}

		public bool GetComponent<T>(Entity e, out T value) where T : struct, IComponent
		{
			if (!Contains(e))
			{
				value = default;
				return false;
			}

			var array = GetComponentDataArray<T>();
			if (array == null)
			{
				value = default;
				return false;
			}

			return array.Get(e.slot, out value);
		}

		public T GetComponent<T>(Entity e) where T : struct, IComponent
		{
			if (!Contains(e))
				return default;

			var array = GetComponentDataArray<T>();
			if (array == null)
				return default;

			array.Get(e.slot, out var value);
			return value;
		}

		public bool SetComponent<T>(Entity e, T value) where T : struct, IComponent
		{
			if (!Contains(e))
				return false;

			var array = GetComponentDataArray<T>();
			if (array == null)
				return false;

			array.Set(e.slot, value);
			return true;
		}

		public bool RemoveComponent<T>(Entity e) where T : struct, IComponent
		{
			if (!Contains(e))
				return false;

			var array = GetComponentDataArray<T>();
			if (array == null)
				return false;

			return array.Remove(e.location & Entity.slotMask);
		}

		private ComponentDataArray<T> GetComponentDataArray<T>() where T : struct, IComponent
		{
			int componentIndex = 0;
			
			// TODO

			var array = _components[componentIndex];
			return array as ComponentDataArray<T>;
		}
	}
}

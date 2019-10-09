using System;
using System.Collections.Generic;

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

		private IComponentList[] _componentLists;

		private int _lastEntityId = 0;

		private object _syncObject = new object();


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

		private EntityData[] _entities;
		private int _entityCount = 0;


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

				var entity = new Entity(id, slot | _contextIdMask);
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
				if (!IsValid(e))
					return false;

				// destroy all components 
				foreach (var componentList in _componentLists)
					componentList.Remove(e.slot & Entity.slotMask);

				// remove entity
				int slot = e.slot & Entity.slotMask;
				_entities[slot] = EntityData.Empty;
				_entityCount--;

				return true;
			}
		}

		internal bool IsValid(Entity e)
		{
			if (e.id <= 0 || e.slot <= 0 || (e.slot & Entity.contextIdMask) != _contextIdMask)
				return false;

			int slot = e.slot & Entity.slotMask;
			if (slot >= _entities.Length)
				return false;

			return _entities[slot].entity == e;
		}

		public bool GetComponent<T>(Entity e, out T value) where T : struct, IComponent
		{
			if (!IsValid(e))
			{
				value = default;
				return false;
			}

			var componentList = GetComponentList<T>();
			value = (T)componentList.Get(e.slot & Entity.slotMask);
			return true;
		}

		public T GetComponent<T>(Entity e) where T : struct, IComponent
		{
			if (!IsValid(e))
				return default;

			var componentList = GetComponentList<T>();
			return (T)componentList.Get(e.slot & Entity.slotMask);
		}

		public bool SetComponent<T>(Entity e, T value) where T : struct, IComponent
		{
			if (!IsValid(e))
				return false;

			var componentList = GetComponentList<T>();
			componentList.Set(e.slot & Entity.slotMask, value);
			return true;
		}

		public bool RemoveComponent<T>(Entity e) where T : struct, IComponent
		{
			if (!IsValid(e))
				return false;

			var componentList = GetComponentList<T>();
			componentList.Remove(e.slot & Entity.slotMask);
			return true;
		}

		private IComponentList GetComponentList<T>() where T : struct, IComponent
		{
			int componentIndex = 0;

			var componentList = _componentLists[componentIndex];
			return componentList;
		}
	}
}

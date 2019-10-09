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

		private readonly List<EntityData> _entities;
		private int _entityCount = 0;


		internal Context(int contextId, string name = null)
		{
			if (contextId < 0 || contextId >= Entity.contextIdMax)
				throw new ContextIdOverflowException();

			_contextId = contextId;
			_contextIdMask = contextId << Entity.slotBits;
			_name = name;

			_entities = new List<EntityData>(_defaultCapacity);
		}


		public Entity Create(string name = null)
		{
			lock (_syncObject)
			{
				_lastEntityId++;

				var id = _lastEntityId;
				int slot = FindEmptySlot(id);

				var entity = new Entity(id, slot | _contextIdMask);
				var entityData = new EntityData(entity, name);
				_entities[slot] = entityData;

				_entityCount++;

				return entity;

				/*
				// TODO
				var e = _entities[slot];
				if (e.id == 0)
				{
					e = new Entity(id, slot | _contextIdMask);
					
					return e;
				}

				slot = _entities.FindIndex(x => x.id == 0);
				if (slot > 0)
				{
					e = new Entity(id, slot | _contextIdMask);
					_entities[slot] = e;
					return e;
				}

				// TODO enlarge
				slot = _entities.Count;
				e = new Entity(id, slot | _contextIdMask);
				_entities.Add(e);
				*/


			}
		}

		private int FindEmptySlot(int id)
		{
			int slot = 0;
			if (_entityCount < _entities.Count)
			{
				int start = id % _slotModDenominator % _entities.Count;
				slot = start;

				// find empty slot, at _findEmptySlotStep
				for (;;)
				{
					if (_entities[slot].IsEmpty())
						break;

					slot = (slot + _findEmptySlotStep) % _entities.Count;

					if (slot == start)
					{

					}
				}
			}
			else
			{
				// enlarge the entity list when full
				slot = _entities.Count;
				_entities.Add(EntityData.Empty);
			}

			return slot;
		}

		public bool Destroy(Entity e)
		{
			if (!IsValid(e))
				return false;

			int slot = e.slot & Entity.slotMask;
			_entities[slot] = EntityData.Empty;

			return true;
		}

		public bool IsValid(Entity e)
		{
			if (e.id <= 0 || e.slot <= 0 || (e.slot & Entity.contextIdMask) != _contextIdMask)
				return false;

			int slot = e.slot & Entity.slotMask;
			if (slot >= _entities.Count)
				return false;

			return _entities[slot] == e;
		}


		public Entity CreateEntity()
		{
			var e = _slotList.Create();

			return e;
		}

		public void DestroyEntity(Entity e)
		{
			if (!_slotList.Destroy(e))
				return;

			foreach (var componentList in _componentLists)
				componentList.Remove(e.slot & Entity.slotMask);
		}

		public bool GetComponent<T>(Entity e, out T value) where T : struct, IComponent
		{
			if (!_slotList.IsValid(e))
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
			if (!_slotList.IsValid(e))
				return default;

			var componentList = GetComponentList<T>();
			return (T)componentList.Get(e.slot & Entity.slotMask);
		}

		public bool SetComponent<T>(Entity e, T value) where T : struct, IComponent
		{
			if (!_slotList.IsValid(e))
				return false;

			var componentList = GetComponentList<T>();
			componentList.Set(e.slot & Entity.slotMask, value);
			return true;
		}

		public bool RemoveComponent<T>(Entity e) where T : struct, IComponent
		{
			if (!_slotList.IsValid(e))
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

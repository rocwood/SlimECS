using System.Collections.Generic;

namespace microECS
{
	public class IdMap
	{
		private int _modFactor = 1999;
		private int _lastId = 0;

		//private HashSet<int> _freeIndices;
		private List<int> _index2Id;

		/// quick lookup for (id % modFactor) => (index)
		private int[] _idMod2Index;

		private Dictionary<int, int> _id2IndexLookup;

		public int Create()
		{
			_lastId++;

			var id = _lastId;
			var index = GetFreeIndex(id);
			var idMod = id % _modFactor;

			if (_idMod2Index[idMod] == 0)
				_idMod2Index[idMod] = index;
			else 
				_id2IndexLookup[id] = index;

			return id;
		}

		private int GetFreeIndex(int id)
		{

		}
	}

	public class Context
	{
		private IComponentList[] _componentData;
		private Dictionary<int, int> _entityIdRemap;

		private int _id = 0;

		public int CreateEntity()
		{
			// TODO
			_id++;

			return _id;
		}

		public void DestroyEntity(int id)
		{
			if (id == 0)
				return;

			if (id < 0)
				_entityIdRemap.Remove(-id);

			// TODO
		}

		public T GetComponent<T>(int id) where T : struct, IComponent
		{
			if (id == 0)
				return default;

			int entityIndex = GetEntityIndex(id);
			if (entityIndex == 0)
				return default;

			var componentList = GetComponentList<T>();
			return (T)componentList.Get(entityIndex);
		}

		public void SetComponent<T>(int id, T value) where T:struct, IComponent
		{
			if (id == 0)
				return;

			int entityIndex = GetEntityIndex(id);
			if (entityIndex <= 0)
				return;

			var componentList = GetComponentList<T>();
			componentList.Set(entityIndex, value);
		}

		public void RemoveComponent<T>(int id) where T:struct, IComponent
		{
			if (id == 0)
				return;

			int entityIndex = GetEntityIndex(id);
			if (entityIndex <= 0)
				return;

			var componentList = GetComponentList<T>();
			componentList.Remove(entityIndex);
		}

		private int GetEntityIndex(int id)
		{
			if (id == 0)
				return 0;

			int entityIndex = id;

			if (id < 0)
			{
				if (!_entityIdRemap.TryGetValue(-id, out entityIndex))
					return 0;
			}

			return entityIndex;
		}

		private IComponentList GetComponentList<T>() where T:struct, IComponent
		{
			int componentIndex = 0;

			var componentList = _componentData[componentIndex];
			return componentList;
		}
	}
}

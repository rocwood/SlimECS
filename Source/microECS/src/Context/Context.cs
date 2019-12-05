using System;

namespace microECS
{
	public class Context
	{
		private readonly int _contextId;
		private readonly int _contextIdShift;
		private readonly string _name;

		private readonly IComponentDataList[] _components;
		private readonly EntityDataList _entities;

		internal Context(int contextId, string name)
		{
			if (contextId < 0 || contextId >= Entity.contextIdMax)
				throw new ContextIdOverflowException();

			// context base info
			_contextId = contextId;
			_contextIdShift = contextId << Entity.slotBits;
			_name = name;

			// setup entities container
			_entities = new EntityDataList(_contextIdShift);

			// setup components container
			var componentInfoList = ContextInfo.GetComponentInfoList();
			int count = componentInfoList.Length;

			_components = new IComponentDataList[count];
			for (int i = 0; i < count; i++)
				_components[i] = CreateComponentDataList(componentInfoList[i]);
		}

		public Entity Create(string name = null)
		{
			return _entities.Create(name);
		}

		public bool Destroy(Entity e)
		{
			if (!Contains(e))
				return false;

			// could destroy entity first?
			foreach (var array in _components)
				array.Remove(e.slot);

			return _entities.Destroy(e);
		}

		public void SetName(Entity e, string name)
		{
			_entities.SetName(e, name);
		}

		public bool Contains(Entity e)
		{
			return _entities.Contains(e);
		}

		public bool HasComponent<T>(Entity e) where T : struct, IComponent
		{
			if (!Contains(e))
				return false;

			var c = GetComponentDataList<T>();
			if (c == null)
				return false;

			return c.Has(e.slot);
		}

		public bool GetComponent<T>(Entity e, out T value) where T : struct, IComponent
		{
			if (!Contains(e))
			{
				value = default;
				return false;
			}

			var c = GetComponentDataList<T>();
			if (c == null)
			{
				value = default;
				return false;
			}

			return c.Get(e.slot, out value);
		}

		public bool SetComponent<T>(Entity e, T value) where T : struct, IComponent
		{
			if (!Contains(e))
				return false;

			var c = GetComponentDataList<T>();
			if (c == null)
				return false;

			c.Set(e.slot, value);
			return true;
		}

		public bool RemoveComponent<T>(Entity e) where T : struct, IComponent
		{
			if (!Contains(e))
				return false;

			var c = GetComponentDataList<T>();
			if (c == null)
				return false;

			return c.Remove(e.slot);
		}

		private IComponentDataList<T> GetComponentDataList<T>() where T : struct, IComponent
		{
			int componentIndex = ContextInfo.GetIndexOf<T>();
			if (componentIndex < 0 || componentIndex >= _components.Length)
				return null;

			var c = _components[componentIndex];
			return c as IComponentDataList<T>;
		}

		private IComponentDataList CreateComponentDataList(ComponentTypeInfo info)
		{
			if (info == null || info.type == null)
				return null;

			var cType = info.zeroSize
				? typeof(ZeroSizeComponentDataList<>).MakeGenericType(info.type) // TODO: check il2cpp failure
				: typeof(ComponentDataList<>).MakeGenericType(info.type);

			if (cType == null)
				return null;

			return (IComponentDataList)Activator.CreateInstance(cType);
		}
	}
}

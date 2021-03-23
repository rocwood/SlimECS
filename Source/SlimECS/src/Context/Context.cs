using System;

namespace SlimECS
{
	public partial class Context
	{
		public static int DefaultCapacity = 256;

		private readonly string _name;

		private readonly IComponentDataList[] _components;
		private readonly EntityDataList _entities;

		public Context(string name)
		{
			_name = name;

			_entities = new EntityDataList(DefaultCapacity);

			var componentInfoList = ContextInfo.GetComponentInfoList();
			int count = componentInfoList.Length;

			_components = new IComponentDataList[count];
			for (int i = 0; i < count; i++)
				_components[i] = CreateComponentDataList(componentInfoList[i]);
		}

		public int Count => _entities.Count;
		public string Name => _name;

		public Entity Create(string name = null) => _entities.Create(name);
		public void Destroy(Entity e) => _entities.Destroy(e);

		public void SetName(Entity e, string name) => _entities.SetName(e, name);
		public bool Contains(Entity e) => _entities.Contains(e);

		public void Poll()
		{
			// TODO
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

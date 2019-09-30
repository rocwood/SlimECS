using System.Collections.Generic;

namespace microECS
{
	public interface IComponentList
	{
		IComponent Get(int index);

		void Set(int index, IComponent value);
		void Remove(int index);
	}

	public class ComponentList<T> : IComponentList where T:struct, IComponent
	{
		private List<T> _data = new List<T>();

		public T Get(int index)
		{
			// TODO check index
			return _data[index];
		}

		public void Set(int index, T value)
		{
			// TODO check index
			// TODO enlarge
			_data[index] = value;
		}

		IComponent IComponentList.Get(int index)
		{
			return Get(index);
		}

		void IComponentList.Set(int index, IComponent value)
		{
			Set(index, (T)value);
		}

		public void Remove(int index)
		{
			_data[index] = default;
		}
	}
}

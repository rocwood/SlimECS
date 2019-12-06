namespace SlimECS
{
	public class ZeroSizeComponentDataList<T> : IComponentDataList<T> where T : struct, IComponent
	{
		private StructArray<bool> _data;

		public ZeroSizeComponentDataList()
		{
			_data = new StructArray<bool>(0);
		}

		public bool Has(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			return _data[index];
		}

		public bool Get(int index, out T value)
		{
			value = default;

			return Has(index);
		}

		public void Set(int index, T value)
		{
			if (index < 0)
				return;

			_data.EnsureAccess(index);
			_data[index] = true;
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= _data.Length)
				return false;

			// TODO: onRemove

			_data[index] = false;
			return true;
		}
	}
}

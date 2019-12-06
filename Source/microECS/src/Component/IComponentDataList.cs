namespace microECS
{
	public interface IComponentDataList
	{
		bool Has(int index);
		bool Remove(int index);
	}

	public interface IComponentDataList<T> : IComponentDataList
	{
		bool Get(int index, out T value);
		void Set(int index, T value);
	}
}

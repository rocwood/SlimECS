namespace SlimECS
{
	class EmptyComponentRef<T> where T : struct, IComponent
	{
		public static T value;
	}
}

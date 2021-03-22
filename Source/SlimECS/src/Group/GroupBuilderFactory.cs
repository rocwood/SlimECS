namespace SlimECS
{
	public static class GroupBuilderFactory
	{
		public static GroupBuilder WithAll<T1>(this Context c) where T1 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1>.WithAll());
		public static GroupBuilder WithAll<T1, T2>(this Context c) where T1 : IComponent where T2 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent where T5 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5, T6>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent where T5 : IComponent where T6 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5, T6, T7>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent where T5 : IComponent where T6 : IComponent where T7 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6, T7>.WithAll());

		public static GroupBuilder WithAny<T1>(this Context c) where T1 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1>.WithAny());
		public static GroupBuilder WithAny<T1, T2>(this Context c) where T1 : IComponent where T2 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent where T5 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5, T6>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent where T5 : IComponent where T6 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5, T6, T7>(this Context c) where T1 : IComponent where T2 : IComponent where T3 : IComponent where T4 : IComponent where T5 : IComponent where T6 : IComponent where T7 : IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6, T7>.WithAny());
	}
}

namespace SlimECS
{
	public static class GroupBuilderFactory
	{
		public static GroupBuilder WithAll<T1>(this Context c) where T1: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1>.WithAll());
		public static GroupBuilder WithAll<T1, T2>(this Context c) where T1: struct, IComponent where T2: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5, T6>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6>.WithAll());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5, T6, T7>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent where T7: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6, T7>.WithAll());

		public static GroupBuilder WithAny<T1>(this Context c) where T1: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1>.WithAny());
		public static GroupBuilder WithAny<T1, T2>(this Context c) where T1: struct, IComponent where T2: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5, T6>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6>.WithAny());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5, T6, T7>(this Context c) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent where T7: struct, IComponent
			=> new GroupBuilder(c, MatcherCache<T1, T2, T3, T4, T5, T6, T7>.WithAny());
	}
}

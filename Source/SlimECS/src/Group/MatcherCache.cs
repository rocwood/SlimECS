using System.Collections.Generic;

namespace SlimECS
{
	class MatcherCache<T1>  where T1: struct, IComponent
	{
		private static IReadOnlyList<int> indices => ComponentIndexList<T1>.Get();

		private static Matcher _allCached;
		private static Matcher _anyCached;
		public static Matcher WithAll() => _allCached ?? (_allCached = new Matcher(true, indices, null, null));
		public static Matcher WithAny() => _anyCached ?? (_anyCached = new Matcher(true, null, indices, null));
	}
	class MatcherCache<T1, T2>  where T1: struct, IComponent where T2: struct, IComponent
	{
		private static IReadOnlyList<int> indices => ComponentIndexList<T1,T2>.Get();

		private static Matcher _allCached;
		private static Matcher _anyCached;
		public static Matcher WithAll() => _allCached ?? (_allCached = new Matcher(true, indices, null, null));
		public static Matcher WithAny() => _anyCached ?? (_anyCached = new Matcher(true, null, indices, null));
	}
	class MatcherCache<T1, T2, T3>  where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent
	{
		private static IReadOnlyList<int> indices => ComponentIndexList<T1,T2,T3>.Get();

		private static Matcher _allCached;
		private static Matcher _anyCached;
		public static Matcher WithAll() => _allCached ?? (_allCached = new Matcher(true, indices, null, null));
		public static Matcher WithAny() => _anyCached ?? (_anyCached = new Matcher(true, null, indices, null));
	}
	class MatcherCache<T1, T2, T3, T4>  where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent
	{
		private static IReadOnlyList<int> indices => ComponentIndexList<T1,T2,T3,T4>.Get();

		private static Matcher _allCached;
		private static Matcher _anyCached;
		public static Matcher WithAll() => _allCached ?? (_allCached = new Matcher(true, indices, null, null));
		public static Matcher WithAny() => _anyCached ?? (_anyCached = new Matcher(true, null, indices, null));
	}
	class MatcherCache<T1, T2, T3, T4, T5>  where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent
	{
		private static IReadOnlyList<int> indices => ComponentIndexList<T1,T2,T3,T4,T5>.Get();

		private static Matcher _allCached;
		private static Matcher _anyCached;
		public static Matcher WithAll() => _allCached ?? (_allCached = new Matcher(true, indices, null, null));
		public static Matcher WithAny() => _anyCached ?? (_anyCached = new Matcher(true, null, indices, null));
	}
	class MatcherCache<T1, T2, T3, T4, T5, T6>  where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent
	{
		private static IReadOnlyList<int> indices => ComponentIndexList<T1,T2,T3,T4,T5,T6>.Get();

		private static Matcher _allCached;
		private static Matcher _anyCached;
		public static Matcher WithAll() => _allCached ?? (_allCached = new Matcher(true, indices, null, null));
		public static Matcher WithAny() => _anyCached ?? (_anyCached = new Matcher(true, null, indices, null));
	}
	class MatcherCache<T1, T2, T3, T4, T5, T6, T7>  where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent where T7: struct, IComponent
	{
		private static IReadOnlyList<int> indices => ComponentIndexList<T1,T2,T3,T4,T5,T6,T7>.Get();

		private static Matcher _allCached;
		private static Matcher _anyCached;
		public static Matcher WithAll() => _allCached ?? (_allCached = new Matcher(true, indices, null, null));
		public static Matcher WithAny() => _anyCached ?? (_anyCached = new Matcher(true, null, indices, null));
	}
}

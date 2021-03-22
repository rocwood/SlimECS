using System.Collections.Generic;

namespace SlimECS
{
	class ComponentIndexListBase
	{
		protected static IReadOnlyList<int> Make(params int[] indices) => indices;

		protected static int idx<T>() where T: struct, IComponent => ContextInfo.GetIndexOf<T>();
	}

	class ComponentIndexList<T1> : ComponentIndexListBase where T1: struct, IComponent
	{
		private static IReadOnlyList<int> _cacheList;
		public static IReadOnlyList<int> Get() => _cacheList ?? (_cacheList = Make(idx<T1>()));
	}
	class ComponentIndexList<T1, T2> : ComponentIndexListBase where T1: struct, IComponent where T2: struct, IComponent
	{
		private static IReadOnlyList<int> _cacheList;
		public static IReadOnlyList<int> Get() => _cacheList ?? (_cacheList = Make(idx<T1>(), idx<T2>()));
	}
	class ComponentIndexList<T1, T2, T3> : ComponentIndexListBase where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent
	{
		private static IReadOnlyList<int> _cacheList;
		public static IReadOnlyList<int> Get() => _cacheList ?? (_cacheList = Make(idx<T1>(), idx<T2>(), idx<T3>()));
	}
	class ComponentIndexList<T1, T2, T3, T4> : ComponentIndexListBase where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent
	{
		private static IReadOnlyList<int> _cacheList;
		public static IReadOnlyList<int> Get() => _cacheList ?? (_cacheList = Make(idx<T1>(), idx<T2>(), idx<T3>(), idx<T4>()));
	}
	class ComponentIndexList<T1, T2, T3, T4, T5> : ComponentIndexListBase where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent
	{
		private static IReadOnlyList<int> _cacheList;
		public static IReadOnlyList<int> Get() => _cacheList ?? (_cacheList = Make(idx<T1>(), idx<T2>(), idx<T3>(), idx<T4>(), idx<T5>()));
	}
	class ComponentIndexList<T1, T2, T3, T4, T5, T6> : ComponentIndexListBase where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent
	{
		private static IReadOnlyList<int> _cacheList;
		public static IReadOnlyList<int> Get() => _cacheList ?? (_cacheList = Make(idx<T1>(), idx<T2>(), idx<T3>(), idx<T4>(), idx<T5>(), idx<T6>()));
	}
	class ComponentIndexList<T1, T2, T3, T4, T5, T6, T7> : ComponentIndexListBase where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent where T7: struct, IComponent
	{
		private static IReadOnlyList<int> _cacheList;
		public static IReadOnlyList<int> Get() => _cacheList ?? (_cacheList = Make(idx<T1>(), idx<T2>(), idx<T3>(), idx<T4>(), idx<T5>(), idx<T6>(), idx<T7>()));
	}
}

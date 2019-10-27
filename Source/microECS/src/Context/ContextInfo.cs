using System;

namespace microECS
{
	class ContextInfo
	{
		private Type[] _componentTypes;

		public Type[] GetComponentTypes()
		{
			if (_componentTypes == null)
			{


			}

			return _componentTypes;
		}

		public int GetIndexOf<T>() where T:struct, IComponent
		{

		}

		static class ComponentInfo<T> where T:struct, IComponent
		{
			public static int index;
			public static bool empty;
		}
	}
}

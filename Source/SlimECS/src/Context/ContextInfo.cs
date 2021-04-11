using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SlimECS
{
	public class ComponentTypeInfo
	{
		public readonly Type type;
		public readonly int index;
		public readonly bool zeroSize;

		public ComponentTypeInfo(Type type, int index, bool zeroSize)
		{
			this.type = type;
			this.index = index;
			this.zeroSize = zeroSize;
		}
	}

	public class ContextInfo
	{
		private static ComponentTypeInfo[] _componentInfoList;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ComponentTypeInfo[] GetComponentInfoList()
		{
			if (_componentInfoList == null)
				_componentInfoList = CollectComponents();

			return _componentInfoList;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetIndexOf<T>() where T : struct, IComponent
		{
			return ComponentTypeInfo<T>.index;

			/*
			var info = ComponentTypeInfo<T>.info;
			if (info == null)
			{
				if (_componentInfoList == null)
					_componentInfoList = CollectComponents();

				info = Array.Find(_componentInfoList, x => x.type == typeof(T));
				if (info == null)
					return -1;

				ComponentTypeInfo<T>.info = info;
			}

			return info.index;
			*/
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ComponentTypeInfo[] CollectComponents()
		{
			var baseType = typeof(IComponent);

			// TODO: skip some types via Attribute
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.Where(s => !s.FullName.StartsWith("System.") && !s.FullName.StartsWith("SlimECS."))
				.SelectMany(s => s.GetTypes())
				.Where(t => t.IsValueType && !t.IsPrimitive && t.IsPublic && baseType.IsAssignableFrom(t))
				.ToArray();

			Array.Sort(types, (x, y) => string.CompareOrdinal(x.FullName, y.FullName));

			var list = new ComponentTypeInfo[types.Length];
			for (int i = 0; i < types.Length; i++)
			{
				var t = types[i];
				var typeInfo = new ComponentTypeInfo(t, i, IsZeroSizeStruct(t));
				
				list[i] = typeInfo;

				var infoCacheType = typeof(ComponentTypeInfo<>).MakeGenericType(t);

				var fieldIndex = infoCacheType.GetField("index", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				fieldIndex.SetValue(null, i);

				var fieldZeroSize = infoCacheType.GetField("zeroSize", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				fieldZeroSize?.SetValue(null, typeInfo.zeroSize);
			}

			return list;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		// https://stackoverflow.com/a/27851610
		private static bool IsZeroSizeStruct(Type t)
		{
			return t.IsValueType && !t.IsPrimitive 
				&& t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
					.All(field => IsZeroSizeStruct(field.FieldType));
		}

		class ComponentTypeInfo<T> where T : struct, IComponent
		{
			internal static int index = -1;
			internal static bool zeroSize = false;
		}
	}
}

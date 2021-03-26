using System;
using System.Collections.Generic;

namespace SlimECS
{
	public static class GroupBuilderExtensions
	{
		public static IReadOnlyList<Entity> GetEntities(this GroupBuilder builder)
		{
			var group = builder.GetGroup();
			return group.GetEntities();
		}

		public static void GetEntities(this GroupBuilder builder, IList<Entity> output)
		{
			var group = builder.GetGroup();
			group.GetEntities(output);
		}

		public static void ForEach(this GroupBuilder builder, Action<Entity> func)
		{
			var group = builder.GetGroup();
			group.ForEach(func);
		}

		public static GroupBuilder WithAll<T1>(this GroupBuilder builder) where T1: struct, IComponent
			=> builder.WithAll(ComponentIndexList<T1>.Get());
		public static GroupBuilder WithAll<T1, T2>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent
			=> builder.WithAll(ComponentIndexList<T1, T2>.Get());
		public static GroupBuilder WithAll<T1, T2, T3>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent
			=> builder.WithAll(ComponentIndexList<T1, T2, T3>.Get());
		public static GroupBuilder WithAll<T1, T2, T3, T4>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent
			=> builder.WithAll(ComponentIndexList<T1, T2, T3, T4>.Get());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent
			=> builder.WithAll(ComponentIndexList<T1, T2, T3, T4, T5>.Get());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5, T6>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent
			=> builder.WithAll(ComponentIndexList<T1, T2, T3, T4, T5, T6>.Get());
		public static GroupBuilder WithAll<T1, T2, T3, T4, T5, T6, T7>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent where T7: struct, IComponent
			=> builder.WithAll(ComponentIndexList<T1, T2, T3, T4, T5, T6, T7>.Get());

		public static GroupBuilder WithAny<T1>(this GroupBuilder builder) where T1: struct, IComponent
			=> builder.WithAny(ComponentIndexList<T1>.Get());
		public static GroupBuilder WithAny<T1, T2>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent
			=> builder.WithAny(ComponentIndexList<T1, T2>.Get());
		public static GroupBuilder WithAny<T1, T2, T3>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent
			=> builder.WithAny(ComponentIndexList<T1, T2, T3>.Get());
		public static GroupBuilder WithAny<T1, T2, T3, T4>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent
			=> builder.WithAny(ComponentIndexList<T1, T2, T3, T4>.Get());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent
			=> builder.WithAny(ComponentIndexList<T1, T2, T3, T4, T5>.Get());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5, T6>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent
			=> builder.WithAny(ComponentIndexList<T1, T2, T3, T4, T5, T6>.Get());
		public static GroupBuilder WithAny<T1, T2, T3, T4, T5, T6, T7>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent where T7: struct, IComponent
			=> builder.WithAny(ComponentIndexList<T1, T2, T3, T4, T5, T6, T7>.Get());

		public static GroupBuilder WithNone<T1>(this GroupBuilder builder) where T1: struct, IComponent
			=> builder.WithNone(ComponentIndexList<T1>.Get());
		public static GroupBuilder WithNone<T1, T2>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent
			=> builder.WithNone(ComponentIndexList<T1, T2>.Get());
		public static GroupBuilder WithNone<T1, T2, T3>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent
			=> builder.WithNone(ComponentIndexList<T1, T2, T3>.Get());
		public static GroupBuilder WithNone<T1, T2, T3, T4>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent
			=> builder.WithNone(ComponentIndexList<T1, T2, T3, T4>.Get());
		public static GroupBuilder WithNone<T1, T2, T3, T4, T5>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent
			=> builder.WithNone(ComponentIndexList<T1, T2, T3, T4, T5>.Get());
		public static GroupBuilder WithNone<T1, T2, T3, T4, T5, T6>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent
			=> builder.WithNone(ComponentIndexList<T1, T2, T3, T4, T5, T6>.Get());
		public static GroupBuilder WithNone<T1, T2, T3, T4, T5, T6, T7>(this GroupBuilder builder) where T1: struct, IComponent where T2: struct, IComponent where T3: struct, IComponent where T4: struct, IComponent where T5: struct, IComponent where T6: struct, IComponent where T7: struct, IComponent
			=> builder.WithNone(ComponentIndexList<T1, T2, T3, T4, T5, T6, T7>.Get());
	}
}

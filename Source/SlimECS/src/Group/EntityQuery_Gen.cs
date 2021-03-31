using System;
using System.Collections.Generic;
using System.Text;

namespace SlimECS
{

	public class EntityQuery<T1, T2> : EntityQueryBase where T1 : struct, IComponent where T2 : struct, IComponent
	{
		internal EntityQuery(Context c) : base(c) => MakeIndices(idx<T1>(), idx<T2>());

		public class Without<X1> : EntityQueryBase where X1 : struct, IComponent
		{
			internal Without(Context c) : base(c) => MakeIndices(idx<T1>(), idx<T2>(), -idx<X1>());
		}
	}

	public struct EntityQueryBuilder<T1, T2> where T1 : struct, IComponent where T2 : struct, IComponent
	{
		private readonly Context _context;

		internal EntityQueryBuilder(Context c) => _context = c;

		public EntityQuery<T1, T2> GetQuery()
		{
			return new EntityQuery<T1, T2>(_context);
			//return _context.GetQuery(typeof(EntityQuery<T1, T2>));
		}

		public WithoutBuilder<X1> Without<X1>() where X1 : struct, IComponent
		{
			return new WithoutBuilder<X1>(_context);
		}

		public struct WithoutBuilder<X1> where X1 : struct, IComponent
		{
			private readonly Context _context;

			internal WithoutBuilder(Context c) => _context = c;

			public EntityQuery<T1, T2>.Without<X1> GetQuery()
			{
				return new EntityQuery<T1, T2>.Without<X1>(_context);
				//return _context.GetQuery(typeof(EntityQuery<T1, T2>));
			}
		}
	}

	/*
	public struct EntityQueryBuilderX1<T1, T2, X1>
	{
		private readonly Context _context;

		internal EntityQueryBuilderX1(Context c) => _context = c;

		public EntityQuery<T1, T2>.Without<X1> GetQuery()
		{
			return new EntityQuery<T1, T2>.Without<X1>();
			//return _context.GetQuery(typeof(EntityQuery<T1, T2>));
		}
	}
	*/
}

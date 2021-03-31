using System;
using System.Collections.Generic;
using System.Text;

namespace SlimECS
{
	public abstract class EntityQueryBase
	{
		internal int[] indices { get; private set; }

		internal void HandleEntity(Entity e)
		{
			
		}

		protected void MakeIndices(params int[] indices) => this.indices = indices;
		protected static int idx<T>() where T : struct, IComponent => ContextInfo.GetIndexOf<T>();

		protected EntityQueryBase(Context c) => _context = c;

		protected readonly Context _context;
	}
}

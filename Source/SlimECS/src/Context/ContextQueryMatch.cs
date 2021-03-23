using System.Collections.Generic;

namespace SlimECS
{
	public partial class Context
	{
		private bool HasAllComponents(Entity e, IReadOnlyList<int> indices)
		{
			for (int i = 0; i < indices.Count; i++)
			{
				var c = _components[indices[i]];
				if (c == null || !c.Has(e.id, e.slot))
					return false;
			}

			return true;
		}

		private bool HasAnyComponent(Entity e, IReadOnlyList<int> indices)
		{
			for (int i = 0; i < indices.Count; i++)
			{
				var c = _components[indices[i]];
				if (c != null && c.Has(e.id, e.slot))
					return true;
			}

			return false;
		}

		internal bool IsMatch(Entity e, Matcher matcher)
		{
			if (!Contains(e))
				return false;

			return (matcher.all == null || HasAllComponents(e, matcher.all))
				&& (matcher.any == null || HasAnyComponent(e, matcher.any))
				&& (matcher.none == null || !HasAnyComponent(e, matcher.none));
		}
	}
}

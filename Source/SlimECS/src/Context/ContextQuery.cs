using System.Collections.Generic;

namespace SlimECS
{
	public partial class Context
	{
		private readonly Dictionary<Matcher, Group> _groups = new Dictionary<Matcher, Group>();

		public Group GetGroup(Matcher matcher)
		{
			if (!_groups.TryGetValue(matcher, out var group))
			{
				group = new Group(this, matcher);


				for (int i = 0; i < _entities.Count; i++)
				{
					var entity = _entities[i];

					if (entity.isEnabled)
						group.HandleEntity(entity);
				}

				_groups.Add(matcher, group);
			}

			return group;
		}

		private void HandleGroupChanges()
		{
			int count = _entities.Count;

			for (int i = 0; i < count; i++)
			{
				var entity = _entities[i];

				if (!entity.isEnabled || entity.isModified)
				{
					foreach (var kv in _groups)
						kv.Value?.HandleEntity(entity);

					entity.ResetModified();
				}
			}
		}

		internal bool IsMatch(Entity e, Matcher matcher)
		{
			if (!IsActive(e))
				return false;

			return (matcher.all == null || HasAllComponents(e, matcher.all))
				&& (matcher.any == null || HasAnyComponent(e, matcher.any))
				&& (matcher.none == null || !HasAnyComponent(e, matcher.none));
		}
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
	}
}

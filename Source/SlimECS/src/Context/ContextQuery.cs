using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SlimECS
{
	public partial class Context
	{
		private readonly Dictionary<Type, EntityQuery> _queryMap = new Dictionary<Type, EntityQuery>();
		private readonly List<EntityQuery> _queries = new List<EntityQuery>();

		private readonly object[] _queryParams;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal EntityQuery GetQuery(Type queryType)
		{
			if (!_queryMap.TryGetValue(queryType, out var query))
			{
				if (!typeof(EntityQuery).IsAssignableFrom(queryType))
					return null;

				query = (EntityQuery)Activator.CreateInstance(queryType, BindingFlags.NonPublic|BindingFlags.Instance, null, _queryParams, CultureInfo.InvariantCulture);
				if (query == null)
					return null;

				_entities.ForEachActive(e => query.HandleEntity(e));

				_queryMap.Add(queryType, query);
				_queries.Add(query);
			}

			return query;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQuery<T>(out T query) where T : EntityQuery
		{
			query = (T)GetQuery(typeof(T));
		}

		/*
		internal Group GetGroup(Matcher matcher)
		{
			if (!_groups.TryGetValue(matcher, out var group))
			{
				group = new Group(this, matcher);

				_entities.ForEachActive(e => group.HandleEntity(e));

				_groups.Add(matcher, group);
			}

			return group;
		}
		*/

		private void HandleGroupChanges()
		{
			if (_handleGroupChangedFunc == null)
				_handleGroupChangedFunc = HandleGroupChangesImpl;

			_entities.ForEachChanged(_handleGroupChangedFunc);
			_entities.ResetChanged();
		}

		private Action<Entity> _handleGroupChangedFunc;

		private void HandleGroupChangesImpl(Entity e)
		{
			for (int i = 0; i < _queries.Count; i++)
				_queries[i].HandleEntity(e);
			
			/*
			foreach (var kv in _groups)
				kv.Value?.HandleEntity(e);
			*/
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool IsMatch(Entity e, IReadOnlyList<int> indices, bool matchAny)
		{
			return matchAny ? IsMatchAny(e, indices) : IsMatchAll(e, indices);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsMatchAll(Entity e, IReadOnlyList<int> indices)
		{
			if (!IsActive(e))
				return false;

			if (indices == null)
				return false;

			for (int i = 0; i < indices.Count; i++)
			{
				var index = indices[i];
				if (index >= 0)
				{
					var c = _components[index];
					if (c == null || !c.Has(e.id, e.slot))
						return false;
				}
				else
				{
					var c = _components[-index-1];
					if (c != null && c.Has(e.id, e.slot))
						return false;
				}
			}

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsMatchAny(Entity e, IReadOnlyList<int> indices)
		{
			if (!IsActive(e))
				return false;

			if (indices == null)
				return false;

			bool hasAny = false;

			for (int i = 0; i < indices.Count; i++)
			{
				var index = indices[i];
				if (index >= 0)
				{
					if (!hasAny)
					{
						var c = _components[indices[i]];
						if (c != null && c.Has(e.id, e.slot))
							hasAny = true;
					}
				}
				else
				{
					var c = _components[-index - 1];
					if (c != null && c.Has(e.id, e.slot))
						return false;
				}
			}

			return hasAny;
		}

		/*
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
		*/
	}
}

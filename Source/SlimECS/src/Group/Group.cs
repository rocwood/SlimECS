using System;
using System.Collections.Generic;

namespace SlimECS
{
	public class Group
	{
		private readonly Context _context;
		private readonly Matcher _matcher;
		
		private readonly SortedList<int, Entity> _entities = new SortedList<int, Entity>();

		//private readonly List<Entity> _entitiesCache = new List<Entity>();
		private bool _hasCached = false;

		//private readonly StructArray<Entity> _entitiesList = new StructArray<Entity>();
		//private readonly SortedDictionary<int, int> _entitiesMap = new SortedDictionary<int, int>();

		public int Count => _entities.Count;

		internal Group(Context context, Matcher matcher)
		{
			_context = context;
			_matcher = matcher;
		}

		internal void HandleEntity(Entity e)
		{
			if (e.id <= 0)
				return;

			if (_context.IsMatch(e, _matcher))
				HandleAddEntity(e);
			else
				HandleRemoveEntity(e);
		}

		private void HandleAddEntity(Entity e)
		{
			_entities.TryGetValue(e.id, out var item);
			if (e == item)
				return;

			_entities[e.id] = e;

			_hasCached = false;
		}

		private void HandleRemoveEntity(Entity e)
		{
			if (!_entities.Remove(e.id))
				return;
				
			_hasCached = false;
		}

		public bool Contains(Entity e)
		{
			if (e.id <= 0)
				return false;

			if (!_entities.TryGetValue(e.id, out var item))
				return false;

			return e == item;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(_entities);
		}

		public struct Enumerator
		{
			readonly IEnumerator<KeyValuePair<int, Entity>> _enumerator;

			internal Enumerator(SortedList<int, Entity> list)
			{
				_enumerator = list.GetEnumerator();
			}

			public Entity Current
			{
				get => _enumerator.Current.Value;
			}

			public bool MoveNext()
			{
				return _enumerator.MoveNext();
			}
		}

		/*
		public IReadOnlyList<Entity> GetEntities()
		{
			if (!_hasCached)
			{
				_entitiesCache.Clear();

				var values = _entities.Values;

				if (_entitiesCache.Capacity < values.Count)
					_entitiesCache.Capacity = values.Count;

				for (int i = 0; i < values.Count; i++)
					_entitiesCache.Add(values[i]);
			}

			return _entitiesCache;
		}

		public void GetEntities(IList<Entity> output)
		{
			output.Clear();

			var values = _entities.Values;

			for (int i = 0; i < values.Count; i++)
				output.Add(values[i]);
		}

		public void ForEach(Action<Entity> func)
		{
			if (func == null)
				return;

			foreach (var kv in _entities)
			{
				func(kv.Value);
			}

			//var values = _entities.Values;

			//for (int i = 0; i < values.Count; i++)
			//	func(values[i]);
		}
		*/

		public override string ToString()
		{
			if (_toStringCache == null)
				_toStringCache = $"Group({_matcher})";

			return _toStringCache;
		}

		private string _toStringCache;
	}
}

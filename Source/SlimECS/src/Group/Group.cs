using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace SlimECS
{
	public class Group
	{
		private const int DefaultCapacity = 16;

		private readonly Context _context;
		private readonly Matcher _matcher;

		private readonly Dictionary<int, int> _entityMap = new Dictionary<int, int>(DefaultCapacity);
		private Entity[] _entities = new Entity[DefaultCapacity];
		private int _count;

		public int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _count;
		}

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
			if (_entityMap.TryGetValue(e.id, out var index))
			{
				_entities[index] = e;
			}
			else
			{
				if (_count >= _entities.Length)
					ArrayHelper.EnsureLength(ref _entities, _count + 1);

				_entities[_count] = e;
				_entityMap[e.id] = _count;

				_count++;
			}
		}

		private void HandleRemoveEntity(Entity e)
		{
			if (_count <= 0)
				return;

			if (!_entityMap.TryGetValue(e.id, out var index))
				return;

			_entityMap.Remove(e.id);

			int last = _count - 1;
			if (index < last)
			{
				ref var ee = ref _entities[last];
				_entities[index] = ee;
				_entityMap[ee.id] = index;
			}

			_count--;
		}

		public bool Contains(Entity e)
		{
			if (e.id <= 0)
				return false;

			if (!_entityMap.TryGetValue(e.id, out var index))
				return false;

			return e == _entities[index];
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Entity GetAt(int index) => _entities[index];

		/*
		public IEnumerator<Entity> GetEnumerator()
		{
			return _entities.Values.GetEnumerator();
			//return new Enumerator(_entities);
		}
		*/

		/*
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
		*/

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

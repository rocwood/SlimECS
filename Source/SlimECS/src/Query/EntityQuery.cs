using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SlimECS
{
	public abstract class EntityQuery
	{
		private const int DefaultCapacity = 16;

		private readonly Dictionary<int, int> _entityMap = new Dictionary<int, int>(DefaultCapacity);
		private Entity[] _entities = new Entity[DefaultCapacity];
		private int _count;

		public int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Entity GetAt(int index) => _entities[index];

		internal void HandleEntity(Entity e)
		{
			if (e.id <= 0)
				return;

			if (_context.IsMatch(e, _indices, _matchAny))
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
				ref var other = ref _entities[last];
				_entities[index] = other;
				_entityMap[other.id] = index;
			}

			_count--;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void make(params int[] indices) => this._indices = indices;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static int inc<T>() where T : struct, IComponent => ContextInfo.GetIndexOf<T>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static int outc<T>() where T : struct, IComponent => -ContextInfo.GetIndexOf<T>()-1;

		protected EntityQuery(Context c, bool matchAny)
		{
			_context = c;
			_matchAny = matchAny;
		}

		protected readonly Context _context;
		protected readonly bool _matchAny;
		protected int[] _indices;
	}
}

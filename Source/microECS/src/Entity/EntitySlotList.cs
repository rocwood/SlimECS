using System.Collections.Generic;

namespace microECS
{
    internal class EntitySlotList
    {
        private readonly int _contextIdMask;

        private int _modFactor = 997;
        private int _lastId = 0;

        private readonly List<Entity> _entitySlots;

        internal EntitySlotList(int contextIdMask)
        {
            _contextIdMask = contextIdMask;
            _entitySlots = new List<Entity>(_modFactor + 1);
        }

        public Entity Create()
        {
            _lastId++;

            var id = _lastId;
            int slot = id % _modFactor; // TODO

            // TODO
            var e = _entitySlots[slot];
            if (e.id == 0)
            {
                e = new Entity(id, slot | _contextIdMask);
                _entitySlots[slot] = e;
                return e;
            }

            slot = _entitySlots.FindIndex(e => e.id == 0);
            if (slot > 0)
            {
                e = new Entity(id, slot | _contextIdMask);
                _entitySlots[slot] = e;
                return e;
            }

            // TODO enlarge
            slot = _entitySlots.Count;
            e = new Entity(id, slot | _contextIdMask);
            _entitySlots.Add(e);

            return e;
        }

        public bool Destroy(Entity e)
        {
			if (!IsValid(e))
				return false;

			int slot = e.slot & Entity.slotMask;
			_entitySlots[slot] = Entity.Empty;

			return true;
		}

        public bool IsValid(Entity e)
        {
			if (e.id <= 0 || e.slot <= 0 || (e.slot & Entity.contextIdMask) != _contextIdMask)
				return false;

			int slot = e.slot & Entity.slotMask;
			if (slot >= _entitySlots.Count)
                return false;

            return _entitySlots[slot] == e;
        }
    }
}

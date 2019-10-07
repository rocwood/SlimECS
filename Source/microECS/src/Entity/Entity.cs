using System;

namespace microECS
{
    public struct Entity : IEquatable<Entity>
    {
		internal const int contextIdBits = 7;
		internal const int contextIdMax = 1 << contextIdBits;
		internal const int contextIdMask = (contextIdMax - 1) << slotBits;

		internal const int slotBits = 31 - contextIdBits;
		internal const int slotMax = 1 << slotBits;
		internal const int slotMask = slotMax - 1;

		public static Entity Empty = new Entity();

		public readonly int id;
        internal readonly int slot; // 1 bit sign, 7 bits contextId, 24 bits slot-index

        internal Entity(int id, int slot)
        {
            this.id = id;
            this.slot = slot;
        }

        public override bool Equals(object obj)
        {
            return obj is Entity entity && Equals(entity);
        }

        public bool Equals(Entity other)
        {
            return id == other.id && slot == other.slot;
        }

        public override int GetHashCode()
        {
			// https://referencesource.microsoft.com/#System.Numerics/System/Numerics/HashCodeHelper.cs
			return ((id << 5) + id) ^ slot;

			// https://programmingpraxis.com/2018/06/19/fibonacci-hash/
			//return (int)(id * 2654435769L + slot);

			/*
			// automatically generated hash
			var hashCode = -528825007;
            hashCode = hashCode * -1521134295 + id;
            hashCode = hashCode * -1521134295 + slot;
            return hashCode;
			*/
		}

		public static bool operator ==(Entity left, Entity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}

using System;

namespace SlimECS
{
    public struct Entity : IEquatable<Entity>
    {
		public const int contextIdBits = 7;
		public const int contextIdMax = 1 << contextIdBits;
		public const int contextIdMask = (contextIdMax - 1) << slotBits;
		
		public const int slotBits = 31 - contextIdBits;
		public const int slotMax = 1 << slotBits;
		public const int slotMask = slotMax - 1;

		public static readonly Entity Empty = new Entity();

		public readonly int id;
		internal readonly int location; // 1 bit sign, 7 bits contextId, 24 bits slot-index

		internal int slot => location & slotMask;
		internal int contextIdShift => location & contextIdMask;

		internal Entity(int id, int slot, int contextIdShift)
        {
            this.id = id;
            this.location = slot | contextIdShift;
        }

        public override bool Equals(object obj)
        {
            return obj is Entity entity && Equals(entity);
        }

        public bool Equals(Entity other)
        {
            return id == other.id && location == other.location;
        }

        public override int GetHashCode()
        {
			// https://referencesource.microsoft.com/#System.Numerics/System/Numerics/HashCodeHelper.cs
			return ((id << 5) + id) ^ location;

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

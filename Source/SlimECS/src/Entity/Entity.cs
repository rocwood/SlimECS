using System;

namespace SlimECS
{
    public struct Entity : IEquatable<Entity>
    {
		public static readonly Entity Empty = new Entity();

		public readonly int id;
		internal readonly int slot;

		internal Entity(int id, int slot)
        {
            this.id = id;
            this.slot = slot;
        }

        public override bool Equals(object obj) => obj is Entity entity && Equals(entity);
        public bool Equals(Entity other) => id == other.id && slot == other.slot;
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

		public static bool operator ==(Entity left, Entity right) => left.Equals(right);
        public static bool operator !=(Entity left, Entity right) => !left.Equals(right);
    }
}

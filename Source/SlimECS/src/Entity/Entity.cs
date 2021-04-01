using System;
using System.Runtime.CompilerServices;

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public override bool Equals(object obj) => obj is Entity entity && Equals(entity);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public bool Equals(Entity other) => id == other.id && slot == other.slot;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static bool operator ==(Entity left, Entity right) => left.id == right.id && left.slot == right.slot;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] 
		public static bool operator !=(Entity left, Entity right) => left.id != right.id || left.slot != right.slot;

		// https://referencesource.microsoft.com/#System.Numerics/System/Numerics/HashCodeHelper.cs
		public override int GetHashCode() => ((id << 5) + id) ^ slot;
	}
}

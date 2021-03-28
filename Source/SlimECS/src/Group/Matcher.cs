using System.Collections.Generic;
//using SlimECS.Utils;

namespace SlimECS
{
	class Matcher
	{
		internal List<int> all;
		internal List<int> any;
		internal List<int> none;

		internal bool isSealed { get; private set; }

		internal Matcher(bool isSealed, IReadOnlyList<int> all, IReadOnlyList<int> any, IReadOnlyList<int> none)
		{
			WithAll(all);
			WithAny(any);
			WithNone(none);

			if (isSealed)
				MakeSealed();
		}

		internal Matcher Clone()
		{
			return new Matcher(false, all, any, none);
		}

		internal void WithAll(IReadOnlyList<int> indices) => Add(ref all, indices);
		internal void WithAny(IReadOnlyList<int> indices) => Add(ref any, indices);
		internal void WithNone(IReadOnlyList<int> indices) => Add(ref none, indices);

		internal Matcher MakeSealed()
		{
			if (!isSealed)
			{
				ComputeHashCode();
				isSealed = true;
			}

			return this;
		}

		private void Add(ref List<int> list, IReadOnlyList<int> indices)
		{
			if (isSealed)
				return;

			if (indices == null)
				return;

			if (list == null)
				list = new List<int>(indices.Count);

			list.AddDistinctSorted(indices);
		}

		/*
		public bool Matches(Context context, Entity entity)
		{
			return (all == null || entity.HasAllComponents(all))
				&& (any == null || entity.HasAnyComponent(any))
				&& (none == null || !entity.HasAnyComponent(none));
		}
		*/

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
				return false;

			return Equals((Matcher)obj);
		}

		public bool Equals(Matcher other)
		{
			if (other == null || other.GetHashCode() != GetHashCode())
				return false;

			if (!all.CheckEquals(other.all)) return false;
			if (!any.CheckEquals(other.any)) return false;
			if (!none.CheckEquals(other.none)) return false;

			return true;
		}

		private void ComputeHashCode()
		{
			var hashCode = -80052522;

			hashCode = all.ComputeHashCode(hashCode, -1521134295);
			hashCode = any.ComputeHashCode(hashCode, -1521134295);
			hashCode = none.ComputeHashCode(hashCode, -1521134295);

			_hash = hashCode;
		}

		public override int GetHashCode() => _hash;

		private int _hash;
	}
}

using System.Collections.Generic;

namespace microECS
{
	static class ListExtensions
	{
		internal static void Enlarge<T>(this List<T> list, int count, T fillValue = default(T))
		{
			if (list == null)
				return;

			if (count < 0)
				count = 0;

			int oldCount = list.Count;
			if (oldCount >= count)
				return;

			if (list.Capacity < count)
				list.Capacity = count;

			while (list.Count < count)
				list.Add(fillValue);
		}

		internal static void Resize<T>(this List<T> list, int count, T fillValue = default(T))
		{
			if (list == null)
				return;

			if (count < 0)
				count = 0;

			int oldCount = list.Count;
			if (oldCount > count)
			{
				list.RemoveRange(count, oldCount - count);
			}
			else if (oldCount < count)
			{
				if (list.Capacity < count)
					list.Capacity = count;

				while (list.Count < count)
					list.Add(fillValue);
			}
		}
	}
}

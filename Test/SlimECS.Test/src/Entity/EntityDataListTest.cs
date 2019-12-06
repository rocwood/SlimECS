using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SlimECS.Test
{
	public class EntityDataListTest
	{
		[Test]
		public void Test1()
		{
			int contextId = 1;
			int contextIdShift = contextId << Entity.slotBits;

			var list = new EntityDataList(contextIdShift);


			var e1 = list.Create(null);
			Assert.AreEqual(1, list.Count);
			Assert.IsTrue(list.Contains(e1));

			var e2 = list.Create(null);
			Assert.AreEqual(2, list.Count);
			Assert.IsTrue(list.Contains(e2));

			var e3 = list.Create(null);
			Assert.AreEqual(3, list.Count);
			Assert.IsTrue(list.Contains(e3));

			list.Destroy(e2);
			Assert.AreEqual(2, list.Count);
			Assert.IsFalse(list.Contains(e2));
			Assert.IsTrue(list.Contains(e1));
			Assert.IsTrue(list.Contains(e3));

			list.Destroy(e2);
			Assert.AreEqual(2, list.Count);
			Assert.IsFalse(list.Contains(e2));

			var e4 = list.Create(null);
			Assert.AreEqual(3, list.Count);
			Assert.IsTrue(list.Contains(e4));

			list.Destroy(e1);
			list.Destroy(e3);
			Assert.AreEqual(1, list.Count);
			Assert.IsFalse(list.Contains(e1));
			Assert.IsFalse(list.Contains(e3));
			Assert.IsTrue(list.Contains(e4));
		}

		[Test]
		public void Test2()
		{
			int contextId = 2;
			int contextIdShift = contextId << Entity.slotBits;

			var list = new EntityDataList(contextIdShift);

			var random = new Random(100);
			var world = new List<Entity>(1024);

			for (int i = 0; i < 10000; i++)
			{
				int job = random.Next() % 6;

				switch (job)
				{
					case 0:
						// delete last entity
						if (world.Count > 0)
						{
							int index = world.Count - 1;
							var e = world[index];
							world.RemoveAt(index);
							list.Destroy(e);
						}
						break;
					case 1:
						// delete random entity
						if (world.Count > 0)
						{
							int index = random.Next(world.Count);
							var e = world[index];
							world.RemoveAt(index);
							list.Destroy(e);
						}
						break;
					default:
						{
							// create new entity
							var e = list.Create(null);
							world.Add(e);
						}
						break;
				}
			}

			Assert.AreEqual(world.Count, list.Count);

			foreach (var e in world)
			{
				Assert.IsTrue(list.Contains(e));
			}
		}
	}
}

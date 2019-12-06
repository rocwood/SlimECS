using NUnit.Framework;

namespace microECS.Test
{
	public class ComponentDataListTest
	{
		private Position empty = new Position();
		private Position c1 = new Position { pos = new Vector3() { x = 1, y = 2, z = 3 } };
		private Position c2 = new Position { pos = new Vector3() { x = 0, y = 0, z = 999 } };

		[Test]
		public void Test1()
		{
			var list = new ComponentDataList<Position>();

			Position p1;

			Assert.IsFalse(list.Has(1));
			Assert.IsFalse(list.Get(1, out p1));

			list.Set(1, c1);
			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));
			Assert.AreEqual(c1, p1);

			list.Set(1, empty);
			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));
			Assert.AreEqual(empty, p1);

			list.Set(1, c2);
			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));
			Assert.AreEqual(c2, p1);

			list.Remove(1);
			Assert.IsFalse(list.Has(1));
			Assert.IsFalse(list.Get(1, out p1));
		}

		[Test]
		public void Test2()
		{
			var list = new ComponentDataList<Position>();

			Position p1, p2;

			list.Set(1, c1);
			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));
			Assert.AreEqual(c1, p1);

			list.Set(1000, c2);
			Assert.IsTrue(list.Has(1000));
			Assert.IsTrue(list.Get(1000, out p2));
			Assert.AreEqual(c2, p2);

			Assert.AreNotEqual(p1, p2);

			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));
			Assert.AreEqual(c1, p1);

			list.Remove(1);
			Assert.IsFalse(list.Has(1));
			Assert.IsFalse(list.Get(1, out p1));

			Assert.IsTrue(list.Has(1000));
			Assert.IsTrue(list.Get(1000, out p2));
			Assert.AreEqual(c2, p2);
		}
	}
}

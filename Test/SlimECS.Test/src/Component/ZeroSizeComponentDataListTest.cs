using NUnit.Framework;

namespace SlimECS.Test
{
	public class ZeroSizeComponentDataListTest
	{
		private Frozen c = new Frozen();

		[Test]
		public void Test1()
		{
			var list = new ZeroSizeComponentDataList<Frozen>();

			Frozen p1;

			Assert.IsFalse(list.Has(1));
			Assert.IsFalse(list.Get(1, out p1));

			list.Set(1, c);
			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));

			list.Remove(1);
			Assert.IsFalse(list.Has(1));
			Assert.IsFalse(list.Get(1, out p1));
		}

		[Test]
		public void Test2()
		{
			var list = new ZeroSizeComponentDataList<Frozen>();

			Frozen p1, p2;

			list.Set(1, c);
			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));

			list.Set(1000, c);
			Assert.IsTrue(list.Has(1000));
			Assert.IsTrue(list.Get(1000, out p2));

			Assert.IsTrue(list.Has(1));
			Assert.IsTrue(list.Get(1, out p1));

			list.Remove(1);
			Assert.IsFalse(list.Has(1));
			Assert.IsFalse(list.Get(1, out p1));

			Assert.IsTrue(list.Has(1000));
			Assert.IsTrue(list.Get(1000, out p2));
		}
	}
}

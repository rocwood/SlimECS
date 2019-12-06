using NUnit.Framework;

namespace SlimECS.Test
{
	public class StructArrayTest
	{
		[Test]
		public void Test1()
		{
			var array = new StructArray<int>();
			Assert.AreEqual(0, array.Length);

			array.EnsureAccess(10);
			Assert.Greater(array.Length, 10);

			array.EnsureAccess(100);
			Assert.Greater(array.Length, 100);

			array.EnsureAccess(300);
			Assert.Greater(array.Length, 300);

			array.EnsureAccess(1000);
			Assert.Greater(array.Length, 1000);
		}

		[Test]
		public void Test2()
		{
			var array = new StructArray<int>(20);
			Assert.AreEqual(20, array.Length);

			array.EnsureAccess(30);
			Assert.Greater(array.Length, 30);

			array.EnsureAccess(100);
			Assert.Greater(array.Length, 100);

			array.EnsureAccess(300);
			Assert.Greater(array.Length, 300);

			array.EnsureAccess(1000);
			Assert.Greater(array.Length, 1000);
		}

		[Test]
		public void Test3()
		{
			var array = new StructArray<int>();
			Assert.AreEqual(0, array.Length);

			array.EnsureAccess(30);
			Assert.Greater(array.Length, 30);

			array[20] = -1;
			array[30] = 999;

			Assert.AreEqual(-1, array[20]);
			Assert.AreEqual(999, array[30]);

			ref int d = ref array.Ref(30);
			Assert.AreEqual(999, d);

			d = -999;
			Assert.AreEqual(-999, array[30]);
		}
	}
}

using NUnit.Framework;

namespace microECS.Test
{
	public class ContextInfoTest
	{
		[Test]
		public void Test1()
		{
			var list = ContextInfo.GetComponentInfoList();

			Assert.Greater(list.Length, 0);

			var info1 = ContextInfo.GetComponentInfo<Position>();
			Assert.NotNull(info1);
			Assert.AreEqual(typeof(Position), info1.type);
			Assert.GreaterOrEqual(info1.index, 0);
			Assert.IsFalse(info1.zeroSize);

			info1 = ContextInfo.GetComponentInfo<Velocity>();
			Assert.NotNull(info1);
			Assert.AreEqual(typeof(Velocity), info1.type);
			Assert.GreaterOrEqual(info1.index, 0);
			Assert.IsFalse(info1.zeroSize);

			info1 = ContextInfo.GetComponentInfo<Frozen>();
			Assert.NotNull(info1);
			Assert.AreEqual(typeof(Frozen), info1.type);
			Assert.GreaterOrEqual(info1.index, 0);
			Assert.IsTrue(info1.zeroSize);

			info1 = ContextInfo.GetComponentInfo<ComplexStatus>();
			Assert.NotNull(info1);
			Assert.AreEqual(typeof(ComplexStatus), info1.type);
			Assert.GreaterOrEqual(info1.index, 0);
			Assert.IsTrue(info1.zeroSize);
		}
	}
}

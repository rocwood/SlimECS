using NUnit.Framework;

namespace SlimECS.Test
{
	public class ContextTest
	{
		[Test]
		public void Test1()
		{
			int contextId = 1;
			var context = new Context(contextId, $"Context{contextId}");

			Assert.AreEqual(0, context.Count);

			var e1 = context.CreateEntity();
			Assert.AreEqual(1, context.Count);
			Assert.IsTrue(context.IsActive(e1));

			var e2 = context.CreateEntity();
			Assert.AreEqual(2, context.Count);
			Assert.IsTrue(context.IsActive(e2));

			var e3 = context.CreateEntity();
			Assert.AreEqual(3, context.Count);
			Assert.IsTrue(context.IsActive(e3));

			context.Destroy(e2);
			Assert.AreEqual(2, context.Count);
			Assert.IsFalse(context.IsActive(e2));
			Assert.IsTrue(context.IsActive(e1));
			Assert.IsTrue(context.IsActive(e3));

			context.Destroy(e2);
			Assert.AreEqual(2, context.Count);
			Assert.IsFalse(context.IsActive(e2));

			var e4 = context.CreateEntity();
			Assert.AreEqual(3, context.Count);
			Assert.IsTrue(context.IsActive(e4));

			context.Destroy(e1);
			context.Destroy(e3);
			Assert.AreEqual(1, context.Count);
			Assert.IsFalse(context.IsActive(e1));
			Assert.IsFalse(context.IsActive(e3));
			Assert.IsTrue(context.IsActive(e4));
		}

		private Position empty = new Position();
		private Position cp1 = new Position { pos = new Vector3() { x = 1, y = 2, z = 3 } };
		private Position cp2 = new Position { pos = new Vector3() { x = 0, y = 0, z = 999 } };
		private Velocity cv1 = new Velocity { dir = new Vector3() { x = 0, y = 1, z = 0 }, speed = 1 };

		[Test]
		public void Test2()
		{
			int contextId = 2;
			var context = new Context(contextId, $"Context{contextId}");

			var e1 = context.CreateEntity();

			Assert.IsTrue(context.IsActive(e1));

			context.SetComponent<Position>(e1, cp1);
			context.SetComponent<Velocity>(e1, cv1);

			Assert.IsTrue(context.HasComponent<Position>(e1));
			Assert.IsTrue(context.HasComponent<Velocity>(e1));
			Assert.IsFalse(context.HasComponent<Frozen>(e1));

			Position p1, p2;
			Velocity v1;

			Assert.IsTrue(context.GetComponent<Position>(e1, out p1));
			Assert.IsTrue(context.GetComponent<Velocity>(e1, out v1));

			Assert.AreEqual(cp1, p1);
			Assert.AreEqual(cv1, v1);

			p1.pos += v1.dir * v1.speed;
			context.SetComponent<Position>(e1, p1);

			Assert.IsTrue(context.GetComponent<Position>(e1, out p2));
			Assert.AreEqual(p1, p2);
		}
	}
}

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SlimECS;

namespace ECS.Benchmark
{
	public struct Position : IComponent //, IModifiable
	{
		public float x;
		public float y;
	}

	public struct Velocity : IComponent
	{
		public float x;
		public float y;
	}

	public class MovementSystem : SystemBase
	{
		private const float axisBound = 100;

		private EntitySet query;

		public override void Execute()
		{
			if (query == null)
				query = FakeContext.pvQuery;

			//var items = query._items;

			//for (int i = 0; i < query.Count; i++)
			foreach (var e in query)
			{
				//var e = items[i];
				//var e = query.GetAt(i);

				//ref var v = ref FakeContext.velocityComponents[e.slot];
				//ref var pos = ref FakeContext.positionComponents[e.slot];
				ref var v = ref FakeContext.velocityComponents.items[e.slot];
				ref var pos = ref FakeContext.positionComponents.items[e.slot];

				pos.x += v.x;
				pos.y += v.y;

				// check bound, and reflect velocity
				if ((v.x < 0 && pos.x < -axisBound) ||
					(v.x > 0 && pos.x > axisBound))
					v.x = -v.x;
				if ((v.y < 0 && pos.y < -axisBound) ||
					(v.y > 0 && pos.y > axisBound))
					v.y = -v.y;
			}
		}
	}

	static class FakeContext
	{
		//public static Position[] positionComponents;
		//public static Velocity[] velocityComponents;
		public static StructDataPool<Position> positionComponents;
		public static StructDataPool<Velocity> velocityComponents;
		public static EntitySet pvQuery;
		//public static Entity[] pvQuery0;
	}

	public class BenchmarkCase
	{
		private const float axisBound = 100;
		private const float maxAxisSpeed = 20;

		private const int initialEntityCount = 1000;
		private const int iterateCount = 10000;

		private SystemBase system;

		public void Init()
		{
			//FakeContext.positionComponents = new Position[initialEntityCount];
			//FakeContext.velocityComponents = new Velocity[initialEntityCount];
			FakeContext.positionComponents = new StructDataPool<Position>(initialEntityCount);
			FakeContext.velocityComponents = new StructDataPool<Velocity>(initialEntityCount);
			FakeContext.pvQuery = new EntitySet(initialEntityCount);
			//FakeContext.pvQuery0 = new Entity[initialEntityCount];

			system = new MovementSystem();

			var random = new Random(1);

			for (int i = 0; i < initialEntityCount; i++)
			{
				float x = ((float)random.NextDouble() - 0.5f) * axisBound;
				float y = ((float)random.NextDouble() - 0.5f) * axisBound;
				//FakeContext.positionComponents[i] = new Position { x = x, y = y };
				var ii = FakeContext.positionComponents.Alloc();
				FakeContext.positionComponents.items[ii] = new Position { x = x, y = y };

				float vx = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
				float vy = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
				//FakeContext.velocityComponents[i] = new Velocity { x = vx, y = vy };
				ii = FakeContext.velocityComponents.Alloc();
				FakeContext.velocityComponents.items[ii] = new Velocity { x = vx, y = vy };

				var e = new Entity(i + 1, i, null);
				FakeContext.pvQuery.Add(e);
				//FakeContext.pvQuery0[i] = e;
			}
		}

		public void Execute()
		{
			for (int i = 0; i < iterateCount; i++)
			{
				system.Execute();
			}
		}

		public void Cleanup()
		{
			//ref var pos = ref FakeContext.positionComponents[0];
			ref var pos = ref FakeContext.positionComponents.items[0];
			Console.WriteLine($"e0({pos.x},{pos.y})");
		}
	}

	public class Program
	{
		static void Main(string[] args)
		{
			var sw = new Stopwatch();
			var benchmark = new BenchmarkCase();

			var mem0 = GC.GetTotalMemory(false);

			sw.Start();
			benchmark.Init();
			sw.Stop();
			var initTime = sw.ElapsedMilliseconds;

			var mem1 = GC.GetTotalMemory(false);

			sw.Restart();
			benchmark.Execute();
			sw.Stop();
			var execTime = sw.ElapsedMilliseconds;

			sw.Restart();
			benchmark.Cleanup();
			sw.Stop();
			var cleanupTime = sw.ElapsedMilliseconds;

			var mem2 = GC.GetTotalMemory(false);

			Console.WriteLine($"Init = {initTime}ms, {(mem1 - mem0) / 1024}KB\nExec = {execTime}ms, {(mem2 - mem1) / 1024}KB\nClean = {cleanupTime}");
		}
	}
}

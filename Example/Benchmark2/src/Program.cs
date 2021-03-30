using System;
using System.Diagnostics;
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

		public static Group query;

		public override void Execute()
		{
			//var query = context.WithAll<Position, Velocity>().GetGroup();
			//if (query == null)
			//	query = context.WithAll<Position, Velocity>().GetGroup();

			//foreach (var e in query)

			int count = query.Count;
			for (int i = 0; i < count; i++)
			{
				var e = query.GetAt(i);

				ref var v = ref context.Ref<Velocity>(e);
				ref var pos = ref context.Ref<Position>(e);

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

	public class BenchmarkCase
	{
		private const float axisBound = 100;
		private const float maxAxisSpeed = 20;

		private const int initialEntityCount = 1000;
		private const int iterateCount = 10000;

		private Context context;
		private SystemManager systems;

		public void Init()
		{
			context = new Context("Default");

			systems = new SystemManager(context);
			systems.CollectAll();

			var random = new Random(1);

			for (int i = 0; i < initialEntityCount; i++)
			{
				var child = context.Create();

				float x = ((float)random.NextDouble() - 0.5f) * axisBound;
				float y = ((float)random.NextDouble() - 0.5f) * axisBound;
				context.Set(child, new Position { x = x, y = y });

				float vx = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
				float vy = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
				context.Set(child, new Velocity { x = vx, y = vy });

				if (i == 0)
					e0 = child;
			}

			MovementSystem.query = 
			context.WithAll<Position, Velocity>().GetGroup();
			context.Poll();
		}

		public void Execute()
		{
			for (int i = 0; i < iterateCount; i++)
			{
				systems.Execute();
			}
		}

		private Entity e0;

		public void Cleanup()
		{
			ref var pos = ref context.Ref<Position>(e0);
			Console.WriteLine($"e0({pos.x},{pos.y})");

			systems = null;
			context = null;
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

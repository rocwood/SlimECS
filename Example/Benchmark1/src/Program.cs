using System;
using System.Diagnostics;

namespace SlimECS.Benchmark
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

	public struct LifeTime : IComponent
	{
		public int id;
		public int ticks;
	}

	public class MovementSystem : SystemBase
	{
		private float axisBound = 100;

		private Group query;

		public override void Execute()
		{
			if (query == null)
				query = context.WithAll<Position, Velocity>().GetGroup();

			foreach (var e in query)
			{
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

	public class LifeTimeSystem : SystemBase
	{
		private const int initChildCount = 1000;
		private const int minChildCount = -3;
		private const int maxChildCount = 4;
		private const int maxChildLifeTime = 1000;
		private const float maxAxisSpeed = 20;

		private Group query;

		public override void Execute()
		{
			if (query == null) 
				query = context.WithAll<Position, LifeTime>().GetGroup();

			foreach (var e in query)
			{
				ref var lifeTime = ref context.Ref<LifeTime>(e);

				if (lifeTime.ticks-- > 0)
				{
					//context.Set(e, lifeTime);
					continue;
				}

				ref var pos = ref context.Ref<Position>(e);

				var random = new Random(lifeTime.id);

				var childCount = (lifeTime.id == 1)
					? initChildCount
					: random.Next(minChildCount, maxChildCount);

				for (int i = 0; i < childCount; i++)
					Spawn(pos.x, pos.y, random);

				context.Destroy(e);
			}
		}

		private void Spawn(float x, float y, Random random)
		{
			var child = context.Create();

			context.Set(child, new LifeTime { id = random.Next(1000, int.MaxValue), ticks = random.Next(1, maxChildLifeTime) });
			context.Set(child, new Position { x = x, y = y });

			float vx = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
			float vy = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
			context.Set(child, new Velocity { x = vx, y = vy });
		}
	}

	public class BenchmarkCase
	{
		private Context context;
		private SystemManager systems;

		public void Init()
		{
			context = new Context("Default");

			var e = context.Create();

			context.Set(e, new Position());				// x = y = 0, without Velocity
			context.Set(e, new LifeTime { id = 1 });	// ticks = 0

			systems = new SystemManager(context);
			systems.CollectAll();
		}

		public int frameId { get; private set; }

		public void Execute()
		{
			frameId = 0;

			while (context.Count > 0)
			{
				systems.Execute();

				frameId++;
			}
		}

		public void Cleanup()
		{
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

			Console.WriteLine($"Frame = {benchmark.frameId}\n");
			Console.WriteLine($"Init = {initTime}ms, {(mem1 - mem0) / 1024}KB\nExec = {execTime}ms, {(mem2 - mem1) / 1024}KB\nClean = {cleanupTime}");
		}
	}
}

using System;
using System.Collections.Generic;
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
		public int ticks;
	}

	public class MovementSystem : SystemBase
	{
		private float axisBound = 100;

		public override void Execute()
		{
			//var entities = context.WithAll<Position, Velocity>().GetEntities();
			IReadOnlyList<Entity> entities = null;

			foreach (var e in entities)
			{
				var v = context.GetComponent<Velocity>(e);
				var pos = context.GetComponent<Position>(e);

				pos.x += v.x;
				pos.y += v.y;

				// check bound, and reflect velocity
				if ((v.x < 0 && pos.x < -axisBound) ||
					(v.x > 0 && pos.x > axisBound))
					v.x = -v.x;
				if ((v.y < 0 && pos.y < -axisBound) ||
					(v.y > 0 && pos.y > axisBound))
					v.y = -v.y;

				context.SetComponent(e, pos);
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

		public override void Execute()
		{
			//var entities = context.WithAll<Position, LifeTime>().GetEntities();

			IReadOnlyList<Entity> entities = null;

			foreach (var e in entities)
			{
				var lifeTime = context.GetComponent<LifeTime>(e);

				if (lifeTime.ticks-- > 0)
				{
					context.SetComponent(e, lifeTime);
					continue;
				}

				var pos = context.GetComponent<Position>(e);

				var random = new Random(e.id);

				var childCount = (e.id == 1) 
					? initChildCount 
					: random.Next(minChildCount, maxChildCount);

				for (int i = 0; i < childCount; i++)
					Spawn(pos.x, pos.y, random);

				context.Destroy(e);
			}
		}

		private void Spawn(float x, float y, Random random)
		{
			var child = context.CreateEntity();

			context.SetComponent(child, new LifeTime { ticks = random.Next(1, maxChildLifeTime) });
			context.SetComponent(child, new Position { x = x, y = y });

			float vx = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
			float vy = ((float)random.NextDouble() - 0.5f) * maxAxisSpeed;
			context.SetComponent(child, new Velocity { x = vx, y = vy });
		}
	}

	public class BenchmarkCase
	{
		private Context context;
		private SystemManager systems;

		public void Init()
		{
			context = new Context(0, "Default");

			var e = context.CreateEntity();

			context.SetComponent(e, new Position()); // x = y = 0, without Velocity
			context.SetComponent(e, new LifeTime()); // ticks = 0

			systems = new SystemManager(context);
			systems.CollectAll();
		}

		public int frameId { get; private set; }

		public void Execute()
		{
			frameId = 0;

			return;

			while (context.Count > 0)
			{
				//Dump();

				systems.Execute();
				//systems.Cleanup();

				frameId++;
			}
		}

		public void Cleanup()
		{
			//systems.TearDown();
			//context.Reset();

			systems = null;
			context = null;

			//Contexts.DestroyInstance();
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

			//File.WriteAllText("DumpResult.txt", benchmark.GetDumpResult());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SlimECS.Temp
{
	/*
	[StructLayout(LayoutKind.Sequential, Pack = 0)]
	unsafe struct EntityData
	{
		public int id;				// 4 
		public fixed byte flags[100]; // 4
	}
	*/

	struct Entity
	{
		public int id;
		public int slot;
		public Context context;

		public Entity(int id, int slot, Context context)
		{
			this.id = id;
			this.slot = slot;
			this.context = context;
		}
	}

	static class EntityComponentAccess
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetName(this Entity e, string name)
		{
			ref var d = ref e.context._entities.items[e.slot];
			d.name = name;
		}
	}

	class Context
	{
		internal struct EntityData
		{
			public int id;
			public string name;
		}

		internal readonly StructDataPool<EntityData> _entities = new StructDataPool<EntityData>();

		private int _lastId;
		private int _entityCount;

		public Entity CreateEntity()
		{
			var id = ++_lastId;
			int slot = _entities.Alloc();

			ref var d = ref _entities.items[slot];
			{
				d.id = id;
			}

			_entityCount++;

			return new Entity(id, slot, this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetName(Entity e, string name)
		{
			ref var d = ref _entities.items[e.slot];
			d.name = name;
		}
	}


	public class Program
	{
		const int loopCount = 10000000;

		static void Main(string[] args)
		{
			var context = new Context();

			var sw = new Stopwatch();

			sw.Start();
			{
				var e1 = context.CreateEntity();

				for (int i = 0; i < loopCount; i++)
					e1.SetName("hello");
			}
			sw.Stop();
			var t1 = sw.ElapsedMilliseconds;

			sw.Start();
			{
				var e2 = context.CreateEntity();

				for (int i = 0; i < loopCount; i++)
					context.SetName(e2, "hello");
			}
			sw.Stop();
			var t2 = sw.ElapsedMilliseconds;

			Console.WriteLine($"t1 = {t1}ms, t2 = {t2}ms");

			/*
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

			unsafe
			{
				Console.WriteLine($"sizeof(EntityData) = {sizeof(EntityData)}");

				var ed = new EntityData();

				ed.id = 100;
				ed.flags[0] = 22;

				Console.WriteLine($"id={ed.id} flags={ed.flags[0]},{ed.flags[1]},{ed.flags[2]},{ed.flags[3]},{ed.flags[4]},{ed.flags[5]}");
			}

			unsafe
			{
				var bytes = new byte[] { 200, 0, 0, 0, 1, 2, 3, 4, 5, 6 };

				fixed (byte* ptr = &bytes[0])
				{
					var ed2 = Marshal.PtrToStructure<EntityData>((IntPtr)ptr);

					Console.WriteLine($"id={ed2.id} flags={ed2.flags[0]},{ed2.flags[1]},{ed2.flags[2]},{ed2.flags[3]},{ed2.flags[4]},{ed2.flags[5]}");

					//Console.WriteLine($"id={ed2.id} flags={ed2.flags[0]},{ed2.flags[1]},{ed2.flags[2]},{ed2.flags[3]}");

					//Console.WriteLine($"{string.Join(',', bytes)}");
					
					for (int i = 0; i < 50; i++)
					{
						Console.WriteLine($"{ptr[i + 4]} {ed2.flags[i]}");
					}
				}
			}


			*/
		}
	}
}

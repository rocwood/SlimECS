using System;
using System.Runtime.InteropServices;

namespace SlimECS.Temp
{
	[StructLayout(LayoutKind.Sequential, Pack = 0)]
	unsafe struct EntityData
	{
		public int id;				// 4 
		public fixed byte flags[100]; // 4
	}

	public class Program
	{
		static void Main(string[] args)
		{
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
		}
	}
}

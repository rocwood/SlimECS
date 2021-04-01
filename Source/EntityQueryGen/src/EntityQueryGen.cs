using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SlimECS
{
	public static class EntityQueryGen
	{
		const int IncludeCount = 6;
		const int ExcludeCount = 2;
		const string OutPath = "../../../../SlimECS/src/Query/";

		static void WriteParamList(this StreamWriter o, string prefix, int n)
		{
			for (int i = 1; i <= n; i++)
			{
				o.Write($"{prefix}{i}");
				if (i < n)
					o.Write(",");
			}
		}

		static void WriteConstraintList(this StreamWriter o, string prefix, int n)
		{
			for (int i = 1; i <= n; i++)
			{
				o.Write($"where {prefix}{i}:struct,IComponent ");
			}
		}

		static void WriteMakeList(this StreamWriter o, string prefix, int n)
		{
			for (int i = 1; i <= n; i++)
			{
				o.Write($"{prefix}{i}>()");
				if (i < n)
					o.Write(",");
			}
		}

		static void Main(string[] args)
		{
			WriteQueryFile();
			WriteBuilderFile();
			WriteFactoryFile();
		}

		private static void WriteQueryFile()
		{
			var o = new StreamWriter($"{OutPath}EntityQueryGeneric.cs", false, Encoding.UTF8);
			o.NewLine = "\n";

			o.WriteLine("namespace SlimECS\n{");

			o.WriteQueryTemplates("All", "false");
			o.WriteQueryTemplates("Any", "true");

			o.WriteLine("}");
			o.Close();
		}

		private static void WriteBuilderFile()
		{
			var o = new StreamWriter($"{OutPath}EntityQueryBuilder.cs", false, Encoding.UTF8);
			o.NewLine = "\n";

			o.WriteLine("using System.Runtime.CompilerServices;\n");
			o.WriteLine("namespace SlimECS\n{");
			o.WriteBuilderTemplates("All");
			o.WriteBuilderTemplates("Any");

			o.WriteLine("}");
			o.Close();
		}

		private static void WriteFactoryFile()
		{
			var o = new StreamWriter($"{OutPath}EntityQueryBuilderFactory.cs", false, Encoding.UTF8);
			o.NewLine = "\n";

			o.WriteLine("using System.Runtime.CompilerServices;\n");
			o.WriteLine("namespace SlimECS\n{");
			o.WriteLine("\tpublic static class EntityQueryBuilderFactory\n\t{");

			o.WriteFactoryTemplates("All");
			
			o.WriteLine();
			o.WriteFactoryTemplates("Any");

			o.WriteLine("\t}\n}");
			o.Close();
		}

		private static void WriteQueryTemplates(this StreamWriter o, string mode, string matchAny)
		{
			for (int qIndex = 1; qIndex <= IncludeCount; qIndex++)
			{
				o.Write($"\tpublic class EntityQuery{mode}<");
				o.WriteParamList("T", qIndex);
				o.Write("> : EntityQuery ");
				o.WriteConstraintList("T", qIndex);
				o.WriteLine("\n\t{");

				o.Write($"\t\tinternal EntityQuery{mode}(Context c) : base(c,{matchAny}) => make(");
				o.WriteMakeList("inc<T", qIndex);
				o.WriteLine(");");
				o.WriteLine();

				for (int exIndex = 1; exIndex <= ExcludeCount; exIndex++)
				{
					o.Write("\t\tpublic class Without<");
					o.WriteParamList("X", exIndex);
					o.Write("> : EntityQuery ");
					o.WriteConstraintList("X", exIndex);
					o.WriteLine("{");

					o.Write($"\t\t\tinternal Without(Context c) : base(c,{matchAny}) => make(");
					o.WriteMakeList("inc<T", qIndex);
					o.Write(",");
					o.WriteMakeList("outc<X", exIndex);
					o.WriteLine(");");

					o.WriteLine("\t\t}");
				}

				o.WriteLine("\t}");
				o.WriteLine();
			}
		}

		private static void WriteBuilderTemplates(this StreamWriter o, string mode)
		{
			for (int qIndex = 1; qIndex <= IncludeCount; qIndex++)
			{
				o.Write($"\tpublic struct EntityQuery{mode}Builder<");
				o.WriteParamList("T", qIndex);
				o.Write("> ");
				o.WriteConstraintList("T", qIndex);
				o.WriteLine("\n\t{");

				o.WriteLine($"\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				o.Write($"\t\tpublic EntityQuery{mode}<");
				o.WriteParamList("T", qIndex);
				o.Write($"> Get()\n\t\t\t=> (EntityQuery{mode}<");
				o.WriteParamList("T", qIndex);
				o.Write($">)_context.GetQuery(typeof(EntityQuery{mode}<");
				o.WriteParamList("T", qIndex);
				o.WriteLine(">));");

				for (int exIndex = 1; exIndex <= ExcludeCount; exIndex++)
				{
					o.WriteLine($"\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					o.Write($"\t\tpublic EntityQuery{mode}<");
					o.WriteParamList("T", qIndex);
					o.Write(">.Without<");
					o.WriteParamList("X", exIndex);
					o.Write("> Without<");
					o.WriteParamList("X", exIndex);
					o.Write(">() ");
					o.WriteConstraintList("X", exIndex);
					o.WriteLine();
					o.Write($"\t\t\t=> (EntityQuery{mode}<");
					o.WriteParamList("T", qIndex);
					o.Write(">.Without<");
					o.WriteParamList("X", exIndex);
					o.Write($">)_context.GetQuery(typeof(EntityQuery{mode}<");
					o.WriteParamList("T", qIndex);
					o.Write(">.Without<");
					o.WriteParamList("X", exIndex);
					o.WriteLine(">));");
				}

				o.WriteLine();

				o.WriteLine($"\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				o.Write($"\t\tpublic static implicit operator EntityQuery{mode}<");
				o.WriteParamList("T", qIndex);
				o.Write($">(EntityQuery{mode}Builder<");
				o.WriteParamList("T", qIndex);
				o.Write("> b");
				o.WriteLine(") => b.Get();");

				o.WriteLine();
				o.WriteLine($"\t\tinternal EntityQuery{mode}Builder(Context c) => _context = c;");
				o.WriteLine("\t\treadonly Context _context;");

				o.WriteLine("\t}");
				o.WriteLine();
			}
		}

		private static void WriteFactoryTemplates(this StreamWriter o, string mode)
		{
			for (int qIndex = 1; qIndex <= IncludeCount; qIndex++)
			{
				o.WriteLine($"\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				o.Write($"\t\tpublic static EntityQuery{mode}Builder<");
				o.WriteParamList("T", qIndex);
				o.Write($"> With{mode}<");
				o.WriteParamList("T", qIndex);
				o.Write(">(this Context c) ");
				o.WriteConstraintList("T", qIndex);
				o.WriteLine();

				o.Write($"\t\t\t=> new EntityQuery{mode}Builder<");
				o.WriteParamList("T", qIndex);
				o.WriteLine($">(c);\n");
			}
		}
	}
}

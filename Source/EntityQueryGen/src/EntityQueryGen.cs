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
			WriteQueryBuilderFile();
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

		private static void WriteQueryBuilderFile()
		{
			var o = new StreamWriter($"{OutPath}EntityQueryBuilder.cs", false, Encoding.UTF8);
			o.NewLine = "\n";

			o.WriteLine("namespace SlimECS\n{");

			for (int qIndex = 1; qIndex <= IncludeCount; qIndex++)
			{
				o.Write("\tpublic struct EntityQueryBuilder<");
				o.WriteParamList("T", qIndex);
				o.Write("> ");
				o.WriteConstraintList("T", qIndex);
				o.WriteLine("\n\t{");

				o.Write("\t\tpublic EntityQueryAll<");
				o.WriteParamList("T", qIndex);
				o.Write("> GetQuery() => (EntityQueryAll<");
				o.WriteParamList("T", qIndex);
				o.Write(">)_context.GetQuery(typeof(EntityQueryAll<");
				o.WriteParamList("T", qIndex);
				o.WriteLine(">));");

				o.WriteLine("\t\tinternal EntityQueryBuilder(Context c) => _context = c;");
				o.WriteLine("\t\treadonly Context _context;");
				o.WriteLine();

				for (int exIndex = 1; exIndex <= ExcludeCount; exIndex++)
				{
					o.Write("\t\tpublic WithoutBuilder<");
					o.WriteParamList("X", exIndex);
					o.Write("> Without<");
					o.WriteParamList("X", exIndex);
					o.Write(">() ");
					o.WriteConstraintList("X", exIndex);
					o.Write("=> new WithoutBuilder<");
					o.WriteParamList("X", exIndex);
					o.WriteLine(">(_context);");
				}

				o.WriteLine();

				for (int exIndex = 1; exIndex <= ExcludeCount; exIndex++)
				{
					o.Write("\t\tpublic struct WithoutBuilder<");
					o.WriteParamList("X", exIndex);
					o.Write("> ");
					o.WriteConstraintList("X", exIndex);
					o.WriteLine("{");

					o.Write("\t\t\tpublic EntityQueryAll<");
					o.WriteParamList("T", qIndex);
					o.Write(">.Without<");
					o.WriteParamList("X", exIndex);
					o.Write("> GetQuery() => (EntityQueryAll<");
					o.WriteParamList("T", qIndex);
					o.Write(">.Without<");
					o.WriteParamList("X", exIndex);
					o.Write(">)_context.GetQuery(typeof(EntityQueryAll<");
					o.WriteParamList("T", qIndex);
					o.Write(">.Without<");
					o.WriteParamList("X", exIndex);
					o.WriteLine(">));");

					o.WriteLine("\t\t\tinternal WithoutBuilder(Context c) => _context = c;");
					o.WriteLine("\t\t\treadonly Context _context;");

					o.WriteLine("\t\t}");
				}

				o.WriteLine("\t}");
				o.WriteLine();
			}

			o.WriteLine("}");
			o.Close();
		}
	}
}

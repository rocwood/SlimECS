namespace SlimECS
{
	public abstract class SystemBase
	{
		public Context context { get; internal set; }

		public abstract void Execute();
	}
}

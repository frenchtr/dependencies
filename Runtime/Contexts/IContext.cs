using TravisRFrench.Dependencies.Containers;

namespace TravisRFrench.Dependencies.Contexts
{
	/// <summary>
	/// Represents a source of a DI container, such as a global or scene-scoped context.
	/// </summary>
	public interface IContext
	{
		/// <summary>
		/// Gets the <see cref="IContainer"/> instance provided by this context.
		/// </summary>
		IContainer Container { get; }
	}
}

using TravisRFrench.Dependencies.Containers.Registration;
using TravisRFrench.Dependencies.Injection;
using TravisRFrench.Dependencies.Registration;
using TravisRFrench.Dependencies.Resolution;

namespace TravisRFrench.Dependencies.Containers
{
	/// <summary>
	/// A dependency injection container capable of registering bindings, resolving instances, and injecting into existing objects.
	/// Supports parent containers for fallback resolution.
	/// </summary>
	/// <inheritdoc cref="IRegistry"/>
	/// <inheritdoc cref="IResolver"/>
	/// <inheritdoc cref="IInjector"/>
	public interface IContainer : IRegistry, IResolver, IInjector
	{
		/// <summary>
		/// The parent container used for fallback resolution, or null if this is the root container.
		/// </summary>
		IContainer Parent { get; }
		
		/// <summary>
		/// Creates a child of this container.
		/// </summary>
		/// <returns>The child container.</returns>
		IContainer CreateChildContainer();
	}
}

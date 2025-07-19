using System;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.Bindings
{
	/// <summary>
	/// Represents a resolved binding between an interface and an implementation, including construction metadata.
	/// </summary>
	public interface IBinding
	{
		/// <summary>
		/// The interface type associated with this binding.
		/// </summary>
		Type InterfaceType { get; }

		/// <summary>
		/// The implementation type that fulfills the interface.
		/// </summary>
		Type ImplementationType { get; }

		/// <summary>
		/// The lifetime of the bound instance (e.g., transient or singleton).
		/// </summary>
		/// <seealso cref="Lifetime"/>
		Lifetime Lifetime { get; }

		/// <summary>
		/// The source used to construct this binding (e.g., new instance, provided instance, or factory).
		/// </summary>
		/// <seealso cref="ConstructionSource"/>
		ConstructionSource Source { get; }

		/// <summary>
		/// A pre-existing instance for this binding, if <see cref="Source"/> is <see cref="ConstructionSource.FromInstance"/>.
		/// </summary>
		object Instance { get; }

		/// <summary>
		/// A factory function to produce instances, if <see cref="Source"/> is <see cref="ConstructionSource.FromFactory"/>.
		/// </summary>
		Func<object> Factory { get; }
		
		/// <summary>
		/// A condition to evaluate against an injection context to determine if the binding is suitable.
		/// </summary>
		Func<IInjectionContext, bool> Condition { get; }
	}
}

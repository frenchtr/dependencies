using System;
using TravisRFrench.Dependencies.Bindings;

namespace TravisRFrench.Dependencies.Registration
{
	/// <summary>
	/// Defines a registry for storing and managing interface-to-binding mappings.
	/// </summary>
	public interface IRegistry
	{
		/// <summary>
		/// Registers a new binding with the registry.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		void Register(IBinding binding);

		/// <summary>
		/// Unregisters the binding associated with the given interface type.
		/// </summary>
		/// <param name="interfaceType">The interface type whose binding should be removed.</param>
		void Unregister(Type interfaceType);

		/// <summary>
		/// Attempts to retrieve a registered binding for the specified type.
		/// </summary>
		/// <param name="type">The type to query for a binding.</param>
		/// <param name="binding">The resulting binding, if found.</param>
		/// <returns>True if a binding exists; otherwise, false.</returns>
		bool TryGetBinding(Type type, out IBinding binding);
	}
}

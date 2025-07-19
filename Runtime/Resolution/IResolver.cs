using System;

namespace TravisRFrench.Dependencies.Resolution
{
	/// <summary>
	/// Defines a contract for resolving service instances by type from a container.
	/// </summary>
	public interface IResolver
	{
		/// <summary>
		/// Resolves an instance of the given type from the container.
		/// </summary>
		/// <param name="type">The type of service to resolve.</param>
		/// <returns>An instance of the requested type.</returns>
		object Resolve(Type type);
	}
}

using System;

namespace TravisRFrench.Dependencies.Resolution
{
	/// <summary>
	/// Defines a simple cache interface for storing and retrieving type-to-instance mappings.
	/// Typically used to manage singleton lifetimes.
	/// </summary>
	public interface ICache
	{
		/// <summary>
		/// Attempts to retrieve a cached instance for the given type.
		/// </summary>
		/// <param name="type">The type key to look up.</param>
		/// <param name="instance">The retrieved instance, if found.</param>
		/// <returns>True if an instance was found; otherwise, false.</returns>
		bool TryGet(Type type, out object instance);

		/// <summary>
		/// Stores an instance in the cache for the given type.
		/// </summary>
		/// <param name="type">The type key to associate with the instance.</param>
		/// <param name="instance">The object to store.</param>
		void Store(Type type, object instance);

		/// <summary>
		/// Removes the cached instance associated with the given type.
		/// </summary>
		/// <param name="type">The type key to remove.</param>
		void Clear(Type type);
	}
}

using System;
using System.Collections.Generic;

namespace TravisRFrench.Dependencies.Resolution
{
	/// <summary>
	/// A simple type-to-instance mapping cache used for storing resolved singletons or other reusable objects.
	/// </summary>
	public class Cache : ICache
	{
		private readonly Dictionary<Type, object> instances = new();

		/// <summary>
		/// Attempts to retrieve a cached instance for the specified type.
		/// </summary>
		/// <param name="type">The type to look up.</param>
		/// <param name="instance">The cached instance, if found.</param>
		/// <returns>True if the instance was found; otherwise, false.</returns>
		public bool TryGet(Type type, out object instance)
		{
			return this.instances.TryGetValue(type, out instance);
		}

		/// <summary>
		/// Stores an instance for the specified type.
		/// </summary>
		/// <param name="type">The type to associate the instance with.</param>
		/// <param name="instance">The instance to store.</param>
		public void Store(Type type, object instance)
		{
			this.instances[type] = instance;
		}

		/// <summary>
		/// Removes a cached instance for the specified type, if present.
		/// </summary>
		/// <param name="type">The type to remove from the cache.</param>
		public void Clear(Type type)
		{
			this.instances.Remove(type);
		}
	}
}

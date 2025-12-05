using System;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Contexts;
using TravisRFrench.Dependencies.Injection;
using UnityEngine;

namespace TravisRFrench.Dependencies
{
	/// <summary>
	/// Static access point for dependency injection operations using the global context.
	/// </summary>
	public static class DI
	{
		/// <summary>
		/// Gets the global container initialized by <see cref="GlobalContext"/> at runtime.
		/// </summary>
		public static IContainer Container => GlobalContext.Container;

		/// <summary>
		/// Injects all <see cref="InjectAttribute"/> dependencies into the given target object using the provided container.
		/// If no container is provided, the global container is used.
		/// </summary>
		/// <param name="target">The object to inject dependencies into.</param>
		/// <param name="container">An optional container to use instead of the global one.</param>
		/// <exception cref="InvalidOperationException">Thrown if no container is available.</exception>
		public static void Inject(object target, IContainer container = null)
		{
			container ??= Container;

			if (container == null)
			{
				throw new InvalidOperationException("[DI] No container is available for injection.");
			}

			container.Inject(target);
		}

		/// <summary>
		/// Attempts to inject dependencies into the given object using the provided container or the global container.
		/// </summary>
		/// <param name="target">The object to inject into.</param>
		/// <param name="container">An optional container to use instead of the global one.</param>
		/// <returns>True if injection succeeded; false if no container was available.</returns>
		public static bool TryInject(object target, IContainer container = null)
		{
			container ??= Container;

			if (container == null)
			{
				return false;
			}

			container.Inject(target);
			return true;
		}
        
        public static void Inject(GameObject gameObject, IContainer container = null)
        {
			foreach (var monoBehaviour in gameObject.GetComponents<MonoBehaviour>())
			{
				Inject(monoBehaviour, container);
			}
        }
	}
}

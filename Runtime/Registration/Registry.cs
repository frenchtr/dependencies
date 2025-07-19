using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Bindings;

namespace TravisRFrench.Dependencies.Registration
{
	/// <summary>
	/// Default implementation of <see cref="IRegistry"/> that stores interface-to-binding mappings.
	/// </summary>
	public class Registry : IRegistry
	{
		private readonly Dictionary<Type, IBinding> bindings = new();

		/// <summary>
		/// Registers a new binding in the registry.
		/// If a binding for the same interface already exists, it is replaced.
		/// </summary>
		/// <param name="binding">The binding to register.</param>
		public void Register(IBinding binding)
		{
			this.ValidateBinding(binding);
			this.bindings[binding.InterfaceType] = binding;
		}

		/// <summary>
		/// Removes the binding associated with the specified interface type.
		/// </summary>
		/// <param name="interfaceType">The interface type to unregister.</param>
		public void Unregister(Type interfaceType)
		{
			this.bindings.Remove(interfaceType);
		}

		/// <summary>
		/// Attempts to retrieve a binding for the given type.
		/// </summary>
		/// <param name="type">The type to look up in the registry.</param>
		/// <param name="binding">The resulting binding, if found.</param>
		/// <returns>True if a binding exists for the type; otherwise, false.</returns>
		public bool TryGetBinding(Type type, out IBinding binding)
		{
			return this.bindings.TryGetValue(type, out binding);
		}

		private void ValidateBinding(IBinding binding)
		{
			var interfaceType = binding.InterfaceType;
			var implementationType = binding.ImplementationType;
			var lifetime = binding.Lifetime;
			var source = binding.Source;
			var instance = binding.Instance;
			var factory = binding.Factory;

			if (interfaceType == null)
			{
				throw new InvalidOperationException("Binding interface type must not be null.");
			}

			if (implementationType == null)
			{
				throw new InvalidOperationException("Binding implementation type must not be null.");
			}

			if (!interfaceType.IsAssignableFrom(implementationType))
			{
				throw new InvalidOperationException($"{implementationType.Name} is not assignable to {interfaceType.Name}");
			}

			if (source == ConstructionSource.FromInstance && instance == null)
			{
				throw new InvalidOperationException(
					$"An instance must be provided when using {ConstructionSource.FromInstance}.");
			}

			if (source == ConstructionSource.FromFactory && factory == null)
			{
				throw new InvalidOperationException(
					$"A factory must be provided when using {ConstructionSource.FromFactory}.");
			}
		}
	}
}

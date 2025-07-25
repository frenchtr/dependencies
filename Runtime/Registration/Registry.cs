using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Bindings;
using TravisRFrench.Dependencies.Injection;

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
		public bool TryGetBinding(Type type, out IBinding binding, IInjectionContext context = null)
		{
			this.bindings.TryGetValue(type, out binding);

			if (binding is null)
			{
				return false;
			}

			if (binding is { Condition: null })
			{
				return true;
			}

			if (binding is { Condition: not null})
			{
				try
				{
					if (context == null)
					{
						binding = null;
						return false;
					}
					
					return binding.Condition.Invoke(context);
				}
				catch
				{
					binding = null;
					return false;
				}
			}

			return true;
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
				throw new BindingValidationException(this, binding,"Binding interface type must not be null.");
			}

			if (implementationType == null)
			{
				throw new BindingValidationException(this, binding, "Binding implementation type must not be null.");
			}

			if (!interfaceType.IsAssignableFrom(implementationType))
			{
				throw new BindingValidationException(this, binding, $"{implementationType.Name} is not assignable to {interfaceType.Name}");
			}

			if (source == ConstructionSource.FromInstance && instance == null)
			{
				throw new BindingValidationException(this, binding, $"An instance must be provided when using {ConstructionSource.FromInstance}.");
			}
			
			if (source == ConstructionSource.FromFactory && factory == null)
			{
				throw new BindingValidationException(this, binding, $"A factory must be provided when using {ConstructionSource.FromFactory}.");
			}
		}
	}
}

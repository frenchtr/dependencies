using System;
using System.Linq;
using System.Reflection;
using TravisRFrench.Dependencies.Bindings;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Registration;

namespace TravisRFrench.Dependencies.Resolution
{
	/// <summary>
	/// Default implementation of <see cref="IResolver"/> that resolves instances using registered bindings,
	/// supporting singleton caching and fallback to a parent container.
	/// </summary>
	public class Resolver : IResolver
	{
		private readonly IRegistry registry;
		private readonly ICache singletons;
		private readonly IContainer parent;

		/// <summary>
		/// Constructs a new <see cref="Resolver"/>.
		/// </summary>
		/// <param name="registry">The registry from which to retrieve bindings.</param>
		/// <param name="singletons">The singleton cache to use for storing resolved instances.</param>
		/// <param name="parent">An optional parent container to use as fallback if no binding is found locally.</param>
		public Resolver(IRegistry registry, ICache singletons, IContainer parent = null)
		{
			this.registry = registry;
			this.singletons = singletons;
			this.parent = parent;
		}

		/// <summary>
		/// Resolves an instance of the specified generic type.
		/// </summary>
		/// <typeparam name="TInterface">The type to resolve.</typeparam>
		/// <returns>An instance of <typeparamref name="TInterface"/>.</returns>
		public TInterface Resolve<TInterface>()
		{
			return (TInterface)this.Resolve(typeof(TInterface));
		}

		/// <inheritdoc/>
		public object Resolve(Type type)
		{
			if (!this.registry.TryGetBinding(type, out var binding))
			{
				if (this.parent == null)
				{
					throw new InvalidOperationException($"No binding registered for type {type.Name}");
				}

				try
				{
					return this.parent.Resolve(type);
				}
				catch (Exception e)
				{
					throw new InvalidOperationException(
						$"No binding registered for type {type.Name} in this container or its parent.", e);
				}
			}

			if (binding.Lifetime == Lifetime.Singleton)
			{
				if (!this.singletons.TryGet(type, out var instance))
				{
					instance = this.CreateInstance(binding);
					this.singletons.Store(type, instance);
				}

				return instance;
			}

			return this.CreateInstance(binding);
		}

		private object CreateInstance(IBinding binding)
		{
			return binding.Source switch
			{
				ConstructionSource.FromNew => this.CreateInstanceFromNew(binding.ImplementationType),
				ConstructionSource.FromInstance => binding.Instance,
				ConstructionSource.FromFactory => binding.Factory.Invoke(),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private object CreateInstanceFromNew(Type implementationType)
		{
			var constructors = implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
				.OrderBy(c => c.GetParameters().Length);

			foreach (var constructor in constructors)
			{
				var parameters = constructor.GetParameters();
				var arguments = new object[parameters.Length];

				try
				{
					for (var index = 0; index < parameters.Length; index++)
					{
						var parameter = parameters[index];
						var argument = this.Resolve(parameter.ParameterType);
						arguments[index] = argument;
					}

					return constructor.Invoke(arguments);
				}
				catch
				{
					continue;
				}
			}

			throw new InvalidOperationException($"No suitable constructor found for type {implementationType.Name}");
		}
	}
}

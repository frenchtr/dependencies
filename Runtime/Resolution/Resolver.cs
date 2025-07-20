using System;
using TravisRFrench.Dependencies.Bindings;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Injection;
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
		private readonly IInjector injector;
		private readonly IContainer parent;

		/// <summary>
		/// Constructs a new <see cref="Resolver"/>.
		/// </summary>
		/// <param name="registry">The registry from which to retrieve bindings.</param>
		/// <param name="singletons">The singleton cache to use for storing resolved instances.</param>
		/// <param name="parent">An optional parent container to use as fallback if no binding is found locally.</param>
		public Resolver(IRegistry registry, ICache singletons, IInjector injector, IContainer parent = null)
		{
			this.registry = registry;
			this.singletons = singletons;
			this.injector = injector;
			this.parent = parent;
		}

		/// <summary>
		/// Resolves an instance of the specified generic type.
		/// </summary>
		/// <typeparam name="TInterface">The type to resolve.</typeparam>
		/// <returns>An instance of <typeparamref name="TInterface"/>.</returns>
		public TInterface Resolve<TInterface>(IInjectionContext context = null)
		{
			return (TInterface)this.Resolve(typeof(TInterface), context);
		}

		/// <inheritdoc/>
		public object Resolve(Type type, IInjectionContext context = null)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}
			
			try
			{
				IBinding binding;

				try
				{
					if (!this.registry.TryGetBinding(type, out binding, context))
					{
						var instance = this.parent.Resolve(type, context);
						return instance;
					}
				}
				catch (Exception exception)
				{
					throw new BindingNotFoundException(this, type,
						"Unable to find a suitable binding in the container or any parent.", exception);
				}

				if (binding.Lifetime == Lifetime.Singleton)
				{
					if (!this.singletons.TryGet(type, out var instance))
					{
						instance = this.GetInstance(binding, context);
						this.singletons.Store(type, instance);
					}

					return instance;
				}

				return this.GetInstance(binding, context);
			}
			catch (Exception exception)
			{
				var suffix = (type == null) ? string.Empty : $" of type {type.Name}";
				throw new TypeResolutionException(this, type, $"Unable to resolve binding{suffix}.", exception);
			}
		}
		
		private object GetInstance(IBinding binding, IInjectionContext context = null)
		{
			return binding.Source switch
			{
				ConstructionSource.FromInstance => binding.Instance,
				ConstructionSource.FromNew => this.injector.Instantiate(binding.ImplementationType),
				ConstructionSource.FromFactory => this.injector.InstantiateFromFactory(binding.Factory),
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}

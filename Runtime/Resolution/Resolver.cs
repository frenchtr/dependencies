using System;
using System.Linq;
using System.Reflection;
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
						return this.InjectInstance(instance);
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

		private object InjectInstance(object instance)
		{
			this.injector.Inject(instance);
			return instance;
		}
		
		private object GetInstance(IBinding binding, IInjectionContext context = null)
		{
			switch (binding.Source)
			{
				case ConstructionSource.FromInstance:
				{
					return binding.Instance;
				}
				case ConstructionSource.FromNew:
				{
					var instance = this.CreateInstanceFromNew(binding, context);
					var injected  = this.InjectInstance(instance);

					return injected;
				}
				case ConstructionSource.FromFactory:
				{
					var instance = this.CreateInstanceFromFactory(binding);
					var injected = this.InjectInstance(instance);
					
					return injected;
				}
				default:
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private object CreateInstanceFromNew(IBinding binding, IInjectionContext context = null)
		{
			try
			{
				var constructors = binding.ImplementationType
					.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
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
							var argument = this.Resolve(parameter.ParameterType, context);
							arguments[index] = argument;
						}

						return constructor.Invoke(arguments);
					}
					catch
					{
						continue;
					}
				}
				
				throw new InvalidOperationException($"No suitable constructor found for type {binding.ImplementationType.Name}");
			}
			catch (Exception exception)
			{
				throw new ConstructorCreationException(this, binding.ImplementationType, $"Unable to create instance from constructor.", exception);
			}
		}

		private object CreateInstanceFromFactory(IBinding binding)
		{
			try
			{
				var factory = binding.Factory;

				return factory.Invoke();
			}
			catch (Exception exception)
			{
				throw new FactoryCreationException(this, binding.ImplementationType, $"Unable to create instance from factory.", exception);
			}
		}
	}
}

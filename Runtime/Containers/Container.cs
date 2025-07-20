using System;
using TravisRFrench.Dependencies.Bindings;
using TravisRFrench.Dependencies.Injection;
using TravisRFrench.Dependencies.Registration;
using TravisRFrench.Dependencies.Resolution;

namespace TravisRFrench.Dependencies.Containers
{
	/// <summary>
	/// The default implementation of <see cref="IContainer"/>. 
	/// Manages bindings, singletons, resolution, and dependency injection.
	/// </summary>
	public class Container : IContainer
	{
		private readonly IRegistry registry;
		private readonly ICache singletons;
		private readonly IResolver resolver;
		private readonly IInjector injector;

		/// <summary>
		/// The parent container used for fallback resolution, if any.
		/// </summary>
		public IContainer Parent { get; }

		/// <summary>
		/// Creates a new container, optionally scoped to a parent container.
		/// </summary>
		/// <param name="parent">An optional parent container for fallback resolution.</param>
		public Container(IContainer parent = null)
		{
			this.Parent = parent;

			this.singletons = new Cache();
			this.registry = new Registry();
			this.injector = new Injector(this);
			this.resolver = new Resolver(this.registry, this.singletons, this.injector, this.Parent);
		}
		
		/// <inheritdoc/>>
		public IContainer CreateChildContainer()
		{
			return new Container(this);
		}

		/// <inheritdoc />
		void IRegistry.Register(IBinding binding)
		{
			this.registry.Register(binding);
		}

		/// <inheritdoc />
		void IRegistry.Unregister(Type interfaceType)
		{
			this.registry.Unregister(interfaceType);
		}

		/// <inheritdoc />
		bool IRegistry.TryGetBinding(Type type, out IBinding binding, IInjectionContext context)
		{
			return this.registry.TryGetBinding(type, out binding);
		}

		/// <inheritdoc />
		object IResolver.Resolve(Type type, IInjectionContext context)
		{
			return this.resolver.Resolve(type, context);
		}

		/// <summary>
		/// Performs injection on an existing object instance using this container's injector.
		/// </summary>
		/// <param name="obj">The object to inject into.</param>
		public void Inject(object obj)
		{
			this.injector.Inject(obj);
		}

		public object Instantiate(Type type)
		{
			return this.injector.Instantiate(type);
		}

		public object InstantiateFromFactory(Func<object> factory)
		{
			return this.injector.InstantiateFromFactory(factory);
		}
	}
}

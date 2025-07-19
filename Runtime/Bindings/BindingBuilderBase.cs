using System;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.Bindings
{
	/// <summary>
	/// Base class for all binding builders, providing fluent APIs to define interface-to-implementation relationships.
	/// </summary>
	public abstract class BindingBuilderBase : IBindingBuilder
	{
		Type IBindingBuilder.InterfaceType => this.InterfaceType;
		Type IBindingBuilder.ImplementationType => this.ImplementationType;
		Lifetime IBindingBuilder.Lifetime => this.Lifetime;
		ConstructionSource IBindingBuilder.Source => this.Source;
		Func<IInjectionContext, bool> IBindingBuilder.Condition => this.Condition;

		/// <summary>
		/// The interface type to bind.
		/// </summary>
		protected Type InterfaceType { get; set; }

		/// <summary>
		/// The concrete implementation type to bind to.
		/// </summary>
		protected Type ImplementationType { get; set; }

		/// <summary>
		/// The desired object lifetime (singleton or transient).
		/// </summary>
		protected Lifetime Lifetime { get; set; } = Lifetime.Transient;

		/// <summary>
		/// The source of object construction (new, instance, or factory).
		/// </summary>
		protected ConstructionSource Source { get; set; }
		
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public Func<IInjectionContext, bool> Condition { get; set; }

		/// <summary>
		/// The container to which the final binding will be registered.
		/// </summary>
		protected IContainer Container { get; }

		private object instance;
		private Func<object> factory;

		/// <summary>
		/// Creates a new builder for a given interface type.
		/// </summary>
		/// <param name="container">The DI container.</param>
		/// <param name="interfaceType">The interface type to bind.</param>
		protected BindingBuilderBase(IContainer container, Type interfaceType)
		{
			this.Container = container;
			this.InterfaceType = interfaceType;
			this.Source = ConstructionSource.FromNew;
			this.Lifetime = Lifetime.Transient;
		}

		/// <summary>
		/// Creates a new builder with both interface and implementation types.
		/// </summary>
		/// <param name="container">The DI container.</param>
		/// <param name="interfaceType">The interface type.</param>
		/// <param name="implementationType">The implementation type.</param>
		protected BindingBuilderBase(IContainer container, Type interfaceType, Type implementationType)
			: this(container, interfaceType)
		{
			this.ImplementationType = implementationType;
		}

		/// <summary>
		/// Sets the implementation type explicitly.
		/// </summary>
		/// <param name="implementationType">The concrete type to bind to.</param>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder To(Type implementationType)
		{
			this.ImplementationType = implementationType;
			return this;
		}

		/// <summary>
		/// Sets the implementation type using a generic type argument.
		/// </summary>
		/// <typeparam name="TImplementation">The concrete type to bind to.</typeparam>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder To<TImplementation>()
		{
			return this.To(typeof(TImplementation));
		}

		/// <summary>
		/// Sets the implementation type to the interface type (self-binding).
		/// </summary>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder ToSelf()
		{
			this.ImplementationType = this.InterfaceType;
			return this;
		}

		/// <summary>
		/// Specifies that the container should construct new instances.
		/// </summary>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder FromNew()
		{
			this.Source = ConstructionSource.FromNew;
			return this.Register();
		}

		/// <summary>
		/// Specifies that a pre-created instance should be used.
		/// </summary>
		/// <param name="instance">The instance to register.</param>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder FromInstance(object instance)
		{
			this.Source = ConstructionSource.FromInstance;
			this.instance = instance;
			return this.Register();
		}

		/// <summary>
		/// Specifies that a factory function should be used to produce instances.
		/// </summary>
		/// <param name="factory">The factory method to invoke.</param>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder FromFactory(Func<object> factory)
		{
			this.Source = ConstructionSource.FromFactory;
			this.factory = factory;
			return this.Register();
		}

		/// <summary>
		/// Sets the lifetime of the binding to transient.
		/// </summary>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder AsTransient()
		{
			this.Lifetime = Lifetime.Transient;
			return this.Register();
		}

		/// <summary>
		/// Sets the lifetime of the binding to singleton.
		/// </summary>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder AsSingleton()
		{
			this.Lifetime = Lifetime.Singleton;
			return this.Register();
		}

		/// <summary>
		/// Sets the binding's lifetime explicitly.
		/// </summary>
		/// <param name="lifetime">The desired lifetime.</param>
		/// <returns>The builder instance.</returns>
		public IBindingBuilder WithLifetime(Lifetime lifetime)
		{
			this.Lifetime = lifetime;
			return this.Register();
		}

		/// <inheritdoc/>
		public IBindingBuilder When(Func<IInjectionContext, bool> condition)
		{
			this.Condition = condition;
			return this.Register();
		}

		/// <summary>
		/// Registers the constructed binding with the container.
		/// </summary>
		/// <returns>The builder instance.</returns>
		protected IBindingBuilder Register()
		{
			var binding = this.Source switch
			{
				ConstructionSource.FromNew => new Binding(this.InterfaceType, this.ImplementationType, this.Lifetime, this.Condition),
				ConstructionSource.FromInstance => new Binding(this.InterfaceType, this.ImplementationType, this.instance, this.Lifetime, this.Condition),
				ConstructionSource.FromFactory => new Binding(this.InterfaceType, this.ImplementationType, this.factory, this.Lifetime, this.Condition),
				_ => throw new InvalidOperationException($"Unknown construction source for binding {this.InterfaceType.FullName}.")
			};

			this.ValidateBinding(binding);
			this.Container.Register(binding);

			return this;
		}

		private void ValidateBinding(IBinding binding)
		{
			var interfaceType = binding.InterfaceType;
			var implementationType = binding.ImplementationType;
			var lifetime = binding.Lifetime;
			var source = binding.Source;

			if (interfaceType == null)
			{
				throw new InvalidOperationException("You must specify an interface type for the binding.");
			}

			if (implementationType == null)
			{
				throw new InvalidOperationException($"You must call {nameof(To)} or {nameof(this.ToSelf)}.");
			}

			if (!interfaceType.IsAssignableFrom(implementationType))
			{
				throw new InvalidOperationException($"{implementationType.Name} is not assignable to {interfaceType.Name}");
			}

			if (source == ConstructionSource.FromInstance && this.instance == null)
			{
				throw new InvalidOperationException($"You must provide a non-null instance when calling {nameof(this.FromInstance)}.");
			}

			if (source == ConstructionSource.FromFactory && this.factory == null)
			{
				throw new InvalidOperationException($"You must provide a non-null factory when calling {nameof(this.FromFactory)}.");
			}
		}
	}
}

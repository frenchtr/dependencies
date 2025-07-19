using System;
using TravisRFrench.Dependencies.Injection;

namespace TravisRFrench.Dependencies.Bindings
{
	/// <summary>
	/// Defines the fluent API for building and registering bindings in the container.
	/// </summary>
	public interface IBindingBuilder
	{
		/// <summary>
		/// The interface type being bound.
		/// </summary>
		Type InterfaceType { get; }

		/// <summary>
		/// The implementation type that fulfills the binding.
		/// </summary>
		Type ImplementationType { get; }

		/// <summary>
		/// The lifetime of the binding (transient or singleton).
		/// </summary>
		/// <seealso cref="Lifetime"/>
		Lifetime Lifetime { get; }

		/// <summary>
		/// Specifies how instances should be constructed for this binding.
		/// </summary>
		/// <seealso cref="ConstructionSource"/>
		ConstructionSource Source { get; }
		
		/// <summary>
		/// The condition to evaluate.
		/// </summary>
		Func<IInjectionContext, bool> Condition { get; }

		/// <summary>
		/// Sets the implementation type for this binding.
		/// </summary>
		/// <param name="implementationType">The concrete type that implements the interface.</param>
		/// <returns>The builder instance.</returns>
		IBindingBuilder To(Type implementationType);

		/// <summary>
		/// Sets the implementation type using a generic type parameter.
		/// </summary>
		/// <typeparam name="TImplementation">The concrete type that implements the interface.</typeparam>
		/// <returns>The builder instance.</returns>
		IBindingBuilder To<TImplementation>();

		/// <summary>
		/// Sets the implementation type to the same type as the interface (self-binding).
		/// </summary>
		/// <returns>The builder instance.</returns>
		IBindingBuilder ToSelf();

		/// <summary>
		/// Specifies that new instances should be constructed for this binding.
		/// </summary>
		/// <returns>The builder instance.</returns>
		IBindingBuilder FromNew();

		/// <summary>
		/// Specifies that a pre-created instance should be used.
		/// </summary>
		/// <param name="instance">The instance to use for resolution.</param>
		/// <returns>The builder instance.</returns>
		IBindingBuilder FromInstance(object instance);

		/// <summary>
		/// Specifies that a factory method should be used to construct instances.
		/// </summary>
		/// <param name="factory">The factory function to invoke when resolving.</param>
		/// <returns>The builder instance.</returns>
		IBindingBuilder FromFactory(Func<object> factory);

		/// <summary>
		/// Sets the lifetime of the binding to transient (a new instance per resolution).
		/// </summary>
		/// <returns>The builder instance.</returns>
		IBindingBuilder AsTransient();

		/// <summary>
		/// Sets the lifetime of the binding to singleton (a shared instance).
		/// </summary>
		/// <returns>The builder instance.</returns>
		IBindingBuilder AsSingleton();

		/// <summary>
		/// Explicitly sets the desired lifetime for this binding.
		/// </summary>
		/// <param name="lifetime">The lifetime to apply.</param>
		/// <returns>The builder instance.</returns>
		IBindingBuilder WithLifetime(Lifetime lifetime);

		/// <summary>
		/// Defines a condition that must be true of any resolved binding.
		/// </summary>
		/// <param name="condition">The predicate to evaluate.</param>
		/// <returns></returns>
		IBindingBuilder When(Func<IInjectionContext, bool> condition);
	}
}

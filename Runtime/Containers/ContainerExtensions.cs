using System;
using TravisRFrench.Dependencies.Bindings;

namespace TravisRFrench.Dependencies.Containers
{
	/// <summary>
	/// Extension methods for <see cref="IContainer"/> to simplify binding and resolution.
	/// </summary>
	public static class ContainerExtensions
	{
		/// <summary>
		/// Begins a binding for a single generic interface type.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to bind.</typeparam>
		/// <param name="container">The container on which to register the binding.</param>
		/// <returns>A binding builder for fluent configuration.</returns>
		public static BindingBuilder<TInterface> Bind<TInterface>(this IContainer container)
		{
			return new BindingBuilder<TInterface>(container);
		}

		/// <summary>
		/// Begins a binding between an interface and implementation using generic types.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to bind.</typeparam>
		/// <typeparam name="TImplementation">The concrete type implementing <typeparamref name="TInterface"/>.</typeparam>
		/// <param name="container">The container on which to register the binding.</param>
		/// <returns>A binding builder for fluent configuration.</returns>
		public static BindingBuilder<TInterface, TImplementation> Bind<TInterface, TImplementation>(this IContainer container)
			where TImplementation : TInterface
		{
			return new BindingBuilder<TInterface, TImplementation>(container);
		}

		/// <summary>
		/// Begins a binding for a specified interface type using reflection.
		/// </summary>
		/// <param name="container">The container on which to register the binding.</param>
		/// <param name="interfaceType">The interface type to bind.</param>
		/// <returns>A binding builder for fluent configuration.</returns>
		public static IBindingBuilder Bind(this IContainer container, Type interfaceType)
		{
			return new BindingBuilder(container, interfaceType);
		}

		/// <summary>
		/// Begins a binding between two specified types using reflection.
		/// </summary>
		/// <param name="container">The container on which to register the binding.</param>
		/// <param name="interfaceType">The interface type to bind.</param>
		/// <param name="implementationType">The concrete implementation type.</param>
		/// <returns>A binding builder for fluent configuration.</returns>
		public static IBindingBuilder Bind(this IContainer container, Type interfaceType, Type implementationType)
		{
			return new BindingBuilder(container, interfaceType, implementationType);
		}

		/// <summary>
		/// Unregisters a binding for the specified generic interface type.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to remove from the container.</typeparam>
		/// <param name="container">The container to modify.</param>
		public static void Unregister<TInterface>(this IContainer container)
		{
			container.Unregister(typeof(TInterface));
		}

		/// <summary>
		/// Attempts to retrieve a binding for the specified generic interface type.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to query.</typeparam>
		/// <param name="container">The container to query.</param>
		/// <param name="binding">The resulting binding if found.</param>
		/// <returns>True if a binding exists; otherwise, false.</returns>
		public static bool TryGetBinding<TInterface>(this IContainer container, out IBinding binding)
		{
			return container.TryGetBinding(typeof(TInterface), out binding);
		}

		/// <summary>
		/// Resolves an instance of the specified generic interface type.
		/// </summary>
		/// <typeparam name="TInterface">The interface type to resolve.</typeparam>
		/// <param name="container">The container to resolve from.</param>
		/// <returns>An instance of <typeparamref name="TInterface"/>.</returns>
		public static TInterface Resolve<TInterface>(this IContainer container)
		{
			return (TInterface)container.Resolve(typeof(TInterface));
		}
	}
}

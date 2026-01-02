using System;
using TravisRFrench.Dependencies.Containers;

namespace TravisRFrench.Dependencies.Bindings
{
	/// <summary>
	/// A non-generic binding builder used for fluent API configuration.
	/// </summary>
	public class BindingBuilder : BindingBuilderBase
	{
		/// <summary>
		/// Constructs a binding builder with an interface type only.
		/// </summary>
		/// <param name="container">The container where the binding will be registered.</param>
		/// <param name="interfaceType">The interface type to bind.</param>
		public BindingBuilder(IContainer container, Type interfaceType)
			: base(container, interfaceType)
		{
		}
	}

	/// <summary>
	/// A generic binding builder for a single interface type.
	/// </summary>
	/// <typeparam name="TInterface">The interface type to bind.</typeparam>
	public class BindingBuilder<TInterface> : BindingBuilderBase
	{
		/// <summary>
		/// Constructs a binding builder with only the interface type.
		/// </summary>
		/// <param name="container">The container where the binding will be registered.</param>
		public BindingBuilder(IContainer container)
			: base(container, typeof(TInterface))
		{
		}
	}
}

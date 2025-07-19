using System;

namespace TravisRFrench.Dependencies.Bindings
{
	/// <summary>
	/// Represents a binding between an interface and its implementation in the Dependencies Framework.
	/// </summary>
	public class Binding : IBinding
	{
		/// <summary>
		/// The interface type this binding applies to.
		/// </summary>
		public Type InterfaceType { get; }

		/// <summary>
		/// The concrete implementation type this binding resolves to.
		/// </summary>
		public Type ImplementationType { get; }

		/// <summary>
		/// The lifetime of the bound instance.
		/// </summary>
		public Lifetime Lifetime { get; }

		/// <summary>
		/// Indicates how this binding constructs the instance (e.g., new, instance, or factory).
		/// </summary>
		public ConstructionSource Source { get; private set; }

		/// <summary>
		/// A pre-existing instance to be used for this binding (if any).
		/// </summary>
		public object Instance { get; }

		/// <summary>
		/// A factory function that creates instances for this binding (if any).
		/// </summary>
		public Func<object> Factory { get; }

		/// <summary>
		/// Creates a binding that constructs new instances of the implementation type.
		/// </summary>
		/// <param name="interfaceType">The interface type being bound.</param>
		/// <param name="implementationType">The concrete implementation type.</param>
		/// <param name="lifetime">The desired object lifetime.</param>
		public Binding(
			Type interfaceType, 
			Type implementationType, 
			Lifetime lifetime = Lifetime.Transient)
		{
			this.InterfaceType = interfaceType;
			this.ImplementationType = implementationType;
			this.Lifetime = lifetime;
			this.Source = ConstructionSource.FromNew;
		}

		/// <summary>
		/// Creates a binding to a pre-existing instance.
		/// </summary>
		/// <param name="interfaceType">The interface type being bound.</param>
		/// <param name="implementationType">The concrete implementation type.</param>
		/// <param name="instance">The instance to use.</param>
		/// <param name="lifetime">The desired object lifetime.</param>
		public Binding(
			Type interfaceType, 
			Type implementationType,
			object instance,
			Lifetime lifetime = Lifetime.Transient)
			: this(interfaceType, implementationType, lifetime)
		{
			this.Instance = instance;
			this.Source = ConstructionSource.FromInstance;
		}

		/// <summary>
		/// Creates a binding that uses a factory function to produce instances.
		/// </summary>
		/// <param name="interfaceType">The interface type being bound.</param>
		/// <param name="implementationType">The concrete implementation type.</param>
		/// <param name="factory">The factory function to generate instances.</param>
		/// <param name="lifetime">The desired object lifetime.</param>
		public Binding(
			Type interfaceType, 
			Type implementationType,
			Func<object> factory,
			Lifetime lifetime = Lifetime.Transient)
			: this(interfaceType, implementationType, lifetime)
		{
			this.Factory = factory;
			this.Source = ConstructionSource.FromFactory;
		}
	}
}

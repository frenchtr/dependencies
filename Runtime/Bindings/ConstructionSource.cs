namespace TravisRFrench.Dependencies.Bindings
{
	/// <summary>
	/// Indicates how an object should be constructed for a given binding.
	/// </summary>
	public enum ConstructionSource
	{
		/// <summary>
		/// Create a new instance using a constructor.
		/// </summary>
		FromNew,

		/// <summary>
		/// Use a pre-existing instance provided at registration time.
		/// </summary>
		FromInstance,

		/// <summary>
		/// Use a factory method to construct the instance.
		/// </summary>
		FromFactory,
	}
}

namespace TravisRFrench.Dependencies.Bindings
{
	/// <summary>
	/// Specifies how instances are managed and reused by the container.
	/// </summary>
	public enum Lifetime
	{
		/// <summary>
		/// A new instance is created every time the service is resolved.
		/// </summary>
		Transient,

		/// <summary>
		/// A single shared instance is reused for all resolutions.
		/// </summary>
		Singleton
	}
}

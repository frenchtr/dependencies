namespace TravisRFrench.Dependencies.Injection
{
	/// <summary>
	/// Provides functionality to inject dependencies into an existing object instance.
	/// </summary>
	public interface IInjector
	{
		/// <summary>
		/// Injects all <see cref="InjectAttribute"/> dependencies into the given object.
		/// </summary>
		/// <param name="obj">The object to inject dependencies into.</param>
		void Inject(object obj);
	}
}

using TravisRFrench.Dependencies.Containers;

namespace TravisRFrench.Dependencies.Installers
{
	/// <summary>
	/// Defines a contract for binding installers that register services with a container.
	/// </summary>
	public interface IInstaller
	{
		/// <summary>
		/// Called to register all necessary bindings to the provided container.
		/// </summary>
		/// <param name="container">The container to which bindings should be added.</param>
		void InstallBindings(IContainer container);
	}
}

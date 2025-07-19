using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Contexts;
using UnityEngine;

namespace TravisRFrench.Dependencies.Installers
{
	/// <summary>
	/// Base class for MonoBehaviour-based installers that register bindings into a container at runtime.
	/// </summary>
	/// <remarks>
	/// Commonly used in conjunction with <see cref="SceneContext"/>.
	/// </remarks>
	public abstract class MonoInstaller : MonoBehaviour, IInstaller
	{
		/// <summary>
		/// Called by the container to install bindings for this MonoInstaller.
		/// </summary>
		/// <param name="container">The container to register bindings with.</param>
		public abstract void InstallBindings(IContainer container);
	}
}

using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Contexts;
using UnityEngine;

namespace TravisRFrench.Dependencies.Installers
{
	/// <summary>
	/// An abstract base class for defining DI bindings in a <see cref="ScriptableObject"/>.
	/// Used by <see cref="GlobalContext"/> and <see cref="SceneContext"/> to install dependencies.
	/// </summary>
	public abstract class ScriptableInstaller : ScriptableObject, IInstaller
	{
		/// <summary>
		/// Installs bindings into the provided container. Called automatically by the context.
		/// </summary>
		/// <param name="container">The container to register bindings into.</param>
		public abstract void InstallBindings(IContainer container);
	}
}

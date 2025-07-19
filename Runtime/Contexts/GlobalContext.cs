using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;

namespace TravisRFrench.Dependencies.Contexts
{
	/// <summary>
	/// A <see cref="ScriptableObject"/>-based implementation of <see cref="IContext"/> that initializes 
	/// a global container and installs all registered <see cref="ScriptableInstaller"/>s at application startup.
	/// </summary>
	[CreateAssetMenu(menuName = "Scriptables/DI/Global Context")]
	public class GlobalContext : ScriptableObject, IContext
	{
		[SerializeField]
		private List<ScriptableInstaller> installers;

		private static IContainer container;

		/// <summary>
		/// Gets the global container initialized at runtime from the GlobalContext asset.
		/// </summary>
		public static IContainer Container => container;

		private static bool isInitialized;

		/// <summary>
		/// Initializes the <see cref="GlobalContext"/> and all its installers before the first scene loads.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			if (isInitialized)
			{
				return;
			}

			isInitialized = true;

			var asset = Resources.Load<GlobalContext>("Global Context");
			if (asset == null)
			{
				Debug.LogWarning("[DI] GlobalContext asset not found in Resources.");
				return;
			}

			container = new Container();

			foreach (var installer in asset.installers)
			{
				try
				{
					installer.InstallBindings(container);
				}
				catch (Exception e)
				{
					Debug.LogError($"[DI] Installer {installer.name} failed:\n{e}");
					throw;
				}
			}
		}

		/// <inheritdoc/>
		IContainer IContext.Container => container;
	}
}

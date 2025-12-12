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
		
		/// <inheritdoc/>
		IContainer IContext.Container => container;

		/// <summary>
		/// Gets the global container initialized at runtime from the GlobalContext asset.
		/// </summary>
		public static IContainer Container => container;

		private static bool isInitialized;
		private static GlobalContext instance;

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

			var wasLoaded = LoadGlobalContext();

			if (!wasLoaded)
			{
				return;
			}
			
			container = new Container();
			RunInstallers();
			
			isInitialized = true;
		}

		private static bool LoadGlobalContext()
		{
			instance = Resources.Load<GlobalContext>("Global Context");
			
			if (instance == null)
			{
				Debug.LogWarning("[DI] GlobalContext asset not found in Resources.");
				return false;
			}

			return true;
		}
		
		private static void RunInstallers()
		{
			foreach (var installer in instance.installers)
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
	}
}

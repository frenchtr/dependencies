using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Containers.Registration;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;

namespace TravisRFrench.Dependencies.Contexts
{
    /// <summary>
    /// Global DI composition root. Initializes the global container and runs global installers.
    /// Scene bootstrapping is handled by SceneContextBootstrapper.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptables/DI/Global Context")]
    public class GlobalContext : ScriptableObject, IContext
    {
        [SerializeField] private List<ScriptableInstaller> installers;

        private static ContextRegistry contextRegistry;
        private static IContainer container;

        private static bool isInitialized;
        private static GlobalContext instance;

        /// <inheritdoc/>
        IContainer IContext.Container => container;

        public static GlobalContext Instance => instance;

        public static IContainer Container => container;

        public static IContextRegistry ContextRegistry => contextRegistry;
        
        internal static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            if (!LoadGlobalContext())
            {
                return;
            }

            contextRegistry = new ContextRegistry();
            container = new Container();

            // Optional: register global context under a reserved key for explicit parenting.
            contextRegistry.Register("Global", instance);

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
            if (instance.installers == null)
            {
                return;
            }

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

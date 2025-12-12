using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TravisRFrench.Dependencies.Contexts
{
    /// <summary>
    /// Provides a scene-level DI container that inherits from the global context.
    /// Installs all bindings from <see cref="MonoInstaller"/> and <see cref="ScriptableInstaller"/> sources,
    /// and injects all scene objects automatically on load.
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class SceneContext : MonoBehaviour, IContext
    {
        [SerializeField]
        private List<MonoInstaller> monoInstallers;
        [SerializeField]
        private List<ScriptableInstaller> scriptableInstallers;

        private static SceneContext Instance { get; set; }

        /// <inheritdoc/>
        public IContainer Container { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[DI] Multiple SceneContext instances found. Only one will be active.");
                Destroy(this);
                return;
            }

            Instance = this;

            var parent = GlobalContext.Container;
            this.Container = parent.CreateChildContainer();

            this.InstallBindings();
            this.InjectSceneObjects();
        }

        private void InstallBindings()
        {
            var allInstallers = new List<IInstaller>();

            if (this.monoInstallers != null)
            {
                allInstallers.AddRange(this.monoInstallers);
            }

            if (this.scriptableInstallers != null)
            {
                allInstallers.AddRange(this.scriptableInstallers);
            }

            foreach (var installer in allInstallers)
            {
                try
                {
                    installer.InstallBindings(this.Container);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[DI] Installer {installer.GetType().Name} failed:\n{e}");
                    throw;
                }
            }
        }

        private void InjectSceneObjects()
        {
            var objects = FindObjectsByType<Object>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var obj in objects)
            {
                if (obj is MonoBehaviour or ScriptableObject)
                {
                    this.Container.Inject(obj);
                }
            }
        }
    }
}

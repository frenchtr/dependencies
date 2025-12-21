using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TravisRFrench.Dependencies.Contexts
{
    /// <summary>
    /// Provides a scene-level DI container that inherits from a parent context container
    /// (or the GlobalContext container by default). Initialization is deterministic and
    /// performed by GlobalContext after the scene loads.
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class SceneContext : MonoBehaviour, IContext
    {
        [Header("Keys")]
        [SerializeField] private string key;
        [SerializeField] private string parentKey;
        [Header("Installers")]
        [SerializeField] private List<MonoInstaller> monoInstallers;
        [SerializeField] private List<ScriptableInstaller> scriptableInstallers;

        public string Key => this.key;
        public string ParentKey => this.parentKey;

        /// <inheritdoc/>
        public IContainer Container { get; private set; }

        private List<IInstaller> allInstallers;

        private void Awake()
        {
            // Phase 1: Register the context instance only (no container creation).
            // This is what makes parent lookup deterministic later.
            GlobalContext.ContextRegistry.Register(this.key, this);

            // Cache installers now; install later during Initialize().
            this.allInstallers = new List<IInstaller>();
            if (this.monoInstallers != null) this.allInstallers.AddRange(this.monoInstallers);
            if (this.scriptableInstallers != null) this.allInstallers.AddRange(this.scriptableInstallers);
        }

        /// <summary>
        /// Phase 2: Called by GlobalContext after scene load, in deterministic order.
        /// </summary>
        internal void Initialize(IContainer parent)
        {
            if (this.Container != null)
            {
                // Guard against double-initialization.
                return;
            }

            this.Container = parent.CreateChildContainer();

            InstallBindings();
            InjectSceneObjects();
        }

        private void InstallBindings()
        {
            foreach (var installer in this.allInstallers)
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
            var sceneContextScene = this.gameObject.scene;

            // NOTE: This is intentionally performed after all contexts are initialized for the scene.
            var objects = FindObjectsByType<Object>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

            foreach (var obj in objects)
            {
                if (obj is not MonoBehaviour monoBehaviour)
                {
                    continue;
                }

                if (monoBehaviour.gameObject.scene != sceneContextScene)
                {
                    continue;
                }

                this.Container.Inject(obj);
            }
        }
    }
}

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

        public IList<MonoInstaller> MonoInstallers => this.monoInstallers;
        public IList<ScriptableInstaller> ScriptableInstallers => this.scriptableInstallers;

        private void Awake()
        {
            // Phase 1: Register the context instance only (no container creation).
            // This is what makes parent lookup deterministic later.
            GlobalContext.ContextRegistry.Register(this.key, this);
        }

        private void OnDestroy()
        {
            // Optional but recommended; avoids stale registry entries on scene unload.
            if (!string.IsNullOrWhiteSpace(this.key))
            {
                GlobalContext.ContextRegistry.Unregister(this.key);
            }
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
            var allInstallers = this.GetAllInstallers();
            
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

                // IMPORTANT: GameObjectContexts own injection for their subtrees.
                // This ensures GO-level resolution happens first for those objects.
                if (monoBehaviour.GetComponentInParent<GameObjectContext>(true) != null)
                {
                    continue;
                }

                this.Container.Inject(obj);
            }
        }
        
        private IEnumerable<IInstaller> GetAllInstallers()
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

            return allInstallers;
        }
    }
}

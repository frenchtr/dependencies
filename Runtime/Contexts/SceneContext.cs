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
    /// (or the GlobalContext container by default). Owns its full initialization lifecycle;
    /// defers into a static pending list when a parent context is not yet available, then
    /// retries all pending contexts via TryFlushPending() after each successful initialization.
    /// </summary>
    [DefaultExecutionOrder(-32000)]
    public class SceneContext : MonoBehaviour, IContext
    {
        private static readonly List<SceneContext> pending = new();

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
            if (string.IsNullOrWhiteSpace(this.key))
            {
                throw new InvalidOperationException(
                    $"[DI] SceneContext on '{this.gameObject.name}' in scene '{this.gameObject.scene.name}' has an empty key."
                );
            }

            GlobalContext.ContextRegistry.Register(this.key, this);
            this.TryInitialize();
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrWhiteSpace(this.key))
            {
                GlobalContext.ContextRegistry.Unregister(this.key);
            }

            pending.Remove(this);
        }

        private bool TryInitialize()
        {
            if (this.Container != null)
            {
                return false;
            }

            if (!GlobalContext.IsInitialized)
            {
                return false;
            }

            if (!this.TryResolveParentContainer(out var parent))
            {
                if (!pending.Contains(this))
                {
                    pending.Add(this);
                }

                return false;
            }

            this.Initialize(parent);
            pending.Remove(this);
            TryFlushPending();

            return true;
        }

        private bool TryResolveParentContainer(out IContainer parent)
        {
            parent = null;

            if (string.IsNullOrWhiteSpace(this.parentKey))
            {
                parent = GlobalContext.Container;
                return true;
            }

            if (!GlobalContext.ContextRegistry.TryGet(this.parentKey, out var parentContext) || parentContext == null)
            {
                return false;
            }

            if (parentContext.Container == null)
            {
                return false;
            }

            parent = parentContext.Container;
            return true;
        }

        private static void TryFlushPending()
        {
            bool madeProgress;
            var passGuard = 0;

            do
            {
                madeProgress = false;
                passGuard++;

                if (passGuard > 256)
                {
                    throw new InvalidOperationException(
                        "[DI] SceneContext.TryFlushPending exceeded pass guard (256). Likely a parentKey cycle."
                    );
                }

                // Iterate a snapshot to allow the list to be mutated during iteration.
                for (var i = pending.Count - 1; i >= 0; i--)
                {
                    var ctx = pending[i];
                    if (ctx.TryInitialize())
                    {
                        madeProgress = true;
                    }
                }
            }
            while (madeProgress);

            if (pending.Count > 0)
            {
                foreach (var ctx in pending)
                {
                    Debug.LogWarning(
                        $"[DI] SceneContext '{ctx.Key}' could not be initialized: parentKey '{ctx.ParentKey}' is not available. " +
                        $"Scene: '{ctx.gameObject.scene.name}'."
                    );
                }
            }
        }

        /// <summary>
        /// Initializes this context's container as a child of the given parent container,
        /// installs bindings, and injects scene objects.
        /// </summary>
        internal void Initialize(IContainer parent)
        {
            if (this.Container != null)
            {
                return;
            }

            this.Container = parent.CreateChildContainer();

            this.InstallBindings();
            this.InjectSceneObjects();
        }

        private void InstallBindings()
        {
            foreach (var installer in this.GetAllInstallers())
            {
                try
                {
                    installer.InstallBindings(this.Container);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[DI Binding] Installer '{installer.GetType().Name}' on SceneContext " +
                                   $"'{this.key}' failed to install bindings:\n{e}");
                }
            }
        }

        private void InjectSceneObjects()
        {
            var sceneContextScene = this.gameObject.scene;

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
                if (monoBehaviour.GetComponentInParent<GameObjectContext>(true) != null)
                {
                    continue;
                }

                try
                {
                    this.Container.Inject(obj);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex, monoBehaviour);
                }
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

using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TravisRFrench.Dependencies.Contexts
{
    /// <summary>
    /// Provides a GameObject-level DI container that inherits from the nearest parent
    /// GameObjectContext, or the owning SceneContext as the fallback. Owns its full
    /// initialization lifecycle via Awake().
    /// </summary>
    [DefaultExecutionOrder(-31999)]
    public class GameObjectContext : MonoBehaviour, IContext
    {
        private static readonly List<GameObjectContext> pending = new();

        [Header("Installers")]
        [SerializeField] private List<MonoInstaller> monoInstallers;
        [SerializeField] private List<ScriptableInstaller> scriptableInstallers;

        public string Key { get; private set; }

        public IContainer Container { get; private set; }

        private bool registered;

        private void Awake()
        {
            this.Key = this.GenerateKey();

            if (!this.registered)
            {
                GlobalContext.ContextRegistry.Register(this.Key, this);
                this.registered = true;
            }

            this.TryInitialize();
        }

        private void OnDestroy()
        {
            if (this.registered && !string.IsNullOrWhiteSpace(this.Key))
            {
                GlobalContext.ContextRegistry.Unregister(this.Key);
                this.registered = false;
            }

            pending.Remove(this);
        }

        private void TryInitialize()
        {
            if (this.Container != null)
            {
                return;
            }

            var parentContainer = this.ResolveParentContainer();

            // Defensive: SceneContext at -32000 guarantees it runs before this at -31999,
            // so a null parent container is only reachable in unusual runtime circumstances.
            if (parentContainer == null)
            {
                if (!pending.Contains(this))
                {
                    pending.Add(this);
                }

                return;
            }

            this.Initialize(parentContainer);
            pending.Remove(this);
        }

        private IContainer ResolveParentContainer()
        {
            // Walk up the hierarchy for the nearest initialized GameObjectContext.
            var t = this.transform.parent;
            while (t != null)
            {
                if (t.TryGetComponent<GameObjectContext>(out var parentContext))
                {
                    return parentContext.Container;
                }

                t = t.parent;
            }

            // Fall back to the SceneContext that belongs to the same scene.
            // GetComponentInParent is intentionally NOT used here — a GameObjectContext
            // root is not required to be a child of the SceneContext GameObject in the hierarchy.
            var thisScene = this.gameObject.scene;
            var sceneContexts = FindObjectsByType<SceneContext>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

            foreach (var sc in sceneContexts)
            {
                if (sc.gameObject.scene == thisScene)
                {
                    return sc.Container;
                }
            }

            return null;
        }

        /// <summary>
        /// Initializes this context's container as a child of the given parent container,
        /// installs bindings, and injects objects within this subtree.
        /// </summary>
        internal void Initialize(IContainer parentContainer)
        {
            if (this.Container != null)
            {
                return;
            }

            if (parentContainer == null)
            {
                throw new InvalidOperationException(
                    "[DI] GameObjectContext requires a parent container, but Initialize() was called with null."
                );
            }

            this.Container = parentContainer.CreateChildContainer();

            this.InstallBindings();
            this.InjectHierarchyObjects();
        }

        private string GenerateKey()
        {
            var scene = this.gameObject.scene;
            return $"{scene.handle}/{this.gameObject.name}/{this.GetInstanceID()}";
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
                    Debug.LogError($"[DI Binding] Installer '{installer.GetType().Name}' on GameObjectContext " +
                                   $"'{this.gameObject.name}' failed to install bindings:\n{e}");
                }
            }
        }

        private void InjectHierarchyObjects()
        {
            var behaviours = this.GetComponentsInChildren<MonoBehaviour>(true);

            foreach (var mb in behaviours)
            {
                if (mb == null)
                {
                    continue;
                }

                if (mb is IContext)
                {
                    continue;
                }

                // IMPORTANT: Do not inject objects that belong to a nested GameObjectContext subtree.
                // The closest (deepest) GameObjectContext should own injection for those objects.
                var owningContext = mb.GetComponentInParent<GameObjectContext>(true);
                if (owningContext != this)
                {
                    continue;
                }

                try
                {
                    this.Container.Inject((Object)mb);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex, mb);
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

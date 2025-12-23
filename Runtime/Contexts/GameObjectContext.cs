using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Installers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TravisRFrench.Dependencies.Contexts
{
    [DefaultExecutionOrder(-9998)]
    public class GameObjectContext : MonoBehaviour, IContext
    {
        [Header("Installers")]
        [SerializeField] private List<MonoInstaller> monoInstallers;
        [SerializeField] private List<ScriptableInstaller> scriptableInstallers;

        public string Key { get; private set; }

        public IContainer Container { get; private set; }

        private bool registered;

        private void Awake()
        {
            this.Key = GenerateKey();

            // Phase 1: register instance (no container yet).
            // Guard avoids duplicate registration attempts for the same instance in edge cases.
            if (!this.registered)
            {
                GlobalContext.ContextRegistry.Register(this.Key, this);
                this.registered = true;
            }
        }

        private void OnDestroy()
        {
            if (this.registered && !string.IsNullOrWhiteSpace(this.Key))
            {
                GlobalContext.ContextRegistry.Unregister(this.Key);
                this.registered = false;
            }
        }

        internal void Initialize(IContainer activeSceneContainer)
        {
            if (this.Container != null)
            {
                return;
            }

            if (activeSceneContainer == null)
            {
                throw new InvalidOperationException(
                    "[DI] GameObjectContext requires an activeSceneContainer, but Initialize() was called with null."
                );
            }

            var parentContainer = ResolveParentContainer(activeSceneContainer);
            this.Container = parentContainer.CreateChildContainer();

            InstallBindings();
            InjectHierarchyObjects();
        }

        private string GenerateKey()
        {
            var scene = this.gameObject.scene;
            return $"{scene.handle}/{this.gameObject.name}/{this.GetInstanceID()}";
        }

        private IContainer ResolveParentContainer(IContainer activeSceneContainer)
        {
            var t = this.transform.parent;
            while (t != null)
            {
                if (t.TryGetComponent<GameObjectContext>(out var parentContext))
                {
                    if (parentContext.Container == null)
                    {
                        throw new InvalidOperationException(
                            $"[DI] Parent GameObjectContext '{parentContext.Key}' has not been initialized yet. " +
                            "Bootstrap order must initialize parents before children."
                        );
                    }

                    return parentContext.Container;
                }

                t = t.parent;
            }

            return activeSceneContainer;
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

                this.Container.Inject((Object)mb);
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

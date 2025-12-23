using System;
using System.Collections.Generic;
using System.Linq;
using TravisRFrench.Dependencies.Containers;
using TravisRFrench.Dependencies.Contexts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TravisRFrench.Dependencies
{
    /// <summary>
    /// Bootstraps SceneContexts deterministically after scene load.
    /// Supports deferred initialization when parent contexts load later (additive or staged loads).
    ///
    /// Also bootstraps GameObjectContexts after SceneContexts, ensuring parent-before-child order.
    /// </summary>
    public static class Bootstrapper
    {
        private static bool hooked;

        // contexts waiting for their parent context/container to exist
        private static readonly Dictionary<string, SceneContext> pending = new(StringComparer.Ordinal);

        // initialized keys (avoid double init)
        private static readonly HashSet<string> initializedSceneKeys = new(StringComparer.Ordinal);

        // initialized GameObjectContexts (avoid double init)
        private static readonly HashSet<int> initializedGameObjectContextIds = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            GlobalContext.Initialize();
            
            // If GlobalContext wasn't present, there is nothing to bootstrap.
            if (GlobalContext.Container == null || GlobalContext.ContextRegistry == null)
            {
                return;
            }

            HookOnce();

            // Seed with all loaded scenes
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                {
                    continue;
                }

                EnqueueSceneContexts(scene);
            }

            TryInitializeAllPending(logUnresolved: true);

            // After scene contexts initialize, initialize GO contexts.
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                {
                    continue;
                }

                InitializeGameObjectContexts(scene);
            }
        }

        private static void HookOnce()
        {
            if (hooked)
            {
                return;
            }
            hooked = true;

            SceneManager.sceneLoaded += (scene, _) =>
            {
                // Enqueue contexts for the scene that just loaded.
                EnqueueSceneContexts(scene);

                // Also enqueue any other loaded scenes (covers additive loads).
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var s = SceneManager.GetSceneAt(i);
                    if (!s.isLoaded)
                    {
                        continue;
                    }

                    EnqueueSceneContexts(s);
                }

                TryInitializeAllPending(logUnresolved: true);

                // After scene contexts initialize, initialize GO contexts.
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var s = SceneManager.GetSceneAt(i);
                    if (!s.isLoaded)
                    {
                        continue;
                    }

                    InitializeGameObjectContexts(s);
                }
            };
        }

        private static void EnqueueSceneContexts(Scene scene)
        {
            var contexts = GameObject.FindObjectsByType<SceneContext>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.InstanceID
                )
                .Where(c => c != null && c.gameObject.scene == scene)
                .ToList();

            foreach (var ctx in contexts)
            {
                if (string.IsNullOrWhiteSpace(ctx.Key))
                {
                    throw new InvalidOperationException($"[DI] SceneContext in scene '{scene.name}' has an empty key.");
                }

                if (initializedSceneKeys.Contains(ctx.Key))
                {
                    continue;
                }

                if (pending.TryGetValue(ctx.Key, out var existing) && existing != ctx)
                {
                    throw new InvalidOperationException(
                        $"[DI] Duplicate SceneContext key '{ctx.Key}'. " +
                        $"Existing scene: '{existing.gameObject.scene.name}', New scene: '{scene.name}'. " +
                        $"Keys must be globally unique when used for parenting.");
                }

                pending[ctx.Key] = ctx;

                // IMPORTANT:
                // Do NOT register here; SceneContext.Awake() already registers.
                // Registering again can throw if the registry doesn't support duplicates.
            }
        }

        private static void TryInitializeAllPending(bool logUnresolved)
        {
            // Deterministic: process keys sorted
            bool madeProgress;
            int passGuard = 0;

            do
            {
                madeProgress = false;
                passGuard++;

                if (passGuard > 256)
                {
                    throw new InvalidOperationException("[DI] Bootstrapper exceeded pass guard. Likely a cycle or repeated re-adding.");
                }

                var keys = pending.Keys.OrderBy(k => k, StringComparer.Ordinal).ToList();

                foreach (var key in keys)
                {
                    var ctx = pending[key];

                    if (!TryResolveParentContainer(ctx, out var parent))
                    {
                        continue; // parent not available yet
                    }

                    ctx.Initialize(parent);

                    initializedSceneKeys.Add(ctx.Key);
                    pending.Remove(ctx.Key);
                    madeProgress = true;
                }
            }
            while (madeProgress);

            if (logUnresolved && pending.Count > 0)
            {
                foreach (var ctx in pending.Values.OrderBy(c => c.Key, StringComparer.Ordinal))
                {
                    Debug.LogWarning(
                        $"[DI] SceneContext '{ctx.Key}' pending: parentKey '{ctx.ParentKey}' not available yet. " +
                        $"Scene '{ctx.gameObject.scene.name}'.");
                }
            }
        }

        private static bool TryResolveParentContainer(SceneContext ctx, out IContainer parent)
        {
            parent = null;

            if (string.IsNullOrWhiteSpace(ctx.ParentKey))
            {
                parent = GlobalContext.Container;
                return true;
            }

            if (!GlobalContext.ContextRegistry.TryGet(ctx.ParentKey, out var parentContext) || parentContext == null)
            {
                // Parent context not loaded/registered yet
                return false;
            }

            if (parentContext.Container == null)
            {
                // Parent exists but has not been initialized yet
                return false;
            }

            parent = parentContext.Container;
            return true;
        }

        private static void InitializeGameObjectContexts(Scene scene)
        {
            var activeSceneContext = GetActiveSceneContextForScene(scene);

            // If this scene has no SceneContext (or it hasn't been initialized yet), skip for now.
            if (activeSceneContext == null || activeSceneContext.Container == null)
            {
                return;
            }

            var goContexts = GameObject.FindObjectsByType<GameObjectContext>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.InstanceID
                )
                .Where(c => c != null && c.gameObject.scene == scene)
                .ToArray();

            // Parent-before-child: sort by transform depth, then InstanceID for determinism.
            Array.Sort(goContexts, (a, b) =>
            {
                int da = GetDepth(a.transform);
                int db = GetDepth(b.transform);

                int cmp = da.CompareTo(db);
                return cmp != 0 ? cmp : a.GetInstanceID().CompareTo(b.GetInstanceID());
            });

            foreach (var ctx in goContexts)
            {
                int id = ctx.GetInstanceID();
                if (initializedGameObjectContextIds.Contains(id))
                {
                    continue;
                }

                ctx.Initialize(activeSceneContext.Container);
                initializedGameObjectContextIds.Add(id);
            }
        }

        private static SceneContext GetActiveSceneContextForScene(Scene scene)
        {
            // Deterministic selection rule:
            // Pick the SceneContext in this scene with the lowest InstanceID.
            // (If you want exactly one per scene, enforce that here.)
            var contexts = GameObject.FindObjectsByType<SceneContext>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.InstanceID
                )
                .Where(c => c != null && c.gameObject.scene == scene)
                .OrderBy(c => c.GetInstanceID())
                .ToList();

            if (contexts.Count == 0)
            {
                return null;
            }

            return contexts[0];
        }

        private static int GetDepth(Transform t)
        {
            int depth = 0;
            while (t.parent != null)
            {
                depth++;
                t = t.parent;
            }
            return depth;
        }
    }
}

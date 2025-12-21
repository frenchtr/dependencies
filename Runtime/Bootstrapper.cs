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
    /// </summary>
    public static class Bootstrapper
    {
        private static bool hooked;

        // contexts waiting for their parent context/container to exist
        private static readonly Dictionary<string, SceneContext> pending = new(StringComparer.Ordinal);

        // initialized keys (avoid double init)
        private static readonly HashSet<string> initialized = new(StringComparer.Ordinal);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            if (GlobalContext.Container == null || GlobalContext.ContextRegistry == null)
            {
                return;
            }

            HookOnce();

            // Seed with all loaded scenes
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                EnqueueSceneContexts(scene);
            }

            TryInitializeAllPending(logUnresolved: true);
        }

        private static void HookOnce()
        {
            if (hooked) return;
            hooked = true;

            SceneManager.sceneLoaded += (_, __) =>
            {
                // after any scene load, enqueue and retry
                var scene = SceneManager.GetActiveScene();
                EnqueueSceneContexts(scene);

                // also enqueue any other loaded scenes (covers additive loads where active scene isn't the new one)
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var s = SceneManager.GetSceneAt(i);
                    if (!s.isLoaded) continue;
                    EnqueueSceneContexts(s);
                }

                TryInitializeAllPending(logUnresolved: true);
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

                if (initialized.Contains(ctx.Key))
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

                // Ensure the context instance is in the registry so others can reference it by key.
                // IMPORTANT: if Register throws on duplicate keys, you need TryRegister/RegisterOrReplace.
                GlobalContext.ContextRegistry.Register(ctx.Key, ctx);
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

                    initialized.Add(ctx.Key);
                    pending.Remove(ctx.Key);
                    madeProgress = true;
                }
            }
            while (madeProgress);

            // Optional: warn if something is still pending
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
    }
}

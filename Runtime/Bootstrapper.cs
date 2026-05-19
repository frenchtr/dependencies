using TravisRFrench.Dependencies.Contexts;
using UnityEngine;

namespace TravisRFrench.Dependencies
{
    /// <summary>
    /// Static entry point for the DI system. Initializes GlobalContext before any scene loads.
    /// All further initialization is handled by SceneContext and GameObjectContext via their Awake() methods.
    /// </summary>
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            GlobalContext.Initialize();

            // If GlobalContext wasn't present, there is nothing to bootstrap.
            if (GlobalContext.Container == null)
            {
                return;
            }
        }
    }
}

using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            Dependencies.Initialize();
            InjectMonoBehaviours(Dependencies.Context);
        }
        
        private static void InjectMonoBehaviours(IContext context)
        {
            var container = context.Container;
            var monoBehaviours = Resources.FindObjectsOfTypeAll<MonoBehaviour>();

            foreach (var monoBehaviour in monoBehaviours)
            {
                container.Inject(monoBehaviour);
            }
        }
    }
}

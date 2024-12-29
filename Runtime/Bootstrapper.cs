using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            var runtimeContext = InitializeRuntimeContext();
            InjectMonoBehaviours(runtimeContext);
        }

        public static IContext InitializeRuntimeContext()
        {
            var context = Dependencies.Settings.RuntimeContext;
            Dependencies.SetContext(context);
            
            context.Initialize();
            context.Setup(context.Container);

            return context;
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

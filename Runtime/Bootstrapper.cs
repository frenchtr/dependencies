using System;
using TravisRFrench.Dependencies.Runtime.Contextualization;
using TravisRFrench.Dependencies.Runtime.Injection;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            SetupContexts();
            Inject();
        }

        private static void SetupContexts()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    // If the type implements the IContext interface...
                    if (typeof(IContext).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                    {
                    }
                }
            }
        }

        private static void Inject()
        {
            
        }
    }
}
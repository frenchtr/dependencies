﻿using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime
{
    public static class Dependencies
    {
        private static DependenciesSettings settings;
        
        public static DependenciesSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = Resources.Load<DependenciesSettings>("DependencyInjectionSettings");
                }

                return settings;
            }
        }
        
        public static IContext Context { get; private set; }
        
        public static void SetContext(IContext context)
        {
            Context = context;
        }
    }
}
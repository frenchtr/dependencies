using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Runtime.Binding;

namespace TravisRFrench.Dependencies.Runtime.Registration
{
    public class Registry : IRegistry
    {
        private readonly Dictionary<Type, IBinding> bindings;

        public Registry()
        {
            this.bindings = new();
        }
        
        public void Register(IBinding binding)
        {
            this.bindings[binding.InterfaceType] = binding;
        }

        public IBinding Get<T>()
        {
            return this.bindings[typeof(T)];
        }

        public IBinding Get(Type type)
        {
            return this.bindings[type];
        }
    }
}

using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Scopes;

namespace TravisRFrench.Dependencies.Runtime.Registration
{
    public class Registry : IRegistry
    {
        private readonly IScope scope;
        private readonly Dictionary<Type, IBinding> bindings;

        public Registry(IScope scope)
        {
            this.scope = scope;
            this.bindings = new();
        }
        
        public void Register(IBinding binding)
        {
            this.bindings[binding.InterfaceType] = binding;
        }

        public IBinding Get<T>()
        {
            return this.Get(typeof(T));
        }

        public IBinding Get(Type type)
        {
            if (!this.TryGet(type, out var binding))
            {
                throw new BindingNotFoundException(type, $"Unable to locate binding for type {type}.");
            }

            return binding;
        }

        public bool TryGet(Type type, out IBinding binding)
        {
            binding = this.bindings[type];
            var workingScope = this.scope;
            
            while (binding == null && workingScope.Parent != null)
            {
                try
                {
                    workingScope = workingScope.Parent;
                    binding = workingScope.Get(type);
                }
                catch (BindingNotFoundException exception)
                {
                    continue;
                }
            }

            return binding != null;
        }
    }
}

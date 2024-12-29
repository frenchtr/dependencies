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
            if (this.DoTryGet(type, out var binding))
            {
                return binding;
            }

            throw new BindingNotFoundException(type, $"Unable to locate binding for type {type}.");
        }

        public bool TryGet(Type type, out IBinding binding)
        {
            return this.DoTryGet(type, out binding);
        }

        private bool DoTryGet(Type type, out IBinding binding)
        {
            // Check the current registry
            if (this.bindings.TryGetValue(type, out binding))
            {
                return true;
            }

            // Check the parent scope
            var parent = this.scope.Parent;
            if (parent != null)
            {
                return parent.TryGet(type, out binding);
            }

            // Not found
            binding = null;
            return false;
        }
    }
}

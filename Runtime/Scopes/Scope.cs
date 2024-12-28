using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Containerization;
using TravisRFrench.Dependencies.Runtime.Injection;
using TravisRFrench.Dependencies.Runtime.Registration;
using TravisRFrench.Dependencies.Runtime.Resolution;

namespace TravisRFrench.Dependencies.Runtime.Scopes
{
    public class Scope : IScope
    {
        private readonly IRegistry registry;
        private readonly IResolver resolver;
        private readonly IInjector injector;

        public IContainer Container { get; }
        public IScope Parent { get; }
        
        public Scope(IContainer container, IScope parent, Stack<Type> resolveStack)
        {
            this.Container = container;
            this.Parent = parent;
            
            this.registry = new Registry(this);
            this.resolver = new Resolver(this, resolveStack);
            this.injector = new Injector(this);
        }
        
        public void Register(IBinding binding)
        {
            this.registry.Register(binding);
        }

        public IBinding Get<T>()
        {
            return this.registry.Get<T>();
        }

        public IBinding Get(Type type)
        {
            return this.registry.Get(type);
        }

        public object Resolve(Type type)
        {
            return this.resolver.Resolve(type);
        }

        public T Resolve<T>()
        {
            return this.resolver.Resolve<T>();
        }

        public void Inject<T>(T target)
        {
            this.injector.Inject(target);
        }
    }
}

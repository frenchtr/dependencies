using System;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Injection;
using TravisRFrench.Dependencies.Runtime.Registration;
using TravisRFrench.Dependencies.Runtime.Resolution;

namespace TravisRFrench.Dependencies.Runtime.Containerization
{
    public class Container : IContainer
    {
        private readonly IRegistry registry;
        private readonly IResolver resolver;
        private readonly IInjector injector;
        
        public Container()
        {
            this.registry = new Registry();
            this.resolver = new Resolver(this.registry);
            this.injector = new Injector(this);
        }

        public object Resolve(Type type)
        {
            return this.resolver.Resolve(type);
        }

        public T Resolve<T>()
        {
            return this.resolver.Resolve<T>();
        }
        
        public void Inject<T>(T obj)
        {
            this.injector.Inject(obj);
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
    }
}

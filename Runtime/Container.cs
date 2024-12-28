using System;

namespace TravisRFrench.Dependencies.Runtime
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
        
        public void Register<T>(T obj)
        {
            this.registry.Register(obj);
        }

        public T Get<T>()
        {
            return this.registry.Get<T>();
        }

        public object Get(Type type)
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
        
        public void Inject<T>(T obj)
        {
            this.injector.Inject(obj);
        }
    }
}

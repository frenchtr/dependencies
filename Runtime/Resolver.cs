using System;

namespace TravisRFrench.Dependencies.Runtime
{
    public class Resolver : IResolver
    {
        private readonly IRegistry registry;

        public Resolver(IRegistry registry)
        {
            this.registry = registry;
        }
        
        public object Resolve(Type type)
        {
            return this.registry.Get(type);
        }
        
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }
    }
}
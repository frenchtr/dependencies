using System;
using TravisRFrench.Dependencies.Runtime.Registration;

namespace TravisRFrench.Dependencies.Runtime.Resolution
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
            var binding = this.registry.Get(type);

            return binding.Instance;
        }
        
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }
    }
}

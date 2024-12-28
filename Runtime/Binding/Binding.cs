using System;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public class Binding : IBinding
    {
        public Type InterfaceType { get; }
        public Type ImplementationType { get; }
        public object Instance { get; }
        public Func<object> Factory { get; }
        public SourceType SourceType { get; }
        public Lifetime Lifetime { get; }

        public Binding(Type interfaceType, Type implementationType, SourceType sourceType = SourceType.FromInstance, Lifetime lifetime = Lifetime.Transient)
        {
            this.InterfaceType = interfaceType;
            this.ImplementationType = implementationType;

            this.SourceType = sourceType;
            this.Lifetime = lifetime;
        }
        
        public Binding(Type interfaceType, Type implementationType, object instance, SourceType sourceType = SourceType.FromInstance, Lifetime lifetime = Lifetime.Transient)
            : this(interfaceType, implementationType, sourceType, lifetime)
        {
            this.Instance = instance;
        }
        
        public Binding(Type interfaceType, Type implementationType, Func<object> factory, SourceType sourceType = SourceType.FromFactory, Lifetime lifetime = Lifetime.Transient)
            : this(interfaceType, implementationType, sourceType, lifetime)
        {
            this.Factory = factory;
        }
    }
}

using System;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public class Binding : IBinding
    {
        public Type InterfaceType { get; }
        public Type ImplementationType { get; }
        public object Instance { get; }
        public SourceType SourceType { get; }
        public Lifetime Lifetime { get; }

        public Binding(Type interfaceType, Type implementationType, object instance, SourceType sourceType = SourceType.FromInstance, Lifetime lifetime = Lifetime.Transient)
        {
            this.InterfaceType = interfaceType;
            this.ImplementationType = implementationType;
            this.Instance = instance;
            this.SourceType = sourceType;
            this.Lifetime = lifetime;
        }
    }
}

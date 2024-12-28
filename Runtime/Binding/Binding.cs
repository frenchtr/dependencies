using System;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public class Binding : IBinding
    {
        public Type InterfaceType { get; }
        public Type ImplementationType { get; }
        public object Instance { get; }

        public Binding(Type interfaceType, Type implementationType, object instance)
        {
            this.InterfaceType = interfaceType;
            this.ImplementationType = implementationType;
            this.Instance = instance;
        }
    }
}

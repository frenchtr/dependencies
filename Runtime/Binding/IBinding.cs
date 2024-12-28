using System;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public interface IBinding
    {
        Type InterfaceType { get; }
        Type ImplementationType { get; }
        object Instance { get; }
    }
}

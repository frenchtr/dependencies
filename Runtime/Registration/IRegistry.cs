using System;
using TravisRFrench.Dependencies.Runtime.Binding;

namespace TravisRFrench.Dependencies.Runtime.Registration
{
    public interface IRegistry
    {
        void Register(IBinding binding);
        IBinding Get<T>();
        IBinding Get(Type type);
    }
}

using System;

namespace TravisRFrench.Dependencies.Runtime
{
    public interface IResolver
    {
        object Resolve(Type type);
        T Resolve<T>();
    }
}

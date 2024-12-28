using System;

namespace TravisRFrench.Dependencies.Runtime.Resolution
{
    public interface IResolver
    {
        object Resolve(Type type);
        T Resolve<T>();
    }
}

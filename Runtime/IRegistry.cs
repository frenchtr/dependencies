using System;

namespace TravisRFrench.Dependencies.Runtime
{
    public interface IRegistry
    {
        void Register<T>(T obj);
        T Get<T>();
        object Get(Type type);
    }
}

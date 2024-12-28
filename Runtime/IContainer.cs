using System;

namespace TravisRFrench.Dependencies
{
    public interface IContainer
    {
        void Register<T>(T obj);
        object Resolve(Type type);
        T Resolve<T>();
    }
}

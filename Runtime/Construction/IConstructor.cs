using System;

namespace TravisRFrench.Dependencies.Runtime.Construction
{
    public interface IConstructor
    {
        object Construct(Type type);
    }
}

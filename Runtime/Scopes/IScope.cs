using System;
using TravisRFrench.Dependencies.Runtime.Containerization;

namespace TravisRFrench.Dependencies.Runtime.Scopes
{
    public interface IScope : IBindingContainer, IDisposable
    {
        IContainer Container { get; }
        IScope Parent { get; }
    }
}

using TravisRFrench.Dependencies.Runtime.Containerization;

namespace TravisRFrench.Dependencies.Runtime.Scopes
{
    public interface IScope : IBindingContainer
    {
        IContainer Container { get; }
        IScope Parent { get; }
    }
}

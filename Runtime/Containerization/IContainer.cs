using TravisRFrench.Dependencies.Runtime.Scopes;

namespace TravisRFrench.Dependencies.Runtime.Containerization
{
    public interface IContainer : IBindingContainer
    {
        IScope PushScope();
        void PopScope();
    }
}

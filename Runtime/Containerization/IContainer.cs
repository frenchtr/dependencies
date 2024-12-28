using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Injection;

namespace TravisRFrench.Dependencies.Runtime.Containerization
{
    public interface IContainer : IBindingContainer, IInjector 
    {
    }
}

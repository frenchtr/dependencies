using TravisRFrench.Dependencies.Runtime.Registration;
using TravisRFrench.Dependencies.Runtime.Resolution;

namespace TravisRFrench.Dependencies.Runtime.Containerization
{
    public interface IBindingContainer : IRegistry, IResolver
    {
    }
}

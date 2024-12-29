using TravisRFrench.Dependencies.Runtime.Containerization;

namespace TravisRFrench.Dependencies.Runtime.Contextualization
{
    public interface IContext
    {
        IContainer Container { get; }
        
        void Initialize();
        void Setup(IContainer container);
    }
}

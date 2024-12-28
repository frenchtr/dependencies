namespace TravisRFrench.Dependencies.Runtime
{
    public interface IContext
    {
        IContainer Container { get; }
        
        void Initialize();
    }
}

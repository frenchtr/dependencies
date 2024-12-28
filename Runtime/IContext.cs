namespace TravisRFrench.Dependencies
{
    public interface IContext
    {
        IContainer Container { get; }
        
        void Initialize();
    }
}

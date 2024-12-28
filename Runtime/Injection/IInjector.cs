namespace TravisRFrench.Dependencies.Runtime.Injection
{
    public interface IInjector
    {
        void Inject<T>(T target);
    }
}

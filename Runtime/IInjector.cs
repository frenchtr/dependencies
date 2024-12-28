namespace TravisRFrench.Dependencies.Runtime
{
    public interface IInjector
    {
        void Inject<T>(T obj);
    }
}

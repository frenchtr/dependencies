using TravisRFrench.Dependencies.Runtime.Containerization;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public static class BindingExtensions
    {
        public static IBindingBuilder<TInterface> Bind<TInterface>(this IBindingContainer container)
        {
            return new BindingBuilder<TInterface>(container);
        }
    }
}

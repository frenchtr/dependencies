namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public interface IBindingBuilder<TInterface>
    {
        BindingBuilder<TInterface> To<TImplementation>();
        BindingBuilder<TInterface> ToSelf();
        BindingBuilder<TInterface> FromInstance<TImplementation>(TImplementation instance)
            where TImplementation : TInterface;
    }
}

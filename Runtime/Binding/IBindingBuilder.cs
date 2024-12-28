using System;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public interface IBindingBuilder<TInterface>
    {
        BindingBuilder<TInterface> To<TImplementation>();
        BindingBuilder<TInterface> ToSelf();
        BindingBuilder<TInterface> FromInstance<TImplementation>(TImplementation instance)
            where TImplementation : TInterface;
        BindingBuilder<TInterface> FromFactory<TImplementation>(Func<TImplementation> factory)
            where TImplementation : TInterface;
        BindingBuilder<TInterface> FromNew<TImplementation>()
            where TImplementation : TInterface;
        BindingBuilder<TInterface> FromResolve<TImplementation>();
        BindingBuilder<TInterface> AsTransient();
        BindingBuilder<TInterface> AsSingleton();
    }
}

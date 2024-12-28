using System;
using TravisRFrench.Dependencies.Runtime.Containerization;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public class BindingBuilder<TInterface> : IBindingBuilder<TInterface>
    {
        private readonly IBindingContainer container;
        
        public Type InterfaceType { get; }
        public Type ImplementationType { get; private set; }
        public TInterface Instance { get; private set; }
        public Func<TInterface> Factory { get; private set; }
        public SourceType SourceType { get; private set; }
        public Lifetime Lifetime { get; private set; }

        public BindingBuilder(IBindingContainer container)
        {
            this.container = container;
            this.InterfaceType = typeof(TInterface);
            this.BuildAndRegisterBinding();
        }
        
        public BindingBuilder<TInterface> To<TImplementation>()
        {
            this.ImplementationType = typeof(TImplementation);
            this.BuildAndRegisterBinding();
            
            return this;
        }

        public BindingBuilder<TInterface> ToSelf()
        {
            this.ImplementationType = this.InterfaceType;
            this.BuildAndRegisterBinding();
            
            return this;
        }

        public BindingBuilder<TInterface> FromInstance<TImplementation>(TImplementation instance)
            where TImplementation : TInterface
        {
            this.Lifetime = Lifetime.Singleton;
            this.SourceType = SourceType.FromInstance;
            this.Instance = instance;
            this.BuildAndRegisterBinding();
            
            return this;
        }

        public BindingBuilder<TInterface> FromFactory(Func<TInterface> factory)
        {
            this.Factory = factory;
            this.SourceType = SourceType.FromFactory;
            this.BuildAndRegisterBinding();
            
            return this;
        }

        public BindingBuilder<TInterface> FromNew<TImplementation>() where TImplementation : TInterface
        {
            this.SourceType = SourceType.FromNew;
            this.BuildAndRegisterBinding();
            
            return this;
        }

        public BindingBuilder<TInterface> FromResolve<TImplementation>()
        {
            this.SourceType = SourceType.FromResolve;
            this.BuildAndRegisterBinding();

            return this;
        }

        public BindingBuilder<TInterface> AsTransient()
        {
            this.Lifetime = Lifetime.Transient;
            this.BuildAndRegisterBinding();
            
            return this;
        }

        public BindingBuilder<TInterface> AsSingleton()
        {
            this.Lifetime = Lifetime.Singleton;
            this.BuildAndRegisterBinding();
            
            return this;
        }

        private void BuildAndRegisterBinding()
        {
            var factory = this.Factory as Func<object>; 
            var binding = this.SourceType switch
            {
                SourceType.FromNew => new(this.InterfaceType, this.ImplementationType, this.SourceType, this.Lifetime),
                SourceType.FromInstance => new(this.InterfaceType, this.ImplementationType, this.Instance, this.SourceType, this.Lifetime),
                SourceType.FromFactory => new(this.InterfaceType, this.ImplementationType, factory: factory, this.SourceType, this.Lifetime),
                SourceType.FromResolve => new Binding(this.InterfaceType, this.ImplementationType, this.SourceType, this.Lifetime),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            this.container.Register(binding);
        }
    }
}

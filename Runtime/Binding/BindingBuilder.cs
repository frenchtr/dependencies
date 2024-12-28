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

        public BindingBuilder<TInterface> FromFactory<TImplementation>(Func<TImplementation> factory)
            where TImplementation : TInterface
        {
            throw new NotImplementedException();
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
            var binding = new Binding(
                this.InterfaceType,
                this.ImplementationType,
                this.Instance,
                this.SourceType,
                this.Lifetime
                );
            
            this.container.Register(binding);
        }
    }
}

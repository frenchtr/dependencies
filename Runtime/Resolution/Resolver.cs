using System;
using System.Collections.Generic;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Construction;
using TravisRFrench.Dependencies.Runtime.Scopes;

namespace TravisRFrench.Dependencies.Runtime.Resolution
{
    public class Resolver : IResolver
    {
        private readonly IScope scope;
        private readonly Stack<Type> resolveStack;
        private readonly Dictionary<Type, object> singletons;
        private readonly IConstructor constructor;
        
        public Resolver(IScope scope, Stack<Type> resolveStack)
        {
            this.scope = scope;
            this.resolveStack = resolveStack;
            this.singletons = new();
            this.constructor = new Constructor(scope, this);
        }
        
        public object Resolve(Type type)
        {
            try
            {
                // Step 1: Circular Dependency Check
                if (this.resolveStack.Contains(type))
                {
                    // We push the type anyway so that the finally block pops the correct type
                    this.resolveStack.Push(type);
                    throw new ResolveException(type, "A circular dependency was detected.");
                }
                
                this.resolveStack.Push(type);

                // Step 2: Retrieve Binding
                var binding = this.scope.Get(type);
                if (binding == null)
                {
                    throw new ResolveException(type, "A binding could not be found for the requested type.");
                }
                
                // Step 3: Resolve Based on Source Type
                var resolvedInstance = binding.SourceType switch
                {
                    SourceType.FromInstance => binding.Instance,
                    SourceType.FromNew => this.ResolveFromNew(binding),
                    SourceType.FromFactory => this.ResolveFromFactory(binding),
                    _ => this.ResolveFromNew(binding) // Handles FromType and similar cases
                };

                // Step 4: Handle Lifetime
                resolvedInstance = this.ApplyLifetimePolicy(type, binding, resolvedInstance);
                
                return resolvedInstance;
            }
            catch (Exception exception)
            {
                throw new ResolveException(type, $"Failed to resolve type {type}. {exception.Message}".TrimEnd(), exception);
            }
            finally
            {
                this.resolveStack.Pop();
            }
        }

        private object ResolveFromFactory(IBinding binding)
        {
            return binding.Factory.Invoke();
        }

        private object ResolveFromNew(IBinding binding)
        {
            return this.constructor.Construct(binding.ImplementationType);
        }

        private object ApplyLifetimePolicy(Type type, IBinding binding, object instance)
        {
            if (binding.Lifetime == Lifetime.Singleton)
            {
                this.singletons.TryAdd(type, instance);
                return this.singletons[type];
            }

            return instance;
        }
        
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Scopes;

namespace TravisRFrench.Dependencies.Runtime.Containerization
{
    public class Container : IContainer
    {
        private readonly Stack<IScope> scopes;
        private readonly Stack<Type> resolveStack;

        public IScope GlobalScope { get; }
        public IScope CurrentScope => this.scopes.FirstOrDefault();
        
        public Container()
        {
            this.scopes = new();
            this.resolveStack = new();
            this.GlobalScope = this.PushScope();
        }

        public void Register(IBinding binding)
        {
            this.CurrentScope.Register(binding);
        }

        public IBinding Get<T>()
        {
            return this.CurrentScope.Get<T>();
        }

        public IBinding Get(Type type)
        {
            return this.CurrentScope.Get(type);
        }

        public object Resolve(Type type)
        {
            return this.CurrentScope.Resolve(type);
        }

        public T Resolve<T>()
        {
            return this.CurrentScope.Resolve<T>();
        }

        public void Inject<T>(T obj)
        {
            this.CurrentScope.Inject(obj);
        }

        public IScope PushScope()
        {
            var scope = new Scope(this, this.CurrentScope, this.resolveStack);
            this.scopes.Push(scope);
            
            return scope;
        }

        public void PopScope()
        {
            if (this.scopes.Count <= 1)
            {
                throw new InvalidOperationException("Cannot pop the global scope.");
            }
            
            this.scopes.Pop();
        }
    }
}

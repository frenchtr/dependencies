using System;
using System.Collections.Generic;
using System.Reflection;

namespace TravisRFrench.Dependencies
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, object> dictionary;
        
        public Container()
        {
            this.dictionary = new();
        }
        
        public void Register<T>(T obj)
        {
            this.dictionary[typeof(T)] = obj;
        }

        public object Resolve(Type type)
        {
            return this.dictionary[type];
        }
        
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }
    }
}
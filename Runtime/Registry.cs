using System;
using System.Collections.Generic;

namespace TravisRFrench.Dependencies.Runtime
{
    public class Registry : IRegistry
    {
        private readonly Dictionary<Type, object> dictionary;

        public Registry()
        {
            this.dictionary = new();
        }
        
        public void Register<T>(T obj)
        {
            this.dictionary[typeof(T)] = obj;
        }

        public T Get<T>()
        {
            return (T)this.dictionary[typeof(T)];
        }

        public object Get(Type type)
        {
            return this.dictionary[type];
        }
    }
}

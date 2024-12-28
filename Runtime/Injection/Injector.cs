using System;
using System.Linq;
using System.Reflection;
using TravisRFrench.Dependencies.Runtime.Resolution;

namespace TravisRFrench.Dependencies.Runtime.Injection
{
    public class Injector : IInjector
    {
        private readonly IResolver resolver;

        public Injector(IResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Inject<T>(T obj)
        {
            const BindingFlags flags =
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic;

            var type = obj.GetType();
            
            this.InjectFields(type, obj, flags);
            this.InjectProperties(type, obj, flags);
            this.InjectMethods(type, obj, flags);
        }

        private void InjectFields(Type type, object obj, BindingFlags flags)
        {
            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<InjectAttribute>() != null)
                {
                    var value = this.resolver.Resolve(field.FieldType);
                    field.SetValue(obj, value);
                }
            }
        }
        
        private void InjectProperties(Type type, object obj, BindingFlags flags)
        {
            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<InjectAttribute>() != null && property.CanWrite)
                {
                    var value = this.resolver.Resolve(property.PropertyType);
                    property.SetValue(obj, value);
                }
            }
        }
        
        private void InjectMethods(Type type, object obj, BindingFlags flags)
        {
            var methods = type.GetMethods(flags);
            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<InjectAttribute>() != null)
                {
                    var parameters = method.GetParameters()
                        .Select(param => this.resolver.Resolve(param.ParameterType))
                        .ToArray();
                    method.Invoke(obj, parameters);
                }
            }
        }
    }
}

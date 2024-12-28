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

        public void Inject<T>(T target)
        {
            try
            {
                const BindingFlags flags =
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic;

                var type = target.GetType();
            
                this.InjectFields(type, target, flags);
                this.InjectProperties(type, target, flags);
                this.InjectMethods(type, target, flags);
            }
            catch (Exception exception)
            {
                throw new InjectException(target, $"Failed to inject dependencies into object of type {typeof(T)}. {exception.Message}".TrimEnd(), exception);
            }
        }

        private void InjectFields(Type type, object obj, BindingFlags flags)
        {
            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                try
                {
                    if (field.GetCustomAttribute<InjectAttribute>() != null)
                    {
                        var value = this.resolver.Resolve(field.FieldType);
                        field.SetValue(obj, value);
                    }
                }
                catch (Exception exception)
                {
                    throw new InjectException(obj, $"Failed to inject field {field.Name}. {exception.Message}".TrimEnd(), exception);
                }
            }
        }
        
        private void InjectProperties(Type type, object obj, BindingFlags flags)
        {
            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                try
                {
                    if (property.GetCustomAttribute<InjectAttribute>() != null && property.CanWrite)
                    {
                        var value = this.resolver.Resolve(property.PropertyType);
                        property.SetValue(obj, value);
                    }
                }
                catch (Exception exception)
                {
                    throw new InjectException(obj, $"Failed to inject property {property.Name}. {exception.Message}".TrimEnd(), exception);
                }
            }
        }
        
        private void InjectMethods(Type type, object obj, BindingFlags flags)
        {
            var methods = type.GetMethods(flags);
            foreach (var method in methods)
            {
                try
                {
                    if (method.GetCustomAttribute<InjectAttribute>() != null)
                    {
                        var parameters = method.GetParameters()
                            .Select(param => this.resolver.Resolve(param.ParameterType))
                            .ToArray();
                        method.Invoke(obj, parameters);
                    }
                }
                catch (Exception exception)
                {
                    throw new InjectException(obj, $"Failed to inject method {method.Name}. {exception.Message}".TrimEnd(), exception);
                }
            }
        }
    }
}

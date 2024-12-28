using System.Reflection;
using TravisRFrench.Dependencies.Runtime.Containerization;
using TravisRFrench.Dependencies.Runtime.Resolution;

namespace TravisRFrench.Dependencies.Runtime.Injection
{
    public class Injector : IInjector
    {
        private readonly IResolver resolver;
        
        public Injector(IContainer container)
        {
            this.resolver = container;
        }
        
        public void Inject<T>(T obj)
        {
            const BindingFlags flags =
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic;
            
            var type = obj.GetType();
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
    }
}

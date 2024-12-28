using System;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public class BindingNotFoundException : DependencyException
    {
        public Type Type { get; }

        public BindingNotFoundException(Type type, string message = default, Exception innerException = default)
            : base(message, innerException)
        {
            this.Type = type;
        }
    }
}

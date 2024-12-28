using System;

namespace TravisRFrench.Dependencies.Runtime.Injection
{
    public class InjectException : DependencyException
    {
        public object Target { get; }

        public InjectException(object target, string message = default, Exception innerException = default)
            : base(message, innerException)
        {
            this.Target = target;
        }
    }
}

using System;

namespace TravisRFrench.Dependencies.Runtime.Resolution
{
    public class ResolveException : DependencyException
    {
        public Type Type { get; }

        public ResolveException(Type type, string message = default, Exception innerException = default)
            : base(message, innerException)
        {
            this.Type = type;
        }
    }
}

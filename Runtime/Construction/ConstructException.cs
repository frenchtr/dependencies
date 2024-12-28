using System;

namespace TravisRFrench.Dependencies.Runtime.Construction
{
    public class ConstructException : DependencyException
    {
        public Type Type { get; }

        public ConstructException(Type type, string message = default, Exception innerException = default)
            : base(message, innerException)
        {
            this.Type = type;
        }
    }
}

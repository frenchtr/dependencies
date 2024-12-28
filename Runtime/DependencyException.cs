using System;

namespace TravisRFrench.Dependencies.Runtime
{
    public class DependencyException : Exception
    {
        protected DependencyException(string message = default, Exception innerException = default)
            : base(message, innerException)
        {
        }
    }
}

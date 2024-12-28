using System;

namespace TravisRFrench.Dependencies.Runtime
{
    [AttributeUsage(AttributeTargets.Constructor| AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
    }
}

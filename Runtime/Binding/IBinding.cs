﻿using System;

namespace TravisRFrench.Dependencies.Runtime.Binding
{
    public interface IBinding
    {
        Type InterfaceType { get; }
        Type ImplementationType { get; }
        object Instance { get; }
        Func<object> Factory { get; }
        Lifetime Lifetime { get; }
        SourceType SourceType { get; }
    }
}

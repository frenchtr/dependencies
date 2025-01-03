﻿using TravisRFrench.Dependencies.Runtime.Containerization;

namespace TravisRFrench.Dependencies.Runtime.Contextualization
{
    public class Context : IContext
    {
        public IContainer Container { get; }

        public Context()
        {
            this.Container = new Container();
        }

        public virtual void Initialize()
        {
        }

        public virtual void Setup(IContainer container)
        {
        }
    }
}

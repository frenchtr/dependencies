using TravisRFrench.Dependencies.Runtime.Containerization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime.Contextualization
{
    public abstract class ScriptableContext : ScriptableObject, IContext
    {
        private IContext context;
        
        public IContainer Container => this.context.Container;

        public void Initialize()
        {
            this.context ??= new Context();
            this.context.Initialize();
        }

        public virtual void Setup(IContainer container)
        {
        }
    }
}

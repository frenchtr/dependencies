using TravisRFrench.Dependencies.Runtime.Containerization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime.Contextualization
{
    [CreateAssetMenu(menuName = "Scriptables/Dependencies/Context")]
    public class ScriptableContext : ScriptableObject, IContext
    {
        private Context context;

        public IContainer Container => this.context.Container;

        public void Initialize()
        {
            this.context = new();
            this.context.Initialize();
        }

        protected virtual void Setup(IContainer container)
        {
        }
    }
}

using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime
{
    [CreateAssetMenu(menuName = "Scriptables/Dependencies/Dependency Configuration")]
    public class DependencyConfiguration : ScriptableObject
    {
        [SerializeField]
        private ScriptableContext runtimeContext;

        public IContext RuntimeContext => this.runtimeContext;
    }
}

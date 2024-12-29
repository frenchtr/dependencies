using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime
{
    [CreateAssetMenu(menuName = "Scriptables/Dependencies/Settings")]
    public class DependenciesSettings : ScriptableObject
    {
        [SerializeField]
        private ScriptableContext runtimeContext;

        public IContext RuntimeContext => this.runtimeContext;
    }
}

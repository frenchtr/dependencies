using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Runtime
{
    [CreateAssetMenu(menuName = "Scriptables/Dependencies/Context Loader")]
    public class ContextLoader : ScriptableObject
    {
        private static ContextLoader instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError($"There can only be one instance of {nameof(ContextLoader)} in the project.");
            }

            instance = this;
        }

        [SerializeField]
        private ScriptableContext runtimeContext;

        public IContext LoadRuntimeContext()
        {
            this.runtimeContext.Initialize();
            this.runtimeContext.Setup(this.runtimeContext.Container);

            return this.runtimeContext;
        }
    }
}

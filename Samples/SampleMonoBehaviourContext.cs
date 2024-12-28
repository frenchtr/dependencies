using TravisRFrench.Dependencies.Runtime;
using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
    [DefaultExecutionOrder(-1000)]
    public class SampleMonoBehaviourContext : MonoBehaviour
    {
        [SerializeField]
        private ScriptableContext context;
        [SerializeField]
        private GameObject player;
        
        private void Awake()
        {
            this.context.Initialize();
            
            var container = this.context.Container;
            var gameService = new GameService();
            
            container
                .Bind<GameService>()
                .ToSelf()
                .FromInstance(gameService);
            
            container
                .Bind<GameObject>()
                .ToSelf()
                .FromInstance(this.player);

            var behaviours = this.GetComponents<MonoBehaviour>();
            
            foreach (var behaviour in behaviours)
            {
                container.Inject(behaviour);
            }
        }
    }
}

using TravisRFrench.Dependencies.Runtime.Binding;
using TravisRFrench.Dependencies.Runtime.Containerization;
using TravisRFrench.Dependencies.Runtime.Contextualization;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
    public class RuntimeContext : ScriptableContext
    {
        [SerializeField]
        private GameObject player;
        
        public override void Setup(IContainer container)
        {
            container
                .Bind<ILogger>()
                .To<DebugLogger>()
                .AsSingleton();

            container.Bind<IGameService>()
                .To<GameService>()
                .AsSingleton();

            container.Bind<GameObject>()
                .FromFactory(() => Instantiate(this.player))
                .AsSingleton();
        }
    }
}

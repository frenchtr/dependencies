using System;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
    public class SampleMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ScriptableContext context;
        
        private void OnEnable()
        {
            var container = this.context.Container;
            var gameService = container.Resolve<GameService>();
            var player = container.Resolve<GameObject>();
            
            gameService.StartGame();
            player.SetActive(true);
        }
    }
}

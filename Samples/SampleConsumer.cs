using System;
using TravisRFrench.Dependencies.Runtime;
using TravisRFrench.Dependencies.Runtime.Injection;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
    public class SampleConsumer : MonoBehaviour
    {
        [Inject]
        private IGameService gameService;
        [Inject]
        private GameObject player;
        
        private void OnEnable()
        {
            this.gameService.StartGame();
            this.player.SetActive(false);
        }
    }
}

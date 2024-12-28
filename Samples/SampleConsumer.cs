using System;
using TravisRFrench.Dependencies.Runtime;
using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
    public class SampleConsumer : MonoBehaviour
    {
        [Inject]
        private GameService gameService;
        [Inject]
        private GameObject player;
        
        private void OnEnable()
        {
            this.gameService.StartGame();
            this.player.SetActive(true);
        }
    }
}

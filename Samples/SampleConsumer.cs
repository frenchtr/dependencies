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
        [Inject]
        private SomeComponentDependency dependency1;
        [Inject]
        private SomeComponentDependency dependency2;
        [Inject]
        private SomeComponentDependency dependency3;
        [Inject]
        private SomeComponentDependency dependency4;
        [Inject]
        private SomeComponentDependency dependency5;
        
        private void OnEnable()
        {
            this.gameService.StartGame();
            this.player.SetActive(true);
        }
    }
}

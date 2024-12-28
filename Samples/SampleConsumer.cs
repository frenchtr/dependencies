﻿using System;
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
        private Rigidbody rigidbody;
        
        private void OnEnable()
        {
            this.gameService.StartGame();
            this.player.SetActive(true);
        }
    }
}

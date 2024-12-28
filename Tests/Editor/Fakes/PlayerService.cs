using UnityEngine;

namespace TravisRFrench.Dependencies.Tests.Editor.Fakes
{
    public class PlayerService : IPlayerService
    {
        private readonly IGameService gameService;
        private readonly GameObject player;
            
        public GameObject Player => this.player;

        public PlayerService(IGameService gameService, GameObject player)
        {
            this.gameService = gameService;
            this.player = player;
        }
    }
}

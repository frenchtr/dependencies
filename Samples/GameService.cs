using UnityEngine;

namespace TravisRFrench.Dependencies.Samples
{
    public class GameService : IGameService
    {
        private readonly ILogger logger;

        public GameService(ILogger logger)
        {
            this.logger = logger;
        }
        
        public void StartGame()
        {
            this.logger.Log($"Game has been started.");
        }
    }
}

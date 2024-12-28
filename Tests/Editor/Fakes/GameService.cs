namespace TravisRFrench.Dependencies.Tests.Editor.Fakes
{
    public class GameService : IGameService
    {
        public bool IsGameRunning { get; private set; }
            
        public void StartGame()
        {
            this.IsGameRunning = true;
        }

        public void StopGame()
        {
            this.IsGameRunning = false;
        }
    }
}
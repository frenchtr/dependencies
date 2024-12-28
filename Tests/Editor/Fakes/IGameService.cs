namespace TravisRFrench.Dependencies.Tests.Editor.Fakes
{
    public interface IGameService
    {
        public bool IsGameRunning { get; }
        void StartGame();
        void StopGame();
    }
}

namespace TravisRFrench.Dependencies.Samples
{
    public class DebugLogger : ILogger
    {
        public void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}

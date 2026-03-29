namespace JFramework
{
    public interface ILogger
    {
        void Log(object message);
        void LogError(object message);
    }
}

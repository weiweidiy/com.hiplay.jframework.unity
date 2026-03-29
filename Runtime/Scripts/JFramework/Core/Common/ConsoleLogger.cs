namespace JFramework
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object message)
        {
            System.Console.WriteLine(message);
        }
        public void LogError(object message)
        {
            System.Console.Error.WriteLine(message);
        }
    }
}

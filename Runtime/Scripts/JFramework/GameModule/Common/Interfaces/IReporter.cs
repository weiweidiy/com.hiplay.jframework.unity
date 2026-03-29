namespace JFramework
{
    /// <summary>
    /// Reporter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReporter<T>
    {
        T GetReport();
    }
}


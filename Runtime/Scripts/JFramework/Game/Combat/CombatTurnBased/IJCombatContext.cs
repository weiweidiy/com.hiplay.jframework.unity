namespace JFramework.Game
{
    public interface IJCombatContext
    {
        ILogger Logger { get; }

        IJCombatTurnBasedEventRecorder EventRecorder { get; }
    }
}

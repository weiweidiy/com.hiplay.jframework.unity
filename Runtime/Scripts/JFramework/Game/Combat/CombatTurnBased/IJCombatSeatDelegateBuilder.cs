using System;

namespace JFramework.Game
{

    public interface IJCombatSeatDelegateBuilder
    {
        Func<string, int> Build();
    }
}

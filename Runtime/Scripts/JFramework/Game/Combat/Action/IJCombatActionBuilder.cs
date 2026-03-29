using System.Collections.Generic;

namespace JFramework.Game
{
    public interface  IJCombatActionBuilder
    {
        List<IJCombatAction> Create();
    }
}

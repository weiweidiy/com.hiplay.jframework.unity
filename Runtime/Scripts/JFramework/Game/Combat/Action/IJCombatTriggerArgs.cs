using System.Collections.Generic;

namespace JFramework.Game
{
    public interface IJCombatTriggerArgs
    {
        List<IJCombatCasterTargetableUnit> GetTargets();
    }

}

using System.Collections.Generic;

namespace JFramework.Game
{
    public interface IJCombatTargetsFinder : IJCombatActionComponent
    {
        IJCombatExecutorExecuteArgs GetTargetsData(/*IJCombatQuery query*/);
    }
}

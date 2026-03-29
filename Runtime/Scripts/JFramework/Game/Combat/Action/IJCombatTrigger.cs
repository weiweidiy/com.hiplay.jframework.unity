using JFramework;
using System;
using System.Collections.Generic;

namespace JFramework.Game
{

    public interface IJCombatTrigger : IJCombatActionComponent
    {
        event Action<IJCombatTrigger, IJCombatExecutorExecuteArgs> onTriggerOn;
        bool IsTriggerOn();

        void Reset();
    }
}

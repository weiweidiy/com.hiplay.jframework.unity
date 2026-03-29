using JFramework;
using System.Collections.Generic;

namespace JFramework.Game
{
    public interface IJCombatAction : IUnique, IRunable, IJCombatCaster, IJCombatCastable
    {
        void SetQuery(IJCombatQuery jCombatQuery);

        List<IJCombatTrigger> GetTriggers();

        IJCombatAcionInfo GetActionInfo();
    }
}

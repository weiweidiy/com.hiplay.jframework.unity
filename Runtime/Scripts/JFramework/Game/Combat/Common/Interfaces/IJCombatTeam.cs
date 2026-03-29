using JFramework;
using System.Collections.Generic;

namespace JFramework.Game
{
    public interface IJCombatTeam : IUnique, IRunable
    {
        List<IJCombatUnit> GetAllUnits();

        IJCombatUnit GetUnit(string uid);

        bool IsAllDead();
        void SetQuery(IJCombatQuery jCombatQuery);
    }

}

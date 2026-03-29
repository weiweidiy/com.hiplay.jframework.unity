using JFramework;
using System.Collections.Generic;

namespace JFramework.Game
{
    public interface IJCombatUnitAttrFactory
    {
        List<IUnique> Create();
    }
}

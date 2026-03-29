using System;
using System.Collections.Generic;

namespace JFramework.Game
{
    [Serializable]
    public class JCombatUnitInfo : IJCombatUnitInfo
    {
        public string Uid { get; set; }

        public List<IUnique> AttrList { get; set; }

        public List<IJCombatAction> Actions { get; set; }
    }
}

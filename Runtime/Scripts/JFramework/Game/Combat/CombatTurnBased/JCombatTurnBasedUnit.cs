using JFramework;
using JFramework.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Game
{
    public class JCombatTurnBasedUnit : JCombatCasterTargetableUnit, IJCombatTurnBasedUnit
    {
        IJCombatUnitInfo unitInfo;
        public JCombatTurnBasedUnit(string uid, List<IUnique> attrList, Func<IUnique, string> keySelector, IJCombatTurnBasedAttrNameQuery combatAttrNameQuery,  List<IJCombatAction> actions, IJCombatContext context) 
            : base(uid, attrList, keySelector, combatAttrNameQuery, actions, context)
        {

        }

        public JCombatTurnBasedUnit(IJCombatUnitInfo unitInfo, IJCombatTurnBasedAttrNameQuery combatAttrNameQuery, IJCombatContext context) 
            : this(unitInfo.Uid, unitInfo.AttrList, (u)=>u.Uid, combatAttrNameQuery, unitInfo.Actions, context)
        {
            this.unitInfo = unitInfo;
        }

        public IJCombatUnitInfo GetUnitInfo()
        {
            return unitInfo;
        }

        public List<IJCombatAcionInfo> GetActionInfos()
        {
            var result = new List<IJCombatAcionInfo>();

            foreach(var action in actions)
            {
                var actionInfo = action.GetActionInfo();
                if (actionInfo != null)
                {
                    result.Add(actionInfo);
                }
            }

            return result;
        }

        public int GetActionPoint()
        {
            var query = combatAttrNameQuery as IJCombatTurnBasedAttrNameQuery;
            var attr = GetAttribute(query.GetActionPointName());
            var attrInt = attr as GameAttributeInt;
            return attrInt.CurValue;
        }




    }
}

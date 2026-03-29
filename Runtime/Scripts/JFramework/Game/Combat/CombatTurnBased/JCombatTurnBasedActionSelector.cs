using JFramework;
using System;
using System.Collections.Generic;

namespace JFramework.Game
{
    public class JCombatTurnBasedActionSelector : DictionaryContainer<IJCombatTurnBasedUnit>, IJCombatTurnBasedCasterSelector
    {
        /// <summary>
        /// 排序后的行动顺序列表
        /// </summary>
        List<IJCombatTurnBasedUnit> actionList = new List<IJCombatTurnBasedUnit>();

        public JCombatTurnBasedActionSelector(List<IJCombatTurnBasedUnit> units,  Func<IJCombatUnit, string> keySelector) : base(keySelector)
        {
            ///to do:需要监听ijcombatUnit属性变化，比如速度变化，要动态调整序列
            AddRange(units);
            ResetActionUnits();
        }

        public JCombatTurnBasedActionSelector(List<IJCombatTurnBasedUnit> units) : this(units, (u) => u.Uid)
        {
        }

        public List<IJCombatTurnBasedUnit> GetActionUnits()
        {
            return actionList;
        }

        public void ResetActionUnits()
        {
            var lst = GetAll();
            lst.BinarySort(new Compare());
            actionList.Clear();
            actionList.AddRange(lst);
        }

        /// <summary>
        /// 全都完成了？排序不能行动的
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsAllComplete()
        {
            foreach (IJCombatTurnBasedUnit unit in actionList)
            {
                if (unit.CanCast())
                    return false;

            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IJCombatTurnBasedUnit PopActionUnit()
        {
            return actionList.PopFirst();
        }

        /// <summary>
        /// 设置unit列表，并重置action列表
        /// </summary>
        /// <param name="units"></param>
        public void AddUnits(List<IJCombatTurnBasedUnit> units)
        {
            AddRange(units);

            ResetActionUnits();
        }

        
        class Compare : IComparer<IJCombatTurnBasedUnit>
        {
            int IComparer<IJCombatTurnBasedUnit>.Compare(IJCombatTurnBasedUnit x, IJCombatTurnBasedUnit y)
            {

                if(x.GetActionPoint() > y.GetActionPoint())
                    return -1;

                if (x.GetActionPoint() < y.GetActionPoint())
                    return 1;

                return 0; //不稳定排序，可能交换位置的
            }
        }
    }
}

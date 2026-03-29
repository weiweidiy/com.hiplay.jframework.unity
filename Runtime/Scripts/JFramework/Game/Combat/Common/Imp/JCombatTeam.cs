using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Game
{
    public class JCombatTeam : RunableDictionaryContainer<IJCombatUnit>, IJCombatTeam
    {
        public string Uid { get; protected set; }

        /// <summary>
        /// to do: 传入 IJCombatUnit, IJCombatCasterUnit, IJCombatTargetableUnit
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="units"></param>
        /// <param name="keySelector"></param>
        public JCombatTeam(string uid, List<IJCombatUnit> units,  Func<IJCombatUnit, string> keySelector) : base(keySelector)
        {
            AddRange(units);
            this.Uid = uid;
        }

        public JCombatTeam(string uid, List<IJCombatUnit> units) : this(uid, units, (u) => u.Uid)
        {
        }

        public void SetQuery(IJCombatQuery jCombatQuery)
        {
            foreach (var unit in GetAll())
            {
                unit.SetQuery(jCombatQuery);
            }
        }


        public List<IJCombatUnit> GetAllUnits()
        {
            return GetAll(); //.OfType<IJAttributeable>().ToList();
        }

        public IJCombatUnit GetUnit(string uid)
        {
            return Get(uid);
        }

        public bool IsAllDead()
        {
            var allUnits = GetAllUnits();

            foreach (var unit in allUnits)
            {
                if (!unit.IsDead())
                    return false;
            }

            return true;
        }

        protected override void OnStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);
            var units = GetAll();
            if (units != null)
            {
                foreach (var unit in units)
                {
                    unit.Start(extraData);
                }
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            var units = GetAll();
            if (units != null)
            {
                foreach (var unit in units)
                {
                    unit.Stop();
                }
            }
        }
    }
}

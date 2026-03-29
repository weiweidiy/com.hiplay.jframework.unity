using System;
using System.Collections.Generic;

namespace JFramework.Game
{
    public class JCombatTurnBasedEventRecorder : DictionaryContainer<JCombatTurnBasedEvent>, IJCombatTurnBasedEventRecorder //, IJCombatUnitEventListener, IJCombatTurnBasedEventListener
    {
        IJCombatFrameRecorder frameRecorder;
        int curIndex;

        JCombatTurnBasedEvent combatEvent;

        public JCombatTurnBasedEventRecorder(IJCombatFrameRecorder frameRecorder,  Func<JCombatTurnBasedEvent,string> keySelector):base(keySelector)
        { 
            this.frameRecorder = frameRecorder;
        }

        public JCombatTurnBasedEventRecorder(IJCombatFrameRecorder frameRecorder) : this(frameRecorder, (e) => e.Uid)
        {
        }

        public List<JCombatTurnBasedEvent> GetAllCombatEvents() => GetAll();


        /// <summary>
        /// 创建一个条新的战斗事件
        /// </summary>
        /// <param name="casterUid"></param>
        /// <returns></returns>
        public JCombatTurnBasedEvent CreateActionEvent(string casterUid, int curHp, int maxHp)
        {
            combatEvent = new JCombatTurnBasedEvent();
            combatEvent.Uid = Guid.NewGuid().ToString();
            combatEvent.SortIndex = GetIndex();
            combatEvent.CurFrame = frameRecorder.GetCurFrame();
            combatEvent.CurHp = curHp;
            combatEvent.MaxHp = maxHp;
            Add(combatEvent);
            return combatEvent;
        }

        /// <summary>
        /// 获取当前的战斗事件
        /// </summary>
        /// <returns></returns>
        public JCombatTurnBasedEvent GetCurrentActionEvent() => combatEvent;


        public void AddEvent(JCombatTurnBasedEvent combatEvent)
        {
            Add(combatEvent);
        }

        private int GetIndex()
        {
            return curIndex++;
        }
    }
}


//public void OnBeforeHurt(IJCombatDamageData damageData)
//{
//    //throw new NotImplementedException();
//}


//public void OnAfterDamage(IJCombatDamageData damageData)
//{
//    //这个UID可能已经存在了，需要合并数据
//    var dataUid = damageData.Uid;
//    var combatEvent = Get(dataUid);
//    //说明已经存在
//    if (combatEvent != null && combatEvent.Uid != null && combatEvent.Uid != "")
//    {
//        //合并目标和伤害
//        var lstTargetEffect = combatEvent.ActionEffect[CombatEventActionType.Damage.ToString()];
//        lstTargetEffect.Add(new ActionEffectInfo() {TargetUid = damageData.GetTargetUid(), Value = damageData.GetDamage() });
//        Update(combatEvent);
//    }
//    else
//    {
//        combatEvent = CreateActionEvent();
//        combatEvent.Uid = dataUid;
//        combatEvent.CurFrame = frameRecorder.GetCurFrame();
//        combatEvent.CasterUid = damageData.GetCasterUid();
//        combatEvent.CastActionUid = damageData.GetActionSourceUid();
//        combatEvent.ActionEffect = new Dictionary<string, List<ActionEffectInfo>> ();
//        var lstTargetEffect = new List<ActionEffectInfo>();
//        lstTargetEffect.Add(new ActionEffectInfo() { TargetUid = damageData.GetTargetUid(), Value = damageData.GetDamage() });
//        combatEvent.ActionEffect.Add(CombatEventActionType.Damage.ToString(), lstTargetEffect);
//        Add(combatEvent);
//    }        
//}
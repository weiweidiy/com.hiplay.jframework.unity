using System.Collections.Generic;

namespace JFramework.Game
{
    /// <summary>
    /// action的效果类型
    /// </summary>
    public enum CombatEventActionType
    {
        None = 0,
        Damage,
        Heal,
        Buff,   
    }


    public class JCombatTurnBasedEvent : IUnique
    {
        public string Uid { get; set; }

        /// <summary>
        /// 索引，用来排序事件
        /// </summary>
        public int SortIndex { get; set; }
        /// <summary>
        /// 当前逻辑帧
        /// </summary>
        public int CurFrame { get; set; }

        /// <summary>
        /// 释放者当前血量
        /// </summary>
        public int CurHp { get; set; }
        /// <summary>
        /// 释放者最大血量
        /// </summary>
        public int MaxHp { get; set; }

        /// <summary>
        /// 触发的所有行为事件
        /// </summary>
        public List<ActionEvent> ActionEvents { get; set; } = new List<ActionEvent>();
    }


    public class ActionEvent
    {
        public string ActionUid { get; set; }
        public string CasterUid { get; set; }
        public Dictionary<CombatEventActionType, List<ActionEffectInfo>> ActionEffect { get; set; } = new Dictionary<CombatEventActionType, List<ActionEffectInfo>>();
    }



    public struct ActionEffectInfo
    {
        public string TargetUid { get; set; }
        public int Value { get; set; }  // -1表示没有值
        public int TargetHp { get; set; } // 目标当前血量
        public int TargetMaxHp { get; set; } // 目标最大血量

        public int CasterHp { get; set; } // 施法者当前血量
        public int CasterMaxHp { get; set; } // 施法者最大血量
    }
}

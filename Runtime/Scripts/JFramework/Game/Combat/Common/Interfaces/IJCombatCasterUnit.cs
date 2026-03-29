using System;

namespace JFramework.Game
{
    /// <summary>
    /// 有属性还可以释放
    /// </summary>
    public interface IJCombatCasterUnit : IJCombatUnit, IJCombatCaster
    {
        event Action<IJCombatCasterUnit, IJCombatAction> onCast;
        event Action<IJCombatCasterUnit, IJCombatDamageData, IJCombatTargetable> onBeforeHitting;
        event Action<IJCombatCasterUnit, IJCombatDamageData> onAfterHitted;

        void NotifyBeforeHitting(IJCombatDamageData data, IJCombatTargetable target);
        void NotifyAfterHitted(IJCombatDamageData data);
    }
}

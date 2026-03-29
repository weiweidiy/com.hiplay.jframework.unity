namespace JFramework.Game
{
    public interface IJCombatExtraData : IUnique
    {
        /// <summary>
        /// 伤害源uid
        /// </summary>
        /// <returns></returns>
        string GetCasterUid();

        /// <summary>
        /// 技能源
        /// </summary>
        /// <returns></returns>
        string GetActionSourceUid();

        /// <summary>
        /// 目标uid
        /// </summary>
        /// <returns></returns>
        string GetTargetUid();
    }
}

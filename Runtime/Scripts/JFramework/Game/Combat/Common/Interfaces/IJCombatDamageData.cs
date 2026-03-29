namespace JFramework.Game
{
    public interface IJCombatDamageData : IJCombatExtraData
    {
        /// <summary>
        /// 获取伤害值
        /// </summary>
        /// <returns></returns>
        int GetDamage();

        /// <summary>
        /// 设置伤害值
        /// </summary>
        /// <param name="damage"></param>
        void SetDamage(int damage);

        /// <summary>
        /// 伤害类型
        /// </summary>
        /// <returns></returns>
        int GetDamageType();

        /// <summary>
        /// 设置伤害类型
        /// </summary>
        /// <param name="damageType"></param>
        void SetDamageType(int damageType);

    }
}

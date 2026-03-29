namespace JFramework.Game
{
    /// <summary>
    /// 战斗单位（有属性）
    /// </summary>
    public interface IJCombatUnit : IJAttributeableUnit, IRunable
    {
        void SetQuery(IJCombatQuery jCombatQuery);
    }
}

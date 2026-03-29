namespace JFramework.Game
{
    /// <summary>
    /// 战斗属性名称查询接口
    /// </summary>
    public interface IJCombatAttrNameQuery
    {
        string GetHpAttrName();

        string GetMaxHpAttrName();

        string GetAttrName(int nameId);
    }
}

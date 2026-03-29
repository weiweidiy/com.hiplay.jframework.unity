namespace JFramework
{
    /// <summary>
    /// 唯一id
    /// </summary>
    public interface IUnique
    {
        string Uid { get; }
    }

    /// <summary>
    /// 拥有typeId
    /// </summary>
    public interface ITypeId
    {
        int TypeId { get; }
    }
}


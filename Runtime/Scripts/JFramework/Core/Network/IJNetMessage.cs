namespace JFramework
{
    public interface IJNetMessage : IUnique, ITypeId {
        string Token { get; }
    }
}

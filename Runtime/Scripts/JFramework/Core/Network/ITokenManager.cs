namespace JFramework
{
    public interface ITokenManager
    {
        string GenerateToken(string clientId);
        bool ValidateToken(string token, out string clientId);
        void InvalidateToken(string token);
    }
}
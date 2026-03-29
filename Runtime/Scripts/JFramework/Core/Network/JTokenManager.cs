using System;
using System.Collections.Concurrent;

namespace JFramework
{
    public class JTokenManager : ITokenManager
    {
        readonly ConcurrentDictionary<string, string> tokenToAccountId = new ConcurrentDictionary<string, string>();

        public string GenerateToken(string accountId)
        {
            var token = Guid.NewGuid().ToString();
            tokenToAccountId[token] = accountId;
            return token;
        }

        public bool ValidateToken(string token, out string accountId)
        {
            return tokenToAccountId.TryGetValue(token, out accountId);
        }

        public void InvalidateToken(string token)
        {
            string removed;
            tokenToAccountId.TryRemove(token, out removed);
        }
    }
}
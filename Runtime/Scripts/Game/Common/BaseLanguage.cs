using JFramework;
using JFramework.Game;

namespace Game.Common
{
    public abstract class BaseLanguage : ILanguage
    {
        protected IJConfigManager configManager;
        public BaseLanguage(IJConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public string Uid { get => GetType().Name; }

        public abstract string GetText(string uid);
    }
}


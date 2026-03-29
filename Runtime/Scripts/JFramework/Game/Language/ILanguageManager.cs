using System;

namespace JFramework
{

    public interface ILanguageManager : ILanguage
    {
        event Action<ILanguage> onLanguageChanged;

        ILanguage GetCurLanguage();

        ILanguage GetLanguage(string name);

        void SetCurLanguage(ILanguage lang);
    }
}

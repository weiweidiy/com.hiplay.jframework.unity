using System;

namespace JFramework
{

    public interface ILanguageManager : ILanguage
    {
        event Action<ILanguage> onLanguageChanged;
        void Initialize(ILanguage[] languages);
        ILanguage GetCurLanguage();

        ILanguage GetLanguage(string name);

        void SetCurLanguage(ILanguage lang);
    }
}

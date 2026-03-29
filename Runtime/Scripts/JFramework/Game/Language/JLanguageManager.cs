using JFramework.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    public class JLanguageManager : DictionaryContainer<ILanguage>, ILanguageManager
    {
        public event Action<ILanguage> onLanguageChanged;

        ILanguage curLanguage;

        public string Uid { get; set; }

        public JLanguageManager(Func<ILanguage, string> keySelector) : base(keySelector)
        {
        }

        /// <summary>
        /// 初始化语言管理器
        /// </summary>
        /// <param name="languages"></param>
        public virtual void Initialize(ILanguage[] languages)
        {
            AddRange(languages);
        }

        /// <summary>
        /// 获取当前语言对象
        /// </summary>
        /// <returns></returns>
        public ILanguage GetCurLanguage() => curLanguage;

        /// <summary>
        /// 设置当前语言对象
        /// </summary>
        /// <param name="lang"></param>
        public void SetCurLanguage(ILanguage lang)
        {
            if (lang != curLanguage)
            {
                curLanguage = lang;
                onLanguageChanged?.Invoke(curLanguage);
            }

        }

        /// <summary>
        /// 获取指定语言对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ILanguage GetLanguage(string name)
        {
            return Get(name);
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetText(string uid)
        {
            if (curLanguage == null)
                throw new Exception("没有初始化当前语言对象 ");

            return curLanguage.GetText(uid) ?? $"#{uid}"; 
        }
    }


}

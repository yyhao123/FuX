using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.Core.handler
{
    public class AutoTestLanguageHandler
    {
        /// <summary>
        /// 获取默认语言模型
        /// </summary>
        /// <returns></returns>
        public FuX.Model.data.LanguageModel GetDefaultLanguageModel() =>
            new FuX.Model.data.LanguageModel("Demo.AutoTest.Language", "Language", "Demo.AutoTest.Language.dll");

        /// <summary>
        /// 获取当前使用的语言
        /// </summary>
        /// <returns>语言类型</returns>
        public FuX.Model.@enum.LanguageType GetLanguage() => Demo.Windows.Core.handler.LanguageHandler.GetLanguage();

        /// <summary>
        /// 根据关键字获取当前语言环境下的提示信息
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public string? GetLanguageValue(string key) => Demo.Windows.Core.handler.LanguageHandler.GetLanguageValue(key, GetDefaultLanguageModel());

        /// <summary>
        /// 根据关键字获取当前语言环境下的提示信息
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="token">传播取消操作的通知</param>
        /// <returns></returns>
        public Task<string?> GetLanguageValueAsync(string key, CancellationToken token = default) => Demo.Windows.Core.handler.LanguageHandler.GetLanguageValueAsync(key, GetDefaultLanguageModel(), token);

        /// <summary>
        /// 语言的切换
        /// </summary>
        /// <param name="languageName">语言类型</param>
        public void SetLanguage(FuX.Model.@enum.LanguageType languageName) => Demo.Windows.Core.handler.LanguageHandler.SetLanguage(languageName);
    }
}

using FuX.Model.data;
using FuX.Model.@enum;
using FuX.Unility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.handler
{
    //
    // 摘要:
    //     语言处理
    public static class LanguageHandler
    {
        //
        // 摘要:
        //     包装器异步
        private static EventingWrapperAsync<EventLanguageResult> OnLanguageEventWrapperAsync;

        //
        // 摘要:
        //     语言信息
        //     默认中文
        private static CultureInfo cultureInfo = new CultureInfo("zh");

        //
        // 摘要:
        //     资源管理
        //     存放已经实例的资源
        //     提升速度,避免每次都要实例化
        private static ConcurrentDictionary<string, ResourceManager>? resourceManager;

        //
        // 摘要:
        //     语言管理
        //     存放已经实例的语言
        //     提升速度,避免每次都要实例化
        private static ConcurrentDictionary<string, CultureInfo>? languageManager;

        //
        // 摘要:
        //     内部默认语言模型
        //
        // 返回结果:
        //     语言模型
        private static LanguageModel internalLanguageModel { get; set; } = new LanguageModel("FuX.Core", "Language", "FuX.Core.dll");


        //
        // 摘要:
        //     语言传递事件
        public static event EventHandler<EventLanguageResult> OnLanguageEvent;

        //
        // 摘要:
        //     语言传递事件异步
        public static event EventHandlerAsync<EventLanguageResult> OnLanguageEventAsync
        {
            add
            {
                OnLanguageEventWrapperAsync.AddHandler(value);
            }
            remove
            {
                OnLanguageEventWrapperAsync.RemoveHandler(value);
            }
        }

        //
        // 摘要:
        //     语言切换传递
        //
        // 参数:
        //   sender:
        //     自身对象
        //
        //   e:
        //     事件结果
        private static void OnLanguageEventHandler(object? sender, EventLanguageResult e)
        {
            LanguageHandler.OnLanguageEvent?.Invoke(sender, e);
            OnLanguageEventWrapperAsync.InvokeAsync(sender, e);
        }

        //
        // 摘要:
        //     根据关键字获取当前语言环境下的对应的键值信息
        //
        // 参数:
        //   key:
        //     关键字
        //
        //   languageModel:
        //     语言模型
        //     存放基础数据，指向对应资源
        //     如果外部自定义了语言资源
        //     此属性"必传"
        //     否则获取对应的键值
        //     也可使用"LanguageOperate"属性来进行根据关键字获取当前语言环境下的对应的键值信息
        //     为空将认定为是内部操作
        //
        // 返回结果:
        //     对应语言的值
        public static string? GetLanguageValue(this string key, LanguageModel? languageModel = null)
        {
            if (languageModel == null)
            {
                languageModel = internalLanguageModel;
            }

            string text = languageModel.Source + "." + languageModel.Dictionary;
            if (LanguageHandler.resourceManager == null)
            {
                LanguageHandler.resourceManager = new ConcurrentDictionary<string, ResourceManager>();
            }

            ResourceManager resourceManager;
            if (LanguageHandler.resourceManager.TryGetValue(text, out ResourceManager value))
            {
                resourceManager = value;
            }
            else
            {
                Assembly assembly = (languageModel.AssemblyFile.IsNullOrWhiteSpace() ? Assembly.GetCallingAssembly() : ((!File.Exists(languageModel.AssemblyFile)) ? Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, languageModel.AssemblyFile)) : Assembly.LoadFrom(languageModel.AssemblyFile)));
                resourceManager = new ResourceManager(text, assembly);
                LanguageHandler.resourceManager.TryAdd(text, resourceManager);
            }

            return resourceManager.GetString(key, cultureInfo);
        }

        //
        // 摘要:
        //     获取当前使用的语言
        //
        // 返回结果:
        //     返回语言类型
        public static LanguageType GetLanguage()
        {
            if (!(cultureInfo.TwoLetterISOLanguageName == "zh"))
            {
                return LanguageType.en;
            }

            return LanguageType.zh;
        }

        //
        // 摘要:
        //     设置语言
        //
        // 参数:
        //   language:
        //     语言类型
        //
        // 返回结果:
        //     成功与失败
        public static bool SetLanguage(this LanguageType language)
        {
            EventLanguageResult eventLanguageResult = null;
            try
            {
                if (languageManager == null)
                {
                    languageManager = new ConcurrentDictionary<string, CultureInfo>();
                }

                if (languageManager.TryGetValue(language.ToString(), out CultureInfo value))
                {
                    cultureInfo = value;
                }
                else
                {
                    cultureInfo = new CultureInfo(language.ToString());
                    languageManager.TryAdd(language.ToString(), cultureInfo);
                }

                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
                eventLanguageResult = EventLanguageResult.CreateSuccessResult("语言设置成功".GetLanguageValue(), language);
            }
            catch (CultureNotFoundException ex)
            {
                eventLanguageResult = EventLanguageResult.CreateFailureResult("找不到指定语言".GetLanguageValue() + ": " + ex.Message, language);
            }
            catch (MissingManifestResourceException ex2)
            {
                eventLanguageResult = EventLanguageResult.CreateFailureResult("找不到资源文件".GetLanguageValue() + ": " + ex2.Message, language);
            }
            catch (Exception ex3)
            {
                eventLanguageResult = EventLanguageResult.CreateFailureResult("语言设置异常".GetLanguageValue() + ": " + ex3.Message, language);
            }

            OnLanguageEventHandler(null, eventLanguageResult);
            return eventLanguageResult.Status;
        }

        //
        // 摘要:
        //     根据关键字获取当前语言环境下的对应的键值信息异步
        //
        // 参数:
        //   key:
        //     关键字
        //
        //   languageModel:
        //     语言模型
        //     存放基础数据，指向对应资源
        //     如果外部自定义了语言资源
        //     此属性"必传"
        //     否则获取对应的键值
        //     也可使用"LanguageOperate"属性来进行根据关键字获取当前语言环境下的对应的键值信息
        //     为空将认定为是内部操作
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     对应语言的值
        public static async Task<string?> GetLanguageValueAsync(this string key, LanguageModel? languageModel = null, CancellationToken token = default(CancellationToken))
        {
            string key2 = key;
            LanguageModel languageModel2 = languageModel;
            return await Task.Run(() => key2.GetLanguageValue(languageModel2), token);
        }

        //
        // 摘要:
        //     获取当前使用的语言异步
        //
        // 参数:
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     返回语言类型
        public static async Task<LanguageType> GetLanguageAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => GetLanguage(), token);
        }

        //
        // 摘要:
        //     设置语言异步
        //
        // 参数:
        //   language:
        //     语言类型
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     成功与失败
        public static async Task<bool> SetLanguageAsync(this LanguageType language, CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => language.SetLanguage(), token);
        }

        //
        // 摘要:
        //     根据关键字获取当前语言环境下的提示信息
        //
        // 参数:
        //   model:
        //     语言模型
        //     存放基础数据，指向指定资源
        //
        //   key:
        //     关键字
        //
        // 返回结果:
        //     对应语言的值
        public static string? GetLanguageValue<T>(this T model, string key) where T : LanguageModel, new()
        {
            return key.GetLanguageValue(model);
        }

        //
        // 摘要:
        //     根据关键字获取当前语言环境下的提示信息异步
        //
        // 参数:
        //   model:
        //     语言模型
        //     存放基础数据，指向指定资源
        //
        //   key:
        //     关键字
        //
        //   token:
        //     传播应取消操作的通知
        //
        // 返回结果:
        //     对应语言的值
        public static async Task<string?> GetLanguageValueAsync<T>(this T model, string key, CancellationToken token = default(CancellationToken)) where T : LanguageModel, new()
        {
            return await key.GetLanguageValueAsync(model, token);
        }
    }
}

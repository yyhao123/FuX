using FuX.Model.data;
using FuX.Model.@enum;


namespace FuX.Model.@interface
{
    //
    // 摘要:
    //     多语言接口
    public interface ILanguage
    {
        //
        // 摘要:
        //     语言操作属性
        //     如果外部自定义了语言资源
        //     重写此属性，实例化指向对应的资源文件
        //     通过此属性的扩展方法根据关键字获取当前语言环境下的对应的键值信息
        LanguageModel LanguageOperate { get; set; }

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
        string? GetLanguageValue(string key, LanguageModel languageModel);

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
        Task<string?> GetLanguageValueAsync(string key, LanguageModel languageModel, CancellationToken token = default(CancellationToken));

        //
        // 摘要:
        //     获取当前使用的语言
        //
        // 返回结果:
        //     返回语言类型
        LanguageType GetLanguage();

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
        Task<LanguageType> GetLanguageAsync(CancellationToken token = default(CancellationToken));

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
        bool SetLanguage(LanguageType language);

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
        Task<bool> SetLanguageAsync(LanguageType language, CancellationToken token = default(CancellationToken));
    }
}

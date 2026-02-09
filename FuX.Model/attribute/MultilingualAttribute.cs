

using FuX.Model.@enum;

namespace FuX.Model.attribute
{
 //
// 摘要:
//     多语言特性
[AttributeUsage(AttributeTargets.All)]
    public class MultilingualAttribute : Attribute
    {
        //
        // 摘要:
        //     中文
        public string Zh { get; set; }

        //
        // 摘要:
        //     英文
        public string En { get; set; }

        //
        // 摘要:
        //     多语言特性构造函数
        //
        // 参数:
        //   zh:
        //     中文
        //
        //   en:
        //     英文
        public MultilingualAttribute(string zh, string en)
        {
            Zh = zh;
            En = en;
        }

        //
        // 摘要:
        //     获取对应的描述
        //
        // 参数:
        //   language:
        //     语言类型
        //
        // 返回结果:
        //     对应的描述
        public string? Get(LanguageType language)
        {
            return language switch
            {
                LanguageType.zh => Zh,
                LanguageType.en => En,
                _ => null,
            };
        }
    }
}

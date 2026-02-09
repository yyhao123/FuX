using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    //
    // 摘要:
    //     语言模型
    public class LanguageModel
    {
        //
        // 摘要:
        //     存放资源字典的解决方按名称
        public string Source { get; set; }

        //
        // 摘要:
        //     资源文件名称
        public string Dictionary { get; set; }

        //
        // 摘要:
        //     包含程序集清单的文件的名称或路径，用于获取程序集
        public string AssemblyFile { get; set; }

        //
        // 摘要:
        //     无参构造函数
        public LanguageModel()
        {
        }

        //
        // 摘要:
        //     有参构造函数
        //
        // 参数:
        //   source:
        //     存放资源字典的解决方按名称
        //
        //   dictionary:
        //     资源文件名称
        //
        //   assemblyFile:
        //     包含程序集清单的文件的名称或路径，用于获取程序集
        public LanguageModel(string source, string dictionary, string assemblyFile)
        {
            Source = source;
            Dictionary = dictionary;
            AssemblyFile = assemblyFile;
        }

        //
        // 摘要:
        //     重写ToString；
        //     响应 json 字符串
        //
        // 返回结果:
        //     json字符串
        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }
    }
}

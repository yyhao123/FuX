using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FuX.Model.attribute
{
    //
    // 摘要:
    //     验证特性
    [AttributeUsage(AttributeTargets.All)]
    public class VerifyAttribute : Attribute
    {
        //
        // 摘要:
        //     正则表达式
        public string Pattern { get; set; }

        //
        // 摘要:
        //     验证失败提示
        public string FailTips { get; set; }

        //
        // 摘要:
        //     验证
        //
        // 参数:
        //   pattern:
        //     正则表达式
        //
        //   failTips:
        //     验证失败提示
        public VerifyAttribute(string pattern, string failTips)
        {
            Pattern = pattern;
            FailTips = failTips;
        }

        //
        // 摘要:
        //     验证
        //
        // 参数:
        //   source:
        //     数据源
        //
        //   failTips:
        //     响应:验证失败提示
        //
        //   ignoreCase:
        //     忽略大小写
        public bool Verify(string source, out string? failTips, bool ignoreCase = false)
        {
            failTips = string.Empty;
            bool num = !Regex.IsMatch(source, Pattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
            if (!num)
            {
                failTips = FailTips;
            }

            return num;
        }
    }
}

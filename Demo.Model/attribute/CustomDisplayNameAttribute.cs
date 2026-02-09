using FuX.Core.handler;
using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.attribute
{

    public class CustomDisplayNameAttribute : DisplayNameAttribute
    {
        private LanguageModel LanguageOperate { get; set; }

        public CustomDisplayNameAttribute(string displayName)
        {
            LanguageOperate = new LanguageModel("Demo.Language", "Language", "Demo.Language.dll");
            DisplayNameValue = GetDisplayName(displayName);
        }

        private string DisplayNameValue { get; }

        public override string DisplayName => DisplayNameValue;

        private string GetDisplayName(string displayName)
        {
            // 返回动态获取的显示名称，可以根据传入的参数进行定制
            return LanguageOperate.GetLanguageValue(displayName);
        }
    }
}

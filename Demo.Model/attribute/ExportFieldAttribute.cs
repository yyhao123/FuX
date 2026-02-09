using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportFieldAttribute : Attribute
    {
        public ExportFieldAttribute()
        {

        }

        public string Name { get; set; }

        /// <summary>
        /// 英文标题
        /// </summary>
        public string TitleEn { get; set; }

        /// <summary>
        /// 中文标题
        /// </summary>
        public string TitleCn { get; set; }

        /// <summary>
        /// hsistory title, Title1|Title2
        /// </summary>
        public string TitleHistory { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        public Func<dynamic, string> Formater { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public Func<string, dynamic> Parser { get; set; }

        /// <summary>
        /// 集合类型添加器 arg1: collection value  arg2: item add to collection
        /// </summary>
        public Action<dynamic, PropertyInfo, dynamic> Adder { get; set; }

        public string[] TitleHistoryArray
        {
            get
            {
                if (string.IsNullOrEmpty(TitleHistory)) return new string[] { };

                return TitleHistory.Split('|').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }

        public bool Match(string title)
        {
            if (title == TitleCn || title == TitleEn) return true;

            return false;
        }
    }
}

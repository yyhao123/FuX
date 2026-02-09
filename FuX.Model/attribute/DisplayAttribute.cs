using FuX.Model.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.attribute
{
    //
    // 摘要:
    //     展示特性
    [AttributeUsage(AttributeTargets.All)]
    public class DisplayAttribute : Attribute
    {
        //
        // 摘要:
        //     是否显示；
        //     true:展示；
        //     false:隐藏
        public bool Show { get; set; }

        //
        // 摘要:
        //     是否使用；
        //     false:不让输入
        //     true:正常输入
        public bool Use { get; set; }

        //
        // 摘要:
        //     必须填写
        public bool MustFillIn { get; set; }

        //
        // 摘要:
        //     详细提示
        public string DetailsTips { get; set; }

        //
        // 摘要:
        //     数据类型
        public ParamModel.dataCate DataCate { get; set; }

        //
        // 摘要:
        //     展示
        //
        // 参数:
        //   Use:
        //     是否使用
        //
        //   Show:
        //     是否显示
        //
        //   MustFillIn:
        //     必须填写
        //
        //   DataCate:
        //     数据类型
        //
        //   DetailsTips:
        //     详细提示
        public DisplayAttribute(bool Use, bool Show, bool MustFillIn, ParamModel.dataCate DataCate, string DetailsTips = null)
        {
            this.Use = Use;
            this.Show = Show;
            this.DataCate = DataCate;
            this.MustFillIn = MustFillIn;
            this.DetailsTips = DetailsTips;
        }
    }
}

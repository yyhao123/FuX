using System.ComponentModel;
using System.Text.Json.Serialization;


namespace FuX.Model.data
{
    //
    // 摘要:
    //     库文件参数模型
    public class ParamModel
    {
        //
        // 摘要:
        //     前端使用的数据类别
        public enum dataCate
        {
            //
            // 摘要:
            //     文本框
            [Description("文本框")]
            text,
            //
            // 摘要:
            //     下拉框
            [Description("下拉框")]
            select,
            //
            // 摘要:
            //     单选框
            [Description("单选框")]
            radio,
            //
            // 摘要:
            //     整数框
            [Description("整数框")]
            unmber,
            //
            // 摘要:
            //     上传
            [Description("上传")]
            upload
        }

        //
        // 摘要:
        //     参数信息
        public class subset
        {
            //
            // 摘要:
            //     名称
            public string Name { get; set; }

            //
            // 摘要:
            //     描述
            public string Description { get; set; }

            //
            // 摘要:
            //     属性集合
            public List<propertie> Propertie { get; set; }
        }

        //
        // 摘要:
        //     属性
        public class propertie
        {
            //
            // 摘要:
            //     数据类型
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public dataCate? DataCate { get; set; }

            //
            // 摘要:
            //     默认值
            public object? Default { get; set; }

            //
            // 摘要:
            //     初始值
            public object? Initial { get; set; }

            //
            // 摘要:
            //     描述
            public string? Description { get; set; }

            //
            // 摘要:
            //     属性名称
            public string? PropertyName { get; set; }

            //
            // 摘要:
            //     是否显示， true:展示 false:隐藏
            public bool Show { get; set; }

            //
            // 摘要:
            //     是否使用， false:不让输入 true:正常输入
            public bool Use { get; set; }

            //
            // 摘要:
            //     必须填写
            public bool MustFillIn { get; set; }

            //
            // 摘要:
            //     详细提示
            public string? DetailsTips { get; set; }

            //
            // 摘要:
            //     正则表达式
            public string? Pattern { get; set; }

            //
            // 摘要:
            //     输入错误提示
            public string? FailTips { get; set; }

            //
            // 摘要:
            //     非必填项集合
            public List<options>? Options { get; set; }
        }

        //
        // 摘要:
        //     非必填项
        public class options
        {
            //
            // 摘要:
            //     键
            public string Key { get; set; }

            //
            // 摘要:
            //     值
            public object Value { get; set; }
        }

        //
        // 摘要:
        //     参数信息 精简版
        public class subsetSimplify
        {
            //
            // 摘要:
            //     名称
            public string Name { get; set; }

            //
            // 摘要:
            //     描述
            public string Description { get; set; }

            //
            // 摘要:
            //     属性集合精简版
            public List<propertieSimplify> Propertie { get; set; }
        }

        //
        // 摘要:
        //     属性 精简版
        public class propertieSimplify
        {
            //
            // 摘要:
            //     描述
            public string? Description { get; set; }

            //
            // 摘要:
            //     属性名称
            public string? PropertyName { get; set; }
        }

        //
        // 摘要:
        //     代码
        public string Code => Name;

        //
        // 摘要:
        //     名称
        public string Name { get; set; }

        //
        // 摘要:
        //     描述
        public string Description { get; set; }

        //
        // 摘要:
        //     参数集合
        public List<subset> Subset { get; set; }
    }
}

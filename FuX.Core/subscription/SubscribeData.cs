using FuX.Model.attribute;
using FuX.Model.data;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.subscription
{
    public class SubscribeData
    {
        public class Basics : SCData
        {
            [Category("基础数据")]
            [Description("唯一标识符")]
            public string? SN { get; set; } = Guid.NewGuid().ToUpperNString();


            [Description("点位，可为空，可后期赋值")]
            public Address? Address { get; set; }

            [Description("执行方法的委托，读取方法( Read )，每个通信库都应该存在")]
            public Func<Address, OperateResult>? Function { get; set; }
        }

        public class SCData
        {
            [Category("基础数据")]
            [Description("处理间隔")]
            [Unit("ms")]
            [Display(true, true, true, ParamModel.dataCate.unmber, null)]
            public int HandleInterval { get; set; } = 1000;


            [Description("变化抛出")]
            [Display(true, true, true, ParamModel.dataCate.radio, null)]
            public bool ChangeOut { get; set; } = true;


            [Description("变化项与未变项一同抛出")]
            [Display(true, true, true, ParamModel.dataCate.radio, null)]
            public bool AllOut { get; set; }

            [Description("任务数量")]
            [Display(true, true, true, ParamModel.dataCate.unmber, null)]
            public int TaskNumber { get; set; } = 5;

        }
    }
}

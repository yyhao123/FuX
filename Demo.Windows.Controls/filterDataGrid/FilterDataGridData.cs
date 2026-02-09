using FuX.Unility;
using Newtonsoft.Json;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Windows.Controls.filterDataGrid
{
    public class FilterDataGridData
    {
        /// <summary>
        /// 基础数据
        /// </summary>
        public class Basics
        {
            /// <summary>
            /// 唯一标识符
            /// </summary>
            public string SN { get; set; } = Guid.NewGuid().ToUpperNString();
            /// <summary>
            /// 表格控件，不能为空
            /// </summary>
            [JsonIgnore]
            public required FilterDataGrid FilterDataGrid { get; set; }

           

        }
    }
}

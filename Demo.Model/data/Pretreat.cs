using Demo.Model.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class Pretreat
    {
        public int Index { get; set; }

        public PretreatMethod Method { get; set; }

        /// <summary>
        /// 采集参数
        /// </summary>
        public string Param { get; set; }
    }
}

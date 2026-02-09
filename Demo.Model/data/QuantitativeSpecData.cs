using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// mapping数据排除
    /// </summary>
    public class QuantitativeSpecData
    {

        /// <summary>
        /// 光谱代码
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 波数范围
        /// </summary>
        public string WelRange { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsCheck { get; set; } = false;

        /// <summary>
        /// 浓度
        /// </summary>
        public double Conc { get; set; }

        /// <summary>
        /// 采集日期
        /// </summary>
        public string CreateData { get; set; }


        /// <summary>
        /// 拉曼位移数集合
        /// </summary>
        public List<double> RamanList { get; set; }


        /// <summary>
        /// 拉曼位移数
        /// </summary>
        public string DeviceRamanShiftId { get; set; }
    }
}

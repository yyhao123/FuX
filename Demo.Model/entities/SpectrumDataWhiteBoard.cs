using FuX.Model.entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.entities
{
    /// <summary>
    /// 谱图原始数据
    /// </summary>
    public class SpectrumDataWhiteBoard : BaseInfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 强度
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public double[] Intensity { get; set; }

        /// <summary>
        /// 强度数据
        /// </summary>
        public string IntensityData { get; set; }

        /// <summary>
        /// 暗底数据ID
        /// </summary>
        public string SpectrumDataDarkId { get; set; }

    }
}

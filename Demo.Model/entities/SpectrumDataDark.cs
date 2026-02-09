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
    /// 谱图暗底数据
    /// </summary>
    public class SpectrumDataDark : BaseInfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 强度
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public double[] Intensity { get; set; }

        public string IntensityData { get; set; }

        /// <summary>
        /// 强度
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public double[] IntensityLe { get; set; }

        public string IntensityDataLe { get; set; }

        public string DeviceRamanShiftId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public double[] RamanShift { get; set; }

    }
}

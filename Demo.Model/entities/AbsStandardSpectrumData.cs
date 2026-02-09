using FuX.Model.entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.entities
{
    public class AbsStandardSpectrumData : BaseInfo
    {

        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 谱图名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 拉曼位移Id
        /// </summary>
        public string DeviceRamanShiftId { get; set; }

        /// <summary>
        /// 是否是标准数据
        /// </summary>
        public bool IsStandard { get; set; }

        /// <summary>
        /// 是否已保存
        /// </summary>
        public bool isSaved { get; set; }

        /// <summary>
        /// 暗底数据ID
        /// </summary>
        public string SpectrumDataDarkId { get; set; }


        /// <summary>
        /// 采集参数JSON
        /// </summary>
        public string PramInfo { get; set; }

        /// <summary>
        ///标准数据集合
        /// </summary>
        public string SpectrumStandardData { get; set; }

        /// <summary>
        /// 样品数据集合
        /// </summary>
        public string SpectrumSampleData { get; set; }

        [SugarColumn(IsIgnore = true)]
        public DeviceRamanShift deviceRamanShift { get; set; }

        [SugarColumn(IsIgnore = true)]
        public SpectrumDataDark spectrumDataDark { get; set; }


    }
}

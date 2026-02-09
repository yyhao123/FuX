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
    /// 测光样品数据
    /// </summary>
    public class AbsSampleSpectrumData : BaseInfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 暗背景id
        /// </summary>
        public string SpectrumDataDarkId { get; set; }

        /// <summary>
        /// 光谱id
        /// </summary>
        public string SpectrumId { get; set; }

        /// <summary>
        /// 测光模块id
        /// </summary>

        public string AbsStandardDataId { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public int batch { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int num { get; set; }

        /// <summary>
        /// 比色皿
        /// </summary>
        public int cuvetteno { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// 浓度
        /// </summary>
        public double conc { get; set; }

        /// <summary>
        /// 吸光度1
        /// </summary>
        public double wel1 { get; set; }

        /// <summary>
        /// 吸光度2
        /// </summary>
        public double wel2 { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string ret { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string mome { get; set; }

        /// <summary>
        /// 吸光度1标准值
        /// </summary>
        public int standardData1 { get; set; }

        /// <summary>
        /// 吸光度1样品值
        /// </summary>
        public int sampleData1 { get; set; }

        /// <summary>
        ///  /// <summary>
        /// 吸光度2标准值
        /// </summary>
        /// </summary>
        public int standardData2 { get; set; }

        /// <summary>
        ///  /// <summary>
        /// 吸光度2样品值
        /// </summary>
        /// </summary>
        public int sampleData2 { get; set; }

    }
}

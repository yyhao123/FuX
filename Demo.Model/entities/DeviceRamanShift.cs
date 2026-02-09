using Demo.Model.@enum;
using FuX.Model.entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.entities
{
    public class DeviceRamanShift : BaseInfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 设备SN
        /// </summary>
        public string DeviceSN { get; set; }
        /// <summary>
        /// 像素大小
        /// </summary>
        public int CCDSize { get; set; }

        /// <summary>
        /// 波数
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public double[] RamanShift { get; set; }


        /// <summary>
        /// 波数
        /// </summary>
        public string RamanShiftData { get; set; }


        /// <summary>
        /// 波长
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public double[] WelShift { get; set; }

        /// <summary>
        /// 编码器值
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 波长
        /// </summary>
        public string WelShiftData { get; set; }

        /// <summary>
        /// 采集模式
        /// </summary>
        public CollectType CollectType { get; set; }

        /// <summary>
        /// 数据处理模式
        /// </summary>
        public DisplayDataType DisplayDataType { get; set; }

        /// <summary>
        /// 采集参数JSON
        /// </summary>
        public string PramInfo { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string MD5 { get; set; }

    }
}

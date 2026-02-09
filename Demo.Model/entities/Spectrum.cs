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
    public class Spectrum : BaseInfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        public string Name { get; set; }

        /// <summary>
        /// 激光功率
        /// </summary>
        public int LaserPower { get; set; }
        /// <summary>
        /// 激发波长
        /// </summary>
        public int WaveLength { get; set; }

        /// <summary>
        /// 光栅线束
        /// </summary>
        public int Grating { get; set; }

        /// <summary>
        /// 中心波数
        /// </summary>

        public int CenterWave { get; set; }

        /// <summary>
        /// 积分时间
        /// </summary>
        public int IntegrationTime { get; set; }

        /// <summary>
        /// 采集方法：快速/精确/高精
        /// </summary>
        public AcquireMethod AcquireMethod { get; set; }

        /// <summary>
        /// 采集类型：单次采集/Mapping采集
        /// </summary>
        public AcquireType AcquireType { get; set; }


        public CollectType CollectType { get; set; }


        /// <summary>
        /// 平均次数
        /// </summary>
        public int Average { get; set; }


        /// <summary>
        /// 拉曼位移Id
        /// </summary>
        public string DeviceRamanShiftId { get; set; }

        public string Comment { get; set; }

        [SugarColumn(IsIgnore = true)]
        public DeviceRamanShift DeviceRamanShift { get; set; }

        /// <summary>
        /// 采集参数JSON
        /// </summary>
        public string PramInfo { get; set; }

        /// <summary>
        /// 采集数据点
        /// </summary>
        public int PixelCount { get; set; }

        /// <summary>
        /// 数据处理模式
        /// </summary>
        public DisplayDataType DisplayDataType { get; set; }

    }
}

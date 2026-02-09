using Demo.Model.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class AcquireParameter
    {
        /// <summary>
        /// 平均次数
        /// </summary>
        public int Average { get; set; }

        /// <summary>
        /// 当前采集次数
        /// </summary>
        public int CurNum { get; set; } = 0;

        /// <summary>
        /// 积分时间
        /// </summary>
        public int IntegrationTime { get; set; }

        /// <summary>
        /// 采集方法
        /// </summary>
        public AcquireMethod AcquireMethod { get; set; }

        /// <summary>
        /// 采集类型：单次采集/Mapping采集
        /// </summary>
        public AcquireType AcquireType { get; set; }

        /// <summary>
        /// 采样间隔
        /// </summary>
        public int IntervalTime { get; set; }

        /// <summary>
        /// 像素个数
        /// </summary>
        public int PixelCount { get; set; }

        /// <summary>
        /// 激光波长
        /// </summary>
        public int WaveLength { get; set; }
        /// <summary>
        /// 光栅
        /// </summary>
        public int Grating { get; set; }

        /// <summary>
        /// 中心波数
        /// </summary>
        public int CenterWave { get; set; }

        #region 新增单元
        /// <summary>
        /// 相机信息
        /// </summary>
        public CCDInfo CCDInfo { get; set; }

        /// <summary>
        /// 采集频率
        /// </summary>
        public string GatherRate { get; set; }

        /// <summary>
        /// 采集频率步进
        /// </summary>
        public string StepNum { get; set; }

        /// <summary>
        /// 入光口
        /// </summary>
        public int InNum { get; set; }

        /// <summary>
        /// 响应时间 ms
        /// </summary>
        public int ResponseTime { get; set; }

        /// <summary>
        /// 定点波长
        /// </summary>
        public int PonitBC { get; set; }

        /// <summary>
        /// 范围类型 0全谱图 1范围
        /// </summary>
        public int RangeType { get; set; }


        /// <summary>
        /// 范围最小值
        /// </summary>
        public int RangeMin { get; set; }


        /// <summary>
        /// 范围最大值
        /// </summary>
        public int RangeMax { get; set; }

        /// <summary>
        /// 定点采集类型 0连续 1指定时长
        /// </summary>
        public int TimeType { get; set; }

        /// <summary>
        /// 扫描时长 ms
        /// </summary>
        public int ScanningTime { get; set; }

        /// <summary>
        /// 采集类型
        /// </summary>
        public CollectType CollectTypes { get; set; }

        /// <summary>
        /// 信号档位
        /// </summary>
        public int SignalGear { get; set; }

        /// <summary>
        /// 滤波强度
        /// </summary>
        public int FilterWeights { get; set; }

        #endregion
    }
}


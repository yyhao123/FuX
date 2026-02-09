using Demo.Model.entities;
using Demo.Model.@enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpectrumHistoryDto
    {
        /// <summary>
        /// Query DB
        /// </summary>
        /// <param name="data"></param>
        public SpectrumHistoryDto(Spectrum data)
        {
            SpectrumId = data.Id;
            Name = data.Name;
            Comment = data.Comment;
            IntegrationTime = data.IntegrationTime;
            LaserPower = data.LaserPower;
            Average = data.Average;
            Created = Convert.ToDateTime( data.Created);
            AcquireMethod = data.AcquireMethod;
            AcquireType = data.AcquireType;
            Grating = data.Grating;
            PramInfo = data.PramInfo;
            PixelCount = data.PixelCount;
            DisplayDataType = data.DisplayDataType;
        }

        /// <summary>
        /// After acquire refresh history
        /// </summary>
        /// <param name="data"></param>
        public SpectrumHistoryDto(SpectrumDto data)
        {
            SpectrumId = data.Id;
            Name = data.Name;
            Comment = data.Comment;
            IntegrationTime = data.IntegrationTime;
            LaserPower = data.LaserPower;
            Average = data.Average;
            Created = data.Created;
            AcquireMethod = data.AcquireMethod;
            AcquireType = data.AcquireType;
            CCDPonit = data.CCDPonit;
            Grating = data.Grating;
            PramInfo = data.PramInfo;
            PixelCount = data.WelShift.Length;
            DisplayDataType = data.DisplayDataType;
        }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string operateuser { get; set; }

        public string SpectrumId { get; set; }

        public int Grating { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 记录光栅的旋转角度
        /// </summary>
        public double Qvalue { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 积分时间
        /// </summary>
        public int IntegrationTime { get; set; }

        /// <summary>
        /// 光谱类型
        /// </summary>
        public DisplayDataType DisplayDataType { get; set; }

        /// <summary>
        /// 激光功率
        /// </summary>
        public int LaserPower { get; set; }

        /// <summary>
        /// 平均次数
        /// </summary>
        public int Average { get; set; }

        /// <summary>
        /// 获取参数对象
        /// </summary>
        public AcquireParameter AcquireParameter
        {
            get
            {
                var pram = JsonConvert.DeserializeObject<AcquireParameter>(PramInfo);
                return pram;
            }
        }

        /// <summary>
        /// 相机名称
        /// </summary>
        public string CCDPonit { get; set; }

        /// <summary>
        /// 采集类型
        /// </summary>
        public CollectType CollectTypes
        {
            get
            {
                return AcquireParameter.CollectTypes;
            }
        }

        /// <summary>
        /// 像素个数
        /// </summary>
        public int PixelCount
        {
            get; set;
        }

        /// <summary>
        /// 采集频率
        /// </summary>
        public string GatherRate
        {
            get
            {
                return AcquireParameter.GatherRate;
            }
        }

        /// <summary>
        /// 采集频率
        /// </summary>
        public string PramInfo { get; set; }

        /// <summary>
        /// 采集方法：快速/精确/高精
        /// </summary>
        public AcquireMethod AcquireMethod { get; set; }

        /// <summary>
        /// 采集类型：单次采集/Mapping采集
        /// </summary>
        public AcquireType AcquireType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }

    }
}

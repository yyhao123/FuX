using Demo.AutoTest.@enum;
using Demo.Communication.constant;
using Demo.Model.data;
using Demo.Model.@enum;
using Demo.Windows.Controls.property.core.DataAnnotations;
using Demo.Windows.Core.handler;
using FuX.Core.services;
using FuX.Unility;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static Microsoft.ClearScript.V8.V8CpuProfile;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using Range = Demo.Model.data.Range;

namespace Demo.AutoTest.data
{
    /// <summary>
    /// 光谱图参数类
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class spectrumEt : BicolorNoEt
    {
        /// <summary>
        /// 波长范围
        /// </summary>
        [Browsable(false)]
        [DescriptionAttribute("波长范围")]
        public string WavelengthRange { get; set; } = "10";

        /// <summary>
        /// 扫描速度
        /// </summary>
        [Browsable(false)]
        [DescriptionAttribute("扫描速度选择下标值")]
        public int SpeedIndex { get; set; }

        [Browsable(false)]
        public string SpeedStr { get; set; }

        /// <summary>
        /// 采样间隔
        /// </summary>
        [DescriptionAttribute("采样间隔")]
        [Browsable(false)]
        public string CollInterval { get; set; }

        /// <summary>
        /// 自动采样
        /// </summary>
        [DescriptionAttribute("自动采样")]
        [Browsable(false)]
        public bool CollAuto { get; set; }

        /// <summary>
        /// 是否单个
        /// </summary>
        [DescriptionAttribute("是否单个")]
        [Browsable(false)]
        public bool CollSingle { get; set; }

        /// <summary>
        /// 重复次数
        /// </summary>
        [DescriptionAttribute("重复次数")]
        [Browsable(false)]
        public int RepeatNum { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        [DescriptionAttribute("间隔时间")]
        [Browsable(false)]
        public int RepeatTime { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [DescriptionAttribute("文件名称")]
        [Browsable(false)]
        public string FlleName { get; set; }

        /// <summary>
        /// 开始采集时间
        /// </summary>
        [DescriptionAttribute("开始采集时间")]
        [Browsable(false)]
        public string StartTime { get; set; } = string.Empty;


        /// <summary>
        /// 结束采集时间
        /// </summary>
        [DescriptionAttribute("结束采集时间")]
        [Browsable(false)]
        public string EndTime { get; set; } = string.Empty;

        /// <summary>
        /// 是否滑动滤波
        /// </summary>
        [DescriptionAttribute("是否滑动滤波")]
        [Browsable(false)]
        public bool Filtering { get; set; }

       


        /// <summary>
        /// 是否峰值
        /// </summary>
        //public bool IsPeak { get; set; } = true;

        /// <summary>
        /// 狭缝
        /// </summary>
        [DescriptionAttribute("狭缝选择下标值")]
        [Browsable(false)]
        public int xf { get; set; }


        [DescriptionAttribute("狭缝选择下标值2")]
        [Browsable(false)]
        public int xf2 { get; set; }

      
       

        [DisplayName("探测器型号")]
        public CCDDevInfo CCDName { get; set; }

        [DisplayName("探测器类型")]
        public CCDType CCDType { get; set; }
        



        [Browsable(false)]
        public bool IsVisableForPoint => this.CCDType == CCDType.Ponit;

        #region ATP系列

        public spectrumEt()
        {
            _localize = InjectionWpf.GetService<ILocalize>();
            this.GatherRate = _localize.GetCfg<List<ComObj>>(_userCfg.GatherRate);
            this.GatherRateSelectedItem = this.GatherRate[1];
            this.Gain = _localize.GetCfg<List<ComObj>>(_userCfg.Gain);
            this.GainSelectedItem = this.Gain[1];
            this.FilterWeights = _localize.GetCfg<List<ComObj>>(_userCfg.FilterWeights);
            this.FilterWeightsSelectedItem = this.FilterWeights[1];
            this.CollectTypes= CollectType.Range;
        }
       

        private ILocalize _localize;

        /// <summary>
        /// 采集频率集合
        /// </summary>
        [Browsable(false)]
        public List<ComObj> GatherRate { get; set; }

        [DisplayName("采集频率")]
        [ItemsSourceProperty("GatherRate")]
        [DisplayMemberPath("key")]
        [VisibleBy("IsVisableForPoint")]
        public ComObj GatherRateSelectedItem { get; set; }

        /// <summary>
        /// 采集增益集合
        /// </summary>
        [Browsable(false)]
        public List<ComObj> Gain { get; set; }

        [DisplayName("增益档位")]
        [ItemsSourceProperty("Gain")]
        [DisplayMemberPath("key")]
        [VisibleBy("CCDType", CCDDevInfo.TC261)]
        public ComObj GainSelectedItem { get; set; }

        /// <summary>
        /// 范围类型 0全谱图 1范围
        /// </summary>
        [VisibleBy("IsVisableForPoint")]
        public RangeTypeEm RangeType { get; set; }

        [DisplayName("波长范围")]
        [VisibleBy("RangeType", RangeTypeEm.Range )]
        public Range Range1 { get; set; }

       

        /// <summary>
        /// 滤波权重
        /// </summary>
        [Browsable(false)]
        public List<ComObj> FilterWeights { get; set; }

        [DisplayName("滤波权重")]
        [ItemsSourceProperty("FilterWeights")]
        [DisplayMemberPath("key")]
        [VisibleBy("IsVisableForPoint")]
        public ComObj FilterWeightsSelectedItem { get; set; }

        /// <summary>
        /// 相机信息
        /// </summary>
        [Browsable(false)]
        public CCDInfo CCDInfo { get; set; }

        /// <summary>
        /// 采集类型
        /// </summary>
        [Browsable(false)]
        public CollectType CollectTypes { get; set; }




        #endregion

    }
   

}

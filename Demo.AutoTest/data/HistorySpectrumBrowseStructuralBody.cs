using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.data
{
    public class HistorySpectrumBrowseStructuralBody:BindNotify
    {
        /// <summary>
        /// 谱图名称
        /// </summary>
        public string SpectrumName
        {
            get
            {
                return GetProperty(() => SpectrumName);
            }
            set
            {
                SetProperty(() => SpectrumName, value);
            }
        }

        /// <summary>
        /// 用户描述
        /// </summary>
        public string Comment
        {
            get
            {
                return GetProperty(() => Comment);
            }
            set
            {
                SetProperty(() => Comment, value);
            }
        }

        /// <summary>
        /// 数据点
        /// </summary>
        public string PixelCount
        {
            get
            {
                return GetProperty(() => PixelCount);
            }
            set
            {
                SetProperty(() => PixelCount, value);
            }
        }


        /// <summary>
        /// 采集时间
        /// </summary>
        public string CreatedDate
        {
            get
            {
                return GetProperty(() => CreatedDate);
            }
            set
            {
                SetProperty(() => CreatedDate, value);
            }
        }

        /// <summary>
        /// 采集模式
        /// </summary>
        public string CollectTypes
        {
            get
            {
                return GetProperty(() => CollectTypes);
            }
            set
            {
                SetProperty(() => CollectTypes, value);
            }
        }

        /// <summary>
        /// 采集频率
        /// </summary>
        public string GatherRate
        {
            get
            {
                return GetProperty(() => GatherRate);
            }
            set
            {
                SetProperty(() => GatherRate, value);
            }
        }

        /// <summary>
        /// 光谱类型
        /// </summary>
        public string DisplayDataType
        {
            get
            {
                return GetProperty(() => DisplayDataType);
            }
            set
            {
                SetProperty(() => DisplayDataType, value);
            }
        }

        public string SpectrumId { get; set; }

    }
}

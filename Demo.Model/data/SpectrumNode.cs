using Demo.Model.@enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 列表谱图节点
    /// </summary>
    public class SpectrumNode
    {
        public SpectrumNode(SpectrumDto spectrum, Color color, ZedTypeBox zedTypeBox = ZedTypeBox.D, SpecInfo specInfo = null)
        {
            if (spectrum.Data.Count < 1)
                throw new ArgumentException($"Spectrum {spectrum.Name} does not have data.");

            SpectrumId = spectrum.Id;
            Name = spectrum.Name;
            Color = color;
            AcquireType = spectrum.AcquireType;
            Source = spectrum.Source;

            var data = spectrum.Data.First();
            White = data.WhiteBoard;
            Dark = data.Dark;
            Raw = data.Raw;
            DarkSubstracted = data.DarkSubtracted;
            TransmissivityData = data.TransmissivityData;
            ReflectivityData = data.ReflectivityData;
            IrradianceData = data.IrradianceData;
            AbsorbanceData = data.AbsorbanceData;
            SpectrumDataRawId = data.RawDto.Id;
            PramInfo = spectrum.PramInfo;

            RamanShift = spectrum.RamanShift;

            WelShift = spectrum.WelShift;
            DisplayDataType = spectrum.DisplayDataType;
            CollectType = spectrum.CollectType;
            ZedTypeBox = zedTypeBox;
            SpecInfo = specInfo;
            Address = spectrum.Address;


        }

        public DisplayDataType DisplayDataType { get; set; }

        public int[] Address { get; set; } //编码器地址

        public CollectType CollectType { get; set; }

        public ZedTypeBox ZedTypeBox { get; set; } = ZedTypeBox.D;

        public SpecInfo SpecInfo { get; set; }

        /// <summary>
        /// 谱图Id
        /// </summary>
        public string SpectrumId { get; set; }
        /// <summary>
        /// 参数数据
        /// </summary>
        public string PramInfo { get; set; }

        /// <summary>
        /// Spectrum data raw Id
        /// </summary>
        public string SpectrumDataRawId { get; set; }

        /// <summary>
        /// 谱图名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 暗背景数据
        /// </summary>
        public double[] Dark { get; set; }

        /// <summary>
        /// 白板
        /// </summary>
        public double[] White { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public double[] Raw { get; set; }

        /// <summary>
        /// 扣除暗底数据
        /// </summary>
        public double[] DarkSubstracted { get; set; }

        /// <summary>
        /// 吸光度
        /// </summary>
        public double[] AbsorbanceData { get; set; }

        /// <summary>
        /// 透射率
        /// </summary>
        public double[] TransmissivityData { get; set; }

        /// <summary>
        /// 反射率
        /// </summary>
        public double[] ReflectivityData { get; set; }


        /// <summary>
        /// 辐照度
        /// </summary>
        public double[] IrradianceData { get; set; }

        /// <summary>
        /// 拉曼位移
        /// </summary>
        public double[] RamanShift { get; set; } = new double[2048];


        /// <summary>
        /// 拉曼位移
        /// </summary>
        public double[] WelShift { get; set; } = new double[2048];

        /// <summary>
        /// 谱图曲线颜色和列表颜色
        /// </summary>
        public Color Color { get; set; }


        /// <summary>
        /// 采集类型，目前用于mapping导出时过滤
        /// </summary>
        public AcquireType AcquireType { get; set; }

        /// <summary>
        /// 光谱来源，为了兼容导入的谱图DataLocation 取值不一样
        /// </summary>
        public SpectrumSource Source { get; set; }

        /// <summary>
        /// indicate is spectrum pined
        /// </summary>
        public bool IsPined { get; set; }

        public IList<SpectrumPeak> SpectrumPeaks { get; set; } = new List<SpectrumPeak>();

        public int[] GetXAdressData()
        {
            var res = default(int[]);
            {
                //if()
                res = Address;
                return res;
            }
        }
        public double[] GetData(SpectrumDataType type)
        {
            var res = default(double[]);

            if (type == SpectrumDataType.Dark)
                res = Dark;
            else if (type == SpectrumDataType.Raw)
                res = Raw;
            else if (type == SpectrumDataType.White)
                res = White;
            else if (type == SpectrumDataType.DarkSubtracted)
                res = DarkSubstracted;
            else if (type == SpectrumDataType.AbsorbanceData)
                res = AbsorbanceData;
            else if (type == SpectrumDataType.TransmissivityData)
                res = TransmissivityData;
            else if (type == SpectrumDataType.ReflectivityData)
                res = ReflectivityData;
            else if (type == SpectrumDataType.IrradianceData)
                res = IrradianceData;
            else
                throw new Exception($"Unsupport spectrum data type {type.ToString()}!");
            if (res == null)
            {
                res = DarkSubstracted;
            }
            return res;
        }
    }
}

using Demo.Model.attribute;
using Demo.Model.data;
using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpectrumDataDto
    {
        public SpectrumDataDto() { }


        public SpectrumDataDto(SpectrumDataRaw spectrumRaw, SpectrumDataDark spectrumDark, SpectrumDataWhiteBoard spectrumDataWhiteBoard = null)
        {
            DarkDto = new SpectrumDataDarkDto(spectrumDark);
            RawDto = new SpectrumDataRawDto(spectrumRaw);
            if (spectrumDataWhiteBoard != null && spectrumDataWhiteBoard.Intensity.Length > 0)
            {
                WhiteBoardDto = new SpectrumDataWhiteBoardDto(spectrumDataWhiteBoard);
            }
            else
            {
                WhiteBoardDto = null;
            }
        }

        /// <summary>
        /// 暗底数据
        /// </summary>
        public SpectrumDataDarkDto DarkDto { get; set; }

        /// <summary>
        /// 白板数据
        /// </summary>
        public SpectrumDataWhiteBoardDto WhiteBoardDto { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        public SpectrumDataRawDto RawDto { get; set; }


        private double[] _darkSubtracted;

        private double[] _reflectivityData;

        private double[] _transmissivityData;

        private double[] _absorbanceData;

        private double[] _irradianceData;

        /// <summary>
        /// 扣减暗底数据（Reflect data process）
        /// </summary>
        public double[] DarkSubtracted
        {
            get
            {
                if (IsProcessed && ProcessedData != null && ProcessedData.DarkSubtracted != null)
                    return ProcessedData.DarkSubtracted;

                if (_darkSubtracted == null && Raw != null && Dark != null)
                {
                    _darkSubtracted = new double[Raw.Length];

                    for (var i = 0; i < _darkSubtracted.Length; i++)
                        _darkSubtracted[i] = Raw[i] - Dark[i];
                }
                return _darkSubtracted;
            }
            set { _darkSubtracted = value; }
        }

        /// <summary>
        /// 吸光度（Reflect data process）
        /// </summary>
        public double[] AbsorbanceData
        {
            get
            {

                if (IsProcessed && ProcessedData != null && ProcessedData.DarkSubtracted != null)
                    return ProcessedData.AbsorbanceData;

                if (_absorbanceData == null && Raw != null && Dark != null && WhiteBoard != null)
                {
                    var len = Raw.Length <= Dark.Length ? Raw.Length : Dark.Length;
                    len = len <= WhiteBoard.Length ? len : WhiteBoard.Length;
                    _absorbanceData = new double[len];

                    for (var i = 0; i < _absorbanceData.Length; i++)
                        _absorbanceData[i] = Math.Log10((WhiteBoard[i] - Dark[i]) / (Raw[i] - Dark[i]));
                }
                return _absorbanceData;
            }
        }

        /// <summary>
        /// 透射率（Reflect data process）
        /// </summary>
        public double[] TransmissivityData
        {
            get
            {

                if (IsProcessed && ProcessedData != null && ProcessedData.DarkSubtracted != null)
                    return ProcessedData.TransmissivityData;

                if (_transmissivityData == null && Raw != null && Dark != null && WhiteBoard != null)
                {
                    var len = Raw.Length <= Dark.Length ? Raw.Length : Dark.Length;
                    len = len <= WhiteBoard.Length ? len : WhiteBoard.Length;
                    _transmissivityData = new double[len];

                    for (var i = 0; i < _transmissivityData.Length; i++)
                        _transmissivityData[i] = (Raw[i] - Dark[i]) / (WhiteBoard[i] - Dark[i]) * 100;
                }
                return _transmissivityData;
            }
        }


        /// <summary>
        /// 反射率
        /// </summary>
        public double[] ReflectivityData
        {
            get
            {
                if (IsProcessed && ProcessedData != null && ProcessedData.DarkSubtracted != null)
                    return ProcessedData.ReflectivityData;

                if (_reflectivityData == null && Raw != null && Dark != null && WhiteBoard != null)
                {
                    var len = Raw.Length <= Dark.Length ? Raw.Length : Dark.Length;
                    len = len <= WhiteBoard.Length ? len : WhiteBoard.Length;
                    _reflectivityData = new double[len];

                    for (var i = 0; i < _reflectivityData.Length; i++)
                        _reflectivityData[i] = (1 - (Raw[i] - Dark[i]) / (WhiteBoard[i] - Dark[i])) * 100;
                }
                return _reflectivityData;
            }
        }

        /// <summary>
        /// 辐照度
        /// </summary>
        public double[] IrradianceData
        {
            get
            {
                if (IsProcessed && ProcessedData != null && ProcessedData.DarkSubtracted != null)
                    return ProcessedData.IrradianceData;

                if (_irradianceData == null && Raw != null && Dark != null && WhiteBoard != null)
                {
                    var len = Raw.Length <= Dark.Length ? Raw.Length : Dark.Length;
                    len = len <= WhiteBoard.Length ? len : WhiteBoard.Length;

                    _irradianceData = new double[len];

                    for (var i = 0; i < _irradianceData.Length; i++)
                        _irradianceData[i] = Raw[i] - Dark[i];
                }
                return _irradianceData;
            }
        }


        /// <summary>
        /// 暗底数据（Reflect data process）
        /// </summary>
        public double[] Dark
        {
            get
            {
                if (IsProcessed && ProcessedData != null)
                    return ProcessedData.Dark;

                return DarkDto.IntensityLe?.Length > 0 ? DarkDto.IntensityLe : DarkDto.Intensity;
            }
        }

        /// <summary>
        /// 原始数据（Reflect data process）
        /// </summary>
        public double[] Raw
        {
            get
            {
                if (IsProcessed && ProcessedData != null)
                    return ProcessedData.Raw;

                return RawDto.IntensityLe?.Length > 0 ? RawDto.IntensityLe : RawDto.Intensity;
            }

        }

        /// <summary>
        /// 白板（Reflect data process）
        /// </summary>
        public double[] WhiteBoard
        {
            get
            {
                if (IsProcessed && ProcessedData != null)
                    return ProcessedData.WhiteBoard;

                return WhiteBoardDto?.Intensity;
            }
        }

        /// <summary>
        /// 是否做了数据处理
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// 数据处理结果
        /// </summary>
        public SpectrumProcessedData ProcessedData { get; set; } = new SpectrumProcessedData();

        /// <summary>
        /// Raw算法处理历史数据
        /// </summary>
        public Stack<double[]> RawTrace { get; set; } = new Stack<double[]>();

        /// <summary>
        /// Dark substract trace
        /// </summary>
        public Stack<double[]> DarkSubtractedTrace { get; set; } = new Stack<double[]>();

        /// <summary>
        /// Dark trace
        /// </summary>
        public Stack<double[]> DarkTrace { get; set; } = new Stack<double[]>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<double[]> WhiteTrace { get; set; } = new Stack<double[]>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<double[]> ReflectivityDataTrace { get; set; } = new Stack<double[]>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<double[]> TransmissivityDataTrace { get; set; } = new Stack<double[]>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<double[]> AbsorbanceDataTrace { get; set; } = new Stack<double[]>();
        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<double[]> IrradianceDataTrace { get; set; } = new Stack<double[]>();



        /// <summary>
        /// Raw data算法处理历史操作
        /// </summary>
        private List<Pretreat> _rawPretreat
        {
            get
            {
                return RawPretreat.ToList();
            }
            set
            {
                if (value != null)
                {
                    var lst = value.ToList();
                    lst.Reverse();

                    foreach (var item in lst)
                    {
                        RawPretreat.Push(item);
                    }
                }
            }
        }

        /// <summary>
        /// Raw data算法处理历史操作
        /// </summary>
        public Stack<Pretreat> RawPretreat { get; set; } = new Stack<Pretreat>();

        /// <summary>
        /// shim property for protobuf
        /// </summary>
        private List<Pretreat> _darkPretreat
        {
            get
            {
                return DarkPretreat.ToList();
            }
            set
            {

                if (value != null)
                {
                    var lst = value.ToList();
                    lst.Reverse();

                    foreach (var item in lst)
                    {
                        DarkPretreat.Push(item);
                    }
                }
            }
        }

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<Pretreat> DarkPretreat { get; set; } = new Stack<Pretreat>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        [ExportField(Name = "DarkSubtractedPretreat", TitleCn = "数据处理", TitleEn = "Pretreat")]
        public Stack<Pretreat> DarkSubtractedPretreat { get; set; } = new Stack<Pretreat>();


        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<Pretreat> WhitePretreat { get; set; } = new Stack<Pretreat>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<Pretreat> ReflectivityDataPretreat { get; set; } = new Stack<Pretreat>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<Pretreat> TransmissivityDataPretreat { get; set; } = new Stack<Pretreat>();

        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<Pretreat> AbsorbanceDataPretreat { get; set; } = new Stack<Pretreat>();
        /// <summary>
        /// 算法处理历史操作
        /// </summary>
        public Stack<Pretreat> IrradianceDataPretreat { get; set; } = new Stack<Pretreat>();
    }
}

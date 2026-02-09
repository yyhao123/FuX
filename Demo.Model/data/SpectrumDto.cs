
using Demo.Model.attribute;
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
    public class SpectrumDto
    {
        public SpectrumDto() { }

        public SpectrumDto(Spectrum spectrum, SpectrumDataRaw spectrumRaw, SpectrumDataDark spectrumDark, SpectrumDataWhiteBoard spectrumDataWhiteBoard = null)
        {

            Id = spectrum.Id;
            Name = spectrum.Name;
            RamanShift = spectrum.DeviceRamanShift.RamanShift;
            WelShift = spectrum.DeviceRamanShift.WelShift;
            DeviceSN = spectrum.DeviceRamanShift.DeviceSN;
            AcquireType = spectrum.AcquireType;
            Created = Convert.ToDateTime( spectrum.Created);
            Comment = spectrum.Comment;
            IntegrationTime = spectrum.IntegrationTime;
            LaserPower = spectrum.LaserPower;
            Average = spectrum.Average;
            AcquireMethod = spectrum.AcquireMethod;
            DeviceRamanShiftId = spectrum.DeviceRamanShiftId;
            DisplayDataType = spectrum.DisplayDataType;
            CenterWave = spectrum.CenterWave;
            Grating = spectrum.Grating;
            Wavelength = spectrum.WaveLength;
            PramInfo = spectrum.PramInfo;
            PixelNumber = spectrum.PixelCount;
            CollectType = spectrum.CollectType;
            Address = spectrum.DeviceRamanShift.Address != null ? JsonConvert.DeserializeObject<int[]>(spectrum.DeviceRamanShift.Address) : new int[] { 0 };

            if (spectrumRaw != null && spectrumDark != null)
            {
                var data = new SpectrumDataDto(spectrumRaw, spectrumDark, spectrumDataWhiteBoard);
                Data.Add(data);
            }
        }

        public string Id { get; set; }

        [ExportField(Name = "Name", TitleCn = "名称", TitleEn = "Name")]
        public string Name { get; set; }

        public int[] Address { get; set; }


        public double[] RamanShift { get; set; }

        public double[] WelShift { get; set; }

        public IList<SpectrumDataDto> Data { get; set; } = new List<SpectrumDataDto>();

        public int gratingLines { get; set; }
        public int Grating { get; private set; }
        public int Wavelength { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Comment { get; set; }

        public string CCDPonit { get; set; }

        /// <summary>
        /// 积分时间
        /// </summary>
        public int IntegrationTime { get; set; }

        /// <summary>
        /// 激光功率
        /// </summary>
        public int LaserPower { get; set; }

        /// <summary>
        /// 平均次数
        /// </summary>
        public int Average { get; set; }

        /// <summary>
        /// 采集方法：快速/精确/高精
        /// </summary>
        public AcquireMethod AcquireMethod { get; set; }

        /// <summary>
        /// 采集类型：单次采集/Mapping采集
        /// </summary>
        public AcquireType AcquireType { get; set; }

        [ExportField(Name = "CollectType", TitleCn = "采集模式", TitleEn = "Acquisition Mode")]
        public CollectType CollectType { get; set; }

        public string DeviceRamanShiftId { get; set; }

        [ExportField(Name = "DisplayDataType", TitleCn = "光谱类型", TitleEn = "Spectrum Type")]
        public DisplayDataType DisplayDataType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [ExportField(Name = "Created", TitleCn = "扫描时间", TitleEn = "Created")]
        public DateTime Created { get; set; }

        [ExportField(Name = "DeviceSN", TitleCn = "设备序列号", TitleEn = "Device SN")]
        public string DeviceSN { get; set; }

        /// <summary>
        /// Mapping acquire total row count
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Mapping acquire total column count
        /// </summary>
        public int Column { get; set; }
        public int CenterWave { get; private set; }



        /// <summary>
        /// 像素个数，用于导入数据时初始化数组长度
        /// </summary>
        [ExportField(Name = "PixelNumber", TitleCn = "数据个数", TitleEn = "Pixel Num")]
        public int PixelNumber { get; set; } = 2048;

        /// <summary>
        /// 创建时间
        /// </summary>
        [ExportField(Name = "PramInfo", TitleCn = "基本信息", TitleEn = "DataInfo")]
        public string PramInfo { get; set; }


        /// <summary>
        /// 光谱来源，为了兼容导入的谱图DataLocation 取值不一样
        /// </summary>
        public SpectrumSource Source { get; set; }
    }
}

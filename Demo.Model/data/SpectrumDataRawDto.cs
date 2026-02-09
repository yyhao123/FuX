using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpectrumDataRawDto
    {
        public SpectrumDataRawDto() { }

        public SpectrumDataRawDto(SpectrumDataRaw data)
        {
            Id = data.Id;
            SpectrumId = data.SpectrumId;
            Intensity = data.Intensity;
            SpectrumDataDarkId = data.SpectrumDataDarkId;
            WhiteBoardId = data.WhiteBoardId;
            IntensityLe = data.IntensityLe;
        }

        public string Id { get; set; }

        public double[] Intensity { get; set; }

        public double[] IntensityLe { get; set; }

        public string SpectrumId { get; set; }

        public string SpectrumDataDarkId { get; set; }

        /// <summary>
        /// 白板数据
        /// </summary>
        public string WhiteBoardId { get; set; }
    }
}

using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpectrumDataWhiteBoardDto
    {
        public SpectrumDataWhiteBoardDto() { }

        public SpectrumDataWhiteBoardDto(SpectrumDataWhiteBoard data)
        {
            Id = data.Id;
            Intensity = data.Intensity;
            SpectrumDataDarkId = data.SpectrumDataDarkId;
        }

        public string Id { get; set; }

        public double[] Intensity { get; set; }


        public string SpectrumDataDarkId { get; set; }

    }
}

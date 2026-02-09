using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpectrumDataDarkDto
    {
        public SpectrumDataDarkDto() { }

        public SpectrumDataDarkDto(SpectrumDataDark data)
        {
            Id = data?.Id;
            Intensity = data?.Intensity;           
            IntensityLe = data?.IntensityLe;
        }

        public string Id { get; set; }

        public double[] Intensity { get; set; }

        public double[] IntensityLe { get; set; }
    }
}

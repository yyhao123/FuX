using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    public enum SpectrumSource
    {
        /// <summary>
        /// Spectrum from internal
        /// </summary>
        Internal = 0,

        /// <summary>
        /// Spectrum from import mapping zop file format
        /// </summary>
        ImportMapping = 1,

        /// <summary>
        /// Spectrum from import single csv/txt file format
        /// </summary>
        ImportSingle = 2
    }
}

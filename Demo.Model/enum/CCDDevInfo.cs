using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.@enum
{
    /// <summary>
    /// CCD信息
    /// </summary>
    public enum CCDDevInfo
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// TC261
        /// </summary>
        [Description("TC261")]
        TC261 = 1,

        /// <summary>
        /// ATD2100USB
        /// </summary>
        [Description("ATD2100USB")]
        ATD2100USB = 2,

        /// <summary>
        /// ATD7100USB
        /// </summary>
        [Description("ATD7100USB")]
        ATD7100USB = 3,

        /// <summary>
        /// ATD2100Com
        /// </summary>
        [Description("ATD2100Com")]
        ATD2100Com = 4,

        /// <summary>
        /// ATD7100Com
        /// </summary>
        [Description("ATD7100Com")]
        ATD7100Com = 5,

        /// <summary>
        /// TC261Area
        /// </summary>
        [Description("TC261Area")]
        TC261Area = 6,

        /// <summary>
        /// Toup2048
        /// </summary>
        [Description("Toup2048")]
        Toup2048 = 7,

        /// <summary>
        /// Toup640
        /// </summary>
        [Description("Toup640")]
        Toup640 = 8,

        /// <summary>
        /// EMCCD
        /// </summary>
        [Description("EMCCD")]
        EMCCD = 9,

        /// <summary>
        /// OPTO-D
        /// </summary>
        [Description("OPTO-D")]
        OPTOD = 10,
    }
}

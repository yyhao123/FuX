using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpectrumProcessedData
    {
        /// <summary>
        /// 基线校正背景数据
        /// </summary>
        public double[] Dark { get; set; }

        /// <summary>
        /// 基线校正-原始数据  
        /// X轴为波数(x,y坐标的取值): (RamanShift[0], Raw[0])
        /// X轴为像素(x,y坐标的取值) (1,Raw[0]) ,(2,Raw[1]),...
        /// </summary>
        public double[] Raw { get; set; }

        /// <summary>
        /// 基线校正扣背景数据
        /// </summary>
        public double[] DarkSubtracted { get; set; }

        /// <summary>
        /// 白板
        /// </summary>
        public double[] WhiteBoard { get; set; }

        /// <summary>
        /// 吸光度
        /// </summary>
        public double[] AbsorbanceData { get; set; }

        /// <summary>
        /// 辐照度
        /// </summary>
        public double[] IrradianceData { get; set; }

        /// <summary>
        /// 透射率
        /// </summary>
        public double[] TransmissivityData { get; set; }

        /// <summary>
        /// 反射率
        /// </summary>
        public double[] ReflectivityData { get; set; }
    }
}

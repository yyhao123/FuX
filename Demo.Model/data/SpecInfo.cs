
using Demo.Model.@enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class SpecInfo
    {
        public string id { get; set; } = Guid.NewGuid().ToString("N");


        /// <summary>
        /// 坐标
        /// </summary>
        public Point DataPoint { get; set; }


        /// <summary>
        /// 线条颜色
        /// </summary>
        public Color lineColor { get; set; }

        /// <summary>
        /// 线条名字
        /// </summary>
        public string lineName { get; set; }

        /// <summary>
        /// 谱图类型 0纵  1横
        /// </summary>
        public ZedType ZedType { get; set; }

        /// <summary>
        /// X数据
        /// </summary>
        public List<double> Xdata { get; set; } = new List<double>();

        /// <summary>
        /// Y数据
        /// </summary>
        public List<double> Ydata { get; set; } = new List<double>();

        /// <summary>
        /// 原数据对象
        /// </summary>
        public SpectrumDto spectrumDto { get; set; } = new SpectrumDto();

        /// <summary>
        /// 谱线
        /// </summary>
        public List<int> LineNum { get; set; } = new List<int>();

        /// <summary>
        /// 运算模式
        /// </summary>
        public AlgorithmMode AlgorithmMode { get; set; } = AlgorithmMode.Avg;


    }
}

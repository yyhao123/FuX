using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class GratingBindWel
    {

        /// <summary>
        /// 光栅
        /// </summary>
        public string grating { get; set; }

        /// <summary>
        /// 波长
        /// </summary>
        public string wavelength { get; set; }

        /// <summary>
        /// 原点
        /// </summary>
        public int point { get; set; }

        /// <summary>
        /// C1入光口1偏移
        /// </summary>
        public int c1r1num { get; set; }


        /// <summary>
        /// C1入光口2偏移
        /// </summary>
        public int c1r2num { get; set; }


        /// <summary>
        /// C2入光口1偏移
        /// </summary>
        public int c2r1num { get; set; }


        /// <summary>
        /// C2入光口2偏移
        /// </summary>
        public int c2r2num { get; set; }

        /// <summary>
        /// 系数
        /// </summary>
        public List<double> cof { get; set; }

        /// <summary>
        /// 总距离
        /// </summary>
        public int sumdis { get; set; }

        /// <summary>
        /// 开始距离
        /// </summary>
        public int startdis { get; set; }

        /// <summary>
        /// 总距离 实际
        /// </summary>
        public int sumdissj { get; set; }

        /// <summary>
        /// 开始距离 实际
        /// </summary>
        public int startdissj { get; set; }

        /// <summary>
        /// 开始波长
        /// </summary>
        public int startwel { get; set; }

        /// <summary>
        /// 显示值
        /// </summary>
        public int shownums { get; set; }

        /// <summary>
        /// 显示值
        /// </summary>
        public int shownume { get; set; }

        /// <summary>
        /// 定标参数
        /// </summary>
        public List<ComObj> comObjs { get; set; }

        /// <summary>
        /// 是否固定
        /// </summary>
        public bool isband { get; set; }

        /// <summary>
        /// 是否正采
        /// </summary>
        public bool IsSide { get; set; }

        /// <summary>
        /// 是否存在算法模型文件
        /// </summary>
        public bool modelFile { get; set; }

    }
}

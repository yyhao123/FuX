using Demo.Model.@enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 设备指令操作管理模块
    /// </summary>
    public class DevCmdInfo
    {
        /// <summary>
        /// 项目代码
        /// </summary>
        [Description("项目代码")]
        public string Pid { get; set; }

        /// <summary>
        /// ATP7810单片机
        /// </summary>
        [Description("光谱仪单片机")]
        public FuncUse Mcon { get; set; } = FuncUse.Activation;
   
        /// <summary>
        /// 光栅
        /// </summary>
        [Description("光栅")]
        public ConBoard GS { get; set; } = ConBoard.None;

        /// <summary>
        /// 反射镜1
        /// </summary>
        [Description("反射镜入光")]
        public ConBoard FS { get; set; } = ConBoard.None;

        /// <summary>
        /// 反射镜2
        /// </summary>
        [Description("反射镜出光")]
        public ConBoard FS2 { get; set; } = ConBoard.None;

        /// <summary>
        /// M2
        /// </summary>
        [Description("M2")]
        public ConBoard M2 { get; set; } = ConBoard.None;

        /// <summary>
        /// 可见光
        /// </summary>
        [Description("可见光")]
        public ConBoard L { get; set; } = ConBoard.None;

        /// <summary>
        /// 激光
        /// </summary>
        [Description("激光")]
        public ConBoard J { get; set; } = ConBoard.None;

        /// <summary>
        /// 系统配置
        /// </summary>
        [Description("系统配置")]
        public ConBoard CFG { get; set; } = ConBoard.None;


    }
}

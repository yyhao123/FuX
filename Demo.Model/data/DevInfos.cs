using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 设备配置信息模块
    /// </summary>
    public class DevInfos
    {

        /// <summary>
        /// 电机配置管理
        /// </summary>
        [Description("电机配置管理")]
        public DevCmdInfo DevCmdInfo { get; set; }

        /// <summary>
        /// 激发波长与光栅管理
        /// </summary>
        [Description("激发波长与光栅管理")]
        public List<GratingBindWel> GratingBindWel { get; set; } = new List<GratingBindWel>();

        /// <summary>
        /// 激发波长与CCD关系
        /// </summary>
        [Description("激发波长与CCD关系")]
        public List<LwInfo> LwInfo { get; set; }


        /// <summary>
        /// 相机配置，可能存在多个相机
        /// </summary>
        [Description("相机配置，可能存在多个相机")]
        public List<CCDInfo> CCDInfos { get; set; }

        /// <summary>
        /// 物镜使用
        /// </summary>
        [Description("物镜使用")]
        public List<ComObj> MicroLen { get; set; }

        /// <summary>
        /// PI信息
        /// </summary>
        [Description("PI信息")]
        public List<ComObj> PIInfo { get; set; }

        /// <summary>
        /// 界面管理
        /// </summary>
        [Description("界面管理")]
        public List<int> UIInfo { get; set; } = new List<int>();

        /// <summary>
        /// 操作用户
        /// </summary>
        [Description("操作用户")]
        public string UpdUser { get; set; }

        /// <summary>
        /// 是否为登录模式
        /// </summary>
        [Description("是否为登录模式")]
        public bool OpenLogin { get; set; } = false;

        /// <summary>
        /// 是否为测试模式
        /// </summary>
        [Description("是否为测试模式")]
        public bool IsTest { get; set; } = false;

       

    }
}

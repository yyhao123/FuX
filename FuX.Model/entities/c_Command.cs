using FuX.Model.@enum;
using FuX.Model.Specenum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.entities
{
    /// <summary>
    /// 指令表
    /// </summary>
    public class c_Command : BaseInfo
    {
        public c_Command()
        {
            
        }
        /// <summary>
        /// 代码
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]

        public string id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// 指令名称
        /// </summary>
        public string cmdName { get; set; }

        /// <summary>
        /// 入参类型
        /// </summary>
        public CmdInDataType InType { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string devName { get; set; }

        /// <summary>
        /// 指令编号
        /// </summary>
        public string cmdNum { get; set; }

        /// <summary>
        /// 指令键值
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 超时时长
        /// </summary>
        public int timeout { get; set; } = 1000;

        /// <summary>
        /// 测试数据
        /// </summary>
        public string testData { get; set; }

        /// <summary>
        /// 测试数据(标识)
        /// </summary>
        public string testDataTip { get; set; }

        /// <summary>
        /// 数值长度
        /// </summary>
        public int strnum { get; set; }

        /// <summary>
        /// 是否传入数据
        /// </summary>
        public Iseffective isData { get; set; }

        /// <summary>
        /// 返回类型
        /// </summary>
        public CmdRetDataType retType { get; set; }


        /// <summary>
        /// 是否透传
        /// </summary>
        public bool IsTransmission { get; set; } = false;

        /// <summary>
        /// 透传指令
        /// </summary>
        public string TransmissionCmd { get; set; }
    }
}

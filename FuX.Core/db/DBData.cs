using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.db
{
    /// <summary>
    /// 数据库数据
    /// </summary>
    public class DBData
    {
        /// <summary>
        /// 自动创建 <br/> 如果存在此数据库则使用，没有则创建
        /// </summary>
        [Description("自动创建")]
        public bool AutoCreate { get; set; } = true;

        /// <summary>
        /// 数据库文件名称
        /// </summary>
        [Description("数据库文件名称")]
        public string DBFileName { get; set; } = "Com_db_Data.db";

        /// <summary>
        /// 数据库文件路径
        /// </summary>
        [Description("数据库文件路径")]
        public string DBFilePath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}db";
    }
}

using FuX.Model.@enum;
using FuX.Model.Specenum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.entities
{
    /// <summary>
    /// 数据库表基类
    /// </summary>
    public class BaseInfo
    {

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 排序编号
        /// </summary>
        public int? sortno { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnName = "Created")]
        public DateTime Created { get; set; } 


        /// <summary>
        /// 是否有效
        /// </summary>
        public Iseffective? iseffective { get; set; } = Iseffective.OK;

        /// <summary>
        /// 操作用户
        /// </summary>
        public string operateuser { get; set; }
    }
}

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
    /// ConfigInfo
    /// </summary>
    /// <summary>
    /// ConfigInfo
    /// </summary>
    public class ConfigInfo
    {
        /// <summary>
        /// ConfigInfo
        /// </summary>
        public ConfigInfo()
        {

        }

        /// <summary>
        /// id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public System.String id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// KeyName
        /// </summary>
       
        public System.String KeyName { get; set; }

        /// <summary>
        /// CfgVal
        /// </summary>
        public System.String CfgVal { get; set; }

        /// <summary>
        /// tipInfo
        /// </summary>
        public System.String tipInfo { get; set; }

        /// <summary>
        /// dataType
        /// </summary>
       // public System.Int64? dataType { get; set; }
        public CfgDataType dataType { get; set; }


        /// <summary>
        /// DataInfo
        /// </summary>
        public System.String DataInfo { get; set; }

        /// <summary>
        /// remark
        /// </summary>
        public System.String remark { get; set; }

        /// <summary>
        /// sortno
        /// </summary>
        public System.Int64? sortno { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        public System.Int64? Created { get; set; }

        /// <summary>
        /// iseffective
        /// </summary>
        public System.Int64? iseffective { get; set; }

        /// <summary>
        /// operateuser
        /// </summary>
        public System.String operateuser { get; set; }
    }
}

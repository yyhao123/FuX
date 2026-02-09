using FuX.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.services
{
    public partial interface IUserCfg
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        List<ConfigInfo> UserCfgInfo { get; set; }
    }

    public partial class UserCfg : IUserCfg
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public List<ConfigInfo> UserCfgInfo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    public class RevData
    {
        /// <summary>
        /// 命令号
        /// </summary>
        public byte bCommand { get; set; }

        /// <summary>
        /// 接受到的数据
        /// </summary>
        public byte[] lDatas { get; set; }

        /// <summary>
        /// 接受到的数据
        /// </summary>
        public byte[] Datas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 分页model
    /// </summary>
    public class Pagination<T>
    {
        /// <summary>
        /// 记录总数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 每页几条数据
        /// </summary>
        public int PageNumber { get; set; } = 50;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        public int SkipCount
        {
            get
            {
                return PageIndex * PageNumber;
            }
        }
    }
}

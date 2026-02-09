using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 固定模型
    /// </summary>
    public  class FixedModel
    {
        /// <summary>
        /// 第一个头
        /// </summary>
        public const byte HEAD_FIRST = 0xAA;

        /// <summary>
        /// 第二个头
        /// </summary>
        public const byte HEAD_SECOND = 0x55;

        /// <summary>
        /// 包头长度
        /// </summary>
        public const int HEAD_LENGTH = 2;

        /// <summary>
        /// 长度byte length
        /// </summary>
        public const int LENGTH_LENGTH = 2;

        public const int COMMAND_OFFSET = 4;

        public const int LENGTH_OFFSET = 2;

        public const int DATA_OFFSET = 5;

        /// <summary>
        /// 最小的包大小
        /// AA 55 00 04 A1 A5
        /// </summary>
        public const int MIN_PACK_LENGTH = 6;
    }
}
